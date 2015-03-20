using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DiplomaProject.Text;

namespace DiplomaProject.Controls.StylePicker
{
    public class StylePicker : ItemsControl
    {
        public static readonly DependencyProperty NewTextStyleProperty;
        public static readonly DependencyProperty AddNewStyleCommandProperty;
        static StylePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof (StylePicker),
                new FrameworkPropertyMetadata(typeof (StylePicker)));
            NewTextStyleProperty = DependencyProperty.Register("NewTextStyle", typeof (ITextStyle),
                typeof (StylePicker), new PropertyMetadata(new TextStyleImpl()));
            AddNewStyleCommandProperty = DependencyProperty.Register("AddNewStyleCommand", typeof(ICommand), typeof(StylePicker));
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