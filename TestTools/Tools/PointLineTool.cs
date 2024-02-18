using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestTools.Model;

namespace TestTools.Tools
{
    /// <summary>
    /// 点线处理工具类
    /// </summary>
    public class PointLineTool
    {
        /// <summary>
        /// 求两点的欧氏距离
        /// </summary>
        /// <returns></returns>
        public double DistanceTo(XYZ p1,XYZ p2)
        {
            if (p1 == null || p2 == null)
                return -1;
            return Math.Sqrt(Math.Pow(p1.X - p2.X, 2) + Math.Pow(p1.Y - p2.Y, 2) + Math.Pow(p1.Z - p2.Z, 2));
        }
        /// <summary>
        /// 求两点的点积
        /// </summary>
        /// <returns></returns>
        public double DotProduct(XYZ p1,XYZ p2)
        {
            if (p1 == null || p2 == null)
                return double.MaxValue;
            return p1.X * p2.X + p1.Y * p2.Y + p1.Z * p2.Z;
        }
        /// <summary>
        /// 点的模长
        /// </summary>
        /// <returns></returns>
        public double ModuleLength(XYZ p)
        {
            return Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
        }
        /// <summary>
        /// 判断点是否同一点
        /// </summary>
        /// <param name="loss">误差值</param>
        /// <returns></returns>
        public bool IsSamePoint(XYZ p1,XYZ p2,double loss = 0.001)
        {
            return Math.Abs(p1.X - p2.X) <= loss && Math.Abs(p1.Y - p2.Y) <= loss && Math.Abs(p1.Z - p2.Z) <= loss;
        }
        /// <summary>
        /// 点与直线的位置
        /// 0：点在直线上，1：点在直线的上(左)侧，-1：点在直线的下(右)侧, -2无效返回值
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public int PointInStraightLineSide(Line l, XYZ p)
        {
            if (l.Dirction.Y == 1)
            {
                if (p.X > l.Start.X)
                    return 1;
                if (p.X < l.Start.X)
                    return -1;
                if (p.X == l.Start.X)
                    return 0;
            }
            else
            {
                double k = l.Dirction.Y / l.Dirction.X;
                double b = l.Start.Y - k * l.Start.X;
                double predict_y = k * p.X + b;
                if (predict_y < p.Y)
                    return -1;
                if (predict_y > p.Y)
                    return 1;
                if (predict_y == p.Y)
                    return 0;
            }
            return -2;
        }
        /// <summary>
        /// 将闭合区域有序点转化为闭合区域线
        /// </summary>
        /// <param name="points">点集合</param>
        /// <returns></returns>
        public List<Line> PointToLine(List<XYZ> points)
        {
            if (points == null || points.Count == 1)
            {
                return new List<Line>();
            }
            List<Line> result = new List<Line>();
            for (int i = 1; i < points.Count; i++)
            {
                result.Add(Line.CreateLine(points[i - 1], points[i]));
            }
            result.Add(Line.CreateLine(points.Last(), points.First()));
            return result;
        }
        ///// <summary>
        ///// 计算两点与原点连线夹角角度(0,Π)
        ///// </summary>
        ///// <param name="p1">点1</param>
        ///// <param name="p2">点2</param>
        ///// <returns></returns>
        //public double CalculateAngle(XYZ p1, XYZ p2)
        //{
        //    double m1 = ModuleLength(p1);
        //    double m2 = ModuleLength(p2);
        //    if (m1 == 0 || m2 == 0)
        //    {
        //        return 0;
        //    }
        //    //cos值
        //    double cos_a = DotProduct(p1,p2) / (m1 * m2);
        //    return Math.Acos(cos_a);
        //}
        /// <summary>
        /// 计算极角(以一点为极点计算)
        /// </summary>
        /// <param name="p1">极点</param>
        /// <param name="p2">计算点</param>
        /// <returns></returns>
        public double CalculatePolorAngle(XYZ p1, XYZ p2)
        {
            XYZ newPoint = new XYZ(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            if (newPoint.X == 0)
            {
                if (newPoint.Y > 0)
                {
                    return Math.PI / 2;
                }
                else
                {
                    return -Math.PI / 2;
                }
            }
            return Math.Atan2(newPoint.Y,newPoint.X);
        }
        /// <summary>
        /// 坐标点去重
        /// </summary>
        /// <param name="points">坐标集合</param>
        /// <returns></returns>
        public List<XYZ> PointsRemoveRepeat(List<XYZ> points)
        {
            List<XYZ> result = new List<XYZ>();
            foreach (var point in points)
            {
                int index = result.FindIndex(it => IsSamePoint(it, point));
                if (index < 0)
                {
                    result.Add(point);
                }
            }
            return result;
        }
        /// <summary>
        /// 获取点到线的投影点
        /// </summary>
        /// <param name="p"></param>
        /// <param name="l"></param>
        /// <returns></returns>
        public XYZ GetPointProjectLine(XYZ p,Line l)
        {
            int flag = JudgeLineType(l);
            if(flag == 1)
            {
                return new XYZ(p.X, l.Start.Y, 0);
            }else if(flag == 2)
            {
                return new XYZ(l.Start.X, p.Y, 0);
            }
            else
            {
                XYZ ps = new XYZ(p.X - l.Start.X, p.Y - l.Start.Y, 0);
                double project_length = DotProduct(l.Dirction, ps) / ModuleLength(l.Dirction);
                return new XYZ(l.Start.X + l.Dirction.X * project_length, l.Start.Y + l.Dirction.Y * project_length, 0);
            }
            return null;
        }
        /// <summary>
        /// 判断线的类型(二维平面)
        /// 1:横线，2:竖线，-1:斜线
        /// </summary>
        /// <param name="loss">误差值</param>
        /// <returns></returns>
        public int JudgeLineType(Line line,double loss = 0.01)
        {
            //横线
            if(Math.Abs(line.Dirction.X) <= 1+loss && Math.Abs(line.Dirction.X) >= 1 - loss)
            {
                return 1;
            }
            //竖线
            if (Math.Abs(line.Dirction.Y) <= 1 + loss && Math.Abs(line.Dirction.Y) >= 1 - loss)
            {
                return 2;
            }
            //斜线
            return -1;
        }
        /// <summary>
        /// 计算三个点的夹角（0,Π）
        /// </summary>
        /// <param name="previous">第一个点</param>
        /// <param name="Apoint">夹角点</param>
        /// <param name="latter">后一个点</param>
        /// <returns></returns>
        public double ThreePointAngle(XYZ previous,XYZ Apoint,XYZ latter)
        {
            XYZ dir1 = new XYZ(previous.X - Apoint.X, previous.Y - Apoint.Y, previous.Z - Apoint.Z);
            XYZ dir2 = new XYZ(latter.X - Apoint.X, latter.Y - Apoint.Y, latter.Z - Apoint.Z);
            double cos_angle = DotProduct(dir1, dir2) / (ModuleLength(dir1) * ModuleLength(dir2));
            return Math.Acos(cos_angle);
        }
        /// <summary>
        /// 获取线段中点
        /// </summary>
        /// <returns></returns>
        public XYZ GetLineMiddlePoint(Line l)
        {
            return new XYZ((l.Start.X + l.End.X) / 2, (l.Start.Y + l.End.Y) / 2, (l.Start.Z + l.End.Z) / 2);
        }
        /// <summary>
        /// 将闭合区域线转为有序点
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public List<XYZ> AreaLinesToPoints(List<Line> lines)
        {
            List<XYZ> points = new List<XYZ>();
            //从第一条线开始（任意线作为起点线）
            foreach (Line line in lines)
            {
                points.Add(line.Start);
            }
            return points;
        }
        /// <summary>
        /// 获取包含所有点的边界矩形
        /// 返回值:左上，右上，右下，左下
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public List<XYZ> GetBoundaryRectangle(List<XYZ> points)
        {
            double max_y = double.MinValue;
            double min_y = double.MaxValue;
            double max_x = double.MinValue;
            double min_x = double.MaxValue;
            foreach (XYZ point in points)
            {
                max_y = Math.Max(max_y, point.Y);
                min_y = Math.Min(min_y, point.Y);
                max_x = Math.Max(max_x, point.X);
                min_x = Math.Min(min_x, point.X);
            }
            List<XYZ> result = new List<XYZ>()
            {
                new XYZ(min_x,max_y,0),
                new XYZ(max_x,max_y,0),
                new XYZ(max_x,min_y,0),
                new XYZ(min_x,min_y,0)
            };
            return result;
        }
        /// <summary>
        /// 获取两个点的中点
        /// </summary>
        /// <returns></returns>
        public XYZ GetTwoPointMid(XYZ p1,XYZ p2)
        {
            return new XYZ((p1.X + p2.X) / 2, (p1.Y + p2.Y) / 2, (p1.Z + p2.Z) / 2);
        }
        /// <summary>
        /// 获取包含点集合所有点的正三角形
        /// </summary>
        /// <param name="points">点集合</param>
        /// <returns></returns>
        public List<XYZ> TriangleContainPoints(List<XYZ> points)
        {
            //获取边界矩形顶点
            var rec = GetBoundaryRectangle(points);
            //判断最短的边
            double width = rec[0].DistanceTo(rec[1]);
            double height = rec[0].DistanceTo(rec[3]);
            //获取中点
            XYZ top_mid = GetTwoPointMid(rec[0], rec[1]);
            //XYZ p1 = new XYZ(top_mid.X - width, top_mid.Y, top_mid.Z);
            //XYZ p2 = new XYZ(top_mid.X + width, top_mid.Y, top_mid.Z);
            //正三角形顶点1(上)
            XYZ triangleP1 = new XYZ(top_mid.X, top_mid.Y + Math.Sqrt(3) * width, top_mid.Z);
            //获取中点
            XYZ bottom_mid = GetTwoPointMid(rec[2], rec[3]);
            XYZ mid_b = new XYZ(bottom_mid.X, bottom_mid.Y - height, bottom_mid.Z);
            //倍数
            double multiple = 2 * height / Math.Sqrt(3) * width + 1;
            //正三角形顶点2（左）
            XYZ triangleP2 = new XYZ(mid_b.X - multiple * width, mid_b.Y, mid_b.Z);
            //正三角形顶点3（右）
            XYZ triangleP3 = new XYZ(mid_b.X + multiple * width, mid_b.Y, mid_b.Z);
            return new List<XYZ>() { triangleP1, triangleP2, triangleP3 };
        }
        /// <summary>
        /// 判断两条线段是否相交
        /// </summary>
        /// <returns></returns>
        public bool JudgeLineIntersect(Line l1,Line l2)
        {
            //两条线段平行不存在交点
            if (CalCrossProduct2D(l1.Dirction,l2.Dirction) == 0)
            {
                return false;
            }
            //获取两条线的端点
            XYZ p1 = l1.Start;
            XYZ p2 = l1.End;
            XYZ p3 = l2.Start;
            XYZ p4 = l2.End;
            //求出所需向量
            XYZ p31 = new XYZ(p1.X - p3.X, p1.Y - p3.Y, p1.Z - p3.Z);
            XYZ p43 = new XYZ(p3.X - p4.X, p3.Y - p4.Y, p3.Z - p4.Z);
            XYZ p21 = new XYZ(p1.X - p2.X, p1.Y - p3.Y, p1.Z - p3.Z);
            //交点p0 = p1 + t * p1p2 和 p0 = p3 + s * p3p4  ( 0<t<1 && 0<s<1)
            double t = CalCrossProduct2D(p31, p43) / CalCrossProduct2D(p21, p43);
            double s = CalCrossProduct2D(p31, p21) / CalCrossProduct2D(p21, p43);
            //线段没有交点
            if((t < 0 && t > 1)|| (s < 0 && s > 1))
            {
                return false;
            }
            //线段交点在端点处
            if((t == 0||t == 1) && (s == 0 || s == 1))
            {
                return true;
            }
            //线段交点在线段内
            if ((t > 0 && t < 1) && (s > 0 && s < 1))
            {
                return true;
            }
            //默认返回值false
            return false;
        }
        /// <summary>
        /// 计算两点之间的叉乘(3D)
        /// </summary>
        /// <returns></returns>
        public XYZ CalCrossProduct3D(XYZ p1,XYZ p2)
        {
            return new XYZ(p1.Y * p2.Z, -(p1.X * p2.Z - p2.X * p1.Z), p1.X * p2.Y - p2.X * p1.Y);
        }
        /// <summary>
        /// 计算两点之间的叉乘(2D)
        /// </summary>
        /// <returns></returns>
        public double CalCrossProduct2D(XYZ p1, XYZ p2)
        {
            return p1.X * p2.Y - p2.X * p1.Y;
        }
    }
}