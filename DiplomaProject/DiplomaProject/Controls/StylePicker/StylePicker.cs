using System.Windows;
using System.Windows.Controls;

namespace DiplomaProject.Controls.StylePicker
{
    public class StylePicker : ItemsControl
    {
        static StylePicker()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StylePicker), new FrameworkPropertyMetadata(typeof(StylePicker)));
        }
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is StylePickerItem;
        }
    }
}