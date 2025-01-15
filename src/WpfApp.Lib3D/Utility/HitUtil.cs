using HelixToolkit.Wpf;
using RacewayDataLib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Visual;

namespace WpfApp.Lib3D.Utility
{
    public static class HitUtil
    {
        #region Visual selections

        /// <summary>
        /// Point selection
        /// </summary>
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

        /// <summary>
        /// Rectangle selection with result returned using callback
        /// </summary>
        /// <param name="_cb">Result returned from call back method</param>
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

        #endregion

        public static void FindLinePointHits(Viewport3D vp, Point point)
        {
            IList<Viewport3DHelper.HitResult> lstHitRes = vp.FindHits(point);
            var lstHits = lstHitRes
                .Where(res => res.Visual is ScreenSpaceVisual3D)
                .OrderBy(r => r.Distance)
                .ToList();

            var lstIdx = new List<int>();
            Visual3D? v = null;
            Visual3D? vpar = null;
            foreach(var res in lstHits.Take(1))
            {
                if (res.Visual is PointsVisual3D pv)
                {
                    v = pv;
                    lstIdx = FindPointPos(res, pv.Points, 0);
                    if (VisualTreeHelper.GetParent(pv) is NodeVisual3D nv) 
                    { 
                        vpar = nv; 
                    }
                }
                else if (res.Visual is LinesVisual3D lv)
                {
                    v = lv;
                    lstIdx = FindPointPos(res, lv.Points, 1);
                    if (VisualTreeHelper.GetParent(lv) is RacewayVisual3D rv && lstIdx.Count > 0) 
                    { 
                        vpar = rv;
                        rv.AddSelection(lv, lstIdx);
                    }
                }
            }
        }

        #region Test

        /// <summary>
        /// Using HelixToolkit.Wpf.Viewport3DHelper
        /// </summary>
        public static void FindHitsTest1(Viewport3D vp, Point point)
        {
            // public static IList<HelixToolkit.Wpf.Viewport3DHelper.HitResult> FindHits(
            // this Viewport3D viewport, Point position)
            IList<Viewport3DHelper.HitResult> lstHitRes = vp.FindHits(point);
            ViewHitResultTest(lstHitRes);

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

        #endregion

        #region Mesh HitTest

        /// <summary>
        /// Assumption - every line has 2 pointd which generate triangles and assocation vertexes.
        /// The hit result will return the vertexes of the hit trangle.
        /// At least 2 vertex should equal the 2 line points.
        /// The indexes of the 2 line point are related to the data for the line.
        /// 
        /// NOTE, THIS METHOD IS NOT FOOL-PROOF
        /// 
        /// </summary>
        static List<int> FindPointPos(Viewport3DHelper.HitResult res, Point3DCollection visualPoints, int mode)
        {
            var r = res.RayHit;
            var phit = res.Position;
            var pxcol = r.MeshHit.Positions;
            var vxp1 = pxcol[r.VertexIndex1];
            var vxp2 = pxcol[r.VertexIndex2];
            var vxp3 = pxcol[r.VertexIndex3];

            var _DIST_ = 0.5;

            var qVisualPoint = visualPoints
                .Select((vsp, i) => new
                {
                    VisualPoint = vsp, // visual point
                    VisPointIndex = i, // index of the visual point
                    // distance between vertex point and visual point
                    Dist1 = (vsp - vxp1).Length,
                    Dist2 = (vsp - vxp2).Length,
                    Dist3 = (vsp - vxp3).Length
                });

            // determine which 3 vertexes best match to visual point
            var qVisPoint2 = qVisualPoint.Select(i =>
                {
                    var vx = r.VertexIndex1;
                    var d = i.Dist1;
                    if (i.Dist2 < d)
                    {
                        vx = r.VertexIndex2;
                        d = i.Dist2;
                    }
                    if (i.Dist3 < d)
                    {
                        vx = r.VertexIndex3;
                        d = i.Dist3;
                    }
                    return new
                    {
                        i.VisualPoint,
                        i.VisPointIndex,
                        VertexIndex = vx,
                        Dist = d
                    };

                })
                // restrict visual point near the vertex point
                .Where(i => i.Dist < _DIST_)
                .ToList();

            var lstIdx = new List<int>();
            // iterate through each visual points
            foreach (var n in qVisPoint2)
            {
                // Find point
                if (mode == 0)
                {
                    lstIdx = new() { n.VisPointIndex };
                    break;
                }

                // Get visual point index before current if exist
                var nb = qVisPoint2.Where(i => i.VertexIndex != n.VertexIndex
                        && i.Dist < 0.5
                        && i.VisPointIndex == n.VisPointIndex - 1
                    ).FirstOrDefault();
                if (nb != null && CheckPoints(pxcol[n.VertexIndex], pxcol[nb.VertexIndex], phit))
                {
                    // found consecutive index of the line selection
                    lstIdx = new() { n.VisPointIndex, nb.VisPointIndex };
                    break;
                }

                // Get visual point index after current if exist
                var na = qVisPoint2.Where(i => i.VertexIndex != n.VertexIndex
                        && i.Dist < 0.5
                        && i.VisPointIndex == n.VisPointIndex + 1
                    ).FirstOrDefault();
                if (na != null && CheckPoints(pxcol[n.VertexIndex], pxcol[na.VertexIndex], phit))
                {
                    // found consecutive index of the line selection
                    lstIdx = new() { n.VisPointIndex, na.VisPointIndex };
                    break;
                }
            }

            return lstIdx;
        }

        /// <summary>
        /// Check that phit is between pvx1 and pvx2
        /// </summary>
        static bool CheckPoints(Point3D pvx1, Point3D pvx2, Point3D phit)
        {
            var l1 = (pvx1 - pvx2).Length;
            var l2 = (pvx1 - phit).Length;
            var l3 = (pvx2 - phit).Length;
            return l2 < l1 && l3 < l1;
        }


        /// <summary>
        /// Inspect hit result from Point Selection
        /// </summary>
        /// <param name="lstRes"></param>
        static void ViewHitResultTest(IList<Viewport3DHelper.HitResult> lstRes)
        {
            foreach (var res in lstRes)
            {
                double d = res.Distance;
                MeshGeometry3D me = res.Mesh;
                Model3D mo = res.Model;
                Vector3D n = res.Normal;
                Point3D p = res.Position;
                RayMeshGeometry3DHitTestResult r = res.RayHit;
                Visual3D v = res.Visual;

                ViewRayMeshHitTest(r);
            }
        }

        /// <summary>
        /// Inspect Mesh hit result
        /// </summary>
        /// <param name="ray"></param>
        static List<Point3D> ViewRayMeshHitTest(RayMeshGeometry3DHitTestResult ray)
        {
            //double d = ray.DistanceToRayOrigin;
            MeshGeometry3D me = ray.MeshHit;
            //Point3D ph = ray.PointHit;
            int vx1 = ray.VertexIndex1;
            int vx2 = ray.VertexIndex2;
            int vx3 = ray.VertexIndex3;
            //double vw1 = ray.VertexWeight1;
            //double vw2 = ray.VertexWeight2;
            //double vw3 = ray.VertexWeight3;

            // Triangle indices
            //Int32Collection indices = me.TriangleIndices;
            //var icnt = indices.Count;

            // Point3D collection
            var pcol = me.Positions;
            //var pcnt = pcol.Count;

            var p1 = pcol[vx1];
            var p2 = pcol[vx2];
            var p3 = pcol[vx3];

            var r1 = new Raceway 
            { 
                ID = 0,
                FromNode = new Node() { X = p1.X, Y = p1.Y, Z = p1.Z }, 
                ToNode = new Node { X = p2.X, Y = p2.Y, Z =  p2.Z }, 
                Length = (p2 - p1).Length 
            };
            var v2 = p2 - p3;
            var v3 = p3 - p1;
            



            var rd = 2;
            var q1 = new Point3D(Math.Round(p1.X, rd), Math.Round(p1.Y, rd), Math.Round(p1.Z, rd));
            var q2 = new Point3D(Math.Round(p2.X, rd), Math.Round(p2.Y, rd), Math.Round(p2.Z, rd));
            var q3 = new Point3D(Math.Round(p3.X, rd), Math.Round(p3.Y, rd), Math.Round(p3.Z, rd));

            return new() { q1, q2, q3 };
        }

        #endregion
    }
}
