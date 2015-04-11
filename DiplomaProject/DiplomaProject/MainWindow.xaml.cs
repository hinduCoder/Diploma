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
        private readonly FlowDocumentSerializer _flowDocumentSerializer = new FlowDocumentSerializer();

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Event Handlers
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
            var block = element != null
                ? RichTextBox.Document.Blocks.First(b => ReferenceEquals(b, element))
                : RichTextBox.CaretPosition.Paragraph;

            RichTextBox.Document.Blocks.InsertAfter(block,
                new ImageBlock {Source = new BitmapImage(new Uri(imageSource))});
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
            RichTextBox.Document = _flowDocumentSerializer.Deserialize(openFileDialog.FileName);
            CurrentDocumentFileName = openFileDialog.FileName;
        }

        private void SaveMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (!IsDocumentChanged)
                SaveAs();
            else
                _flowDocumentSerializer.Serialize(RichTextBox.Document, CurrentDocumentFileName);
        }

        private void SaveAsMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            SaveAs();
        }
        private void RichTextBox_OnTextChanged(object sender, TextChangedEventArgs e) {
            IsDocumentChanged = true;
        }
        #endregion
        private void SaveAs()
        {
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
            };
            if (!saveFileDialog.ShowDialog() ?? false)
                return;
            _flowDocumentSerializer.Serialize(RichTextBox.Document, saveFileDialog.FileName);
        }

      
    }
}