using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace DiplomaProject.Controls
{
    public class FormulaBlock : BlockUIContainer
    {
        private readonly FormulaControl _formulaControl;

        public static readonly DependencyProperty FormulaProperty = DependencyProperty.Register(
            "Formula", typeof (string), typeof (FormulaBlock), new PropertyMetadata(default(string)));

        public string Formula
        {
            get { return (string) GetValue(FormulaProperty); }
            set { SetValue(FormulaProperty, value); }
        }
        public FormulaBlock()
        {
            _formulaControl = new FormulaControl();
            Child = _formulaControl;
            _formulaControl.Loaded += FormulaControlOnLoaded;
        }

        private void FormulaControlOnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _formulaControl.SetBinding(FormulaControl.FormulaProperty, new Binding("Formula") { Source = this });
        }
    }
}