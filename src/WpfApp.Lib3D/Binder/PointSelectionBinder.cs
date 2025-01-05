using HelixToolkit.Wpf;
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
            this.ConfigHandler();
        }

        #region Eventhandler

        protected virtual void ConfigHandler()
        {
            this._vp.MouseDown += OnMouseDown;
        }

        protected virtual void CleanupHandler()
        {
            this._vp.MouseDown -= OnMouseDown;
        }

        protected virtual void OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (this.IsPointSelectionEnabled 
                && e.LeftButton == MouseButtonState.Pressed)
            {
                var p = e.GetPosition(this._vp);
                var v = HitUtil.FindHits(this._vp.Viewport, p);
                //var v = this._vp.Viewport.FindNearestVisual(p);
                //HitUtil.FindHits(this._vp.Viewport, p);
                //HitUtil.FindHits2(this._vp.Viewport, p);
                OnVisualSelected(v);
            }
        }

        #endregion

        #region Selection

        public bool IsPointSelectionEnabled { get; set; }

        public bool IsRectangleSelectionEnabled { get; set; }

        protected virtual void OnVisualSelected(Visual3D? v)
        {
            if (v is ModelVisual3D mv && mv.Content is GeometryModel3D gm)
            {
                gm.Material = RandUtil.GetRand().GetMaterial(gm.Material);
            }
        }

        #endregion

        public void Dispose()
        {
            this.CleanupHandler();
            this._vp = null!;
        }
    }
}
