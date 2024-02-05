using TestTools.Model;
using TestTools.Tools;

namespace TestTools
{
    /// <summary>
    /// 格雷厄姆算法
    /// </summary>
    public class GrahamAlgorithm
    {
        //工具类
        public PointLineTool plt = new PointLineTool();
        /// <summary>
        /// 构造函数
        /// </summary>
        public GrahamAlgorithm() { }
        /// <summary>
        /// 将闭合区域有序点转化为闭合区域线
        /// </summary>
        /// <param name="points">点集合</param>
        /// <returns></returns>
        public List<Line> PointToLine(List<XYZ> points)
        {
            return plt.PointToLine(points);
        }
        /// <summary>
        /// 获取点集合的凸壳顶点
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public List<XYZ> GetConcexShell(List<XYZ> points)
        {
            //点去重
            points = plt.PointsRemoveRepeat(points);
            if (points.Count <= 3)
            {
                return points;
            }
            var ps = PolorSorting(points);
            List<XYZ> result = new List<XYZ>() { ps[0], ps[1] };
            for(int i = 2; i < ps.Count - 1; i++)
            {
                result.Add(ps[i]);
                if(!IsSameSide(Line.CreateLine(result[result.Count - 2], result.Last()), result[result.Count - 3], ps[i + 1]))
                {
                    result.Remove(ps[i]);
                }
            }
            result.Add(ps.Last());
            return result;
        }
        /// <summary>
        /// 取坐标中y值最小中x值最小的坐标作为基点（最下靠右），求与其它点的极角排序
        /// </summary>
        /// <param name="points">坐标集合</param>
        /// <returns></returns>
        private List<XYZ> PolorSorting(List<XYZ> points)
        {
            List<XYZ> result = new List<XYZ>();
            //基点
            XYZ BasePoint = points.OrderBy(p => p.X).OrderBy(t => t.Y).First();
            result.Add(BasePoint);
            points.Remove(BasePoint);
            //极角排序
            points = points.OrderBy(it => plt.CalculatePolorAngle(BasePoint, it)).ToList();
            result.AddRange(points);
            return result;
        }
        /// <summary>
        /// 判断两点是否在线的同一侧
        /// </summary>
        /// <param name="l"></param>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private bool IsSameSide(Line l,XYZ p1,XYZ p2)
        {
            if(l == null)
            {
                return false;
            }
            //去除线段上的点（非顶点）
            if (plt.PointInStraightLineSide(l, p1) == 0 || plt.PointInStraightLineSide(l, p2) == 0)
            {
                return false;
            }
            if ((plt.PointInStraightLineSide(l, p1) == 1 && plt.PointInStraightLineSide(l, p2) == 1) 
                || (plt.PointInStraightLineSide(l, p1) == -1 && plt.PointInStraightLineSide(l, p2) == -1))
            {
                return true;
            }
            return false;
        }
    }
}