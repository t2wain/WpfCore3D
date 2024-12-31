using HelixToolkit.Wpf;
using System.Windows.Documents;
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
            return  Enumerable.Range(1, 10).Select(i => CreateRandomVisual(rand, bound)).SelectMany(v => v).ToList();
        }

        static void Test1()
        {
            Visual3D v = null!;
            v = VisualBuilder.CreateArrowVisual3D();
            v = VisualBuilder.CreateBoundingBoxVisual3D(new(1,1,1));
            v = VisualBuilder.CreateBoundingBoxWireFrameVisual3D(new(1,1,1));
        }

        public static IEnumerable<Visual3D> CreateRandomVisual(Random rand, Rect3D bound)
        {
            var lst = new List<Visual3D> 
            {
                VisualBuilder.CreateBoundingBoxVisual3D(rand.NextSize(1, 5)),
                VisualBuilder.CreateBoundingBoxWireFrameVisual3D(rand.NextSize(1, 10), rand.Next(1, 4)),
                VisualBuilder.CreateBoxVisual3D(rand.NextSize(1, 5)),
                VisualBuilder.CreateCubeVisual3D(),
                VisualBuilder.CreatTeapot()
            };
            ApplyRandomTransform(rand, bound, lst);
            return lst;
        }

        public static void ApplyRandomTransform(Random rand, Rect3D bound, IEnumerable<Visual3D> visuals)
        {
            foreach (var v in visuals)
            {
                v.Transform = v switch
                {
                    BoundingBoxVisual3D o => rand.NextTransform(bound, 1, 1, o.BoundingBox.GetCenter()),
                    BoundingBoxWireFrameVisual3D o => rand.NextTransform(bound, 1, 1, o.BoundingBox.GetCenter()),
                    BoxVisual3D o => rand.NextTransform(bound, 1, 1, o.Center),
                    CubeVisual3D o => rand.NextTransform(bound, 1, 5, o.Center),
                    Teapot o => rand.NextTransform(bound, 0.5, 3, o.Position),
                    _ => CoordUtil.GetIdentityTransformer()
                };
            }
        }
    }
}
