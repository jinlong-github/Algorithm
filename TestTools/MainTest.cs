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
                new XYZ(7,5,0),
            };
            GrahamAlgorithm algorithm = new GrahamAlgorithm();
            //获取点集合的凸壳顶点，使用点与线的位置计算
            var result = algorithm.GetConcexShell(points, 0);
            ////使用三点夹角计算
            //var result = algorithm.GetConcexShell(points, 1);
            //凸壳边界线
            List<Line> boundary = algorithm.PointToLine(result);
            //获取凸壳的直径
            Line maxLine = algorithm.GetConvexShellDiameter(boundary);
        }
        public static void Main()
        {
            TestGrahamAlgorithm();
        }
    }
}
