using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.Mvvm;
using DiplomaProject.Text;
using DiplomaProject.Text.Extenstions;

namespace DiplomaProject.ViewModel
{
    public class StylesGroupViewModel : PartViewModel
    {
        private ObservableCollection<StylePickerItemViewModel> _textStyles;
        private readonly TextStyleProvider _textStyleProvider = new TextStyleProvider();
        private FlowDocument _flowDocument;

        public StylesGroupViewModel(FlowDocument flowDocument, DocumentState documentState)
            : base(new FormattingProvider(flowDocument), documentState)
        {
            _flowDocument = flowDocument;
            _textStyles = new ObservableCollection<StylePickerItemViewModel>(_textStyleProvider.LoadTextStyles().Select(s => new StylePickerItemViewModel(s)));
            Application.Current.MainWindow.Closing += MainWindowOnClosing;
        }
        public ObservableCollection<StylePickerItemViewModel> TextStyles
        {
            get { return _textStyles; }
            set { SetProperty(ref _textStyles, value, () => TextStyles); }
        }

        public ICommand AddStyleCommand
        {
            get { return new DelegateCommand<ITextStyle>(s => _textStyles.Add(new StylePickerItemViewModel(s))); }
        }
        public ICommand DeleteTextStyleCommand {
            get { return new DelegateCommand<StylePickerItemViewModel>(s => _textStyles.Remove(s)); }
        }
        public ICommand ApplyStyleCommand { get { return new DelegateCommand<ITextStyle>(ApplyStyle); } }

        public ICommand ChangeStyleCommand
        {
            get
            {
                return new DelegateCommand<ITextStyle>(ChangeStyle);
            }
        }

        public void Upadate()
        {
            foreach(var style in TextStyles) {
                style.IsActive = false;
            }
            var inline = _documentState.CurrentSelection.Start.Parent as Inline;
            if (inline == null)
                return;
            var styleName = FlowDocumentHelper.GetStyleName(inline);
            if (String.IsNullOrEmpty(styleName))
                return;
            var textStyle = TextStyles.SingleOrDefault(s => s.TextStyle.Name == styleName);
            if (textStyle == null)
                return;
            
            textStyle.IsActive = true;
        }

        private void ChangeStyle(ITextStyle style)
        {
            foreach (var block in _flowDocument.Blocks)
            {
                var paragraph = block as Paragraph;
                if (paragraph == null)
                    continue;
                foreach (var inline in paragraph.Inlines)
                {
                    if (FlowDocumentHelper.GetStyleName(inline) == style.Name)
                        inline.ApplyTextStyle(style);
                }
            }
        }

        private void ApplyStyle(ITextStyle style) {
            base._documentState.CurrentSelection.ApplyTextStyle(style);
        }

        private void MainWindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _textStyleProvider.DumpTextStyles(_textStyles.Select(s => s.TextStyle).ToList());
        }
    }
}