using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Utility
{
    public static class VisualBuilder
    {
        public static ArrowVisual3D CreateArrowVisual3D(Transform3D t)
        {
            var a = new ArrowVisual3D();
            a.Origin = new Point3D();
            a.Point1 = new Point3D();
            a.Point2 = new Point3D(1, 0, 0);
            a.Transform = t;
            return a;
        }

        public static BoundingBoxVisual3D CreateBoundingBoxVisual3D(Transform3D t)
        {
            var b = new BoundingBoxVisual3D();
            b.BoundingBox = new Rect3D(new Point3D(), new Size3D(1, 1, 1));
            b.Transform = t;
            return b;
        }

        public static BoundingBoxWireFrameVisual3D CreateBoundingBoxWireFrameVisual3D(Transform3D t)
        {
            var b = new BoundingBoxWireFrameVisual3D();
            b.BoundingBox = new Rect3D(new Point3D(), new Size3D(1, 1, 1));
            b.Transform = t;
            return b;
        }

        public static BoxVisual3D CreateBoxVisual3D(Transform3D t)
        {
            var b = new BoxVisual3D();
            b.Center = new Point3D();
            b.Height = 1;
            b.Width = 1;
            b.Length = 1;
            b.TopFace = true;
            b.BottomFace = true;
            b.Transform = t;
            return b;
        }

        public static CoordinateSystemVisual3D CreateCoordinateSystemVisual3D(double length = 1)
        {
            var c = new CoordinateSystemVisual3D();
            c.ArrowLengths = length;
            c.XAxisColor = Colors.Red;
            c.YAxisColor = Colors.Green;
            c.ZAxisColor = Colors.Blue;
            return c;
        }

        public static CubeVisual3D CreateCubeVisual3D(Transform3D t)
        {
            var c = new CubeVisual3D();
            c.SideLength = 1;
            c.Center = new Point3D();
            c.Transform = t;
            return c;
        }

        public static Teapot CreatTeapot(Transform3D t)
        {
            var tp = new Teapot();
            tp.Transform = t;
            return tp;
        }

    }
}
