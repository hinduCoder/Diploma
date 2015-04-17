using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Media;
using DiplomaProject.Annotations;
using MathNet.Numerics.Integration;

namespace DiplomaProject.Controls
{
    public class PlotControl : Control {
        public static readonly DependencyProperty StrokesProperty;
        public static readonly DependencyProperty BoxSizeProperty;
        public static readonly DependencyProperty ScaleOfBoxProperty;

        public DrawerControl DrawerControl { get; private set; }

        static PlotControl()
        {
            var registator = new DependencyPropertyRegistator<PlotControl>();
            StrokesProperty = registator.Register("Strokes", new StrokeCollection());
            BoxSizeProperty = registator.Register("BoxSize", 30d, ParametersChanged);
            ScaleOfBoxProperty = registator.Register("ScaleOfBox", 1d, ParametersChanged);
        }

        private static void ParametersChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var plotControl = dependencyObject as PlotControl;
            AdornerLayer.GetAdornerLayer(plotControl.DrawerControl).Update();
        }

        public PlotControl()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(DrawerControl).Add(new PlotGridAdorner(DrawerControl, this));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            DrawerControl = GetTemplateChild("DrawerControl") as DrawerControl;           
        }

        public double BoxSize
        {
            get { return (double) GetValue(BoxSizeProperty); }
            set { SetValue(BoxSizeProperty, value); }
        }
        public double ScaleOfBox {
            get { return (double)GetValue(ScaleOfBoxProperty); }
            set { SetValue(ScaleOfBoxProperty, value); }
        }
        public StrokeCollection Strokes {
            get { return (StrokeCollection)GetValue(StrokesProperty); }
            set { SetValue(StrokesProperty, value); }
        }
    }
    public class PlotGridAdorner : Adorner {
        private PlotControl _plotControl;
        public PlotGridAdorner([NotNull] UIElement adornedElement, PlotControl plotControl)
            : base(adornedElement)
        {
            _plotControl = plotControl;
        }
        protected override void OnRender(DrawingContext drawingContext) {
            double step = _plotControl.BoxSize;
            Rect rect = new Rect(this.AdornedElement.RenderSize);
            Pen renderPen = new Pen(new SolidColorBrush(Colors.CornflowerBlue), 0.1);

            for(double x = 0; x <= rect.Right / 2; x += step) {
                drawingContext.DrawLine(renderPen,
                    new Point(x + rect.Right / 2, rect.Top),
                    new Point(x + rect.Right / 2, rect.Bottom));
                drawingContext.DrawLine(renderPen,
                    new Point(-x + rect.Right / 2, rect.Top),
                    new Point(-x + rect.Right / 2, rect.Bottom));
                DrawPointCoord(drawingContext, GetCoord(x, rect.Width, step), new Point(rect.Right / 2 + x, rect.Height / 2));
                DrawPointCoord(drawingContext, GetCoord(-x, rect.Width, step), new Point(rect.Right / 2 - x, rect.Height / 2));

            }
            for(double y = 0; y <= rect.Bottom / 2; y += step) {
                drawingContext.DrawLine(renderPen,
                    new Point(rect.Left, rect.Bottom / 2 + y),
                    new Point(rect.Right, rect.Bottom / 2 + y));
                drawingContext.DrawLine(renderPen,
                    new Point(rect.Left, rect.Bottom / 2 - y),
                    new Point(rect.Right, rect.Bottom / 2 - y));
                DrawPointCoord(drawingContext, -GetCoord(y, rect.Height, step), new Point(rect.Width / 2, rect.Bottom / 2 + y));
                DrawPointCoord(drawingContext, -GetCoord(-y, rect.Height, step), new Point(rect.Width / 2, rect.Bottom / 2 - y));
            }
        }

        private double GetCoord(double i, double boxSize, double step)
        {
            return i / step * _plotControl.ScaleOfBox;
        }

        private static void DrawPointCoord(DrawingContext drawingContext, double coord, Point point) {
            drawingContext.DrawText(
                new FormattedText(String.Format(coord.ToString()), CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    new Typeface(new FontFamily("Times New Roman"), FontStyles.Normal, FontWeights.Normal,
                        FontStretches.Normal), 10, Brushes.Gainsboro), point);
        }
    }
    public class ArrowControl : Control
    {
        
    }
}