using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Utility
{
    /// <summary>
    /// Build 3D data structures
    /// </summary>
    public static class CoordUtil
    {
        #region Component

        /// <summary>
        /// Create a 3D rectangle boundary with location adjusted
        /// so that the center of the rectangle is at coordinate (0, 0, 0)
        /// </summary>
        public static Rect3D GetBound(Size3D size) => GetBound(new Point3D(), size);

        /// <summary>
        /// Create a 3D rectangle boundary with a specified center coordinate.
        /// </summary>
        public static Rect3D GetBound(Point3D center, Size3D size) =>
            new()
            {
                Location = new Point3D
                {
                    X = size.X / -2 + center.X,
                    Y = size.Y / -2 + center.Y,
                    Z = size.Z / -2 + center.Z,
                },
                Size = size
            };

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
        /// Create a quaternion data structure
        /// </summary>
        public static Quaternion GetQuaternion(Vector3D axisRotation, double angleInDegrees) =>
            new Quaternion(axisRotation, angleInDegrees);

        /// <summary>
        /// Create a rotation data structure
        /// </summary>
        public static Rotation3D GetRotation(Vector3D axisRotation, double angleInDegrees) =>
            new QuaternionRotation3D(GetQuaternion(axisRotation, angleInDegrees));

        /// <summary>
        /// Create a combined matrix of rotation, then translation, then scale
        /// </summary>
        public static Matrix3D GetMatrix(Vector3D axisRotation, double angleInDegrees,
            Vector3D translation, Vector3D scale)
        {
            var m = new Matrix3D();
            m.Rotate(new Quaternion(axisRotation, angleInDegrees));
            m.Translate(translation);
            m.Scale(scale);
            return m;
        }

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
            GetScaleTranform(scale, new());

        /// <summary>
        /// Create a scale transform
        /// </summary>
        public static Transform3D GetScaleTranform(Vector3D scale, Point3D center) =>
            new ScaleTransform3D(scale, center);

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
        public static Transform3D GetTransform(IEnumerable<Transform3D> transforms)
        {
            var g = new Transform3DGroup { Children = new Transform3DCollection(transforms) };
            return GetTransform(g.Value);
        }

        /// <summary>
        /// Create a combined transform of rotation, then translation, then scale
        /// </summary>
        public static Transform3D GetTransform(Vector3D axisRotation, double angleInDegrees,
            Vector3D translation, Vector3D scale) =>
            GetTransform(GetMatrix(axisRotation, angleInDegrees, translation, scale));

        /// <summary>
        /// Create a transform from matrix
        /// </summary>
        public static Transform3D GetTransform(Matrix3D matrix) =>
            new MatrixTransform3D(matrix);

        #endregion

    }
}
