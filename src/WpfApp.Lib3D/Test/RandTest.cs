using System.Windows.Media.Media3D;
using WpfApp.Lib3D.Utility;

namespace WpfApp.Lib3D.Test
{
    public static class RandTest
    {
        public static void Run()
        {
            Test1();
        }

        public static void Test1()
        {
            var rand = new Random();
            var b = CoordUtil.GetBound(new(100, 50, 75));
            var l = Enumerable.Range(0, 100).Select(i => rand.NextLocation(b)).ToList();
            var t = l.Where(p => !b.Contains(p)).ToList();
            if (t.Count > 0)
                throw new Exception();

            var c = new Point3D();
            var r = 100;
            var l2 = Enumerable.Range(0, 100).Select(i => rand.NextLocation(c, r)).ToList();
            var t2 = l2.Where(p => ((Vector3D)p).Length > r).ToList();

            var v = rand.NextUnitVector3D();

        }
    }
}
