using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTools.Model;

namespace TestTools
{
    /// <summary>
    /// 测试类
    /// </summary>
    public class MainTest
    {
        public static void Main()
        {
            List<XYZ> points = new List<XYZ>() {
                new XYZ(6,0,0),
                new XYZ(2,1,0),
                new XYZ(3,9,0),
                new XYZ(2,8,0),
                new XYZ(7,1,0),
                new XYZ(9,4,0),
                new XYZ(0,0,0),
            };
            GrahamAlgorithm algorithm = new GrahamAlgorithm();
            var re = algorithm.PolorSorting(points);
        }
    }
}
