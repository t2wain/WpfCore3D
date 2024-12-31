using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Utility
{
    public static class HitUtil
    {
        public static Visual3D? FindHits(Viewport3D vp, Point point)
        {
            return vp.FindNearestVisual(point);
        }
    }
}
