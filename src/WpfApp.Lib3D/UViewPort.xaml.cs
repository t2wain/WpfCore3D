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
        CommandBinder _cmd = null!;

        public UViewPort()
        {
            InitializeComponent();
            this._vpb = new PointSelectionBinder(this.vport);
            this._cmd = new CommandBinder(this.vport);
            this._cmd.InitViewPort();
        }

        public void AddRandomVisuals(Rect3D bound)
        {
            this._cmd.ClearVisual();
            this._cmd.GenerateRandomVisual(bound);
        }

        public void Dispose()
        {
            this._vpb.Dispose();
            this._cmd.Dispose();
        }
    }
}
