using HelixToolkit.Wpf;
using Microsoft.Extensions.Configuration;
using RacewayDataLib;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Test;
using WpfApp.Lib3D.Utility;
using WpfApp.Lib3D.Visual;

namespace WpfApp.Lib3D.Binder
{
    public class CommandBinder : IDisposable
    {
        private HelixViewport3D _vp = null!;
        private PointSelectionBinder _selBinder = null!;
        string? _rwDataFolder = null;
        int _segSystemId = 0;

        public CommandBinder(HelixViewport3D vp)
        {
            this._vp = vp;
            this._selBinder = new PointSelectionBinder(vp);
            this.ConfigCommand();
        }

        protected DataConfig GetDataConfig()
        {
            var fn = "appsettings.json";
            if (this._rwDataFolder == null && File.Exists(fn))
            {
                var cb = new ConfigurationBuilder();
                cb.AddJsonFile(fn);
                var root = cb.Build();
                this._rwDataFolder = root["RacewayDataFolder"];
                this._segSystemId = Convert.ToInt32(root["SegSystemFilter"]);
            }
            else this._rwDataFolder = "";

            return string.IsNullOrEmpty(this._rwDataFolder) switch
            {
                true => new DataConfig(),
                _ => new DataConfig(this._rwDataFolder)
            };
        }

        #region Command

        virtual protected void ConfigCommand()
        {
            var lst = new List<CommandBinding>
            {
                new CommandBinding(Lib3DCommands.GenerateVisualCommand, this.OnGenerate),
                new CommandBinding(Lib3DCommands.ShuffleVisualCommand, this.OnShuffle, this.OnShuffleCanExecute),
                new CommandBinding(Lib3DCommands.ClearViewPortCommand, this.OnClear, this.OnShuffleCanExecute),
                new CommandBinding(Lib3DCommands.ZoomExtentCommand, this.OnZoomExtent, this.OnShuffleCanExecute),
                new CommandBinding(Lib3DCommands.TogglePointSelectionCommand, this.OnTogglePointSelection, this.OnShuffleCanExecute),
                new CommandBinding(Lib3DCommands.ToggleRectangleSelectionCommand, this.OnToggleRectangleSection, this.OnShuffleCanExecute),
                new CommandBinding(Lib3DCommands.ToggleMeshPointSelectionCommand, this.OnToggleMeshPointSelection, this.OnShuffleCanExecute),

                // Network commands
                new CommandBinding(Lib3DCommands.LoadNetworkCommand, this.OnLoadNetwork),
                new CommandBinding(Lib3DCommands.NetworkHideTrayCommand, this.OnHideTray, this.OnNetworkCommandCanExecute),
                new CommandBinding(Lib3DCommands.NetworkHideJumpCommand, this.OnHideJump, this.OnNetworkCommandCanExecute),
                new CommandBinding(Lib3DCommands.NetworkHideDropCommand, this.OnHideDrop, this.OnNetworkCommandCanExecute),
                new CommandBinding(Lib3DCommands.NetworkHideTrayNodeCommand, this.OnHideTrayNode, this.OnNetworkCommandCanExecute),
                new CommandBinding(Lib3DCommands.NetworkHideEquipNodeCommand, this.OnHideEquipmentNode, this.OnNetworkCommandCanExecute),

                new CommandBinding(Lib3DCommands.NetworkClearSelectionCommand, this.OnClearSelection, this.OnClearSelectionCanExecute),
            };

            foreach (var c in lst)
                this._vp.CommandBindings.Add(c);
        }

        virtual protected void OnGenerate(object sender, RoutedEventArgs e) =>
            this.GenerateRandomVisual();

        virtual protected void OnShuffle(object sender, RoutedEventArgs e) =>
            this.ShuffleVisuals();

        virtual protected void OnClear(object sender, RoutedEventArgs e) => 
            this.ClearVisuals();

        virtual protected void OnShuffleCanExecute(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = this.Current.Children.Count > 0;

        virtual protected void OnZoomExtent(object sender, RoutedEventArgs e) =>
            this.ZoomExtent();

        virtual protected void OnTogglePointSelection(object sender, ExecutedRoutedEventArgs e) =>
            this._selBinder.IsPointSelectionEnabled = (bool)e.Parameter;

        virtual protected void OnToggleRectangleSection(object sender, ExecutedRoutedEventArgs e) =>
            this._selBinder.IsRectangleSelectionEnabled = (bool)e.Parameter;

        virtual protected void OnToggleMeshPointSelection(object sender, ExecutedRoutedEventArgs e) =>
            this._selBinder.IsMeshPointSelectionEnabled = (bool)e.Parameter;

        #endregion

        #region Network Command

        virtual protected void OnLoadNetwork(object sender, RoutedEventArgs e)
        {
            this.LoadNetwork();
            //this.LoadTestNetwork();
            UpdateNetworkView();
        }

        virtual protected void OnHideTray(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsHideTray = (bool)e.Parameter;
            this.UpdateNetworkView();
        }

        virtual protected void OnHideJump(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsHideJump = (bool)e.Parameter;
            this.UpdateNetworkView();
        }

        virtual protected void OnHideDrop(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsHideDrop = (bool)e.Parameter;
            this.UpdateNetworkView();
        }

        virtual protected void OnHideTrayNode(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsHideTrayNode = (bool)e.Parameter;
            this.UpdateNetworkView();
        }

        virtual protected void OnHideEquipmentNode(object sender, ExecutedRoutedEventArgs e)
        {
            this.IsHideEquipmentNode = (bool)e.Parameter;
            this.UpdateNetworkView();
        }

        virtual protected void OnNetworkCommandCanExecute(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = this.NetworkVisual != null && this.NetworkVisual.IsAttachedToViewport3D();

        virtual protected void OnClearSelection(object sender, RoutedEventArgs e) =>
            this.GetRacewayVisual()?.ClearSelection();

        virtual protected void OnClearSelectionCanExecute(object sender, CanExecuteRoutedEventArgs e) =>
            e.CanExecute = this.NetworkVisual != null
                && this.NetworkVisual.IsAttachedToViewport3D()
                && this.GetRacewayVisual()?.SelectRW.Count > 0;

        #endregion

        #region Visual

        public void InitViewPort()
        {
            this._vp.Viewport.Children.Clear();
            this._vp.Viewport.Children.Add(new DefaultLights());
            this.Current = new ModelVisual3D();
            this.Center = new ModelVisual3D();
            this._vp.Viewport.Children.Add(this.Current);
            this._vp.Viewport.Children.Add(this.Center);
            this.Center.Children.Add(VisualBuilder.CreateCoordinateSystemVisual3D(2));
            this.Bound = CoordUtil.GetBound(new(200, 200, 200));
        }
        public ModelVisual3D Center { get; protected set; } = null!;

        public ModelVisual3D Current { get; protected set; } = null!;

        protected ModelVisual3D? NetworkVisual { get; set; }

        protected Rect3D Bound { get; set; }

        public void ClearVisuals()
        {
            this.Center.Children.Clear();
            this.Current.Children.Clear();
            this.NetworkVisual = null;
        }

        public void ShuffleVisuals()
        {
            BuilderTest.ApplyRandomTransform(new(), this.Bound, this.Current.Children);
        }

        public void GenerateRandomVisual(Rect3D bound) 
        {
            this.Bound = bound;
            this.GenerateRandomVisual();
        }

        protected void GenerateRandomVisual()
        {
            this.ClearVisuals();
            this._vp.Background = Brushes.White;
            this.Center.Children.Add(VisualBuilder.CreateCoordinateSystemVisual3D(2));
            var lst = BuilderTest.Run(this.Bound);
            AddVisuals(this.Current.Children, lst);
            this.ZoomExtent();   
        }

        protected void LoadNetwork()
        {
            this.ClearVisuals();
            this._vp.Background = Brushes.Black;
            var d = NetworkDB.LoadData(GetDataConfig());
            var r = d.Raceways
                .Where(r => r.CalcLength() < 1000)
                .SelectSystem(this._segSystemId)
                .ToList();
            NetworkVisual = NetworkTest.BuildNetwork(r,
                d.Cables.Where(c => this._segSystemId == 0 || c.SegSystem == this._segSystemId).ToList(),
                d.Nodes);
            this.Current.Children.Add(NetworkVisual);
            this.ZoomExtent();
        }

        protected void LoadTestNetwork()
        {
            this.ClearVisuals();
            this._vp.Background = Brushes.Black;
            NetworkVisual = NetworkTest.BuildSimpleNetWork();
            this.Current.Children.Add(NetworkVisual);
            this.ZoomExtent();
        }

        protected void AddVisuals(Visual3DCollection col, IEnumerable<Visual3D> visuals)
        {
            foreach (var v in visuals)
                col.Add(v);
        }

        public RacewayVisual3D? GetRacewayVisual() => 
            this.NetworkVisual?.Children
                .Where(v => v is RacewayVisual3D)
                .Cast<RacewayVisual3D>()
                .FirstOrDefault();

        #endregion

        #region View

        protected bool IsHideTray { get; set; }

        protected bool IsHideJump { get; set; }

        protected bool IsHideDrop { get; set; }

        protected bool IsHideTrayNode { get; set; }

        protected bool IsHideEquipmentNode { get; set; }

        protected virtual void UpdateNetworkView()
        {
            if (this.NetworkVisual == null)
                return;
            foreach (var v in this.NetworkVisual.Children)
            {
                if (v is RacewayVisual3D rw)
                {
                    rw.UpdateView(new RacewayVisual3D.ViewSetting { 
                        HideTray = IsHideTray,
                        HideJump = IsHideJump,
                        HideDrop = IsHideDrop,
                    });
                }
                else if (v is NodeVisual3D n)
                {
                    n.UpdateView(new NodeVisual3D.ViewSetting
                    {
                        HideTrayNode = IsHideTrayNode,
                        HideEquipmentNode = IsHideEquipmentNode,
                    });
                }
            }
        }

        public virtual void ZoomExtent()
        {
            this._vp.ZoomExtents();
        }

        #endregion

        public void Dispose()
        {
            this._selBinder.Dispose();
            this._vp.Viewport.Children.Clear();
            this._vp.CommandBindings.Clear();
            this._vp = null!;
        }
    }
}
