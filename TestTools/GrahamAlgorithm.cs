using TestTools.Model;

namespace TestTools
{
    /// <summary>
    /// 格雷厄姆算法
    /// </summary>
    public class GrahamAlgorithm
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GrahamAlgorithm() { }
        /// <summary>
        /// 将有序点转化为闭合区域线
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
            for(int i = 1; i < points.Count; i++)
            {
                result.Add(Line.CreateLine(points[i - 1], points[i]));
            }
            result.Add(Line.CreateLine(points.Last(), points.First()));
            return result;
        }
        /// <summary>
        /// 获取点集合的凸壳顶点
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public List<XYZ> GetConcexShell(List<XYZ> points)
        {
            //点去重
            points = PointsRemoveRepeat(points);
            if (points.Count <= 3)
            {
                return points;
            }
            var ps = PolorSorting(points);
            List<XYZ> result = new List<XYZ>() { ps[0], ps[1] };
            for(int i = 2; i < ps.Count - 1; i++)
            {
                if(IsSameSide(Line.CreateLine(ps[i - 1], ps[i]), ps[i - 2], ps[i + 1]))
                {
                    result.Add(ps[i]);
                }
            }
            result.Add(ps.Last());
            return result;
        }
        /// <summary>
        /// 取坐标中y值最小中x值最小的坐标作为基点（最下靠右），求与其它点的夹角并排序
        /// </summary>
        /// <param name="points">坐标集合</param>
        /// <returns></returns>
        public List<XYZ> PolorSorting(List<XYZ> points)
        {
            List<XYZ> result = new List<XYZ>();
            //基点
            XYZ BasePoint = points.OrderBy(p => p.Y).OrderBy(t => t.X).First();
            result.Add(BasePoint);
            points.Remove(BasePoint);
            //极角排序
            points = points.OrderBy(it => CalculatePolorAngle(BasePoint, it)).ToList();
            result.AddRange(points);
            return result;
        }
        /// <summary>
        /// 计算角度(0,Π)
        /// </summary>
        /// <param name="p1">点1</param>
        /// <param name="p2">点2</param>
        /// <returns></returns>
        public double CalculateAngle(XYZ p1,XYZ p2)
        {
            double m1 = p1.ModuleLength() == 0 ? 1 : p1.ModuleLength();
            double m2 = p2.ModuleLength() == 0 ? 1 : p2.ModuleLength();
            //cos值
            double cos_a = p1.DotProduct(p2) / (m1 * m2);
            return Math.Acos(cos_a);
        }
        /// <summary>
        /// 计算极角
        /// </summary>
        /// <param name="p1">极点</param>
        /// <param name="p2">计算点</param>
        /// <returns></returns>
        public double CalculatePolorAngle(XYZ p1, XYZ p2)
        {
            XYZ newPoint = new XYZ(p2.X - p1.X, p2.Y - p1.Y, p2.Z - p1.Z);
            if(newPoint.X == 0)
            {
                if(newPoint.Y > 0)
                {
                    return Math.PI / 2;
                }
                else
                {
                    return -Math.PI / 2;
                }
            }
            return Math.Atan(newPoint.Y / newPoint.X);
        }
        /// <summary>
        /// 坐标点去重
        /// </summary>
        /// <param name="points">坐标集合</param>
        /// <returns></returns>
        public List<XYZ> PointsRemoveRepeat(List<XYZ> points)
        {
            List<XYZ> result = new List<XYZ>();
            foreach(var point in points)
            {
                int index = result.FindIndex(it => point.IsSamePoint(it));
                if(index < 0)
                {
                    result.Add(point);
                }
            }
            return result;
        }
        /// <summary>
        /// 判断两点是否在线的同一侧
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        public bool IsSameSide(Line l,XYZ p1,XYZ p2)
        {
            if(l == null)
            {
                return false;
            }
            if (JudgeSide(l, p1) == 0 || JudgeSide(l, p2) == 0)
            {
                return true;
            }
            if ((JudgeSide(l, p1) == 1 && JudgeSide(l, p2) == 1) || (JudgeSide(l, p1) == -1 && JudgeSide(l, p2) == -1))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// 判断点与直线的位置
        /// 0：点在直线上，1：点在直线的上(左)侧，-1：点在直线的下(右)侧
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public int JudgeSide(Line l,XYZ p)
        {
            if(l.Dirction.Y == 1)
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
                if(predict_y < p.Y)
                    return -1;
                if(predict_y > p.Y)
                    return 1;
                if(predict_y == p.Y)
                    return 0;
            }
            return -2;
        }
    }
}