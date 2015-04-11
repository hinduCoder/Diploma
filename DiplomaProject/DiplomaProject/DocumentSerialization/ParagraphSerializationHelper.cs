using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

static internal class ParagraphSerializationHelper
{
    public static readonly Dictionary<string, TypeConverter> PropertyConverters = new Dictionary<string, TypeConverter>()
    {
        {"TextIndent", new DoubleConverter()},//Maybe unless
        {"TextAlignment", new EnumConverter(typeof(TextAlignment))},
        {"LineHeight", new Int32Converter()},
        {"FontStyle", new FontStyleConverter()},
        {"FontSize", new Int32Converter()},
        {"FontWeight", new FontWeightConverter()},
        {"Foreground", new BrushConverter()},
        {"FontStretch", new FontStretchConverter()},
        {"FontFamily", new FontFamilyConverter()},
        {"Text", new StringConverter()}
    };
}