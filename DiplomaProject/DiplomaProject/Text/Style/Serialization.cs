using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using DiplomaProject.Properties;

namespace DiplomaProject.Text
{
    public class TextStyleProvider
    {
        private readonly FileInfo _file = new FileInfo(String.Format(@"{0}\{1}\{2}", Environment.CurrentDirectory, Resources.AssetsFolder,
            Resources.TextStylesFile));
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(StyleCollectionSerializationProxy));
        private List<ITextStyle> _styles;

        public TextStyleProvider()
        {
            var directory = String.Format("{0}\\{1}", Environment.CurrentDirectory, Resources.AssetsFolder);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        public IReadOnlyList<ITextStyle> LoadTextStyles()
        {
            if (_styles != null)
            {
                return _styles; // Lazy
            }
            using (var fileStream = _file.OpenRead())
            {
                using (var xmlTextReader = new XmlTextReader(fileStream))
                {
                    var result = _xmlSerializer.Deserialize(xmlTextReader);
                    _styles =
                        ((StyleCollectionSerializationProxy) result).Styles.Select(s => s.ToRealTextStyle()).ToList();
                    return _styles;
                }
            }
        }

        public void AddTextStyle(ITextStyle textStyle)
        {
            _styles.Add(textStyle);
        }

        public void DumpTextStyles(IList<ITextStyle> textStyles)
        {
            _styles = new List<ITextStyle>(textStyles);
            using (var fileStream = _file.OpenWrite())
            {
                using (var xmlTextWriter = new XmlTextWriter(fileStream, Encoding.Default))
                {
                    _xmlSerializer.Serialize(xmlTextWriter,
                        new StyleCollectionSerializationProxy
                        {
                            Styles =
                                new List<StyleSerializationProxy>(textStyles.Select(s => new StyleSerializationProxy(s)))
                        });
                }
            }
        }
    }
    [XmlRoot("Styles")]
    public class StyleCollectionSerializationProxy
    {
        public StyleCollectionSerializationProxy()
        {
            Styles = new List<StyleSerializationProxy>();
        }
        [XmlArray("StyleCollection")]
        [XmlArrayItem("Style")]
        public List<StyleSerializationProxy> Styles { get; set; }
    }
    public class StyleSerializationProxy
    {
        public StyleSerializationProxy() {}
        public StyleSerializationProxy(ITextStyle style)
        {
            Name = style.Name;
            IsOneParagraph = style.IsOneParagraph;
            FontWeight = style.FontWeight.ToString();
            FontFamily = style.FontFamily.ToString();
            FontStyle = style.FontStyle.ToString();
            FontSize = style.FontSize;
            FontColor = style.FontColor.ToString();
        }
        [XmlAttribute]
        public string Name { get; set; }

        [XmlAttribute]
        public bool IsOneParagraph { get; set; }

        [XmlAttribute]
        public string FontFamily { get; set; }

        [XmlAttribute]
        public string FontWeight { get; set; }

        [XmlAttribute]
        public string FontStyle { get; set; }

        [XmlAttribute]
        public int FontSize { get; set; }

        [XmlAttribute]
        public string FontColor { get; set; }

        public ITextStyle ToRealTextStyle()
        {
            return new TextStyleImpl
            {
                Name = Name,
                IsOneParagraph = IsOneParagraph,
                FontSize = FontSize,
                FontFamily = new FontFamily(FontFamily),
                FontStyle = (FontStyle) new FontStyleConverter().ConvertFromString(FontStyle),
                FontWeight = (FontWeight) new FontWeightConverter().ConvertFromString(FontWeight),
                FontColor = (Color) ColorConverter.ConvertFromString(FontColor)
            };
        }
    }
    public class TextStyleImpl : ITextStyle
    {
        public string Name { get; set; }
        public bool IsOneParagraph { get; set; }
        public FontFamily FontFamily { get; set; }
        public FontWeight FontWeight { get; set; }
        public FontStyle FontStyle { get; set; }
        public int FontSize { get; set; }
        public Color FontColor { get; set; }
    }
  
}
