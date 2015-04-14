using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Ink;
using System.Windows.Input;
using DevExpress.Mvvm;
using Xceed.Wpf.Toolkit;

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
}