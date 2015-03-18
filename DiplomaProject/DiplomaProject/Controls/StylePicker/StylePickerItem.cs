using System;
using System.Linq;
using System.Reflection;
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
            var saveStyleButton = this.GetTemplateChild("SaveStyleButton") as Button;
            saveStyleButton.Click += SaveStyleButtonOnClick;
        }

        private void SaveStyleButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            
        }
    }
}