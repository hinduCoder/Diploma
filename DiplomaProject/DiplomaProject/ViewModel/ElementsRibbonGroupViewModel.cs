using System.Windows.Documents;
using System.Windows.Input;
using DevExpress.Mvvm;

namespace DiplomaProject.ViewModel
{
    public class ElementsRibbonGroupViewModel : PartViewModel
    {
        public ICommand AddFormulaCommand
        {
            get { return new DelegateCommand(() => _formattingProvider.AddFormula(_documentState.CurrentSelection)); }
            //TODO at cursor pos
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
            var waitDialogWindow = new WaitDialogWindow {ShowInTaskbar = false};
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