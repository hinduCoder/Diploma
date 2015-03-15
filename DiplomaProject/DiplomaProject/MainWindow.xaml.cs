using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DiplomaProject.Controls;
using DiplomaProject.Text;
using DiplomaProject.Text.Extenstions;
using DiplomaProject.ViewModel;

namespace DiplomaProject {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }
        
        private void TextBoxBase_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
            
        }

        private void RichTextBox_OnDrop(object sender, DragEventArgs e)//TODO bitmap
        {
            var imageSource = ((String[])e.Data.GetData(DataFormats.FileDrop))[0];
            
            Point position = e.GetPosition(RichTextBox);
            var hitTestResult = VisualTreeHelper.HitTest(RichTextBox, position);
            var element = hitTestResult.VisualHit;
            while (!(element is Block) && element != null)
            {
                element = LogicalTreeHelper.GetParent(element);
            }
            Block block;
            block = element != null ? RichTextBox.Document.Blocks.First(b => ReferenceEquals(b, element)) : RichTextBox.CaretPosition.Paragraph;

                RichTextBox.Document.Blocks.InsertAfter(block,
                    new BlockImageContainer {Source = new BitmapImage(new Uri(imageSource))});
        }

        private void RichTextBox_OnPreviewDragOver(object sender, DragEventArgs e) //TODO bitmap
        {
            e.Effects = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None; 
            e.Handled = true;
        }

        private void ApplyStyle1Button_OnClick(object sender, RoutedEventArgs e)
        {
            RichTextBox.Selection.ApplyTextStyle(new Style1());
        }
    }

}
