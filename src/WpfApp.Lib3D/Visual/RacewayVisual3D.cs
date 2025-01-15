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

        public Dictionary<int, int> TrayRWVertex { get; set; } = new();

        public Dictionary<int, int> JumpRWVertex { get; set; } = new();

        public Dictionary<int, int> DropRWVertex { get; set; } = new();

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
            Raceway? rw = null;
            if (selectVis.Equals(TrayVisual))
            {
                var idx = this.TrayRWVertex[pointVertex[0]];
                rw = this.TrayRW[idx];
                //rw = this.TrayRW[idx];
                //rw = rw1;
            }
            else if (selectVis.Equals(JumpVisual))
            {
                var idx = this.JumpRWVertex[pointVertex[0]];
                rw = this.JumpRW[idx];
            }
            else if (selectVis.Equals(DropVisual)) 
            {
                var idx = this.DropRWVertex[pointVertex[0]];
                rw = this.DropRW[idx];
            }
            else if (selectVis.Equals(selectVis))
            {
                var idx = this.SelectRWVertex[pointVertex[0]];
                rw = this.SelectRW.Values
                    .Select((r, i) => (r, i))
                    .Where(j => j.i == idx)
                    .Select(i => i.r)
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
            this.TrayRWVertex = CreateIndices(this.TrayVisual.Points.Count);

            JumpRW = this.Raceways.GetJump().ToList();
            this.JumpVisual.Points = NetworkTest.GetLinePoints(JumpRW);
            this.JumpRWVertex = CreateIndices(this.JumpVisual.Points.Count);

            DropRW = this.Raceways.GetDrop().ToList();
            this.DropVisual.Points = NetworkTest.GetLinePoints(DropRW);
            this.DropRWVertex = CreateIndices(this.DropVisual.Points.Count);
        }

        /// <summary>
        /// Triangular indexes for LineVisual3D from HelixToolkit source code
        /// </summary>
        Dictionary<int, int> CreateIndices(int n)
        {
            var d = new Dictionary<int, int>();
            for (int i = 0; i < n / 2; i++)
            {
                var i4 = i * 4;
                var lstIdx = new List<int> { i4 + 0, i4 + 1, i4 + 2, i4 + 3 };
                lstIdx.Aggregate(d, (a, v) =>
                {
                    a.Add(v, i);
                    return a;
                });
            }
            return d;
        }

        public void BuildMeshSelection()
        {
            this.SelectVisual.Points = NetworkTest.GetLinePoints(SelectRW.Values);
            this.SelectRWVertex = CreateIndices(this.SelectVisual.Points.Count);
        }

        #endregion
    }
}