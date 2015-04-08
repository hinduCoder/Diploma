using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Controls {
    /// <summary>
    /// Interaction logic for Formula.xaml
    /// </summary>
    public partial class Formula : UserControl {
        private TexFormulaParser _formulaParser;

        public Formula() {
            InitializeComponent();
            Loaded += Window_Loaded;
        }
        public void SetExpression(string expression) {
            inputTextBox.Text = expression;

        }

        private void renderButton_Click(object sender, RoutedEventArgs e) {
            var formula = _formulaParser.Parse(inputTextBox.Text);
            var visual = new DrawingVisual();
            var renderer = formula.GetRenderer(TexStyle.Display, 20d);

            using(var drawingContext = visual.RenderOpen()) {
                renderer.Render(drawingContext, 0, 1);
            }

            formulaContainerElement.Visual = visual;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            TexFormulaParser.Initialize();
            _formulaParser = new TexFormulaParser();

            var latex1 = @"\int_0^{\infty}{x^{2n} e^{-a x^2} dx} = \frac{2n-1}{2a} \int_0^{\infty}{x^{2(n-1)} e^{-a x^2} dx} = \frac{(2n-1)!!}{2^{n+1}} \sqrt{\frac{\pi}{a^{2n+1}}}";
            var latex2 = @"\int_a^b{f(x) dx} = (b - a) \sum_{n = 1}^{\infty}  {\sum_{m = 1}^{2^n  - 1} { ( { - 1} )^{m + 1} } } 2^{ - n} f(a + m ( {b - a}  )2^{-n} )";
            var latex3 = @"L = \int_a^b \sqrt[4]{ |\sum_{i,j=1}^ng_{ij}(\gamma(t)) (\frac{d}{dt}x^i\circ\gamma(t) ) (\frac{d}{dt}x^j\circ\gamma(t) ) |}dt";
            inputTextBox.Text = latex3;
        }
    }
}
