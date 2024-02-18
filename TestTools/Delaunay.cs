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
    /// Delauny三角网
    /// </summary>
    public class Delaunay
    {
        /// <summary>
        /// 点集合
        /// </summary>
        public List<Point> points { get; set; }
        /// <summary>
        /// 三角形集合
        /// </summary>
        public List<Triangle> triangles { get; set; }
        /// <summary>
        /// 工具类
        /// </summary>
        public PointLineTool plt { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="points"></param>
        public Delaunay(List<XYZ> points)
        {
            this.points = XYZToPoint(points);
            triangles = new List<Triangle>();
            plt = new PointLineTool();
        }
        /// <summary>
        /// 构建Delaunay三角形
        /// </summary>
        public void Run()
        {
            //获取超三角形
            var maxt = plt.TriangleContainPoints(points.Select(it => it.location).ToList());
        }
        /// <summary>
        /// XYZ转Point
        /// </summary>
        /// <returns></returns>
        public List<Point> XYZToPoint(List<XYZ> points)
        {
            List<Point> result = new List<Point>();
            for(int i = 0; i < points.Count; i++)
            {
                result.Add(new Point(points[i]) { index = i });
            }
            return result;
        }
    }
    /// <summary>
    /// 点类
    /// </summary>
    public class Point
    {
        /// <summary>
        /// 点的索引
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 点的坐标
        /// </summary>
        public XYZ location { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="point">坐标</param>
        public Point(XYZ point)
        {
            location = point;
        }
    }
    /// <summary>
    /// 三角形类
    /// </summary>
    public class Triangle
    {
        /// <summary>
        /// 三角形的索引
        /// </summary>
        public int index { get; set; }
        /// <summary>
        /// 三角形第一个顶点索引
        /// </summary>
        public int indexV1 { get; set; }
        /// <summary>
        /// 三角形第二个顶点索引
        /// </summary>
        public int indexV2 { get; set; }
        /// <summary>
        /// 三角形第三个顶点索引
        /// </summary>
        public int indexV3 { get; set; }
        /// <summary>
        /// 三角形第一个邻接三角形索引
        /// </summary>
        public int indexT1 { get; set; }
        /// <summary>
        /// 三角形第二个邻接三角形索引
        /// </summary>
        public int indexT2 { get; set; }
        /// <summary>
        /// 三角形第三个邻接三角形索引
        /// </summary>
        public int indexT3 { get; set; }
        /// <summary>
        /// 三角形的边界线
        /// </summary>
        public List<Line> bounary { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="index">三角形的索引</param>
        /// <param name="indexV1">三角形第一个顶点索引</param>
        /// <param name="indexV2">三角形第二个顶点索引</param>
        /// <param name="indexV3">三角形第三个顶点索引</param>
        /// <param name="indexT1">三角形第一个邻接三角形索引</param>
        /// <param name="indexT2">三角形第二个邻接三角形索引</param>
        /// <param name="indexT3">三角形第三个邻接三角形索引</param>
        /// <param name="bounary">三角形的边界线</param>
        public Triangle(int index, int indexV1, int indexV2, int indexV3, int indexT1, int indexT2, int indexT3, List<Line> bounary)
        {
            this.index = index;
            this.indexV1 = indexV1;
            this.indexV2 = indexV2;
            this.indexV3 = indexV3;
            this.indexT1 = indexT1;
            this.indexT2 = indexT2;
            this.indexT3 = indexT3;
            if(bounary != null && bounary.Count == 3)
            {
                this.bounary = bounary;
            }
        }
    }
}
