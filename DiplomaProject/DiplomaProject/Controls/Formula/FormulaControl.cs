using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DiplomaProject.Controls
{
    public class FormulaControl : Control
    {
        public static readonly DependencyProperty FormulaProperty;
        public static readonly DependencyProperty FormulaVisualProperty;
        private static readonly DependencyPropertyKey FormulaVisualPropertyKey;

        static FormulaControl()
        {
            var registator = new DependencyPropertyRegistator<FormulaControl>();
            FormulaProperty = registator.Register<string>("Formula", propertyChanged:FormulaChanged);
            FormulaVisualPropertyKey = DependencyProperty.RegisterReadOnly("FormulaVisual", typeof (DrawingVisual),
                typeof (FormulaControl), new PropertyMetadata());
            FormulaVisualProperty = FormulaVisualPropertyKey.DependencyProperty;
        }

        private readonly TexFormulaParser _formulaParser;

        public FormulaControl()
        {
            TexFormulaParser.Initialize();
            _formulaParser = new TexFormulaParser();

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
    }
}

