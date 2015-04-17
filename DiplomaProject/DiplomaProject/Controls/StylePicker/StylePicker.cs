using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DiplomaProject.Text;

namespace DiplomaProject.Controls
{
    public class StylePicker : ItemsControl
    {
        public static readonly DependencyProperty NewTextStyleProperty;
        public static readonly DependencyProperty AddNewStyleCommandProperty;
        static StylePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (StylePicker),
                new FrameworkPropertyMetadata(typeof (StylePicker)));
            var registrator = new DependencyPropertyRegistator<StylePicker>();
            NewTextStyleProperty = registrator.Register("NewTextStyle", new TextStyleImpl());
            AddNewStyleCommandProperty = registrator.Register<ICommand>("AddNewStyleCommand");
        }
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is StylePickerItem;
        }

        public ITextStyle NewTextStyle
        {
            get { return (ITextStyle) GetValue(NewTextStyleProperty); }
            set { SetValue(NewTextStyleProperty, value); }
        }
        public ICommand AddNewStyleCommand
        {
            get { return (ICommand)GetValue(AddNewStyleCommandProperty); }
            set { SetValue(AddNewStyleCommandProperty, value); }
        }
    }
}