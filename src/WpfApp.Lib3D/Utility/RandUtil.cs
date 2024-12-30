using HelixToolkit.Wpf;
using System.Windows.Media.Media3D;

namespace WpfApp.Lib3D.Utility
{
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
            var v = new Vector3D(rand.NextDouble(), rand.NextDouble(), rand.NextDouble());
            v.Normalize();
            return v;
        }

        /// <summary>
        /// Create a random world coordinate within a sphere boundary
        /// </summary>
        public static Point3D NextLocation(this Random rand, Point3D sphereCenter, double radius)
        {
            var v = rand.NextUnitVector3D();
            v = v * rand.NextDouble() * radius;
            return v + sphereCenter;
        }

        /// <summary>
        /// Create a random world coordinate within a 3D rectangle boundary
        /// </summary>
        public static Point3D NextLocation(this Random rand, Rect3D bound)
        {
            var l = bound.Location;
            var x = rand.NextDouble() * bound.SizeX + l.X;
            var y = rand.NextDouble() * bound.SizeY + l.Y;
            var z = rand.NextDouble() * bound.SizeZ + l.Z;
            return new(x, y, z);
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
        public static RotateTransform3D NextRotation(this Random rand)
        {
            var q = new Quaternion(rand.NextUnitVector3D(), rand.NextDouble() * 360);
            var r = new QuaternionRotation3D(q);
            return new(r);
        }

        /// <summary>
        /// Create a random transform of rotation, location, and size
        /// </summary>
        public static Transform3D NextTransform(this Random rand, Rect3D bound, double minSize, double maxSize)
        {
            var r = rand.NextRotation();
            var l = rand.NextLocation(bound);
            var t = new TranslateTransform3D((Vector3D)l);
            var v = rand.NextVal(minSize, maxSize);
            var s = new ScaleTransform3D(v, v, v);
            var g = new Transform3DGroup();
            g.Children = new Transform3DCollection { r, s, t };
            return g;
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
