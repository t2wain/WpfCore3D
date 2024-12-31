using HelixToolkit.Wpf;
using System.Windows.Controls;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Binder;
using WpfApp.Lib3D.Test;
using WpfApp.Lib3D.Utility;

namespace WpfApp.Lib3D
{
    /// <summary>
    /// Interaction logic for UViewPort.xaml
    /// </summary>
    public partial class UViewPort : UserControl, IDisposable
    {
        PointSelectionBinder _vpb = null!;

        public UViewPort()
        {
            InitializeComponent();
            this._vpb = new PointSelectionBinder(this.vport);
        }

        public void AddLight()
        {
            this.vport.Children.Add(new DefaultLights());
            this.vport.Children.Add(VisualBuilder.CreateCoordinateSystemVisual3D(2));
        }

        public void AddRandomVisuals(Rect3D bound)
        {
            var lst = BuilderTest.Run(bound);
            var g = new ModelVisual3D();
            AddVisuals(g.Children, lst);
            this.vport.Children.Add(g);
            this.vport.ZoomExtents(bound.Expand(40));
            //this.AddVisuals(this.vport.Children, lst);
        }

        public void AddVisuals(Visual3DCollection col, IEnumerable<Visual3D> visuals)
        {
            foreach (var v in visuals)
                col.Add(v);
        }

        public void Dispose()
        {
            this._vpb.Dispose();
        }
    }
}
