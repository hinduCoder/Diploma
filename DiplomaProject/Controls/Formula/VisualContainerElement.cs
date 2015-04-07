using System.Windows;
using System.Windows.Media;

namespace Controls
{
    public class VisualContainerElement : FrameworkElement {
        public static readonly DependencyProperty VisualProperty = DependencyProperty.Register(
            "Visual", typeof (DrawingVisual), typeof (VisualContainerElement), new PropertyMetadata(default(DrawingVisual)));

        
        private DrawingVisual _visual;

        public VisualContainerElement()
        {
            _visual = null;
        }

        public DrawingVisual Visual {
            get { return _visual; }
            set { SetVisual(value); }
        }

        private void SetVisual(DrawingVisual value)
        {
            RemoveVisualChild(_visual);
            _visual = value;
            AddVisualChild(_visual);

            InvalidateMeasure();
            InvalidateVisual();
        }

        protected override int VisualChildrenCount {
            get { return 1; }
        }

        protected override Visual GetVisualChild(int index) {
            return _visual;
        }

        protected override Size MeasureOverride(Size availableSize) {
            if(_visual != null)
                return _visual.ContentBounds.Size;
            return base.MeasureOverride(availableSize);
        }

        protected override Size ArrangeOverride(Size finalSize) {
            return base.ArrangeOverride(finalSize);
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved) {
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }
    }
}