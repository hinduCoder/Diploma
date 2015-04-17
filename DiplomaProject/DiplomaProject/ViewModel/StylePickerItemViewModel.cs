using DevExpress.Mvvm;
using DiplomaProject.Text;

namespace DiplomaProject.ViewModel
{
    public class StylePickerItemViewModel : ViewModelBase
    {
        private ITextStyle _textStyle;
        private bool _isActive;

        public ITextStyle TextStyle
        {
            get { return _textStyle; }
            set { SetProperty(ref _textStyle, value, () => TextStyle); }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set { SetProperty(ref _isActive, value, () => IsActive); }
        }

        public StylePickerItemViewModel(ITextStyle textStyle)
        {
            TextStyle = textStyle;
        }
    }
}