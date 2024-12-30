using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Utility
{
    public static class CoordUtil
    {
        #region Component

        /// <summary>
        /// Create a 3D rectangle boundary with location adjusted
        /// so that the center of the rectangle is at coordinate (0, 0, 0)
        /// </summary>
        public static Rect3D GetBound(Size3D size) => GetBound(new Point3D(), size);

        /// <summary>
        /// Create a 3D rectangle boundary with a specified center coordinatge.
        /// </summary>
        public static Rect3D GetBound(Point3D center, Size3D size)
        {
            var x = size.X / -2 + center.X;
            var y = size.Y / -2 + center.Y;
            var z = size.Z / -2 + center.Z;
            return new(new Point3D(x, y, z), size);
        }

        /// <summary>
        /// Calculate the center coordinate of a 3D rectangle
        /// </summary>
        public static Point3D GetCenter(Rect3D bound) =>
            bound.GetCenter();

        /// <summary>
        /// Create a scale data structure
        /// </summary>
        public static Vector3D GetScaleVector(double scale) =>
            new Vector3D(scale, scale, scale);

        /// <summary>
        /// Create a rotation data structure
        /// </summary>
        public static Rotation3D GetRotation(Vector3D axisRotation, double angleInDegrees) =>
            new QuaternionRotation3D(new Quaternion(axisRotation, angleInDegrees));

        #endregion

        #region Transform3D

        /// <summary>
        /// Create an identity transform
        /// </summary>
        public static Transform3D GetIdentityTransformer() =>
            new MatrixTransform3D(Matrix3D.Identity);

        /// <summary>
        /// Create a translation transform
        /// </summary>
        public static Transform3D GetTranslationTransform(Vector3D translation) =>
            new TranslateTransform3D(translation);

        /// <summary>
        /// Create a scale transform
        /// </summary>
        public static Transform3D GetScaleTranform(Vector3D scale) =>
            new ScaleTransform3D(scale);

        /// <summary>
        /// Create a rotation transform
        /// </summary>
        public static Transform3D GetRotationTransform(Vector3D axisRotation, double angleInDegrees) =>
            GetRotationTransform(axisRotation, angleInDegrees, new());

        public static Transform3D GetRotationTransform(Vector3D axisRotation, double angleInDegrees, Point3D center) =>
            new RotateTransform3D(GetRotation(axisRotation, angleInDegrees), center);

        /// <summary>
        ///  Create a combined transforms
        /// </summary>
        public static Transform3D GetTransform(IEnumerable<Transform3D> transforms) =>
            new Transform3DGroup { Children = new Transform3DCollection(transforms) };

        /// <summary>
        /// Create a combined transform of rotation, then translation, then scale
        /// </summary>
        public static Transform3D GetTransform(Vector3D axisRotation, double angleInDegrees,
            Vector3D translation, Vector3D scale)
        {
            var m = new Matrix3D();
            m.Rotate(new Quaternion(axisRotation, angleInDegrees));
            m.Translate(translation);
            m.Scale(scale);
            return new MatrixTransform3D(m);
        }

        #endregion

    }
}
