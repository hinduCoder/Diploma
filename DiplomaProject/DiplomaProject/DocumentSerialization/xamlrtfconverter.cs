using System;
using System.Reflection;
using System.Windows;

namespace DiplomaProject.DocumentSerialization
{
    internal static class XamlRtfConverter
    {
        #region Private Fields

        private const int RtfCodePage = 1252;

        #endregion Private Fields

        #region Internal Methods

        internal static string ConvertXamlToRtf(string xamlContent)
        {
            var assembly = Assembly.GetAssembly(typeof (FrameworkElement));
            var xamlRtfConverterType = assembly.GetType("System.Windows.Documents.XamlRtfConverter");


            var xamlRtfConverter = Activator.CreateInstance(xamlRtfConverterType, /*nonPublic:*/true);


            var convertXamlToRtf = xamlRtfConverterType.GetMethod("ConvertXamlToRtf",
                BindingFlags.Instance | BindingFlags.NonPublic);
            var rtfContent = (string) convertXamlToRtf.Invoke(xamlRtfConverter, new object[] {xamlContent});


            return rtfContent;
        }


        internal static string ConvertRtfToXaml(string rtfContent)
        {
            var assembly = Assembly.GetAssembly(typeof (FrameworkElement));
            var xamlRtfConverterType = assembly.GetType("System.Windows.Documents.XamlRtfConverter");


            var xamlRtfConverter = Activator.CreateInstance(xamlRtfConverterType, /*nonPublic:*/true);


            var convertRtfToXaml = xamlRtfConverterType.GetMethod("ConvertRtfToXaml",
                BindingFlags.Instance | BindingFlags.NonPublic);


            var xamlContent = (string) convertRtfToXaml.Invoke(xamlRtfConverter, new object[] {rtfContent});


            return xamlContent;
        }

        #endregion Internal Methods
    }
}