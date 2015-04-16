using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using DiplomaProject.Annotations;

namespace DiplomaProject.Controls
{
    public class PlotControl : Control
    {
        public static readonly DependencyProperty BoxSizeProperty;
        public static readonly DependencyProperty ScaleOfBoxProperty;

        static PlotControl()
        {
            var registator = new DependencyPropertyRegistator<PlotControl>();
            BoxSizeProperty = registator.Register("BoxSize", 30d);
            ScaleOfBoxProperty = registator.Register("ScaleOfBox", 1d);
        }
        public PlotControl()
        {
            Loaded += OnLoaded;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(this).Add(new PlotGridAdorner(this));
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

    }
    public class PlotGridAdorner : Adorner {
        private PlotControl _plotControl;
        public PlotGridAdorner([NotNull] UIElement adornedElement)
            : base(adornedElement) {
            _plotControl = adornedElement as PlotControl;
        }
        protected override void OnRender(DrawingContext drawingContext) {
            double step = _plotControl.BoxSize;
            Rect rect = new Rect(this.AdornedElement.RenderSize);
            Pen renderPen = new Pen(new SolidColorBrush(Colors.CornflowerBlue), 0.1);

            for(var i = rect.Left; i <= rect.Right; i += step) {
                drawingContext.DrawLine(renderPen,
                    new Point(rect.Left + i, rect.Top),
                    new Point(rect.Left + i, rect.Bottom));
            }
            for(var i = rect.Top; i <= rect.Bottom; i += step) {
                drawingContext.DrawLine(renderPen,
                    new Point(rect.Left, rect.Top + i),
                    new Point(rect.Right, rect.Top + i));
            }
            for(var i = rect.Left; i <= rect.Right; i += step) {
                DrawPointCoord(drawingContext, Math.Floor((i - rect.Left - rect.Width / 2) / step * _plotControl.ScaleOfBox), new Point(rect.Left + i, rect.Height / 2));
            }

            for(var i = rect.Top; i <= rect.Bottom; i += step) {
                DrawPointCoord(drawingContext, -Math.Floor((i - rect.Top - rect.Height / 2) / step * _plotControl.ScaleOfBox), new Point(rect.Width / 2, rect.Top + i));
            }
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