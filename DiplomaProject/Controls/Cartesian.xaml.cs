using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Ekh {
    /// <summary>
    /// Interaction logic for Cartesian.xaml
    /// </summary>
    public partial class Cartesian : UserControl {
        public Cartesian() {
            InitializeComponent();
        }
    }

    public class CartesianPanel : Panel { // или не надо?
        protected override Size MeasureOverride(Size availableSize) {

            return availableSize;
        }
        protected override Size ArrangeOverride(Size finalSize) {
            return base.ArrangeOverride(finalSize);
        }
    }
}
