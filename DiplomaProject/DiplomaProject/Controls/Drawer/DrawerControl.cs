using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using DiplomaProject.Annotations;
using MathNet.Numerics.Interpolation;

namespace DiplomaProject.Controls
{
    public class DrawerControl : Control
    {
        public static readonly DependencyProperty StrokesProperty;
        private InkCanvas canvas;
        private Button paintButton;
        private Button clearButton;

        static DrawerControl()
        {
            var registator = new DependencyPropertyRegistator<DrawerControl>();
            StrokesProperty = registator.Register("Strokes", new StrokeCollection());
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            canvas = GetTemplateChild("Canvas") as InkCanvas;
            paintButton = GetTemplateChild("PaintBtn") as Button;
            clearButton = GetTemplateChild("ClearBtn") as Button;
            paintButton.Click += PaintButtonOnClick;
            clearButton.Click += ClearButtonOnClick;
         }

        private void ClearButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            canvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void PaintButtonOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            canvas.EditingMode = InkCanvasEditingMode.Ink;
        }

        public StrokeCollection Strokes {
            get { return (StrokeCollection)GetValue(StrokesProperty); }
            set { SetValue(StrokesProperty, value); }
        }
    }

    public class SmoothableStroke : Stroke {
        public SmoothableStroke([NotNull] StylusPointCollection stylusPoints) : base(stylusPoints) { }
        public SmoothableStroke([NotNull] StylusPointCollection stylusPoints, [NotNull] DrawingAttributes drawingAttributes) : base(stylusPoints, drawingAttributes) { }
        protected override void DrawCore(DrawingContext context, DrawingAttributes overrides) {
            var points = DrawingAttributes.FitToCurve ? GetBezierStylusPoints() : StylusPoints;
            var reducedPoints = GeometryHelper.DouglasPeuckerReduction(points.Select(sp => sp.ToPoint()).ToList(), 10d);
            var spline = CubicSpline.InterpolateNatural(reducedPoints.Select(p => p.X), reducedPoints.Select(p => p.Y));
            Point? prevPoint = null;
            for(var i = points.Min(sp => sp.X); i <= points.Max(sp => sp.X); i++) {
                var curPoint = new Point(i, spline.Interpolate(i));
                if(prevPoint != null)
                    context.DrawLine(new Pen(Brushes.Black, 2), prevPoint.Value, curPoint);
                prevPoint = curPoint;
            }
        }
    }

    public class InkCanvasEx : InkCanvas {
        protected override void OnStrokeCollected(InkCanvasStrokeCollectedEventArgs e) {
            Strokes.Remove(e.Stroke);
            var customStroke = new SmoothableStroke(e.Stroke.GetBezierStylusPoints());
            Strokes.Add(customStroke);

            InkCanvasStrokeCollectedEventArgs args =
                new InkCanvasStrokeCollectedEventArgs(customStroke);
            base.OnStrokeCollected(args);
        }
    }
}