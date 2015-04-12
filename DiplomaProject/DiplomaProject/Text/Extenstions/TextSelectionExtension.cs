using System.Windows.Documents;

namespace DiplomaProject.Text.Extenstions
{
    public static class TextSelectionExtension
    {
        public static void ApplyTextStyle(this TextSelection selection, ITextStyle style) {
//            if (selection == null)
//                return;
//            var paragraph = selection.Start.Paragraph;
//            if(paragraph == null)
//                return;
//            while(!ReferenceEquals(paragraph, selection.End.Paragraph)) {
//                if(paragraph == null)
//                    continue;
//                paragraph = paragraph.NextBlock as Paragraph;
//            }
//            selection.End.Paragraph.ApplyTextStyle(style);
            if (selection == null || selection.Start == selection.End)
                return;
            selection.ApplyPropertyValue(TextElement.FontFamilyProperty, style.FontFamily);
            selection.ApplyPropertyValue(TextElement.FontSizeProperty, (double)style.FontSize);
            selection.ApplyPropertyValue(TextElement.FontStyleProperty, style.FontStyle);
            selection.ApplyPropertyValue(TextElement.FontWeightProperty, style.FontWeight);
        }
    }
}