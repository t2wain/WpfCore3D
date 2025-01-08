using HelixToolkit.Wpf;
using RacewayDataLib;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Test
{
    public static class NetworkTest
    {
        /// <summary>
        /// Create visuals of the raceway network
        /// </summary>
        public static ModelVisual3D BuildNetwork(IEnumerable<Raceway> raceways, bool asVisual = false)
        {
            var m = new ModelVisual3D();
            if (asVisual)
            {
                // build multiple visuals for the raceway
                m.Children.Add(BuildRacewaysAsVisual(raceways));
                // build multiple visuals for the nodes
                m.Children.Add(BuildNodesAsVisual(raceways));
            }
            else
            {
                // build a single visual contains all the raceways
                m.Children.Add(BuildRacewaysAsModel(raceways));
                // build a single visual contains all the raceway nodes.
                m.Children.Add(BuildNodesAsModel(raceways));
            }
            return m;
        }

        #region Point collection

        /// <summary>
        /// Create a single collection of 3D points with 2 points for each raceway, FromNode and ToNode. 
        /// </summary>
        static Point3DCollection GetLinePoints(IEnumerable<Raceway> raceways) =>
            raceways.Aggregate(new Point3DCollection(), (col, r) => 
            {
                col.Add(new(r.FromNode.X, r.FromNode.Y, r.FromNode.Z));
                col.Add(new(r.ToNode.X, r.ToNode.Y, r.ToNode.Z));
                return col;
            });

        /// <summary>
        /// Create muliple collections of 3D points.
        /// Each raceway creates a new collection of 2 point, FromNode and ToNode.
        /// </summary>
        /// <param name="raceways"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Create a single collection of unique 3D points collected
        /// from the raceway nodes.
        /// </summary>
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

        /// <summary>
        /// Create a single LinesVisual3D visual that contains all the raceways.
        /// Note, 3D performance is much better when there are fewer visuals.
        /// </summary>
        public static LinesVisual3D BuildRacewaysAsModel(IEnumerable<Raceway> raceways) =>
            // note, performance is much better by assigning a new collection to Points
            // rather than adding points to existing Points collection.
            new() { Points = GetLinePoints(raceways) };

        /// <summary>
        /// Create a single PointsVisual3D visual that contains all the nodes of the raceways.
        /// Note, 3D performance is much better when there are fewer visuals.
        /// </summary>
        public static PointsVisual3D BuildNodesAsModel(IEnumerable<Raceway> raceways) =>
            // note, performance is much better by assigning a new collection to Points
            // rather than adding points to existing Points collection.
            new() { Points = GetNodePoints(raceways), Size = 3 };

        #endregion

        #region Muliple Visual3D

        /// <summary>
        /// Create a collection of LinesVisual3D visual. Each visual contains one raceway.
        /// Note, 3D performance deteriorates when there are large number of visuals.
        /// </summary>
        public static ModelVisual3D BuildRacewaysAsVisual(IEnumerable<Raceway> raceways) =>
            GetLinesPoint2(raceways).Aggregate(new ModelVisual3D(), (m, col) =>
            {
                m.Children.Add(new LinesVisual3D() { Points = col });
                return m;
            });

        /// <summary>
        /// Create a collection of PointsVisual3D visual. Each visual contains one raceway node.
        /// Note, 3D performance deteriorates when there are large number of visuals.
        /// </summary>
        public static ModelVisual3D BuildNodesAsVisual(IEnumerable<Raceway> raceways) =>
            GetNodePoints(raceways).Aggregate(new ModelVisual3D(), (m, pt) =>
            {
                m.Children.Add(new PointsVisual3D() { Points = new Point3DCollection() { pt }, Size = 3 });
                return m;
            });

        #endregion

    }
}
