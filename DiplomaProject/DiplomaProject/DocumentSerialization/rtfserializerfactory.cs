using System;
using System.IO;
using System.Windows.Documents.Serialization;

namespace DiplomaProject.DocumentSerialization
{
    internal class RtfSerializerFactory : ISerializerFactory
    {
        public SerializerWriter CreateSerializerWriter(Stream stream)
        {
            return new RtfSerializerWriter(stream);
        }

        public string DisplayName
        {
            get { return "Rtf Document Writer"; }
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
            get { return ".rtf"; }
        }
    }
}