﻿using System;
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
    }
}
