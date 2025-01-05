using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Test;
using WpfApp.Lib3D.Utility;

namespace WpfApp.Lib3D.Binder
{
    public class CommandBinder : IDisposable
    {
        private HelixViewport3D _vp = null!;
        private PointSelectionBinder _selBinder = null!;

        public CommandBinder(HelixViewport3D vp)
        {
            this._vp = vp;
            this._selBinder = new PointSelectionBinder(vp);
            this.ConfigCommand();
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
            };

            foreach (var c in lst)
                this._vp.CommandBindings.Add(c);
        }

        virtual protected void OnGenerate(object sender, RoutedEventArgs e) 
        {
            this.ClearVisuals();
            this.GenerateRandomVisual();
        }

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

        #endregion

        #region Visual

        public void InitViewPort()
        {
            this._vp.Viewport.Children.Clear();
            this._vp.Viewport.Children.Add(new DefaultLights());
            this._vp.Viewport.Children.Add(VisualBuilder.CreateCoordinateSystemVisual3D(2));
            this.Current = new ModelVisual3D();
            this._vp.Viewport.Children.Add(this.Current);
            this.Bound = CoordUtil.GetBound(new(200, 200, 200));
        }

        public ModelVisual3D Current { get; protected set; } = null!;

        protected Rect3D Bound { get; set; }

        public void ClearVisuals() =>
            this.Current.Children.Clear();

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
            var lst = BuilderTest.Run(this.Bound);
            AddVisuals(this.Current.Children, lst);
            this.ZoomExtent();   
        }

        protected void AddVisuals(Visual3DCollection col, IEnumerable<Visual3D> visuals)
        {
            foreach (var v in visuals)
                col.Add(v);
        }

        #endregion

        #region View

        public virtual void ZoomExtent()
        {
            this._vp.ZoomExtents(this.Bound, 200);
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
