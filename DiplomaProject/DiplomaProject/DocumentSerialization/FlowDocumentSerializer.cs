using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Xml;
using DiplomaProject.Controls;
using DiplomaProject.Properties;
using Ionic.Zip;

namespace DiplomaProject.DocumentSerialization
{
    public class FlowDocumentSerializer
    {
        private List<string> imageNames = new List<string>();

        public void Serialize(FlowDocument document, string fileName)
        {
            var xml = new XmlDocument();
            var root = xml.CreateElement("Document");
            xml.AppendChild(root);

            var factory = new SerializeBlockStrategyFactory();
            imageNames.Clear();
            foreach (var block in document.Blocks)
            {
                if (block is ImageBlock)
                {
                    imageNames.Add(new Uri(((ImageBlock) block).Source.ToString()).LocalPath);
                }
                root.AppendChild(factory.GetStrategy(block).Serialize(block, xml));
            }
            Save(xml, fileName);
        }

        public FlowDocument Deserialize(string fileName) {
            var xmlDocument = Extract(fileName);
            var flowDocument = new FlowDocument();
            var factory = new DeserializeBlockStrategyFactory();
            foreach(XmlNode blockNode in xmlDocument.LastChild.ChildNodes) {
                factory.GetStrategy(blockNode.Name).Deserialize(blockNode, flowDocument);
            }
            return flowDocument;
        }

        private void Save(XmlDocument xml, string fileName)
        {
            using (var xmlWriter = XmlWriter.Create(Resources.XmlMarkupFile))
            {
                File.SetAttributes(Resources.XmlMarkupFile, FileAttributes.Hidden);
                xml.WriteTo(xmlWriter);
            }

            using (var zipFile = new ZipFile())
            {
                zipFile.AddFile(Resources.XmlMarkupFile);
                foreach (var imageName in imageNames)
                {
                    zipFile.AddFile(imageName, Resources.ImagesFolder);
                }
                using (var outputStream = File.Open(fileName, FileMode.Create))
                {
                    zipFile.Save(outputStream);
                }
            }
            File.Delete(Resources.XmlMarkupFile);
        }


        private XmlDocument Extract(string fileName)
        {
            if (Directory.Exists(Resources.TempFolder))
                Directory.Delete(Resources.TempFolder, recursive: true);
            var directory = Directory.CreateDirectory(Resources.TempFolder);
            directory.Attributes = FileAttributes.Hidden;
            using (var zipFile = ZipFile.Read(fileName))
            {
                zipFile.ExtractAll(directory.FullName);
            }
            var xmlFile = directory.EnumerateFiles().Single(f => f.Name == Resources.XmlMarkupFile).FullName;
            var xmlDocument = new XmlDocument();
            using (var xmlReader = XmlReader.Create(xmlFile))
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

 
    }
}