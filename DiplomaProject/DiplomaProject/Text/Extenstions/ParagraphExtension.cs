using System.Windows.Documents;

namespace DiplomaProject.Text.Extenstions
{
    public static class ParagraphExtension
    {
        public static void ApplyTextStyle(this Paragraph paragraph, ITextStyle style) {
            paragraph.FontFamily = style.FontFamily;
            paragraph.FontSize = style.FontSize;
            paragraph.FontWeight = style.FontWeight;
            paragraph.FontStyle = style.FontStyle;
        } 
    }
}