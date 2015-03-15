using System.Windows;
using System.Windows.Media;

namespace DiplomaProject.Text
{
    public interface ITextStyle {
        bool IsOneParagraph { get; set; }
        FontFamily FontFamily { get; set; }
        FontWeight FontWeight { get; set; }
        FontStyle FontStyle { get; set; }
        int FontSize { get; set; }
    }
}