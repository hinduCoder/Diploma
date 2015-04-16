using System;
using System.Windows.Markup;
using System.Windows.Media;

namespace DiplomaProject
{
    public class RotateTransformExtension : MarkupExtension
    {
        public RotateTransformExtension(double angle)
        {
            Angle = angle;
        }

        public double Angle { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return new RotateTransform(Angle);
        }
    }
}