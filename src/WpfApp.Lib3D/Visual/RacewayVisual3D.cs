using HelixToolkit.Wpf;
using RacewayDataLib;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Test;

namespace WpfApp.Lib3D.Visual
{
    public class RacewayVisual3D : ModelVisual3D
    {
        public RacewayVisual3D()
        {
            this.Children.Add(this.TrayVisual);
            this.Children.Add(this.JumpVisual);
            this.Children.Add(this.DropVisual);
        }

        public IEnumerable<Raceway> Raceways { get; set; } = [];

        protected Dictionary<string, LinesVisual3D> Visuals { get; set; } = new();

        protected LinesVisual3D TrayVisual { get; set; } = new() { Color = Colors.DarkGray };

        protected LinesVisual3D JumpVisual { get; set; } = new() { Color = Colors.LightGreen };

        protected LinesVisual3D DropVisual { get; set; } = new() { Color = Colors.Yellow };

        protected LinesVisual3D AddLineVisual(string name)
        {
            if (!Visuals.TryGetValue(name, out var l))
            {
                l = new LinesVisual3D();
                l.SetName(name);
                Visuals[name] = l;
                this.Children.Add(l);
            }
            return l;
        }

        public void ClearMesh()
        {
            this.TrayVisual.Points.Clear();
            this.JumpVisual.Points.Clear();
            this.DropVisual.Points.Clear();
        }

        public void BuildMesh()
        {
            var t = this.Raceways.GetTray();
            this.TrayVisual.Points = NetworkTest.GetLinePoints(t);

            t = this.Raceways.GetJump();
            this.JumpVisual.Points = NetworkTest.GetLinePoints(t);

            t = this.Raceways.GetDrop();
            this.DropVisual.Points = NetworkTest.GetLinePoints(t);
        }
    }
}
