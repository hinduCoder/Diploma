using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using DevExpress.Mvvm;
using DiplomaProject.Text;
using DiplomaProject.Text.Extenstions;

namespace DiplomaProject.ViewModel
{
    public class StylesGroupViewModel : PartViewModel
    {
        private ObservableCollection<ITextStyle> _textStyles;
        private readonly TextStyleProvider _textStyleProvider = new TextStyleProvider();

        public StylesGroupViewModel(FlowDocument flowDocument, DocumentState documentState)
            : base(new FormattingProvider(flowDocument), documentState)
        {
            _textStyles = new ObservableCollection<ITextStyle>(_textStyleProvider.LoadTextStyles());
            Application.Current.MainWindow.Closing += MainWindowOnClosing;
        }
        public ObservableCollection<ITextStyle> TextStyles
        {
            get { return _textStyles; }
            set { SetProperty(ref _textStyles, value, () => TextStyles); }
        }

        public ICommand AddStyleCommand
        {
            get { return new DelegateCommand<ITextStyle>(s => _textStyles.Add(s)); }
        }
        public ICommand DeleteTextStyleCommand {
            get { return new DelegateCommand<ITextStyle>(s => _textStyles.Remove(s)); }
        }
        public ICommand ApplyStyleCommand { get { return new DelegateCommand<ITextStyle>(ApplyStyle); } }

        private void ApplyStyle(ITextStyle style) {
            _documentState.CurrentSelection.ApplyTextStyle(style);
        }

        private void MainWindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _textStyleProvider.DumpTextStyles(_textStyles);
        }
    }
}