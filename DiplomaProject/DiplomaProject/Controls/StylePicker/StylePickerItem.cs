using System;
using System.Windows;
using System.Windows.Controls;
using DiplomaProject.Text;

namespace DiplomaProject.Controls.StylePicker
{
    public class StylePickerItem : RadioButton
    {


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

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            var huy = GetTemplateChild("Settings") as UIElement;
            huy.MouseLeftButtonDown += huy_MouseLeftButtonDown;
            huy.MouseLeftButtonUp += huy_MouseLeftButtonUp;
        }

        void huy_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e) {

        }

        void huy_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e) {
           
        }
    }
}