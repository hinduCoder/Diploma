using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using DevExpress.Mvvm;
using DiplomaProject.DocumentSerialization;
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

        private FormattingProvider _formattingProvider;

        private FlowDocument _document = new FlowDocument(new Paragraph(new Run("TESTTESTTEST")));

        public MainWindowViewModel()
        {
            StylesGroupVM = new StylesGroupViewModel();
            _formattingProvider = new FormattingProvider(_document);
        }

        public ObservableCollection<ITextStyle> TextStyles
        {
            get { return _textStyles; }
            set
            {
                SetProperty(ref _textStyles, value, () => TextStyles);
            }
        }

        public FlowDocument Document
        {
            get { return _document; }
            set
            {
                SetProperty(ref _document, value, () => Document);
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
            get { return new DelegateCommand(() => _formattingProvider.AddFormula(CurrentSelection)); } //TODO at cursor pos
        }

        public ICommand AddUnorderedListCommand { get { return new DelegateCommand(() => _formattingProvider.AddUnorderedList(CurrentSelection)); } }

        public ICommand AddOrderedListCommand
        {
            get { return new DelegateCommand(() => _formattingProvider.AddOrderedList(CurrentSelection)); }
        }

        public ICommand AddDrawingCommand
        { get { return new DelegateCommand(() => _formattingProvider.AddDrawing(CurrentSelection)); } }

        public ICommand AddFromPhoneCommand
        {
            get { return new DelegateCommand(AddFromPhone);}
        }

        public ICommand AddPlotCommand
        {
            get {  return new DelegateCommand(() => _formattingProvider.AddPlot(CurrentSelection));}
        }

        private async void AddFromPhone()
        {
            var waitDialogWindow = new WaitDialogWindow { ShowInTaskbar = false };
            waitDialogWindow.Show();
            var reciever = new ImageFromPhoneReciever();
            var bitmapSource = await reciever.RecieveAsync();
            _formattingProvider.AddImage(CurrentSelection, bitmapSource);
            waitDialogWindow.Close();
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