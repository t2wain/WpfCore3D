using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Utility;

namespace WpfApp.Lib3D.Binder
{
    public class PointSelectionBinder : IDisposable
    {
        private HelixViewport3D _vp;

        public PointSelectionBinder(HelixViewport3D vp)
        {
            this._vp = vp;
            this.Enable();
        }

        public virtual void Enable()
        {
            if (!this.IsEnabled)
            {
                this._vp.Viewport.MouseDown += OnMouseDown;
                this.IsEnabled = true;
            }
        }

        public virtual void Disable()
        {
            if (this.IsEnabled)
            {
                this._vp.Viewport.MouseDown -= OnMouseDown;
                this.IsEnabled = false;
            }
        }

        public bool IsEnabled { get; set; }

        protected virtual void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                var p = e.GetPosition(this._vp.Viewport);
                var v = this._vp.Viewport.FindNearestVisual(p);
                OnVisualSelected(v);
            }
        }

        protected virtual void OnVisualSelected(Visual3D? v)
        {
            if (v is ModelVisual3D mv && mv.Content is GeometryModel3D gm)
            {
                gm.Material = RandUtil.GetRand().GetMaterial(gm.Material);
            }
        }

        public void Dispose()
        {
            this.Disable();
            this._vp = null!;
        }
    }
}
