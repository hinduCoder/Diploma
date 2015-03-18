using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Media;

namespace DiplomaProject.Text
{
    public interface ITextStyle {
        string Name { get; set; }
        bool IsOneParagraph { get; set; }
        FontFamily FontFamily { get; set; }
        FontWeight FontWeight { get; set; }
        FontStyle FontStyle { get; set; }
        int FontSize { get; set; }
    }

    public static class FontParameters
    {
        private static IEnumerable<String> _fontWeights;
        private static IEnumerable<String> _fontStyles;

        public static IEnumerable<String> FontWeights
        {
            get { return _fontWeights ?? (_fontWeights = GetNames(typeof (FontWeights))); }
        }

        public static IEnumerable<String> FontStyles
        {
            get { return _fontStyles ?? (_fontStyles = GetNames(typeof(FontStyles))); }
        }

        private static IEnumerable<String> GetNames(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Static)
                .Select(p => p.Name);
        }
    }
    
}