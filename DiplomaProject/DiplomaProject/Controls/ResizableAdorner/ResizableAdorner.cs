using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace DiplomaProject.Controls
{
    class ResizableAdorner : Adorner {
        private VisualCollection visualChildren;
        public ResizableAdorner(UIElement adornedElement)
            : base(adornedElement) {
            visualChildren = new VisualCollection(this);
            visualChildren.Add(bottomRightThumb);

            bottomRightThumb.Cursor = Cursors.SizeNWSE;
            bottomRightThumb.Width = bottomRightThumb.Height = 10;
            bottomRightThumb.Background = new SolidColorBrush(Colors.MediumBlue);
            bottomRightThumb.DragDelta += bottomRightThumb_DragDelta;
        }

        void bottomRightThumb_DragDelta(object sender, DragDeltaEventArgs e) {
            FrameworkElement element = AdornedElement as FrameworkElement;
            if(Double.IsNaN(element.Width)) element.Width = ActualWidth;
            if(Double.IsNaN(element.Height)) element.Height = ActualHeight;

            element.Width += e.HorizontalChange;
            element.Height += e.VerticalChange;
            InvalidateArrange();
        }
        Thumb bottomRightThumb = new Thumb();
        protected override Size MeasureOverride(Size constraint) {
            bottomRightThumb.Measure(constraint);
            return base.MeasureOverride(constraint);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            bottomRightThumb.Arrange(new Rect(new Point(finalSize.Width/* - bottomRightThumb.DesiredSize.Width*/, finalSize.Height /*- bottomRightThumb.DesiredSize.Height*/),
                bottomRightThumb.DesiredSize));
            return finalSize;
        }

        protected override int VisualChildrenCount { get { return visualChildren.Count; } }
        protected override Visual GetVisualChild(int index) { return visualChildren[index]; }
    }
}