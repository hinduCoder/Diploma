using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using DiplomaProject.Text;

namespace DiplomaProject
{
    public abstract class ValueConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }

    public class FontStyleToStringConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (String) value;
            return FontParameters.FontStyles.Single(s => s.Equals(name));
        }
    }


    public class FontWeightToStringConverter : ValueConverter {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
            return value.ToString();
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
            var name = (String)value;
            return FontParameters.FontWeights.Single(s => s.Equals(name));
        }
    }
}