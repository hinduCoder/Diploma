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

namespace DiplomaProject {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            //ImageTest.AddHandler(MouseEnterEvent, new MouseEventHandler(ImageTestOnMouseEnter));
            //ImageTest.AddHandler(MouseLeaveEvent, new MouseEventHandler(ImageTestOnMouseLeave));
            //ImageTest.MouseEnter += ImageTestOnMouseEnter; 
            //ImageTest.MouseLeave += ImageTestOnMouseLeave; 
           // Loaded += MainWindow_Loaded;
                
        }

        //void MainWindow_Loaded(object sender, RoutedEventArgs e) {
        //    var adornerLayer = AdornerLayer.GetAdornerLayer(ImageTest);
        //    adornerLayer.Add(new ResizableAdorner(ImageTest));
        //}
        private void TextBoxBase_OnSelectionChanged(object sender, RoutedEventArgs e)
        {
        }
    }

    
}
