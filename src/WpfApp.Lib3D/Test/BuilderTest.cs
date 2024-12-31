using HelixToolkit.Wpf;
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
            Visual3D v = null!;
            v = VisualBuilder.CreateArrowVisual3D();
            v = VisualBuilder.CreateBoundingBoxVisual3D(new(1,1,1));
            v = VisualBuilder.CreateBoundingBoxWireFrameVisual3D(new(1,1,1));
        }

        static IEnumerable<Visual3D> Test2(Random rand, Rect3D bound)
        {
            var v1 = VisualBuilder.CreateBoundingBoxVisual3D(rand.NextSize(1, 5));
            v1.Transform = rand.NextTransform(bound, 1, 1, v1.BoundingBox.GetCenter());

            var v2 = VisualBuilder.CreateBoundingBoxWireFrameVisual3D(rand.NextSize(1, 10), rand.Next(1, 4));
            v2.Transform = rand.NextTransform(bound, 1, 1, v2.BoundingBox.GetCenter());

            var v3 = VisualBuilder.CreateBoxVisual3D(rand.NextSize(1, 5));
            v3.Transform = rand.NextTransform(bound, 1, 1, v3.Center);

            var v4 = VisualBuilder.CreateCubeVisual3D();
            v4.Transform = rand.NextTransform(bound, 1, 5, v4.Center);

            var v5 = VisualBuilder.CreatTeapot();
            v5.Transform = rand.NextTransform(bound, 0.5, 3, v5.Position);

            return [ v1, v2, v3, v4, v5 ];
        }
    }
}
