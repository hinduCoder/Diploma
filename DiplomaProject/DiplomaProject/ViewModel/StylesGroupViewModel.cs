using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using DevExpress.Mvvm;
using DiplomaProject.Text;

namespace DiplomaProject.ViewModel
{
    public class StylesGroupViewModel : ViewModelBase
    {
        private ObservableCollection<ITextStyle> _textStyles;
        private readonly TextStyleProvider _textStyleProvider = new TextStyleProvider();

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
        public StylesGroupViewModel()
        {
            _textStyles = new ObservableCollection<ITextStyle>(_textStyleProvider.LoadTextStyles());
            Application.Current.MainWindow.Closing += MainWindowOnClosing;
        }

        private void MainWindowOnClosing(object sender, CancelEventArgs cancelEventArgs)
        {
            _textStyleProvider.DumpTextStyles(_textStyles);
        }
    }
}