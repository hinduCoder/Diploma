using System.Windows.Documents;
using System.Windows.Media;

namespace DiplomaProject.Text.Extenstions
{
    public static class TextSelectionExtension
    {
        public static void ApplyTextStyle(this TextSelection selection, ITextStyle style) {
            if (selection == null || selection.Start == selection.End)
                return;
            selection.ApplyPropertyValue(TextElement.FontFamilyProperty, style.FontFamily);
            selection.ApplyPropertyValue(TextElement.FontSizeProperty, style.FontSize * 96d / 72d);
            selection.ApplyPropertyValue(TextElement.FontStyleProperty, style.FontStyle);
            selection.ApplyPropertyValue(TextElement.FontWeightProperty, style.FontWeight);
            selection.ApplyPropertyValue(TextElement.ForegroundProperty, new SolidColorBrush(style.FontColor));
            FlowDocumentHelper.SetStyleName(selection.Start.Parent, style.Name);
        }
    }

}