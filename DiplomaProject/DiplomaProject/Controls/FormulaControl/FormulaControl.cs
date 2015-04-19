using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DiplomaProject.Controls
{
    public class FormulaControl : Control
    {
        public static readonly DependencyProperty FormulaProperty;
        public static readonly DependencyProperty FormulaVisualProperty;
        public static readonly DependencyProperty EditableNowProperty;
        private static readonly DependencyPropertyKey FormulaVisualPropertyKey;
        private static readonly DependencyPropertyKey EditableNowPropertyKey;

        static FormulaControl()
        {
            var registator = new DependencyPropertyRegistator<FormulaControl>();
            FormulaProperty = registator.Register<string>("Formula", propertyChanged:FormulaChanged, coerceValueCallback:OnFormulaCoerceCallback);
            FormulaVisualPropertyKey = registator.RegisterReadOnly<DrawingVisual>("FormulaVisual");
            EditableNowPropertyKey = registator.RegisterReadOnly<bool>("EditableNow");
            FormulaVisualProperty = FormulaVisualPropertyKey.DependencyProperty;
            EditableNowProperty = EditableNowPropertyKey.DependencyProperty;
        }

        private static object OnFormulaCoerceCallback(DependencyObject dependencyObject, object value)
        {
            return value.ToString().Replace(" ", String.Empty).Replace("\n", String.Empty);
        }

        private readonly TexFormulaParser _formulaParser;

        public FormulaControl()
        {
            TexFormulaParser.Initialize();
            _formulaParser = new TexFormulaParser();
        }

        protected override void OnMouseDoubleClick(MouseButtonEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            SetValue(EditableNowPropertyKey, true);
        }

        private static void FormulaChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var formulaControl = dependencyObject as FormulaControl;
            Debug.Assert(formulaControl != null, "formulaControl != null");

            var formula = formulaControl._formulaParser.Parse(formulaControl.Formula);
            var visual = new DrawingVisual();
            var renderer = formula.GetRenderer(TexStyle.Display, 20d);

            using (var drawingContext = visual.RenderOpen())
            {
                renderer.Render(drawingContext, 0, 1);
            }

            formulaControl.SetValue(FormulaVisualPropertyKey, visual);
            formulaControl.SetValue(EditableNowPropertyKey, false);
        }

        public string Formula
        {
            get { return (string) GetValue(FormulaProperty); }
            set { SetValue(FormulaProperty, value); }
        }

        public DrawingVisual FormulaVisual
        {
            get { return (DrawingVisual) GetValue(FormulaVisualProperty); }
        }

        public bool EditableNow
        {
            get { return (bool) GetValue(EditableNowProperty); }
        }
    }
}

