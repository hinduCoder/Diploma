using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Controls;

namespace DiplomaProject.Controls
{
    public class DrawerBlock : BlockUIContainer
    {
        public DrawerBlock()
        {
            Child = new Border { BorderBrush = Brushes.SkyBlue, BorderThickness = new Thickness(5), Child = new Drawer() };
        }

        public static readonly DependencyProperty MyPropProperty = DependencyProperty.Register(
            "MyProp", typeof (int), typeof (DrawerBlock), new PropertyMetadata(5));

        public int MyProp
        {
            get { return (int) GetValue(MyPropProperty); }
            set { SetValue(MyPropProperty, value); }
        }       
    }

}