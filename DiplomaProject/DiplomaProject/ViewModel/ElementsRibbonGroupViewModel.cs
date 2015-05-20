using System;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using DevExpress.Mvvm;
using Microsoft.Win32;

namespace DiplomaProject.ViewModel
{
    public class ElementsRibbonGroupViewModel : PartViewModel
    {
        public ICommand AddFormulaCommand
        {
            get { return new DelegateCommand(() => _formattingProvider.AddFormula(_documentState.CurrentSelection)); }
        }

        public ICommand AddUnorderedListCommand
        {
            get { return new DelegateCommand(() => _formattingProvider.AddUnorderedList(_documentState.CurrentSelection)); }
        }

        public ICommand AddOrderedListCommand
        {
            get { return new DelegateCommand(() => _formattingProvider.AddOrderedList(_documentState.CurrentSelection)); }
        }

        public ICommand AddDrawingCommand
        {
            get { return new DelegateCommand(() => _formattingProvider.AddDrawing(_documentState.CurrentSelection)); }
        }

        public ICommand AddImageCommand
        {
            get { return new DelegateCommand(AddImage); }
        }

        private void AddImage()
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.png) | *.jpg;  *.png"
            };
            openFileDialog.ShowDialog();
            _formattingProvider.AddImage(_documentState.CurrentSelection, new BitmapImage(new Uri(openFileDialog.FileName)));
        }

        public ICommand AddFromPhoneCommand
        {
            get { return new DelegateCommand(AddFromPhone); }
        }

        public ICommand AddPlotCommand
        {
            get { return new DelegateCommand(() => _formattingProvider.AddPlot(_documentState.CurrentSelection)); }
        }

        private async void AddFromPhone()
        {
            var waitDialogWindow = new WaitDialogWindow {ShowInTaskbar = false, Owner = Application.Current.MainWindow};
            waitDialogWindow.Show();
            var reciever = new ImageFromPhoneReciever();
            var bitmapSource = await reciever.RecieveAsync();
            _formattingProvider.AddImage(_documentState.CurrentSelection, bitmapSource);
            waitDialogWindow.Close();
        }

        public ElementsRibbonGroupViewModel(FlowDocument flowDocument, DocumentState documentState)
            : base(new FormattingProvider(flowDocument), documentState)
        {
        }
    }
}