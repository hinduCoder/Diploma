using System;
using System.IO;
using System.Windows.Documents;
using System.Windows.Media.Imaging;
using System.Xml;
using DiplomaProject.Controls;
using DiplomaProject.Properties;

namespace DiplomaProject.DocumentSerialization
{
    public class DeserializeBlockStrategyFactory {
        public IDeserializeBlockStrategy GetStrategy(string elementName) {
            switch (elementName)
            {
                case "Paragraph":
                    return new ParagraphDeserializeStrategy();
                case "Formula":
                    return new FormulaDeserializeStrategy();
                case "Image":
                    return new ImageDeserializeStrategy();
            }
            return null;
        }
    }
    public interface IDeserializeBlockStrategy
    {
        void Deserialize(XmlNode xmlNode, FlowDocument flowDocument);
    }

    public class ParagraphDeserializeStrategy : IDeserializeBlockStrategy {
        public void Deserialize(XmlNode xmlNode, FlowDocument flowDocument) {
            var paragraph = new Paragraph();
            foreach(XmlNode inline in xmlNode.ChildNodes) {
                var run = new Run(); //TODO: other inlines ?
                foreach(XmlAttribute attribute in inline.Attributes) {
                    run.GetType().GetProperty(attribute.Name).SetValue(run, ParagraphSerializationHelper.PropertyConverters[attribute.Name].ConvertFromString(attribute.InnerText));
                }
                paragraph.Inlines.Add(run);
            }
            flowDocument.Blocks.Add(paragraph);
        }
    }
    public class ImageDeserializeStrategy : IDeserializeBlockStrategy {
        public void Deserialize(XmlNode xmlNode, FlowDocument flowDocument) {
            flowDocument.Blocks.Add(new ImageBlock {
                Source = new BitmapImage(new Uri(Path.GetFullPath(Path.Combine(Path.Combine(Resources.TempFolder, Resources.ImagesFolder), xmlNode.Attributes["Src"].InnerText))))
            });
        }
    }
    public class FormulaDeserializeStrategy : IDeserializeBlockStrategy {
        public void Deserialize(XmlNode xmlNode, FlowDocument flowDocument) {
            flowDocument.Blocks.Add(new FormulaBlock { Formula = xmlNode.Attributes["Tex"].InnerText });
        }
    }
}