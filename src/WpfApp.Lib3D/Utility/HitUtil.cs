using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Utility
{
    public static class HitUtil
    {
        public static void FindHits(Viewport3D vp, Point point)
        {
            var res = vp.FindHits(point);
            if (res == null) return;

            var v = res.OrderBy(h => h.Distance).First().Visual;
            if (v is ModelVisual3D mv && mv.Content is GeometryModel3D gm) {
                gm.Material = RandUtil.GetRand().GetMaterial(gm.Material);
            }

        }
    }
}
