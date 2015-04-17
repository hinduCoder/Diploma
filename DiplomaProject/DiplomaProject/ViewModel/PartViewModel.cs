using DevExpress.Mvvm;

namespace DiplomaProject.ViewModel
{
    public class PartViewModel : ViewModelBase
    {
        protected FormattingProvider _formattingProvider;
        protected DocumentState _documentState;

        public PartViewModel(FormattingProvider formattingProvider, DocumentState documentState)
        {
            _formattingProvider = formattingProvider;
            _documentState = documentState;
        }
    }
}