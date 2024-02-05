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
        /// <summary>
        /// 测试格雷厄姆算法
        /// </summary>
        public static void TestGrahamAlgorithm()
        {
            List<XYZ> points = new List<XYZ>() {
                new XYZ(4,1,0),
                new XYZ(2,2,0),
                new XYZ(5,2,0),
                new XYZ(6,2,0),
                new XYZ(3,3,0),
                new XYZ(4,4,0),
                new XYZ(5,4,0),
                new XYZ(6,4,0),
            };
            GrahamAlgorithm algorithm = new GrahamAlgorithm();
            var result = algorithm.GetConcexShell(points);
        }
        public static void Main()
        {
            TestGrahamAlgorithm();
        }
    }
}
