using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
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
                case "UList":
                    return new ListDeserializeStrategy(TextMarkerStyle.Disc);
                case "OList":
                    return new ListDeserializeStrategy(TextMarkerStyle.Decimal);
                case "Drawing":
                    return new DrawerDeserializeStrategy();
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
                    if (attribute.Name != "StyleName")
                        run.GetType().GetProperty(attribute.Name).SetValue(run, ParagraphSerializationHelper.PropertyConverters[attribute.Name].ConvertFromString(attribute.InnerText));
                   
                }
                if(xmlNode.Attributes["StyleName"] != null)
                    FlowDocumentHelper.SetStyleName(run, xmlNode.Attributes["StyleName"].InnerText);
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

    public class ListDeserializeStrategy : IDeserializeBlockStrategy
    {
        private TextMarkerStyle _markerStyle;
        public ListDeserializeStrategy(TextMarkerStyle markerStyle)
        {
            _markerStyle = markerStyle;
        }

        public void Deserialize(XmlNode xmlNode, FlowDocument flowDocument)
        {
            var list = new List{ MarkerStyle = _markerStyle };
            foreach (XmlNode listItemNode in xmlNode.ChildNodes)
            {
                var listItem = new ListItem();
                foreach (XmlNode listItemBlockNode in listItemNode.ChildNodes)
                {
                    var paragraph = new Paragraph();

                    foreach (XmlNode listItemInlineNode in listItemBlockNode.ChildNodes)
                    {
                        var run = new Run();
                        foreach (XmlAttribute attribute in listItemInlineNode.Attributes)
                        {
                            if (attribute.Name != "StyleName")
                                run.GetType().GetProperty(attribute.Name).SetValue(run,
                                    ParagraphSerializationHelper.PropertyConverters[attribute.Name].ConvertFromString(
                                        attribute.InnerText));
                        }
                        if(xmlNode.Attributes["StyleName"] != null)
                            FlowDocumentHelper.SetStyleName(run, xmlNode.Attributes["StyleName"].InnerText);
                        paragraph.Inlines.Add(run);
                    }
                    listItem.Blocks.Add(paragraph);
                }
                list.ListItems.Add(listItem);
            }
            flowDocument.Blocks.Add(list);
        }
    }
    public class DrawerDeserializeStrategy : IDeserializeBlockStrategy
    {
        public void Deserialize(XmlNode xmlNode, FlowDocument flowDocument)
        {
            var inkCanvas = new InkCanvas();
            foreach (XmlNode strokeNode in xmlNode.ChildNodes)
            {
                var points = new StylusPointCollection();
                foreach (XmlNode pointNode in strokeNode.ChildNodes)
                {
                    points.Add(new StylusPoint(Double.Parse(pointNode.Attributes["X"].InnerText),
                        Double.Parse(pointNode.Attributes["Y"].InnerText)));
                }
                inkCanvas.Strokes.Add(new Stroke(points)); //TODO DrawingAttributes
            }
            flowDocument.Blocks.Add(new DrawerBlock { Child = new DrawerControl { Strokes = inkCanvas.Strokes }});
        }
    }
}