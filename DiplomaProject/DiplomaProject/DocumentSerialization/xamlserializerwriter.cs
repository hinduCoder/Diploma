using System;
using System.IO;
using System.Printing;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml;

namespace DiplomaProject.DocumentSerialization
{
    internal class XamlSerializerWriter : SerializerWriter
    {
        private readonly Stream _stream;

        public XamlSerializerWriter(Stream stream)
        {
            _stream = stream;
        }

        public override void Write(Visual visual)
        {
            Write(visual, null);
        }

        public override void Write(Visual visual, PrintTicket printTicket)
        {
            SerializeObjectTree(visual);
        }

        public override void WriteAsync(Visual visual)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(Visual visual, object userState)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(Visual visual, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(Visual visual, PrintTicket printTicket, object userState)
        {
            throw new NotSupportedException();
        }

        public override void Write(DocumentPaginator documentPaginator)
        {
            Write(documentPaginator, null);
        }

        public override void Write(DocumentPaginator documentPaginator, PrintTicket printTicket)
        {
            SerializeObjectTree(documentPaginator.Source);
        }

        public override void WriteAsync(DocumentPaginator documentPaginator)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(DocumentPaginator documentPaginator, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(DocumentPaginator documentPaginator, object userState)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(DocumentPaginator documentPaginator, PrintTicket printTicket, object userState)
        {
            throw new NotSupportedException();
        }

        public override void Write(FixedPage fixedPage)
        {
            Write(fixedPage, null);
        }

        public override void Write(FixedPage fixedPage, PrintTicket printTicket)
        {
            SerializeObjectTree(fixedPage);
        }

        public override void WriteAsync(FixedPage fixedPage)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(FixedPage fixedPage, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(FixedPage fixedPage, object userState)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(FixedPage fixedPage, PrintTicket printTicket, object userState)
        {
            throw new NotSupportedException();
        }

        public override void Write(FixedDocument fixedDocument)
        {
            Write(fixedDocument, null);
        }

        public override void Write(FixedDocument fixedDocument, PrintTicket printTicket)
        {
            SerializeObjectTree(fixedDocument);
        }

        public override void WriteAsync(FixedDocument fixedDocument)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(FixedDocument fixedDocument, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(FixedDocument fixedDocument, object userState)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(FixedDocument fixedDocument, PrintTicket printTicket, object userState)
        {
            throw new NotSupportedException();
        }

        public override void Write(FixedDocumentSequence fixedDocumentSequence)
        {
            Write(fixedDocumentSequence, null);
        }

        public override void Write(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket)
        {
            SerializeObjectTree(fixedDocumentSequence);
        }

        public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence, object userState)
        {
            throw new NotSupportedException();
        }

        public override void WriteAsync(FixedDocumentSequence fixedDocumentSequence, PrintTicket printTicket,
            object userState)
        {
            throw new NotSupportedException();
        }

        public override void CancelAsync()
        {
            throw new NotSupportedException();
        }

        public override SerializerWriterCollator CreateVisualsCollator()
        {
            throw new NotSupportedException();
        }

        public override SerializerWriterCollator CreateVisualsCollator(PrintTicket documentSequencePT,
            PrintTicket documentPT)
        {
            throw new NotSupportedException();
        }

        public override event WritingPrintTicketRequiredEventHandler WritingPrintTicketRequired;
        public override event WritingProgressChangedEventHandler WritingProgressChanged;
        public override event WritingCompletedEventHandler WritingCompleted;
        public override event WritingCancelledEventHandler WritingCancelled;

        private void SerializeObjectTree(object objectTree)
        {
            TextWriter writer = new StreamWriter(_stream);
            XmlTextWriter xmlWriter = null;

            try
            {
                // Create XmlTextWriter
                xmlWriter = new XmlTextWriter(writer);

                // Set serialization mode
                var manager = new XamlDesignerSerializationManager(xmlWriter);
                // SerializeToRtf
                XamlWriter.Save(objectTree, manager);
            }
            finally
            {
                if (xmlWriter != null)
                    xmlWriter.Close();
            }
        }
    }
}