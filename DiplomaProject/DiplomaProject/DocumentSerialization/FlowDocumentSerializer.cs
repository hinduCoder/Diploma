using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using DiplomaProject.Controls;
using Ionic.Zip;

namespace DiplomaProject.DocumentSerialization
{
    public class FlowDocumentSerializer
    {
        private static List<string> imageNames = new List<string>();

        public static void Serialize(FlowDocument document)
        {
            var xml = new XmlDocument();
            var root = xml.CreateElement("Document");
            xml.AppendChild(root);

            var factory = new SerializeBlockStrategyFactory();
            foreach (var block in document.Blocks)
            {
                if (block is BlockImageContainer)
                {
                    imageNames.Add(new Uri(((BlockImageContainer) block).Source.ToString()).LocalPath);
                }
                root.AppendChild(factory.GetStrategy(block).Serialize(block, xml));
            }
            var outputFileName = "xml.xml";
            using (var xmlWriter = XmlWriter.Create(outputFileName))
            {
                try {
                    File.SetAttributes(outputFileName, FileAttributes.Hidden);
                    xml.WriteTo(xmlWriter);
                } catch(UnauthorizedAccessException) {
                    File.Delete(outputFileName);
                }
            }

            using (var zipFile = new ZipFile())
            {
                zipFile.AddFile(outputFileName);
                foreach (var imageName in imageNames)
                {
                    zipFile.AddFile(imageName, "Images");
                }
                using(var outputStream = File.Open("doca.zip", FileMode.Create))
                {
                    zipFile.Save(outputStream);
                }
            }
            File.Delete(outputFileName);
        }

        public static FlowDocument Deserialize()
        {
            //Directory.Delete("doca", true);
            var directory = Directory.CreateDirectory("doca");
            directory.Attributes = FileAttributes.Hidden;
            using (var zipFile = ZipFile.Read("doca.zip"))
            {
                zipFile.ExtractAll(directory.FullName);
            }
            var xmlFile = directory.EnumerateFiles().Single(f => f.Name == "xml.xml").FullName;
            var xmlDocument = new XmlDocument();
            using (var xmlReader = XmlReader.Create(xmlFile))
            {
                xmlDocument.Load(xmlReader);
            }
            var flowDocument = new FlowDocument();
            foreach (XmlNode block in xmlDocument.LastChild.ChildNodes)
            {
                switch (block.Name)
                {
                    case "Formula":
                        flowDocument.Blocks.Add(new FormulaBlock { Formula = block.Attributes["Tex"].InnerText });
                        break;
                    case "Image":
                        flowDocument.Blocks.Add(new BlockImageContainer { Source = new BitmapImage(new Uri(block.Attributes["Src"].InnerText)) });
                        break;
                    case "Paragraph":
                        var paragraph = new Paragraph();
                        foreach (XmlNode inline in block.ChildNodes)
                        {
                            var run = new Run();
                            foreach (XmlAttribute attribute in inline.Attributes)
                            {
                                run.GetType().GetProperty(attribute.Name).SetValue(run, ParagraphSerializeStrategy.PropertyConverters[attribute.Name].ConvertFromString(attribute.InnerText));
                            }
                            paragraph.Inlines.Add(run);
                        }
                        flowDocument.Blocks.Add(paragraph);
                        break;
                }
            }
            return flowDocument;
        }
        public void SerializeToRtf(FlowDocument flowDocument, string fileName)
        {
            SerializerProvider serializerProvider = new SerializerProvider();
            var rtfSerializerDescriptor =
                serializerProvider.InstalledSerializers.Single(sd => sd.DisplayName.Contains("Rtf"));
            Serialize(rtfSerializerDescriptor, flowDocument, fileName);
        }

        public void SerializeToXaml(FlowDocument flowDocument, string fileName)
        {
            SerializerProvider serializerProvider = new SerializerProvider();
            var rtfSerializerDescriptor =
                serializerProvider.InstalledSerializers.Single(sd => sd.DisplayName.Contains("Xaml"));
            Serialize(rtfSerializerDescriptor, flowDocument, fileName);
        }
        private void Serialize(SerializerDescriptor serializerDescriptor, FlowDocument flowDocument, string fileName)
        {
            if(File.Exists(fileName))
                File.Delete(fileName);

            SerializerProvider serializerProvider = new SerializerProvider();
            using(var file = File.Create(fileName)) {
                SerializerWriter serializerWriter =
                    serializerProvider.CreateSerializerWriter(serializerDescriptor,
                        file);

                serializerWriter.Write(((IDocumentPaginatorSource)flowDocument).DocumentPaginator, null);
            }
        }

        public static void SaveDocumentToXaml(FlowDocument document, string fileName) {
            using(var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)) {
                var content = new TextRange(document.ContentStart, document.ContentEnd);
                content.Save(fs, DataFormats.XamlPackage, true);
            }
        }

        public static void SaveDocumentToRtf(FlowDocument document, string fileName) {
            using(var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)) {
                var content = new TextRange(document.ContentStart, document.ContentEnd);
                content.Save(fs, DataFormats.Rtf);
            }
        }
    }

    public class SerializeBlockStrategyFactory
    {
        public ISerializeBlockStrategy GetStrategy(Block block)
        {
            if (block is Paragraph)
                return new ParagraphSerializeStrategy();
            if (block is FormulaBlock)
                return new FormulaSerializeStrategy();
            if (block is BlockImageContainer)
                return new ImageSerializeStrategy();
            return null;
        }
    }

    public interface ISerializeBlockStrategy
    {
        XmlElement Serialize(Block block, XmlDocument xmlDocument);
    }

    public class ParagraphSerializeStrategy : ISerializeBlockStrategy
    {
        public static readonly Dictionary<string, TypeConverter> PropertyConverters = new Dictionary<string, TypeConverter>()
        {
            {"TextIndent", new DoubleConverter()},//Maybe unless
            {"TextAlignment", new EnumConverter(typeof(TextAlignment))},
            {"LineHeight", new Int32Converter()},
            {"FontStyle", new FontStyleConverter()},
            {"FontSize", new Int32Converter()},
            {"FontWeight", new FontWeightConverter()},
            {"Foreground", new BrushConverter()},
            {"FontStretch", new FontStretchConverter()},
            {"FontFamily", new FontFamilyConverter()},
            {"Text", new StringConverter()}
        };
        public XmlElement Serialize(Block block, XmlDocument xmlDocument)
        {
            var paragraph = (Paragraph) block;
            var paragraphElement = xmlDocument.CreateElement("Paragraph");

            foreach (var inline in paragraph.Inlines)
            {
                var properties = inline.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
                    .Where(p =>  PropertyConverters.Keys.Contains(p.Name))
                    .Select(p => Tuple.Create(p.Name, p.GetValue(inline)));
                var inlineElement = xmlDocument.CreateElement("Inline");
                foreach(var prop in properties) {
                    var xmlAttribute = xmlDocument.CreateAttribute(prop.Item1);
                    if(prop.Item2 != null)
                        xmlAttribute.InnerText = prop.Item2.ToString(); //TODO сложные объекты?
                    inlineElement.Attributes.Append(xmlAttribute);
                }
                paragraphElement.AppendChild(inlineElement);
            }
            return paragraphElement;
        }
    }

    public class FormulaSerializeStrategy : ISerializeBlockStrategy
    {
        public XmlElement Serialize(Block block, XmlDocument xmlDocument)
        {
            var formula = (FormulaBlock) block;
            var formulaElement = xmlDocument.CreateElement("Formula");
            var texAttribute = xmlDocument.CreateAttribute("Tex");
            texAttribute.InnerText = formula.Formula;
            formulaElement.Attributes.Append(texAttribute);
            return formulaElement;
        }
    }
    public class ImageSerializeStrategy : ISerializeBlockStrategy
    {
        public XmlElement Serialize(Block block, XmlDocument xmlDocument)
        {
            var image = (BlockImageContainer)block;
            var imageElement = xmlDocument.CreateElement("Image");
            var srcAttribute = xmlDocument.CreateAttribute("Src");
            srcAttribute.InnerText = image.Source.ToString();
            imageElement.Attributes.Append(srcAttribute);
            return imageElement;
        }
    }
}