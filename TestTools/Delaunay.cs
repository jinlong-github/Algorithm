using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTools.Model;

namespace TestTools
{
    /// <summary>
    /// Delauny三角网
    /// </summary>
    public class Delaunay
    {

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
    }
}
