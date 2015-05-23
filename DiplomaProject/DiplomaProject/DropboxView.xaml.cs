using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DevExpress.Mvvm;
using DiplomaProject.Properties;
using DropNet;
using DropNet.Authenticators;
using DropNet.Models;
using Microsoft.Win32;

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
        private RegistryKey _registyKey;

        static DropboxView()
        {
            var registrator = new DependencyPropertyRegistator<DropboxView>();
            DownloadCommandProperty = registrator.Register<ICommand>("DownloadCommand");
            UploadCommandProperty = registrator.Register<ICommand>("UploadCommand");
        }

        public DropboxView()
        {
            InitializeComponent();
            _registyKey = Registry.CurrentUser.CreateSubKey("Software\\HinduCoder\\LectionEditor");

            _client = new DropNetClient("kg97rxrsyodaipj", "z7heg39nx4j1y7e", authenticationMethod:DropNetClient.AuthenticationMethod.OAuth2) { UseSandbox = true };
            
            if (String.IsNullOrEmpty(_registyKey.GetValue("DropboxToken") as String))
            {
                NotAutorizedView.Visibility = Visibility.Visible;
                AutorizedView.Visibility = Visibility.Collapsed;
            }
            else
            {
                AutorizedView.Visibility = Visibility.Visible;
                NotAutorizedView.Visibility = Visibility.Collapsed;
                _client.UserLogin = new UserLogin { Token = _registyKey.GetValue("DropboxToken").ToString() };
            }
            Loaded += OnLoaded;
        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e) {
            Autorize();
        }

        private void Autorize()
        {
            var redirectUri = "http://localhost:8080";
            Process.Start(_client.BuildAuthorizeUrl(OAuth2AuthorizationFlow.Code, redirectUri));
            var http = new HttpListener {Prefixes = {"http://localhost:8080/"}};
            http.Start();
            http.BeginGetContext(result =>
            {
                var context = http.EndGetContext(result);
                var response =
                    Encoding.UTF8.GetBytes(
                        "<script>window.onload=function(){open('http://dropbox.com', '_self');};</script>");
                context.Response.ContentLength64 = response.Length;
                context.Response.OutputStream.Write(response, 0, response.Length);
                context.Response.OutputStream.Flush();
                var ul = _client.GetAccessToken(context.Request.QueryString["code"], redirectUri);

                http.Stop();
                http.Close();

                _registyKey.SetValue("DropboxToken", ul.Token);
                _client.UserLogin = ul;
                Dispatcher.Invoke(delegate
                {
                    AutorizedView.Visibility = Visibility.Visible;
                    NotAutorizedView.Visibility = Visibility.Collapsed;
                    UpdateFileTree();
                });
            },null);
            

        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            if(!String.IsNullOrEmpty(_registyKey.GetValue("DropboxToken") as String))
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
                    dir.IsDirectory = true;
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
            if (file.IsDirectory)
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

    public class FileTreeNode : ViewModelBase
    {
        private string _name;
        private ObservableCollection<FileTreeNode> _children = new ObservableCollection<FileTreeNode>();
        private string _path;
        private bool _isDirectory;

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value, () => Name); }
        }

        public ObservableCollection<FileTreeNode> Children
        {
            get { return _children; }
            set { SetProperty(ref _children, value, () => Children); }
        }

        public string Path
        {
            get { return _path; }
            set { SetProperty(ref _path, value, () => Path); }
        }

        public bool IsDirectory
        {
            get { return _isDirectory; }
            set { SetProperty(ref _isDirectory, value, () => IsDirectory); }
        }
    }
}