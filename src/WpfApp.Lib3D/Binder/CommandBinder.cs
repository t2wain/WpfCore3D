using HelixToolkit.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Test;
using WpfApp.Lib3D.Utility;

namespace WpfApp.Lib3D.Binder
{
    public class CommandBinder : IDisposable
    {
        private HelixViewport3D _vp = null!;

        public CommandBinder(HelixViewport3D vp)
        {
            this._vp = vp;
            this.ConfigCommand();
        }

        #region Command

        virtual protected void ConfigCommand()
        {
            var lst = new List<CommandBinding>
            {
                new CommandBinding(Lib3DCommands.RefreshVisualCommand, this.OnRefresh),
                new CommandBinding(Lib3DCommands.ReShuffleVisualCommand, this.OnReshuffle),
                new CommandBinding(Lib3DCommands.ClearViewPortCommand, this.OnClear),
            };

            foreach (var c in lst)
                this._vp.CommandBindings.Add(c);
        }

        virtual protected void OnRefresh(object sender, RoutedEventArgs e) 
        {
            this.ClearVisual();
            this.GenerateRandomVisual();
        }

        virtual protected void OnReshuffle(object sender, RoutedEventArgs e) =>
            this.Reshuffle();

        virtual protected void OnClear(object sender, RoutedEventArgs e) => 
            this.ClearVisual();

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

        public void ClearVisual() =>
            this.Current.Children.Clear();

        public void Reshuffle()
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
            this._vp.ZoomExtents(this.Bound);
        }

        protected void AddVisuals(Visual3DCollection col, IEnumerable<Visual3D> visuals)
        {
            foreach (var v in visuals)
                col.Add(v);
        }

        #endregion

        public void Dispose()
        {
            this._vp = null!;
        }
    }
}
