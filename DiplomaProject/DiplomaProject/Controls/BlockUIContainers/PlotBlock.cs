using System.Windows.Documents;
using System.Windows.Ink;

namespace DiplomaProject.Controls
{
    public class PlotBlock : BlockUIContainer
    {
        public PlotControl PlotControl { get; private set; }

        public PlotBlock()
        {
            PlotControl = new PlotControl() { Width = 390, Height = 390 };
            Child = PlotControl;
        }

        internal PlotBlock(StrokeCollection strokes)
        {
            PlotControl = new PlotControl() { Width = 390, Height = 390, Strokes = strokes };
            Child = PlotControl;
        }
    }
}