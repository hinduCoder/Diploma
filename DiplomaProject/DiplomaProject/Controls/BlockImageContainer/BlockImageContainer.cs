using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;

namespace DiplomaProject.Controls
{
    public class BlockImageContainer : BlockUIContainer
    {
        public static readonly DependencyProperty SourceProperty = DependencyProperty.Register(
            "Source", typeof (ImageSource), typeof (BlockImageContainer), new PropertyMetadata(default(ImageSource)));

        public ImageSource Source
        {
            get { return (ImageSource) GetValue(SourceProperty); }
            set { SetValue(SourceProperty, value); }
        }

        private Image partImage;
        public BlockImageContainer()
        {
            partImage = new Image { Margin = new Thickness(20), HorizontalAlignment = HorizontalAlignment.Left };
            Child = partImage;
            partImage.Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            partImage.SetBinding(Image.SourceProperty, new Binding("Source") {Source = this});
            var adornerLayer = AdornerLayer.GetAdornerLayer(partImage);
            adornerLayer.Add(new ResizableAdorner(partImage));
        }
    }
}