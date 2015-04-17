using System;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DiplomaProject.Text;

namespace DiplomaProject.Controls
{
    public class StylePickerItem : RadioButton
    {
        public static readonly DependencyProperty ChangeStyleCommandProperty;
        public static readonly DependencyProperty TextStyleProperty;

        static StylePickerItem() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StylePickerItem),
              new FrameworkPropertyMetadata(typeof(StylePickerItem)));
            var registrator = new DependencyPropertyRegistator<StylePickerItem>();
            ChangeStyleCommandProperty = registrator.Register<ICommand>("ChangeStyleCommand", null);
            TextStyleProperty = registrator.Register<ITextStyle>("TextStyle", null);
        }

        public ITextStyle TextStyle
        {
            get { return (ITextStyle) GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }

        public ICommand ChangeStyleCommand {
            get { return (ICommand)GetValue(ChangeStyleCommandProperty); }
            set { SetValue(ChangeStyleCommandProperty, value); }
        }
    }
}