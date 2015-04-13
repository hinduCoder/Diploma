﻿using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Serialization;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI.Interactivity;
using DiplomaProject.Controls;
using DiplomaProject.DocumentSerialization;
using DiplomaProject.Properties;
using DiplomaProject.Text;
using DiplomaProject.Text.Extenstions;
using Microsoft.Win32;

namespace DiplomaProject.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool IsDocumentChanged { get; set; }
        private string CurrentDocumentFileName { get; set; }
        private readonly FlowDocumentSerializer _flowDocumentSerializer = new FlowDocumentSerializer();
        public TextSelection CurrentSelection { get; set; }

        private ObservableCollection<ITextStyle> _textStyles =
            new ObservableCollection<ITextStyle>(new TextStyleProvider().LoadTextStyles());

        //.Select(s => new TextStyle { Name = "Style", Style = s}));

        private FlowDocument _document = new FlowDocument(new Paragraph(new Run("TESTTESTTEST")));

        public MainWindowViewModel()
        {
            StylesGroupVM = new StylesGroupViewModel();
        }

        public ObservableCollection<ITextStyle> TextStyles
        {
            get { return _textStyles; }
            set
            {
                SetProperty(ref _textStyles,
                    value,
                    () => TextStyles);
            }
        }

        public FlowDocument Document
        {
            get { return _document; }
            set
            {
                SetProperty(ref _document,
                    value,
                    () => Document);
            }
        }

        #region Commands

        public ICommand ApplyStyleCommand { get { return new DelegateCommand<ITextStyle>(ApplyStyle); } }
        public ICommand OpenFileCommand { get { return new DelegateCommand(OpenFile); } }
        public ICommand SaveFileCommand { get { return new DelegateCommand(SaveFile); } }
        public ICommand SaveAsFileCommand { get { return new DelegateCommand(SaveAsFile); } }
        public ICommand TextChangedCommand { get { return new DelegateCommand(() => IsDocumentChanged = true); } }

        public ICommand AddFormulaCommand
        {
            get { return new DelegateCommand(AddFormula); } //TODO at cursor pos
        }

        public ICommand BoldCommand
        {
            get
            {
                return new DelegateCommand(() => CurrentSelection.ApplyPropertyValue(TextElement.FontWeightProperty,
                    FontWeights.Bold));
            }
        }

        public ICommand ItalicCommand
        {
            get
            {
                return new DelegateCommand(() => CurrentSelection.ApplyPropertyValue(TextElement.FontStyleProperty,
                    FontStyles.Italic));
            }
        }

        public ICommand UnderlinedCommand
        {
            get
            {
                return new DelegateCommand(() => CurrentSelection.ApplyPropertyValue(Inline.TextDecorationsProperty,
                    TextDecorations.Underline));
            }
        }

        public ICommand LeftAlignCommand
        {
            get
            {
                return new DelegateCommand(() => CurrentSelection.ApplyPropertyValue(Block.TextAlignmentProperty,
                    TextAlignment.Left));
            }
        }

        public ICommand CenterAlignCommand
        {
            get
            {
                return new DelegateCommand(() => CurrentSelection.ApplyPropertyValue(Block.TextAlignmentProperty,
                    TextAlignment.Center));
            }
        }

        public ICommand RightAlignCommand
        {
            get
            {
                return new DelegateCommand(() => CurrentSelection.ApplyPropertyValue(Block.TextAlignmentProperty,
                    TextAlignment.Right));
            }
        }

        public ICommand OverlineDecorationCommand
        {
            get
            {
                return new DelegateCommand(() => CurrentSelection.ApplyPropertyValue(Inline.TextDecorationsProperty,
                    TextDecorations.OverLine));
            }
        }

        public ICommand UnderlineDecorationCommand
        {
            get
            {
                return new DelegateCommand(() => CurrentSelection.ApplyPropertyValue(Inline.TextDecorationsProperty,
                    TextDecorations.Underline));
            }
        }

        #endregion

        private void AddFormula()
        {
//            if (CurrentSelection.Start == CurrentSelection.End)
//                CurrentSelection.Text = "";

            Document.Blocks.InsertAfter(CurrentSelection.End.Paragraph,
                new FormulaBlock {Formula = "f(x)"});
        }

        #region Open&Save File

        private void SaveAsFile()
        {
            var saveFileDialog = new SaveFileDialog
            {
                AddExtension = true,
            };
            if (!saveFileDialog.ShowDialog() ?? false)
                return;
            _flowDocumentSerializer.Serialize(Document,
                saveFileDialog.FileName);
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

        public ViewModelBase StylesGroupVM { get; set; }

        private void ApplyStyle(ITextStyle style)
        {
            CurrentSelection.ApplyTextStyle(style);
        }

        public ICommand TextBoxSelectionChangedCommand
        {
            get { return new DelegateCommand<RichTextBox>(rtb => CurrentSelection = rtb.Selection); }
        }
    }
}