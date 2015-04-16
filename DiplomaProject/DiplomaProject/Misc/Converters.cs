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

    public class FontWeightToBooleanConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return  (FontWeight)value == FontWeights.Bold; 
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
                return FontWeights.Bold;
            return FontWeights.Normal;
        }
    }

    public class FontStyleToBooleanConverter : ValueConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (FontStyle) value == FontStyles.Italic;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if ((bool) value)
                return FontStyles.Italic;
            return FontStyles.Normal;
        }
    }

    public class DivideByTwoConverter : ValueConverter {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double doubleValue = 0;
            try {
                doubleValue = System.Convert.ToDouble(value);
            } catch {
                return DependencyProperty.UnsetValue;
            }
            return doubleValue / 2;
        }
    }
}