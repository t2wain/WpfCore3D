using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Input;
using WpfApp.Lib3D.Utility;

namespace WpfApp.Lib3D.Binder
{
    public class ViewPortBinder : IDisposable
    {
        private readonly HelixViewport3D _vp;

        public ViewPortBinder(HelixViewport3D vp)
        {
            this._vp = vp;
            this.ConfigEvent();
        }

        protected virtual void ConfigEvent()
        {
            this._vp.Viewport.MouseDown += OnMouseDown;
        }

        protected virtual void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
                OnLeftMouseDobleClick(e.GetPosition(this._vp.Viewport));
        }

        protected virtual void OnLeftMouseDobleClick(Point p)
        {
            HitUtil.FindHits(this._vp.Viewport, p);
        }

        public void Dispose()
        {
            this._vp.Viewport.MouseDown -= OnMouseDown;
        }
    }
}
