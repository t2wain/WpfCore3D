using HelixToolkit.Wpf;
using RacewayDataLib;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Test
{
    public static class NetworkTest
    {
        public static ModelVisual3D BuildNetwork(IEnumerable<Raceway> raceways, bool asVisual = false)
        {
            var m = new ModelVisual3D();
            if (asVisual)
            {
                m.Children.Add(BuildRacewaysAsVisual(raceways));
                m.Children.Add(BuildNodesAsVisual(raceways));
            }
            else
            {
                m.Children.Add(BuildRacewaysAsModel(raceways));
                m.Children.Add(BuildNodesAsModel(raceways));
            }
            return m;
        }

        #region Point collection

        static Point3DCollection GetLinePoints(IEnumerable<Raceway> raceways) =>
            raceways.Aggregate(new Point3DCollection(), (col, r) => 
            {
                col.Add(new(r.FromNode.X, r.FromNode.Y, r.FromNode.Z));
                col.Add(new(r.ToNode.X, r.ToNode.Y, r.ToNode.Z));
                return col;
            });

        static IEnumerable<Point3DCollection> GetLinesPoint2(IEnumerable<Raceway> raceways) =>
            raceways.Aggregate(new List<Point3DCollection>(), (lst, r) =>
            {
                lst.Add(new Point3DCollection() 
                {
                    new(r.FromNode.X, r.FromNode.Y, r.FromNode.Z),
                    new(r.ToNode.X, r.ToNode.Y, r.ToNode.Z)
                });
                return lst;
            });

        static Point3DCollection GetNodePoints(IEnumerable<Raceway> raceways) =>
            raceways.Aggregate(new Dictionary<int, Node>(), (d, r) =>
                {
                    d.TryAdd(r.FromNodeID, r.FromNode);
                    d.TryAdd(r.ToNodeID, r.ToNode);
                    return d;
                })
                .Values
                .Aggregate(new Point3DCollection(), (col, n) => 
                {
                    col.Add(new(n.X, n.Y, n.Z));
                    return col;
                });

        #endregion

        #region Single Visual3D

        public static LinesVisual3D BuildRacewaysAsModel(IEnumerable<Raceway> raceways) =>
            new() { Points = GetLinePoints(raceways) };

        public static PointsVisual3D BuildNodesAsModel(IEnumerable<Raceway> raceways) =>
            new() { Points = GetNodePoints(raceways), Size = 3 };

        #endregion

        #region Muliple Visual3D

        public static ModelVisual3D BuildRacewaysAsVisual(IEnumerable<Raceway> raceways) =>
            GetLinesPoint2(raceways).Aggregate(new ModelVisual3D(), (m, col) =>
            {
                m.Children.Add(new LinesVisual3D() { Points = col });
                return m;
            });

        public static ModelVisual3D BuildNodesAsVisual(IEnumerable<Raceway> raceways) =>
            GetNodePoints(raceways).Aggregate(new ModelVisual3D(), (m, pt) =>
            {
                m.Children.Add(new PointsVisual3D() { Points = new Point3DCollection() { pt }, Size = 3 });
                return m;
            });

        #endregion

    }
}
