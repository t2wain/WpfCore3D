using HelixToolkit.Wpf;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Utility
{
    public static class VisualBuilder
    {
        public static ArrowVisual3D CreateArrowVisual3D()
        {
            var a = new ArrowVisual3D();
            a.Origin = new Point3D();
            a.Point1 = new Point3D();
            a.Point2 = new Point3D(1, 0, 0);
            return a;
        }

        public static BoundingBoxVisual3D CreateBoundingBoxVisual3D(Size3D size)
        {
            var b = new BoundingBoxVisual3D();
            b.BoundingBox = new Rect3D(new Point3D(), size);
            return b;
        }

        public static BoundingBoxWireFrameVisual3D CreateBoundingBoxWireFrameVisual3D(Size3D size, double thickness = 1)
        {
            var b = new BoundingBoxWireFrameVisual3D();
            b.BoundingBox = new Rect3D(new Point3D(), size);
            b.Thickness = thickness;
            return b;
        }

        public static BoxVisual3D CreateBoxVisual3D(Size3D size)
        {
            var b = new BoxVisual3D();
            b.Center = new Point3D();
            b.Height = size.Y;
            b.Width = size.Z;
            b.Length = size.X;
            b.TopFace = true;
            b.BottomFace = true;
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

        public static CubeVisual3D CreateCubeVisual3D()
        {
            var c = new CubeVisual3D();
            c.SideLength = 1;
            c.Center = new Point3D();
            return c;
        }

        public static Teapot CreatTeapot()
        {
            var tp = new Teapot();
            return tp;
        }

    }
}
