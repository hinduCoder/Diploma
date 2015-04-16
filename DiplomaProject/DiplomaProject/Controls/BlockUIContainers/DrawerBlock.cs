using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace DiplomaProject.Controls
{
    public class DrawerBlock : BlockUIContainer
    {
        private DrawerControl _drawer = new DrawerControl();

        public DrawerControl Drawer { get { return _drawer; } }

        public DrawerBlock()
        {
            Child = new Border { BorderBrush = Brushes.SkyBlue, BorderThickness = new Thickness(5), Child = _drawer };
        }      
    }

}