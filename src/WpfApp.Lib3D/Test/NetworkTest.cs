﻿using HelixToolkit.Wpf;
using RacewayDataLib;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Visual;

namespace WpfApp.Lib3D.Test
{
    public static class NetworkTest
    {
        /// <summary>
        /// Create visuals of the raceway network
        /// </summary>
        public static ModelVisual3D BuildNetwork(IEnumerable<Raceway> raceways, int testMode = 0)
        {
            var m = new ModelVisual3D();
            if (testMode == 0)
            {
                // build a single visual contains all the raceways
                m.Children.Add(BuildRacewaysAsModel(raceways));
                // build a single visual contains all the raceway nodes.
                m.Children.Add(BuildNodesAsModel(raceways));
            }
            else
            {
                // build multiple visuals for the raceway
                m.Children.Add(BuildRacewaysAsVisual(raceways));
                // build multiple visuals for the nodes
                m.Children.Add(BuildNodesAsVisual(raceways));
            }
            return m;
        }

        public static ModelVisual3D BuildNetwork(IEnumerable<Raceway> raceways, 
            IEnumerable<Cable> cables, IEnumerable<Node> allNodes)
        {
            var m = new ModelVisual3D();
            // build a single visual contains all the raceways
            m.Children.Add(BuildRacewaysVisual3D(raceways));
            // build a single visual contains all the raceway nodes.
            m.Children.Add(BuildNodeVisual3D(raceways, cables, allNodes));

            return m;
        }

        #region Point collection

        /// <summary>
        /// Create a single collection of 3D points with 2 points for each raceway, FromNode and ToNode. 
        /// </summary>
        public static Point3DCollection GetLinePoints(IEnumerable<Raceway> raceways) =>
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
        public static IEnumerable<Point3DCollection> GetLinesPoint2(IEnumerable<Raceway> raceways) =>
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
        public static Point3DCollection GetNodePoints(IEnumerable<Raceway> raceways) =>
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

        public static Point3DCollection GetNodePoints(IEnumerable<Node> nodes) =>
            nodes
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
            new() { Points = GetNodePoints(raceways), Size = 3, Color = Colors.Gray };

        #endregion

        #region Custom Visual

        public static RacewayVisual3D BuildRacewaysVisual3D(IEnumerable<Raceway> raceways)
        {
            var v = new RacewayVisual3D();
            v.Raceways = raceways;
            v.BuildMesh();
            return v;
        }

        public static NodeVisual3D BuildNodeVisual3D(IEnumerable<Raceway> raceways, 
            IEnumerable<Cable> cables, IEnumerable<Node> allNodes)
        {
            var allRwNodes = raceways.GetNodes();
            var rwNodes = allRwNodes.Where(n => n.NodeType != "EQUIPMENT");
            var dEqNodes = allRwNodes.Where(n => n.NodeType == "EQUIPMENT").ToDictionary(n => n.ID);

            var cblNodes = allNodes.GetNodes(cables.GetNodeIds());
            var eqNodes  = cblNodes.Aggregate(dEqNodes, (d, n) =>
            {
                d.TryAdd(n.ID, n);
                return d;
            })
            .Values
            .Where(n => n.X + n.Y + n.Z > 10);


            var v = new NodeVisual3D();
            v.RacewayNode = rwNodes;
            v.EquipNode = eqNodes;
            v.BuildMesh();
            return v;
        }

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
