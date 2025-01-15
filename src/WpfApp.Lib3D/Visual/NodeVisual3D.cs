using HelixToolkit.Wpf;
using RacewayDataLib;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Test;

namespace WpfApp.Lib3D.Visual
{
    public class NodeVisual3D : ModelVisual3D
    {
        public record ViewSetting
        {
            public bool HideTrayNode { get; set; }
            public bool HideEquipmentNode { get; set; }
        }

        public NodeVisual3D() : this(new()) { }

        public NodeVisual3D(ViewSetting setting)
        {
            this.UpdateView(setting);
        }

        /// <summary>
        /// Racway node data
        /// </summary>
        public IEnumerable<Node> RacewayNode { get; set; } = [];

        /// <summary>
        /// Equipment node data
        /// </summary>
        public IEnumerable<Node> EquipNode { get; set; } = [];

        protected Dictionary<string, PointsVisual3D> Visuals { get; set; } = new();

        /// <summary>
        /// Raceway node layer
        /// </summary>
        public PointsVisual3D RacewayNodeVisual { get; protected set; } = new() { Size = 5, Color = Colors.Gray };

        /// <summary>
        /// Equipment node layer
        /// </summary>
        public PointsVisual3D EquipNodeVisual { get; protected set; } = new() { Size = 5, Color = Colors.DarkRed };

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

        /// <summary>
        /// Hide / show the display Node layers
        /// </summary>
        public void UpdateView(ViewSetting setting)
        {
            if (setting.HideTrayNode && this.RacewayNodeVisual.IsAttachedToViewport3D())
                this.Children.Remove(this.RacewayNodeVisual);
            else if (!setting.HideTrayNode && !this.RacewayNodeVisual.IsAttachedToViewport3D())
                this.Children.Add(this.RacewayNodeVisual);

            if (setting.HideEquipmentNode && this.EquipNodeVisual.IsAttachedToViewport3D())
                this.Children.Remove(this.EquipNodeVisual);
            else if (!setting.HideEquipmentNode && !this.EquipNodeVisual.IsAttachedToViewport3D())
                this.Children.Add(this.EquipNodeVisual);
        }

        public void ClearMesh()
        {
            this.RacewayNodeVisual.Points.Clear();
            this.EquipNodeVisual.Points.Clear();
        }

        /// <summary>
        /// Build mesh for the raceway node and equipment node visuals
        /// </summary>
        public void BuildMesh()
        {
            this.RacewayNodeVisual.Points = NetworkTest.GetNodePoints(this.RacewayNode);
            this.EquipNodeVisual.Points = NetworkTest.GetNodePoints(this.EquipNode);
        }


    }
}
