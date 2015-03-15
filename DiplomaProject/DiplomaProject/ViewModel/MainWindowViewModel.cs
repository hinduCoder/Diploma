using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI.Interactivity;
using DiplomaProject.Text;
using DiplomaProject.Text.Extenstions;

namespace DiplomaProject.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<TextStyle> _textStyles = new ObservableCollection<TextStyle> { new TextStyle { Style = new Style1(), Name = "Style1" }};

        public ObservableCollection<TextStyle> TextStyles
        {
            get { return _textStyles; }
            set { SetProperty(ref _textStyles, value, () => TextStyles); }
        }

        public ICommand ApplyStyleCommand
        {
            get {  return new DelegateCommand<ITextStyle>(ApplyStyle);}
        }

        private void ApplyStyle(ITextStyle style)
        {
            CurrentSelection.ApplyTextStyle(style);
        }

        public ICommand TextBoxSelectionChangedCommand
        {
            get {  return new DelegateCommand<RichTextBox>(rtb => CurrentSelection = rtb.Selection);}
        }

        public TextSelection CurrentSelection { get; set; }

        public MainWindowViewModel()
        {

        }
    }

    public class TextStyle
    {
        public ITextStyle Style { get; set; }
        public string Name { get; set; }
    }

}