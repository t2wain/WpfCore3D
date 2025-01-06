using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Utility
{
    public static class HitUtil
    {
        public static Visual3D? FindHits(Viewport3D vp, Point point)
        {
            Visual3D? v = vp.FindNearestVisual(point);
            if (v != null)
                return v;

            var d = vp.RenderSize.Width * .01;
            var r = new Rect(point.X - d/2, point.Y - d/2, d, d);
            IEnumerable<Viewport3DHelper.RectangleHitResult> lstHitRes2 = vp.FindHits(r, SelectionHitMode.Touch);
            if (lstHitRes2.Count() > 0)
                v = lstHitRes2.First().Visual;
            return v;
        }

        static CombinedSelectionCommand _cmd = null!;

        public static void FindHits(Viewport3D vp, Point point, Action<IEnumerable<Visual3D>> _cb)
        {
            if (_cmd == null)
            {
                _cmd = new CombinedSelectionCommand(vp, (object? sender, VisualsSelectedEventArgs e) =>
                    {
                        var lst = new List<Visual3D>();
                        lst.AddRange(e.SelectedVisuals);
                        _cb(lst);
                    });
            }
            _cmd.Execute(point);
        }

        /// <summary>
        /// Using HelixToolkit.Wpf.Viewport3DHelper
        /// </summary>
        public static void FindHitsTest1(Viewport3D vp, Point point)
        {
            // public static IList<HelixToolkit.Wpf.Viewport3DHelper.HitResult> FindHits(
            // this Viewport3D viewport, Point position)
            IList<Viewport3DHelper.HitResult> lstHitRes = vp.FindHits(point);

            // public static IEnumerable<HelixToolkit.Wpf.Viewport3DHelper.RectangleHitResult> FindHits(
            // this Viewport3D viewport, Rect rectangle, HelixToolkit.Wpf.SelectionHitMode mode)
            var d = vp.RenderSize.Width * .01;
            var r = new Rect(point.X - d/2, point.Y - d/2, d, d);
            IEnumerable<Viewport3DHelper.RectangleHitResult> lstHitRes2 = vp.FindHits(r, SelectionHitMode.Touch);

            // public static bool FindNearest(this Viewport3D viewport, Point position,
            // out Point3D point, out Vector3D normal, out DependencyObject visual)
            bool isFound = vp.FindNearest(point, out var pos, out var normal, out var obj);

            // public static Point3D? FindNearestPoint(this Viewport3D viewport, Point position)
            Point3D? pt = vp.FindNearestPoint(point);

            // public static Visual3D FindNearestVisual(this Viewport3D viewport, position)
            Visual3D? v = vp.FindNearestVisual(point);
        }

        /// <summary>
        /// Using System.Windows.Media.VisualTreeHelper
        /// </summary>
        public static void FindHitsTest2(Viewport3D vp, Point point)
        {
            HitTestResult ht = VisualTreeHelper.HitTest(vp, point);
            if (ht != null)
            {
                DependencyObject v = ht.VisualHit;
            }

            if (ht is RayMeshGeometry3DHitTestResult ht2)
            {
                Visual3D v2 = ht2.VisualHit;
            }
        }
    }
}
