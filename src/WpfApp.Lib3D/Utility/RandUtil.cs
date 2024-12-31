using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Utility
{
    /// <summary>
    /// Create random 3D data structures
    /// </summary>
    public static class RandUtil
    {

        static Random _rand = null!;
        public static Random GetRand()
        {
            if (_rand == null)
                _rand = new Random();
            return _rand;
        }

        #region Transform

        /// <summary>
        /// Create a random unit 3D vector
        /// </summary>
        public static Vector3D NextUnitVector3D(this Random rand)
        {
            var v = rand.NextVector3D(0, 1);
            v.Normalize();
            return v;
        }

        /// <summary>
        /// Create a random 3D vector
        /// </summary>
        public static Vector3D NextVector3D(this Random rand, double min, double max)
        {
            var rng = max - min;
            return new()
            {
                X = rand.NextDouble() * rng + min,
                Y = rand.NextDouble() * rng + min,
                Z = rand.NextDouble() * rng + min
            };
        }

        /// <summary>
        /// Create a random size
        /// </summary>
        public static Size3D NextSize(this Random rand, double min, double max)
        {
            var v = rand.NextVector3D(min, max);
            return new(v.X, v.Y, v.Z);
        }

        /// <summary>
        /// Create a random world coordinate within a sphere boundary
        /// </summary>
        public static Point3D NextLocation(this Random rand, BoundingSphere sphere)
        {
            var v = rand.NextUnitVector3D();
            v = v * rand.NextDouble() * (sphere.Radius - 0.00000001);
            return v + sphere.Center;
        }

        /// <summary>
        /// Create a random world coordinate on a sphere boundary
        /// </summary>
        public static Point3D NextLocationOnSphere(this Random rand, BoundingSphere sphere)
        {
            var v = rand.NextUnitVector3D();
            v = v * (sphere.Radius - 0.00000001);
            return v + sphere.Center;
        }

        /// <summary>
        /// Create a random world coordinate within a 3D rectangle boundary
        /// </summary>
        public static Point3D NextLocation(this Random rand, Rect3D bound)
        {
            var l = bound.Location;
            return new()
            {
                X = rand.NextDouble() * bound.SizeX + l.X,
                Y = rand.NextDouble() * bound.SizeY + l.Y,
                Z = rand.NextDouble() * bound.SizeZ + l.Z
            };
        }

        /// <summary>
        /// Create a random positive number
        /// </summary>
        public static double NextVal(this Random rand, double max) =>
            rand.NextVal(0, max);

        /// <summary>
        /// Create a random number between min and max
        /// </summary>
        public static double NextVal(this Random rand, double min, double max) =>
            rand.NextDouble() * (max - min) + min;

        /// <summary>
        /// Create a random rotation transform
        /// </summary>
        public static Transform3D NextRotation(this Random rand, Point3D center) =>
            CoordUtil.GetRotationTransform(rand.NextUnitVector3D(), rand.NextDouble() * 360, center);

        /// <summary>
        /// Create a random transform of rotation, location, and size
        /// </summary>
        public static Transform3D NextTransform(this Random rand, Rect3D bound, 
            double minSize, double maxSize, Point3D center)
        {
            // random rotation
            var r = rand.NextRotation(center);

            // random translation
            var l = rand.NextLocation(bound);
            var t = CoordUtil.GetTranslationTransform(l - center);

            // random scale
            var v = rand.NextVal(minSize, maxSize);
            var s = CoordUtil.GetScaleTranform(CoordUtil.GetScaleVector(v), center);

            // combine random transforms
            return CoordUtil.GetTransform([r, s, t]);
        }

        #endregion

        #region Material

        static List<Material> _lstMats = null!;
        public static Material GetMaterial(this Random rand, Material current)
        {
            if (_lstMats == null)
                _lstMats = new List<Material> 
                { 
                    Materials.Black,
                    Materials.Blue,
                    Materials.Brown,
                    Materials.DarkGray,
                    Materials.Gold,
                    Materials.Gray,
                    Materials.Green,
                    Materials.Hue,
                    Materials.Indigo,
                    Materials.LightGray,
                    Materials.Orange,
                    Materials.Rainbow,
                    Materials.Red,
                    Materials.Violet,
                    Materials.White,
                    Materials.Yellow,
                };

            var q = _lstMats.Where(m => m != current).ToList();
            var idx = rand.Next(q.Count - 1);
            return q[idx];
        }

        #endregion
    }
}
