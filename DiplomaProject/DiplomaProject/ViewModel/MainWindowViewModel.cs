﻿using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using DevExpress.Mvvm;
using DiplomaProject.DocumentSerialization;
using Microsoft.Win32;

namespace DiplomaProject.ViewModel
{
    public class DocumentState
    {
        public TextSelection CurrentSelection { get; set; }
    }
    public class MainWindowViewModel : ViewModelBase
    {
        private bool IsDocumentChanged { get; set; }
        private string CurrentDocumentFileName { get; set; }
        private readonly FlowDocumentSerializer _flowDocumentSerializer = new FlowDocumentSerializer();
        private DocumentState _documentState = new DocumentState();

        private FlowDocument _document = new FlowDocument(new Paragraph(new Run("TESTTESTTEST")));

        private ElementsRibbonGroupViewModel _elementsRibbonGroupViewModel;
        private FormattingRibbonGroupViewModel _formattingRibbonGroupViewModel;
        private StylesGroupViewModel _stylesGroupViewModel;
        public MainWindowViewModel()
        {
            _elementsRibbonGroupViewModel = new ElementsRibbonGroupViewModel(_document, _documentState);
            _formattingRibbonGroupViewModel = new FormattingRibbonGroupViewModel(_document, _documentState);
            _stylesGroupViewModel = new StylesGroupViewModel(_document, _documentState);
        }

        public FlowDocument Document
        {
            get { return _document; }
            set
            {
                SetProperty(ref _document, value, () => Document);
            }
        }

        public ElementsRibbonGroupViewModel ElementsRibbonGroupViewModel
        {
            get { return _elementsRibbonGroupViewModel; }
            set { SetProperty(ref _elementsRibbonGroupViewModel, value, () => ElementsRibbonGroupViewModel); }
        }

        public FormattingRibbonGroupViewModel FormattingRibbonGroupViewModel {
            get { return _formattingRibbonGroupViewModel; }
            set { SetProperty(ref _formattingRibbonGroupViewModel, value, () => FormattingRibbonGroupViewModel); }
        }
        public StylesGroupViewModel StylesGroupViewModel {
            get { return _stylesGroupViewModel; }
            set { SetProperty(ref _stylesGroupViewModel, value, () => StylesGroupViewModel); }
        }

        #region Commands

        public ICommand OpenFileCommand { get { return new DelegateCommand(OpenFile); } }
        public ICommand SaveFileCommand { get { return new DelegateCommand(SaveFile); } }
        public ICommand SaveAsFileCommand { get { return new DelegateCommand(SaveAsFile); } }
        public ICommand TextChangedCommand { get { return new DelegateCommand(() => IsDocumentChanged = true); } }

        #endregion

        #region Open&Save Files

        private void SaveAsFile()
        {
            var saveFileDialog = new SaveFileDialog();
            if (!saveFileDialog.ShowDialog() ?? false)
                return;
            _flowDocumentSerializer.Serialize(Document,
                saveFileDialog.FileName);
        }

        public ICommand OpenFromDropboxCommand
        {
            get {
                return new DelegateCommand<byte[]>(bytes =>
                {
                    File.WriteAllBytes("temp", bytes);
                    Document = _flowDocumentSerializer.Deserialize("temp");
                    File.Delete("temp");
                }); 
            }
        }

        public ICommand SaveToDropboxCommand
        {
            get
            {
                return new DelegateCommand<Action<byte[]>>(a =>
                {
                    _flowDocumentSerializer.Serialize(Document, "temp");
                    a(File.ReadAllBytes("temp"));
                    File.Delete("temp");
                });
            }
        }
        private void SaveFile()
        {
            if (!IsDocumentChanged)
                SaveAsFile();
            else
                _flowDocumentSerializer.Serialize(Document,
                    CurrentDocumentFileName);
        }

        private void OpenFile()
        {
            var openFileDialog = new OpenFileDialog();
            if (!openFileDialog.ShowDialog() ?? false)
                return;
            Document = _flowDocumentSerializer.Deserialize(openFileDialog.FileName);
            CurrentDocumentFileName = openFileDialog.FileName;
        }

        #endregion

        public ICommand TextBoxSelectionChangedCommand
        {
            get
            {
                return new DelegateCommand<RichTextBox>(rtb =>
                {
                    _documentState.CurrentSelection = rtb.Selection;
                    StylesGroupViewModel.Upadate();
                    FormattingRibbonGroupViewModel.Update();
                });
            }
        }

    }
}