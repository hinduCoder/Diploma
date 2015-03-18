using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Xml;
using System.Xml.Serialization;
using DiplomaProject.Properties;

namespace DiplomaProject.Text
{
    public class TextStyleProvider
    {
        private readonly FileInfo _file = new FileInfo(String.Format(@"{0}\{1}\{2}", Environment.CurrentDirectory, Resources.AssetsFolder,
            Resources.TextStylesFile));
        private readonly XmlSerializer _xmlSerializer = new XmlSerializer(typeof(StyleCollectionSerializationWrapper));

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
            using (var xmlTextReader = new XmlTextReader(_file.OpenRead()))
            {
                var result = _xmlSerializer.Deserialize(xmlTextReader);
                return ((StyleCollectionSerializationWrapper)result).Styles.Select(s => s.ToRealTextStyle()).ToList();
            }
        }
    }
    [XmlRoot("Styles")]
    public class StyleCollectionSerializationWrapper
    {
        public StyleCollectionSerializationWrapper()
        {
            Styles = new List<StyleSerializationWrapper>();
        }
        [XmlArray("StyleCollection")]
        [XmlArrayItem("Style")]
        public List<StyleSerializationWrapper> Styles { get; set; }
    }
    public class StyleSerializationWrapper
    {
        public StyleSerializationWrapper() {}
        public StyleSerializationWrapper(ITextStyle style)
        {
            Name = style.Name;
            IsOneParagraph = style.IsOneParagraph;
            FontWeight = style.FontWeight.ToString();
            FontFamily = style.FontFamily.ToString();
            FontStyle = style.FontStyle.ToString();
            FontSize = style.FontSize;

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
    }
  
}
