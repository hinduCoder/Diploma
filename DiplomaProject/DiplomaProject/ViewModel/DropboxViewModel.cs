using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DevExpress.Mvvm;
using DiplomaProject.Annotations;
using DropNet;
using DropNet.Authenticators;
using DropNet.Models;
using Microsoft.Win32;

namespace DiplomaProject.ViewModel
{
    public class DropboxViewModel : ViewModelBase
    {
        public IEnumerable<FileTreeNode> _files;
        public bool _isNotAutorized;
        public bool _isWaiting;
        public string _objectName;
        private Dropbox _dropbox;
        private FileTreeNode _selectedNode;

        public DropboxViewModel()
        {
            _dropbox = new Dropbox();
            _isNotAutorized = !_dropbox.IsAutorized;
        }

#region Properties

        public IEnumerable<FileTreeNode> Files
        {
            get { return _files; }
            set { SetProperty(ref _files, value, () => Files); }
        }

        public bool IsNotAutorized
        {
            get { return _isNotAutorized; }
            set { SetProperty(ref _isNotAutorized, value, () => IsNotAutorized); }
        }

        public bool IsWaiting
        {
            get { return _isWaiting; }
            set { SetProperty(ref _isWaiting, value, () => IsWaiting); }
        }

        public string ObejctName
        {
            get { return _objectName; }
            set { SetProperty(ref _objectName, value, () => ObejctName); }
        }

        public FileTreeNode SelectedNode
        {
            get { return _selectedNode; }
            set { SetProperty(ref _selectedNode, value, () => SelectedNode); }
        }

        public ICommand CreateFolderCommand
        {
            get { return new DelegateCommand(() => DoAction(() => _dropbox.CreateFolder(SelectedNode, ObejctName))); }
        }

        public ICommand DownloadCommand
        {
            get { return new DelegateCommand(Download); }
        }

        public ICommand UploadCommand
        {
            get { return new DelegateCommand(() => DoAction(Upload));}
        }

        public ICommand AutorizeCommand {
            get { return new DelegateCommand(() => _dropbox.Autorize(() => _isNotAutorized = false)); }
        }

        public ICommand LoadedCommand
        {
            get { return new DelegateCommand(UpdateFiles);}
        }

        #endregion

        private void DoAction([NotNull] Action action)
        {
            IsWaiting = true;
            action();
            UpdateFiles();
        }

        private async void Download()
        {
            Messenger.Default.Send(await _dropbox.DownloadFile(SelectedNode), "down");
        }

        private void Upload()
        {
            Messenger.Default.Register<byte[]>(this, "up", data =>
            {
                _dropbox.UploadFile(SelectedNode, data, ObejctName);
                Messenger.Default.Unregister(this);
            });
            Messenger.Default.Send<Object>(null, "req");
        }

        private async void UpdateFiles()
        {
            IsWaiting = true;
            if (!IsNotAutorized)
                Files = new[] { await _dropbox.GetFiles()};
            IsWaiting = false;
        }
    }

    public class FileTreeNode : ViewModelBase {
        private string _name;
        private ObservableCollection<FileTreeNode> _children = new ObservableCollection<FileTreeNode>();
        private string _path;
        private bool _isDirectory;

        public string Name {
            get { return _name; }
            set { SetProperty(ref _name, value, () => Name); }
        }

        public ObservableCollection<FileTreeNode> Children {
            get { return _children; }
            set { SetProperty(ref _children, value, () => Children); }
        }

        public string Path {
            get { return _path; }
            set { SetProperty(ref _path, value, () => Path); }
        }

        public bool IsDirectory {
            get { return _isDirectory; }
            set { SetProperty(ref _isDirectory, value, () => IsDirectory); }
        }
    }

    public class Dropbox
    {
        private bool _isAutorized;
        private DropNetClient _client;
        private RegistryKey _registyKey;
        private readonly string RegistryKey = "DropboxToken";

        public bool IsAutorized
        {
            get { return _isAutorized; }
        }

        public Dropbox()
        {
            _registyKey = Registry.CurrentUser.CreateSubKey("Software\\HinduCoder\\LectionEditor");
            _isAutorized = !String.IsNullOrEmpty(_registyKey.GetValue(RegistryKey) as String);
            _client = new DropNetClient("kg97rxrsyodaipj", "z7heg39nx4j1y7e", authenticationMethod:DropNetClient.AuthenticationMethod.OAuth2) { UseSandbox = true };
            if (_isAutorized)
                _client.UserLogin = new UserLogin { Token = _registyKey.GetValue(RegistryKey).ToString() };
        }

        public void Autorize(Action afterAction = null)
        {
            var redirectUri = "http://localhost:8080";
            Process.Start(_client.BuildAuthorizeUrl(OAuth2AuthorizationFlow.Code, redirectUri));
            var http = new HttpListener { Prefixes = { redirectUri } };
            http.Start();
            http.BeginGetContext(result => {
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

                _registyKey.SetValue(RegistryKey, ul.Token);
                _client.UserLogin = ul;
                _isAutorized = true;
                if (afterAction != null)
                    afterAction();
            }, null);
        }

        public async Task<FileTreeNode> GetFiles()
        {
            CheckAutorization();
            var rootDir = new FileTreeNode { Name = "Dropbox", Path = "/", IsDirectory = true };
            await LoadFilesTree(rootDir, "/");
            return rootDir;
        }

        public async void CreateFolder(FileTreeNode folderIn, string name)
        {
            CheckAutorization();
            await _client.CreateFolderTask(String.Format("{0}/{1}",
                folderIn.IsDirectory ? folderIn.Path : Path.GetDirectoryName(folderIn.Path).Replace('\\', '/'), name).TrimEnd('/'), null,
                null);
        }

        public async Task<byte[]> DownloadFile(FileTreeNode file)
        {
            CheckAutorization();
            return (await _client.GetFileTask(file.Path)).RawBytes;
        }

        public async void UploadFile(FileTreeNode folderTo, byte[] data, string name = "auto_name")
        {
            CheckAutorization();
            await _client.UploadFileTask(folderTo.Path, folderTo.IsDirectory ? name : folderTo.Name, data);
        }

        private void CheckAutorization()
        {
            if (!_isAutorized)
                throw new InvalidOperationException("Not autorized");
        }

        private async Task LoadFilesTree(FileTreeNode rootFileTreeNode, String path)
        {
            var currentFolder = (await _client.GetMetaDataTask(path: path)).Contents;
            if (currentFolder != null)
                foreach (var content in currentFolder)
                {
                    var dir = new FileTreeNode {Name = content.Name, Path = content.Path};
                    rootFileTreeNode.Children.Add(dir);
                    if (content.Is_Dir)
                        dir.IsDirectory = true;
                    await LoadFilesTree(dir, Path.Combine(path, dir.Name).Replace('\\', '/'));
                }
        }

    }
} 