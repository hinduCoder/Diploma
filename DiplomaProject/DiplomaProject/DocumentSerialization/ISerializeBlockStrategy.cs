using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Xml;
using DiplomaProject.Controls;

namespace DiplomaProject.DocumentSerialization
{
    public class SerializeBlockStrategyFactory {
        public ISerializeBlockStrategy GetStrategy(Block block) {
            if(block is Paragraph)
                return new ParagraphSerializeStrategy();
            if(block is FormulaBlock)
                return new FormulaSerializeStrategy();
            if(block is ImageBlock)
                return new ImageSerializeStrategy();
            return null;
        }
    }
    public interface ISerializeBlockStrategy
    {
        XmlElement Serialize(Block block, XmlDocument xmlDocument);
    }
   

    public class ParagraphSerializeStrategy : ISerializeBlockStrategy {
        public XmlElement Serialize(Block block, XmlDocument xmlDocument) {
            var paragraph = (Paragraph)block;
            var paragraphElement = xmlDocument.CreateElement("Paragraph");

            foreach(var inline in paragraph.Inlines) {
                var properties = inline.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty)
                    .Where(p => ParagraphSerializationHelper.PropertyConverters.Keys.Contains(p.Name))
                    .Select(p => Tuple.Create(p.Name, p.GetValue(inline)));
                var inlineElement = xmlDocument.CreateElement("Inline");
                foreach(var prop in properties) {
                    var xmlAttribute = xmlDocument.CreateAttribute(prop.Item1);
                    if(prop.Item2 != null)
                        xmlAttribute.InnerText = prop.Item2.ToString(); //TODO сложные объекты?
                    inlineElement.Attributes.Append(xmlAttribute);
                }
                inlineElement.SetAttribute("StyleName", FlowDocumentHelper.GetStyleName(inline));

                paragraphElement.AppendChild(inlineElement);
            }
            return paragraphElement;
        }
    }

    public class FormulaSerializeStrategy : ISerializeBlockStrategy {
        public XmlElement Serialize(Block block, XmlDocument xmlDocument) {
            var formula = (FormulaBlock)block;
            var formulaElement = xmlDocument.CreateElement("Formula");
            var texAttribute = xmlDocument.CreateAttribute("Tex");
            texAttribute.InnerText = formula.Formula;
            formulaElement.Attributes.Append(texAttribute);
            return formulaElement;
        }
    }
    public class ImageSerializeStrategy : ISerializeBlockStrategy {
        public XmlElement Serialize(Block block, XmlDocument xmlDocument) {
            var image = (ImageBlock)block;
            var imageElement = xmlDocument.CreateElement("Image");
            var srcAttribute = xmlDocument.CreateAttribute("Src");
            srcAttribute.InnerText = Path.GetFileName(image.Source.ToString());
            imageElement.Attributes.Append(srcAttribute);
            return imageElement;
        }
    }
}