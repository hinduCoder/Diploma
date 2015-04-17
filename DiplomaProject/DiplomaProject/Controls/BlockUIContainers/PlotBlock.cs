using System.Windows.Documents;
using System.Windows.Ink;

namespace DiplomaProject.Controls
{
    public class PlotBlock : BlockUIContainer
    {
        public PlotControl PlotControl { get; private set; }

        public PlotBlock()
        {
            PlotControl = new PlotControl() { MaxWidth = 400, MaxHeight = 430 };
            Child = PlotControl;
        }

        internal PlotBlock(StrokeCollection strokes)
        {
            PlotControl = new PlotControl() { MaxWidth = 400, MaxHeight = 430, Strokes = strokes };
            Child = PlotControl;
        }
    }
}