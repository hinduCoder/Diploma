using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Documents.Serialization;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using DiplomaProject.Controls;
using DiplomaProject.DocumentSerialization;
using Microsoft.Win32;

namespace DiplomaProject
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool IsDocumentChanged { get; set; }
        private string CurrentDocumentFileName { get; set; }

        public MainWindow()
        {
            //SerializerProvider.RegisterSerializer(SerializerDescriptor.CreateFromFactoryInstance(new XamlSerializerFactory()), false);
            InitializeComponent();
            FlowDocumentSerializer.Serialize(RichTextBox.Document);
        }

        
        private void RichTextBox_OnDrop(object sender, DragEventArgs e) //TODO bitmap
        {
            var imageSource = ((String[]) e.Data.GetData(DataFormats.FileDrop))[0];

            var position = e.GetPosition(RichTextBox);
            var hitTestResult = VisualTreeHelper.HitTest(RichTextBox, position);
            var element = hitTestResult.VisualHit;
            while (!(element is Block) && element != null)
            {
                element = LogicalTreeHelper.GetParent(element);
            }
            Block block;
            block = element != null
                ? RichTextBox.Document.Blocks.First(b => ReferenceEquals(b, element))
                : RichTextBox.CaretPosition.Paragraph;

            RichTextBox.Document.Blocks.InsertAfter(block,
                new BlockImageContainer {Source = new BitmapImage(new Uri(imageSource))});
        }

        private void RichTextBox_OnPreviewDragOver(object sender, DragEventArgs e) //TODO bitmap
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
            e.Handled = true;
        }

        private void OpenMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog {Filter = "Xaml documents|*.xaml"};
            if (!openFileDialog.ShowDialog() ?? false)
                return;
            using (var fileStream = openFileDialog.OpenFile())
            {
                using (var reader = XmlReader.Create(new StreamReader(fileStream)))
                {
                    var xmlReader = reader;
                    var documentRoot = XamlReader.Load(xmlReader) as Section;
                    if (documentRoot == null)
                    {
                        MessageBox.Show("This document cannot be opened", "Problems", MessageBoxButton.OK,
                            MessageBoxImage.Error);
                        return;
                    }
                    RichTextBox.Document = new FlowDocument(documentRoot);
                }
            }
            CurrentDocumentFileName = openFileDialog.FileName;
        }

        private void SaveMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!IsDocumentChanged)
                SaveAs();
            else
                FlowDocumentSerializer.SaveDocumentToXaml(RichTextBox.Document, CurrentDocumentFileName);
        }

        private void SaveAsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }

        private void SaveAs()
        {
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
                Filter = "Xaml document|*.xaml|RTF document|*.rtf"
            };
            if (!saveFileDialog.ShowDialog() ?? false)
                return;
            switch (saveFileDialog.FilterIndex)
            {
                case 1:
                    FlowDocumentSerializer.SaveDocumentToXaml(RichTextBox.Document, saveFileDialog.FileName);
//                    new FlowDocumentSerializer().SerializeToXaml(RichTextBox.Document, saveFileDialog.FileName);
                    IsDocumentChanged = false;
                    break;
                case 2:
                    FlowDocumentSerializer.SaveDocumentToRtf(RichTextBox.Document, saveFileDialog.FileName);
                    break;
            }
        }

        private void RichTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            IsDocumentChanged = true;
        }
    }
}