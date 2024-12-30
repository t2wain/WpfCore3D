using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Utility;

namespace WpfApp.Lib3D.Test
{
    public static class BuilderTest
    {
        public static void Run()
        {
            Test1();
        }

        public static IEnumerable<Visual3D> Run(Rect3D bound)
        {
            var rand = RandUtil.GetRand();
            return Enumerable.Range(1, 10).Select(i => Test2(rand, bound)).SelectMany(v => v).ToList();
        }

        static void Test1()
        {
            var t = new MatrixTransform3D(Matrix3D.Identity);
            Visual3D v = null!;
            v = VisualBuilder.CreateArrowVisual3D(t);
            v = VisualBuilder.CreateBoundingBoxVisual3D(t);
            v = VisualBuilder.CreateBoundingBoxWireFrameVisual3D(t);
        }

        static IEnumerable<Visual3D> Test2(Random rand, Rect3D bound)
        {
            return new List<Visual3D>
            {
                VisualBuilder.CreateBoundingBoxVisual3D(rand.NextTransform(bound, 1, 5)),
                VisualBuilder.CreateBoundingBoxWireFrameVisual3D(rand.NextTransform(bound, 1, 5)),
                VisualBuilder.CreateBoxVisual3D(rand.NextTransform(bound, 1, 5)),
                VisualBuilder.CreateCubeVisual3D(rand.NextTransform(bound, 1, 5)),
                VisualBuilder.CreatTeapot(rand.NextTransform(bound, 0.5, 3)),
            };
        }
    }
}
