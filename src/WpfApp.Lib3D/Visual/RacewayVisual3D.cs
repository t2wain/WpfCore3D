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
            public bool HideSelection { get; set; }
        }

        public RacewayVisual3D() : this(new()) { }

        public RacewayVisual3D(ViewSetting setting)
        {
            this.UpdateView(setting);
        }

        #region Raceway data

        /// <summary>
        /// Raceway data
        /// </summary>
        public IEnumerable<Raceway> Raceways { get; set; } = [];

        public List<Raceway> TrayRW { get; protected set; } = new();

        public List<Raceway> JumpRW { get; protected set; } = new();

        public List<Raceway> DropRW { get; protected set; } = new();

        #endregion

        #region Visuals

        protected Dictionary<string, LinesVisual3D> Visuals { get; set; } = new();

        /// <summary>
        /// Tray layer
        /// </summary>
        public LinesVisual3D TrayVisual { get; protected set; } = new() { Color = Colors.DarkGray, Thickness = 2 };

        /// <summary>
        /// Jump layer
        /// </summary>
        public LinesVisual3D JumpVisual { get; protected set; } = new() { Color = Colors.LightGreen, Thickness = 2 };

        /// <summary>
        /// Drop layer
        /// </summary>
        public LinesVisual3D DropVisual { get; protected set; } = new() { Color = Colors.Yellow, Thickness = 2 };

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

        #endregion

        #region Selection

        public Dictionary<int, Raceway> SelectRW { get; protected set; } = new();
        public Dictionary<int, int> SelectRWVertex { get; set; } = new();

        /// <summary>
        /// Selection layer
        /// </summary>
        public LinesVisual3D SelectVisual { get; protected set; } = new() { Color = Colors.Magenta, Thickness = 3 };

        public void AddSelection(LinesVisual3D selectVis, List<int> pointVertex)
        {
            // retrieve the rw data by index

            // HelixToolkit souce code for ScreenVisual3D
            // show how mesh vertexes are created for LineVisual3D
            var idx = pointVertex[0] / 4;

            Raceway? rw = null;
            if (selectVis.Equals(TrayVisual))
            {
                rw = this.TrayRW[idx];
            }
            else if (selectVis.Equals(JumpVisual))
            {
                rw = this.JumpRW[idx];
            }
            else if (selectVis.Equals(DropVisual)) 
            {
                rw = this.DropRW[idx];
            }
            else if (selectVis.Equals(selectVis))
            {
                rw = this.SelectRW.Values
                    .Skip(idx)
                    .Take(1)
                    .FirstOrDefault();
            }

            if (rw != null)
            {
                if (this.SelectRW.ContainsKey(rw.ID))
                    this.SelectRW.Remove(rw.ID); // unselect
                else this.SelectRW.Add(rw.ID, rw); // select
                this.BuildMeshSelection(); // update visual
            }
        }

        public void ClearSelection()
        {
            this.SelectRW.Clear();
            this.BuildMeshSelection();
        }

        #endregion

        #region View

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

            if (setting.HideSelection && this.SelectVisual.IsAttachedToViewport3D())
                this.Children.Remove(this.SelectVisual);
            else if (!setting.HideSelection && !this.SelectVisual.IsAttachedToViewport3D())
                this.Children.Add(this.SelectVisual);
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
            TrayRW = this.Raceways.GetTray().ToList();
            this.TrayVisual.Points = NetworkTest.GetLinePoints(TrayRW);

            JumpRW = this.Raceways.GetJump().ToList();
            this.JumpVisual.Points = NetworkTest.GetLinePoints(JumpRW);

            DropRW = this.Raceways.GetDrop().ToList();
            this.DropVisual.Points = NetworkTest.GetLinePoints(DropRW);
        }

        public void BuildMeshSelection()
        {
            this.SelectVisual.Points = NetworkTest.GetLinePoints(SelectRW.Values);
        }

        #endregion
    }
}