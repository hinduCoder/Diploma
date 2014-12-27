using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace DiplomaProject.Controls.ResizableContainer {
    public class ResizableContainer : ContentControl
    {
        private ResizeGrip partResizeGrip;
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            partResizeGrip = GetTemplateChild("PartResizeGrip") as ResizeGrip;
            partResizeGrip.MouseEnter += partResizeGrip_MouseEnter;
            partResizeGrip.MouseLeave += partResizeGrip_MouseLeave;
        }

        void partResizeGrip_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Arrow;
        }

        void partResizeGrip_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e) {
            Mouse.OverrideCursor = Cursors.SizeNWSE;
        }
    }
}
