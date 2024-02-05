using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TestTools.Model
{
    /// <summary>
    /// 线类
    /// </summary>
    public class Line
    {
        /// <summary>
        /// 起点坐标
        /// </summary>
        public XYZ Start
        {
            get => Start;
            set
            {
                Start = value;
                Dirction = GetDirction(Start, End);
            }

        }
        /// <summary>
        /// 终点坐标
        /// </summary>
        public XYZ End
        {
            get => End;
            set
            {
                End = value;
                Dirction = GetDirction(Start, End);
            }
        }
        /// <summary>
        /// 方向
        /// </summary>
        public XYZ Dirction { get; private set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Line(XYZ start, XYZ end)
        {
            Start = start;
            End = end;
            Dirction = GetDirction(start, end);
        }

        /// <summary>
        /// 构建线
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public static Line CreateLine(XYZ p1,XYZ p2)
        {
            if (p1 != null && p2 != null)
            {
                return new Line(p1,p2);
            }
            return null;
        }
        /// <summary>
        /// 获取线的方向(方向向量)
        /// </summary>
        /// <returns></returns>
        private XYZ GetDirction(XYZ s, XYZ e)
        {
            double distance = s.DistanceTo(e);
            double x_dis = e.X - s.X;
            double y_dis = e.Y - s.Y;
            double z_dis = e.Z - s.Z;
            return new XYZ(Math.Round(x_dis / distance, 4), Math.Round(y_dis / distance, 4), Math.Round(z_dis / distance, 4));
        }
        
    }
}
