using System;
using System.Windows;
using System.Windows.Controls;
using DiplomaProject.Text;

namespace DiplomaProject.Controls.StylePicker
{
    public class StylePickerItem : RadioButton
    {
//        static StylePickerItem()
//        {
//            DefaultStyleKeyProperty.OverrideMetadata(typeof(StylePickerItem), new FrameworkPropertyMetadata(typeof(StylePickerItem)));
//        }

        public StylePickerItem()
        {
            DefaultStyleKey = typeof(StylePickerItem);
        }
        public static readonly DependencyProperty StyleNameProperty = DependencyProperty.Register(
            "StyleName", typeof (String), typeof (StylePickerItem));
        
        public String StyleName
        {
            get { return (String) GetValue(StyleNameProperty); }
            set { SetValue(StyleNameProperty, value); }
        }

        public static readonly DependencyProperty TextStyleProperty = DependencyProperty.Register(
            "TextStyle", typeof (ITextStyle), typeof (StylePickerItem), new PropertyMetadata(default(ITextStyle)));

        public ITextStyle TextStyle
        {
            get { return (ITextStyle) GetValue(TextStyleProperty); }
            set { SetValue(TextStyleProperty, value); }
        }
    }
}