using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm;
using DropNet;

namespace DiplomaProject
{
    /// <summary>
    /// Interaction logic for DropboxView.xaml
    /// </summary>
    public partial class DropboxView : UserControl
    {
        public static readonly DependencyProperty DownloadCommandProperty;
        public static readonly DependencyProperty UploadCommandProperty;
        private DropNetClient _client;

        static DropboxView()
        {
            var registrator = new DependencyPropertyRegistator<DropboxView>();
            DownloadCommandProperty = registrator.Register<ICommand>("DownloadCommand");
            UploadCommandProperty = registrator.Register<ICommand>("UploadCommand");
        }

        public DropboxView()
        {
            InitializeComponent();
            _client = new DropNetClient("kg97rxrsyodaipj", "z7heg39nx4j1y7e", "am2g3lymgw2wz5zz", "5pwrar6vzbmfnet")
            {
                UseSandbox = true
            }; //TODO store token and secrets where?
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            UpdateFileTree();
        }

        private void UpdateFileTree()
        {
            var rootDir = new FileTreeNode {Name = "Dropbox", Path = "/"};
            LoadFilesTree(rootDir, "/");
            TreeView.ItemsSource = new[] {rootDir};
        }

        private async void LoadFilesTree(FileTreeNode rootFileTreeNode, String path)
        {
            ProgressBar.Visibility = Visibility.Visible;
            var currentFolder = (await _client.GetMetaDataTask(path: path)).Contents;
            foreach (var content in currentFolder)
            {
                var dir = new FileTreeNode {Name = content.Name, Path = content.Path};
                rootFileTreeNode.Children.Add(dir);
                if (content.Is_Dir)
                    LoadFilesTree(dir, Path.Combine(path, dir.Name).Replace('\\', '/'));
            }
            ProgressBar.Visibility = Visibility.Collapsed;
        }

        public ICommand DownloadCommand
        {
            get { return (ICommand) GetValue(DownloadCommandProperty); }
            set { SetValue(DownloadCommandProperty, value); }
        }

        public ICommand UploadCommand
        {
            get { return (ICommand) GetValue(UploadCommandProperty); }
            set { SetValue(UploadCommandProperty, value); }
        }

        private async void OnOpenClick(object sender, RoutedEventArgs e)
        {
            var file = TreeView.SelectedItem as FileTreeNode;
            if (file.Children.Count != 0)
                return;
            var data = await _client.GetFileTask(file.Path);
            if (DownloadCommand != null)
                DownloadCommand.Execute(data.RawBytes);
        }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            if (UploadCommand != null)
                UploadCommand.Execute(new Action<byte[]>(Upload));
        }

        private async void Upload(byte[] data)
        {
            ProgressBar.Visibility = Visibility.Visible;
            var node = TreeView.SelectedItem as FileTreeNode;
            await _client.UploadFileTask(node.Path, !node.IsDirectory ? node.Name : NodeName.Text, data);
            UpdateFileTree();
        }

        private async void CreateFolder(object sender, RoutedEventArgs e)
        {
            ProgressBar.Visibility = Visibility.Visible;
            var node = TreeView.SelectedItem as FileTreeNode;
            await _client.CreateFolderTask(String.Format("{0}/{1}",
                node.IsDirectory ? node.Path : Path.GetDirectoryName(node.Path).Replace('\\', '/'), NodeName.Text), null,
                null);
            UpdateFileTree();
        }
    }

    public class FileTreeNode : ViewModelBase {
        private string _name;
        private ObservableCollection<FileTreeNode> _children = new ObservableCollection<FileTreeNode>();
        private string _path;

        public string Name { get { return _name; } set { SetProperty(ref _name, value, () => Name); } }

        public ObservableCollection<FileTreeNode> Children {
            get { return _children; }
            set { SetProperty(ref _children, value, () => Children); }
        }

        public string Path { get { return _path; } set { SetProperty(ref _path, value, () => Path); } }

        public bool IsDirectory {
            get {
                return Children.Count != 0;
            }
        }
    }
}
