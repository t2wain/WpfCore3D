using System.Windows.Controls;
using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Binder;

namespace WpfApp.Lib3D
{
    /// <summary>
    /// Interaction logic for UViewPort.xaml
    /// </summary>
    public partial class UViewPort : UserControl, IDisposable
    {
        CommandBinder _cmd = null!;

        public UViewPort()
        {
            InitializeComponent();
            this._cmd = new CommandBinder(this.vport);
            this._cmd.InitViewPort();
        }

        public void AddRandomVisuals(Rect3D bound)
        {
            this._cmd.ClearVisuals();
            this._cmd.GenerateRandomVisual(bound);
        }

        public void Dispose()
        {
            this._cmd.Dispose();
        }
    }
}
