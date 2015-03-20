using System;
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
using DiplomaProject.Properties;
using DiplomaProject.Text;
using DiplomaProject.Text.Extenstions;

namespace DiplomaProject.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ObservableCollection<ITextStyle> _textStyles =
            new ObservableCollection<ITextStyle>(new TextStyleProvider().LoadTextStyles());//.Select(s => new TextStyle { Name = "Style", Style = s}));

        public ObservableCollection<ITextStyle> TextStyles
        {
            get { return _textStyles; }
            set { SetProperty(ref _textStyles, value, () => TextStyles); }
        }
        public ICommand ApplyStyleCommand
        {
            get {  return new DelegateCommand<ITextStyle>(ApplyStyle);}
        }
        public ViewModelBase StylesGroupVM { get; set; }

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
            StylesGroupVM = new StylesGroupViewModel();
        }
    }
}