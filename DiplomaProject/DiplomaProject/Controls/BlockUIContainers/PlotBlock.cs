using System.Windows.Documents;

namespace DiplomaProject.Controls
{
    public class PlotBlock : BlockUIContainer
    {
        public PlotBlock()
        {
            Child = new PlotControl() { Width = 390, Height = 390 };
        }
    }
}