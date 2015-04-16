using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using DropNet;

namespace DiplomaProject {
    /// <summary>
    /// Interaction logic for DropboxView.xaml
    /// </summary>
    public partial class DropboxView : UserControl
    {
        public static readonly DependencyProperty DownloadCommandProperty;
        private DropNetClient _client;

        static DropboxView()
        {
            var registrator = new DependencyPropertyRegistator<DropboxView>();
            DownloadCommandProperty = registrator.Register<ICommand>("DownloadCommand");
        }
        public DropboxView() {
            InitializeComponent();
            _client = new DropNetClient("kg97rxrsyodaipj", "z7heg39nx4j1y7e", "am2g3lymgw2wz5zz", "5pwrar6vzbmfnet") { UseSandbox = true };
            Loaded += OnLoaded;
            ListBox.SelectionChanged += ListBoxOnSelectionChanged;
        }

        private async void ListBoxOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs) {
            var fileName = ListBox.SelectedItem.ToString();
            var file = await _client.GetFileTask(fileName);
            if (DownloadCommand != null)
                DownloadCommand.Execute(file.RawBytes);
        }

        private async void OnLoaded(object sender, RoutedEventArgs routedEventArgs) {
            ListBox.ItemsSource = (await _client.GetMetaDataTask(path: "/")).Contents.Select(c => c.Name);
        }

        public ICommand DownloadCommand
        {
            get { return (ICommand) GetValue(DownloadCommandProperty); }
            set { SetValue(DownloadCommandProperty, value);}
        }
    }
}
