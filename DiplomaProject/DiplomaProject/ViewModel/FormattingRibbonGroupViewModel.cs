using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using DevExpress.Mvvm;
using DiplomaProject.Text.Extenstions;

namespace DiplomaProject.ViewModel
{
    public class FormattingRibbonGroupViewModel : PartViewModel
    {
        public bool _isBold;
        public bool _isItalic;
        public bool _isUnderlined;
        public bool _isLeftAlign;
        public bool _isCenterAlign;
        public bool _isRightAlign;
        public FormattingRibbonGroupViewModel(FlowDocument flowDocument, DocumentState documentState)
            : base(new FormattingProvider(flowDocument), documentState)
        {
        }

        public ICommand BoldCommand {
            get {
                return new DelegateCommand(() => _documentState.CurrentSelection.ApplyPropertyValue(TextElement.FontWeightProperty,
                    FontWeights.Bold));
            }
        }

        public ICommand ItalicCommand {
            get {
                return new DelegateCommand(() => _documentState.CurrentSelection.ApplyPropertyValue(TextElement.FontStyleProperty,
                    FontStyles.Italic));
            }
        }

        public ICommand UnderlinedCommand {
            get {
                return new DelegateCommand(() => _documentState.CurrentSelection.ApplyPropertyValue(Inline.TextDecorationsProperty,
                    TextDecorations.Underline));
            }
        }

        public ICommand LeftAlignCommand {
            get {
                return new DelegateCommand(() => _documentState.CurrentSelection.ApplyPropertyValue(Block.TextAlignmentProperty,
                    TextAlignment.Left));
            }
        }

        public ICommand CenterAlignCommand {
            get {
                return new DelegateCommand(() => _documentState.CurrentSelection.ApplyPropertyValue(Block.TextAlignmentProperty,
                    TextAlignment.Center));
            }
        }

        public ICommand RightAlignCommand {
            get {
                return new DelegateCommand(() => _documentState.CurrentSelection.ApplyPropertyValue(Block.TextAlignmentProperty,
                    TextAlignment.Right));
            }
        }

        public bool IsBold
        {
            get { return _isBold; }
            set { SetProperty(ref _isBold, value, () => IsBold); }
        }

        public bool IsItalic {
            get { return _isItalic; }
            set { SetProperty(ref _isItalic, value, () => IsItalic); }
        }
        public bool IsUnderlined {
            get { return _isUnderlined; }
            set { SetProperty(ref _isUnderlined, value, () => IsUnderlined); }
        }
        public bool IsLeftAlign {
            get { return _isLeftAlign; }
            set { SetProperty(ref _isLeftAlign, value, () => IsLeftAlign); }
        }
        public bool IsRightAlign {
            get { return _isRightAlign; }
            set { SetProperty(ref _isRightAlign, value, () => IsRightAlign); }
        }
        public bool IsCenterAlign {
            get { return _isCenterAlign; }
            set { SetProperty(ref _isCenterAlign, value, () => IsCenterAlign); }
        }
        public void Update()
        {
            var paragraph = _documentState.CurrentSelection.Start.Paragraph;
            if (paragraph == null)
                return;
            var inline = _documentState.CurrentSelection.Start.Parent as Inline;
            if(inline == null)
                return;
            IsBold = inline.FontWeight == FontWeights.Bold;
            IsItalic = inline.FontStyle == FontStyles.Italic;
            IsUnderlined = inline.TextDecorations.Contains(TextDecorations.Underline[0]);
            IsLeftAlign = paragraph.TextAlignment == TextAlignment.Left;
            IsRightAlign = paragraph.TextAlignment == TextAlignment.Right;
            IsCenterAlign = paragraph.TextAlignment == TextAlignment.Center;
        }
    }
}