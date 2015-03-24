using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;

namespace DiplomaProject.DocumentSerialization
{
    public class FlowDocumentSerializer
    {
        public void SerializeToRtf(FlowDocument flowDocument, string fileName)
        {
            SerializerProvider serializerProvider = new SerializerProvider();
            var rtfSerializerDescriptor =
                serializerProvider.InstalledSerializers.Single(sd => sd.DisplayName.Contains("Rtf"));
            Serialize(rtfSerializerDescriptor, flowDocument, fileName);
        }

        public void SerializeToXaml(FlowDocument flowDocument, string fileName)
        {
            SerializerProvider serializerProvider = new SerializerProvider();
            var rtfSerializerDescriptor =
                serializerProvider.InstalledSerializers.Single(sd => sd.DisplayName.Contains("Xaml"));
            Serialize(rtfSerializerDescriptor, flowDocument, fileName);
        }
        private void Serialize(SerializerDescriptor serializerDescriptor, FlowDocument flowDocument, string fileName)
        {
            if(File.Exists(fileName))
                File.Delete(fileName);

            SerializerProvider serializerProvider = new SerializerProvider();
            using(var file = File.Create(fileName)) {
                SerializerWriter serializerWriter =
                    serializerProvider.CreateSerializerWriter(serializerDescriptor,
                        file);

                serializerWriter.Write(((IDocumentPaginatorSource)flowDocument).DocumentPaginator, null);
            }
        }

        public static void SaveDocumentToXaml(FlowDocument document, string fileName) {
            using(var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)) {
                var content = new TextRange(document.ContentStart, document.ContentEnd);
                content.Save(fs, DataFormats.Xaml, true);
            }
        }

        public static void SaveDocumentToRtf(FlowDocument document, string fileName) {
            using(var fs = new FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write)) {
                var content = new TextRange(document.ContentStart, document.ContentEnd);
                content.Save(fs, DataFormats.Rtf);
            }
        }

    }
}