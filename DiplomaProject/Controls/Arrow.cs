//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Media;
//using System.Windows.Shapes;

//namespace Ekh {
//    public class _Arrow : Shape {
//        static _Arrow() {
//            StrokeProperty.OverrideMetadata(typeof(_Arrow), new FrameworkPropertyMetadata(Brushes.Black));
//            FillProperty.OverrideMetadata(typeof(_Arrow), new FrameworkPropertyMetadata(Brushes.Black));
//            WidthProperty.OverrideMetadata(typeof(_Arrow), new FrameworkPropertyMetadata(OnWidthChanged));
//            HeightProperty.OverrideMetadata(typeof(_Arrow), new FrameworkPropertyMetadata(OnHeightChanged));
//        }
//        static void OnWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
//            var arrow = d as _Arrow;
//            arrow.UpdateGeometry();
//        }
//        static void OnHeightChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) {
//            var arrow = d as _Arrow;
//            arrow.UpdateGeometry();
//        }
//        LineSegment lineSegment = new LineSegment();
//        ArcSegment arcSegment = new ArcSegment();
//        Geometry geometry = new PathGeometry { 
//                Figures = { 
//                    new PathFigure { 
//                        Segments = { 
//                            lineSegment, 
//                            arcSegment
//                        }, 
//                        IsClosed = true, 
//                        StartPoint = new Point() 
//                    } 
//                } 
//            };
//        Geometry definingGeometry;
//        protected override Geometry DefiningGeometry {
//            get { return DefiningGeometry; }
//        }
        
//        private void UpdateGeometry() {
//            lineSegment.Point = new Point(Width, Height / 2);
//            arcSegment.Point = new Point(Width, Height);
//            arcSegment.Size = new Size(0.08 * Width, Height / 2);
//            ((PathGeometry)DefiningGeometry).Figures[0].StartPoint = new Point(0, Height / 2);
//        }
//    }
//}
