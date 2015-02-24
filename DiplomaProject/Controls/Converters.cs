using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Ekh {
    public class Converter : MarkupExtension, IValueConverter {
        public override sealed object ProvideValue(IServiceProvider serviceProvider) {
            return this; System.Windows.Media.Stretch
        }
        public virtual object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return Convert(value);    
        }

        protected virtual object Convert(object value) {
            return DependencyProperty.UnsetValue;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) {
            return DependencyProperty.UnsetValue;
        }
    }

    public class DivideByTwoConverter : Converter {
        protected override object Convert(object value) {
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
