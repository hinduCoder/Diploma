using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace DiplomaProject.Controls
{
    public class VisualContainerElement : FrameworkElement
    {
        public static readonly DependencyProperty VisualProperty;

        static VisualContainerElement()
        {
            var registator = new DependencyPropertyRegistator<VisualContainerElement>();
            VisualProperty = registator.Register<Visual>("Visual", propertyChanged: VisualChanged);
        }

        private static void VisualChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            var visualContainerElement = dependencyObject as VisualContainerElement;
            Debug.Assert(visualContainerElement != null, "visualContainerElement != null");

            visualContainerElement.RemoveVisualChild(e.OldValue as Visual);
            visualContainerElement.AddVisualChild(e.NewValue as Visual);

            visualContainerElement.InvalidateMeasure();
            visualContainerElement.InvalidateVisual();
        }

        public DrawingVisual Visual
        {
            get { return (DrawingVisual) GetValue(VisualProperty); }
            set { SetValue(VisualProperty, value); }
        }

        protected override int VisualChildrenCount
        {
            get { return 1; }
        }

        protected override Visual GetVisualChild(int index)
        {
            return Visual;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            if (Visual != null)
                return Visual.ContentBounds.Size;
            return base.MeasureOverride(availableSize);
        }
    }
}