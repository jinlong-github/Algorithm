using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTools.Model
{
    /// <summary>
    /// 坐标类
    /// </summary>
    public class XYZ
    {
        /// <summary>
        /// X坐标
        /// </summary>
        public double X { get; set; } = 0;
        /// <summary>
        /// Y坐标
        /// </summary>
        public double Y { get; set; } = 0;
        /// <summary>
        /// Z坐标
        /// </summary>
        public double Z { get; set; } = 0;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="z">Z坐标</param>
        public XYZ(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public XYZ() { }
        /// <summary>
        /// 求两点的欧氏距离
        /// </summary>
        /// <returns></returns>
        public double DistanceTo(XYZ other)
        {
            if(other == null)
                return 0;
            return Math.Sqrt(Math.Pow(X - other.X, 2) + Math.Pow(Y - other.Y, 2) + Math.Pow(Z - other.Z, 2));
        }
        public override string? ToString()
        {
            return $"坐标XYZ：({X}，{Y}，{Z})";
        }
    }
}
