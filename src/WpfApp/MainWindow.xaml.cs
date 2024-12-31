using System.Windows;
using WpfApp.Lib3D.Test;
using WpfApp.Lib3D.Utility;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TestRand();
            TestVisual();
        }

        public void TestRand()
        {
            RandTest.Run();
        }

        public void TestVisual()
        {
            this.vp.AddRandomVisuals(CoordUtil.GetBound(new(200, 200, 200)));
        }
    }
}