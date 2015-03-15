using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace DiplomaProject.Text {

    public class Style1 : ITextStyle {
        public bool IsOneParagraph {
            get { return false; }
            set { throw new NotImplementedException(); }
        }

        public FontFamily FontFamily {
            get { return new FontFamily("Times New Roman"); }
            set { throw new NotImplementedException(); }
        }

        public FontWeight FontWeight {
            get { return FontWeights.Thin; }
            set { throw new NotImplementedException(); }
        }

        public FontStyle FontStyle {
            get { return FontStyles.Italic; }
            set { throw new NotImplementedException(); }
        }

        public int FontSize {
            get { return 14; }
            set { throw new NotImplementedException(); }
        }
    }

}
