using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls.Ribbon;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DiplomaProject.Controls;

namespace DiplomaProject
{
    public partial class MainWindow : Window
    {
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
        #endregion
    }

    public class RibbonToggleButtonGroup : RibbonControlGroup
    {
        protected override void OnItemsChanged(NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems ?? Items)
            {
                var toggleButton = item as RibbonToggleButton;
                if (toggleButton == null)
                    continue;
                toggleButton.Checked += ToggleButtonOnChecked;
                toggleButton.Unchecked += ToggleButtonOnUnchecked;
            }
            foreach(var oldItem in e.OldItems ?? new object[0])
            {
                var toggleButton = oldItem as RibbonToggleButton;
                if(toggleButton == null)
                    continue;
                toggleButton.Checked -= ToggleButtonOnChecked;
                toggleButton.Unchecked -= ToggleButtonOnUnchecked;
            }
        }

        private void ToggleButtonOnUnchecked(object sender, RoutedEventArgs routedEventArgs)
        {
            
        }

        private void ToggleButtonOnChecked(object sender, RoutedEventArgs routedEventArgs)
        {
            foreach(var item in Items) {
                if(!ReferenceEquals(item, sender)) {
                    ((RibbonToggleButton)item).IsChecked = false;
                }
            }
        }
    }
}