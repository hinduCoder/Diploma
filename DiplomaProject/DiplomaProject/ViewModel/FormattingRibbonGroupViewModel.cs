using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using DevExpress.Mvvm;

namespace DiplomaProject.ViewModel
{
    public class FormattingRibbonGroupViewModel : PartViewModel
    {
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
    }
}