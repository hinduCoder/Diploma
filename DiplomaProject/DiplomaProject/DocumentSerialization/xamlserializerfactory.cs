using System;
using System.IO;
using System.Windows.Documents.Serialization;

namespace DiplomaProject.DocumentSerialization
{
    internal class XamlSerializerFactory : ISerializerFactory
    {
        public SerializerWriter CreateSerializerWriter(Stream stream)
        {
            return new XamlSerializerWriter(stream);
        }

        public string DisplayName
        {
            get { return "Xaml Document Writer"; }
        }

        public string ManufacturerName
        {
            get { return "Microsoft"; }
        }

        public Uri ManufacturerWebsite
        {
            get { return new Uri("http://www.microsoft.com"); }
        }

        public string DefaultFileExtension
        {
            get { return ".xaml"; }
        }
    }
}