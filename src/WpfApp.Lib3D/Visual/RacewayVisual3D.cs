using HelixToolkit.Wpf;
using RacewayDataLib;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Test;

namespace WpfApp.Lib3D.Visual
{
    public class RacewayVisual3D : ModelVisual3D
    {
        public record ViewSetting
        {
            public bool HideTray { get; set; }
            public bool HideJump { get; set; }
            public bool HideDrop { get; set; }
        }

        public RacewayVisual3D() : this(new()) { }

        public RacewayVisual3D(ViewSetting setting)
        {
            this.UpdateView(setting);
        }

        /// <summary>
        /// Raceway data
        /// </summary>
        public IEnumerable<Raceway> Raceways { get; set; } = [];

        protected Dictionary<string, LinesVisual3D> Visuals { get; set; } = new();

        /// <summary>
        /// Tray layer
        /// </summary>
        public LinesVisual3D TrayVisual { get; protected set; } = new() { Color = Colors.DarkGray };

        /// <summary>
        /// Jump layer
        /// </summary>
        public LinesVisual3D JumpVisual { get; protected set; } = new() { Color = Colors.LightGreen };

        /// <summary>
        /// Drop layer
        /// </summary>
        public LinesVisual3D DropVisual { get; protected set; } = new() { Color = Colors.Yellow };

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

        /// <summary>
        /// Hide / show the displays of raceway laters
        /// </summary>
        public void UpdateView(ViewSetting setting)
        {
            if (setting.HideTray && this.TrayVisual.IsAttachedToViewport3D())
                this.Children.Remove(this.TrayVisual);
            else if (!setting.HideTray && !this.TrayVisual.IsAttachedToViewport3D())
                this.Children.Add(this.TrayVisual);

            if (setting.HideJump && this.JumpVisual.IsAttachedToViewport3D())
                this.Children.Remove(this.JumpVisual);
            else if (!setting.HideJump && !this.JumpVisual.IsAttachedToViewport3D())
                this.Children.Add(this.JumpVisual);

            if (setting.HideDrop && this.DropVisual.IsAttachedToViewport3D())
                this.Children.Remove(this.DropVisual);
            else if (!setting.HideDrop && !this.DropVisual.IsAttachedToViewport3D())
                this.Children.Add(this.DropVisual);
        }

        public void ClearMesh()
        {
            this.TrayVisual.Points.Clear();
            this.JumpVisual.Points.Clear();
            this.DropVisual.Points.Clear();
        }

        /// <summary>
        /// Build mesh for the tray, jump, drop visuals
        /// </summary>
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
