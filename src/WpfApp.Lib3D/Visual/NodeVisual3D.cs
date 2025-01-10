using HelixToolkit.Wpf;
using RacewayDataLib;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Test;

namespace WpfApp.Lib3D.Visual
{
    public class NodeVisual3D : ModelVisual3D
    {
        public NodeVisual3D()
        {
            this.Children.Add(this.RacewayNodeVisual);
            this.Children.Add(this.EquipNodeVisual);
        }

        public IEnumerable<Node> RacewayNode { get; set; } = [];
        public IEnumerable<Node> EquipNode { get; set; } = [];

        protected Dictionary<string, PointsVisual3D> Visuals { get; set; } = new();

        protected PointsVisual3D RacewayNodeVisual { get; set; } = new() { Size = 3, Color = Colors.Gray };

        protected PointsVisual3D EquipNodeVisual { get; set; } = new() { Size = 3, Color = Colors.DarkRed };

        protected PointsVisual3D AddPointVisual(string name)
        {
            if (!Visuals.TryGetValue(name, out var n))
            {
                n = new PointsVisual3D();
                n.SetName(name);
                Visuals[name] = n;
                this.Children.Add(n);
            }
            return n;
        }

        public void ClearMesh()
        {
            this.RacewayNodeVisual.Points.Clear();
            this.EquipNodeVisual.Points.Clear();
        }

        public void BuildMesh()
        {
            this.RacewayNodeVisual.Points = NetworkTest.GetNodePoints(this.RacewayNode);
            this.EquipNodeVisual.Points = NetworkTest.GetNodePoints(this.EquipNode);
        }


    }
}
