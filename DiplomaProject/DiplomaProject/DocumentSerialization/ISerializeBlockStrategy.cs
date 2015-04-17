using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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
            if (block is List)
            {
                var list = block as List;
                return new ListSerializeStrategy(list.MarkerStyle == TextMarkerStyle.Decimal ? "Olist" : "UList");
            }
            if (block is DrawerBlock)
                return new DrawerSerializeStrategy();
            if (block is PlotBlock)
                return new PlotSerializeStrategy();
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
    public class ListSerializeStrategy : ISerializeBlockStrategy
    {
        private string _tagName;

        public ListSerializeStrategy(string tagName)
        {
            _tagName = tagName;
        }

        public XmlElement Serialize(Block block, XmlDocument xmlDocument)
        {
            var paragraphSerializeStrategy = new ParagraphSerializeStrategy();
            var list = (List) block;
            var listElement = xmlDocument.CreateElement(_tagName);
            foreach (var listItem in list.ListItems)
            {
                var listItemElement = xmlDocument.CreateElement("Item");

                foreach (var listItemBlock in listItem.Blocks)
                {
                    listItemElement.AppendChild(paragraphSerializeStrategy.Serialize(listItemBlock, xmlDocument));
                }
                listElement.AppendChild(listItemElement);
            }
            return listElement;
        }
    }

    public class DrawerSerializeStrategy : ISerializeBlockStrategy
    {
        public XmlElement Serialize(Block block, XmlDocument xmlDocument)
        {
            var drawerBlock = (DrawerBlock) block;
            var drawerElement = xmlDocument.CreateElement("Drawing");
            foreach(SmoothableStroke stroke in drawerBlock.Drawer.Strokes)
            {
                var strokeElement = xmlDocument.CreateElement("Stroke");
                foreach (var point in stroke.Points)
                {
                    var pointElement = xmlDocument.CreateElement("Point");
                    pointElement.SetAttribute("X", point.X.ToString()); //TODO DrawingAttributes
                    pointElement.SetAttribute("Y", point.Y.ToString());
                    strokeElement.AppendChild(pointElement);
                }
                drawerElement.AppendChild(strokeElement);
            }
            return drawerElement;
        }
    }

    public class PlotSerializeStrategy : ISerializeBlockStrategy
    {
        public XmlElement Serialize(Block block, XmlDocument xmlDocument)
        {
            var plotBlock = (PlotBlock) block;
            var plotElement = xmlDocument.CreateElement("Plot");
            foreach(SmoothableStroke stroke in plotBlock.PlotControl.DrawerControl.Strokes) {
                var strokeElement = xmlDocument.CreateElement("Stroke");
                foreach(var point in stroke.Points) {
                    var pointElement = xmlDocument.CreateElement("Point");
                    pointElement.SetAttribute("X", point.X.ToString()); //TODO DrawingAttributes
                    pointElement.SetAttribute("Y", point.Y.ToString());
                    strokeElement.AppendChild(pointElement);
                }
                plotElement.AppendChild(strokeElement);
            }
            return plotElement;
        }
    }
}