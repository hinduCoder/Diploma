using System.Windows.Documents;
using System.Windows.Media;

namespace DiplomaProject.Text.Extenstions
{
    public static class InlineExtenstions
    {
        public static void ApplyTextStyle(this Inline inline, ITextStyle style)
        {
           inline.FontFamily = style.FontFamily;
           inline.FontSize = style.FontSize * 96d / 72d;
           inline.FontStyle = style.FontStyle;
           inline.FontWeight = style.FontWeight;
           inline.Foreground = new SolidColorBrush(style.FontColor);
        }
    }
}