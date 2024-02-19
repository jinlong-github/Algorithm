using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTools.Model;
using TestTools.Tools;

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
        /// <summary>
        /// 判断线段交叉
        /// </summary>
        public static void TestLineIntersect()
        {
            Line l1 = Line.CreateLine(new XYZ(-1.5, 4, 0), new XYZ(0, 1, 0));
            Line l2 = Line.CreateLine(new XYZ(-1.5, 0.5, 0), new XYZ(-0.5, 3.5, 0));
            PointLineTool plt = new PointLineTool();
            //s为true有交点
            var s = plt.JudgeLineIntersect(l1, l2, out XYZ point, 1);
        }
        public static void Main()
        {
            TestGrahamAlgorithm();
            TestLineIntersect();
        }
    }
}
