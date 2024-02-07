using System.ComponentModel.Design;
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
        /// 获取凸壳直径
        /// </summary>
        /// <param name="boundary">凸壳边界</param>
        /// <returns></returns>
        public Line GetConvexShellDiameter(List<Line> boundary)
        {
            List<XYZ> midpoints = new List<XYZ>();
            foreach (Line line in boundary)
            {
                midpoints.Add(plt.GetLineMiddlePoint(line));
            }
            //获取顶点
            List<XYZ> vertexs = plt.AreaLinesToPoints(boundary);
            //每个顶点处理的最大距离线
            List<Line> maxLines = new List<Line>();
            for(int i = 0; i < vertexs.Count; i++)
            {
                //最大距离的顶点序号
                int maxIndex = 0;
                //辅助判断
                int assistIndex = 0;
                for (int k = 0;k < vertexs.Count - 2;k++)
                {
                    //当前计算中点序号,下一个顶点序号
                    int midIndex = (i + k + 1 + vertexs.Count) % vertexs.Count;
                    //下下一个顶点序号
                    int index2 = (i + k + 2 + vertexs.Count) % vertexs.Count;
                    XYZ vetor1 = new XYZ(vertexs[midIndex].X - midpoints[midIndex].X, 
                        vertexs[midIndex].Y - midpoints[midIndex].Y, vertexs[midIndex].Z - midpoints[midIndex].Z);
                    XYZ vetor2 = new XYZ(vertexs[index2].X - midpoints[midIndex].X,
                        vertexs[index2].Y - midpoints[midIndex].Y, vertexs[index2].Z - midpoints[midIndex].Z);
                    XYZ vetor0 = new XYZ(vertexs[i].X - midpoints[midIndex].X,
                        vertexs[i].Y - midpoints[midIndex].Y, vertexs[i].Z - midpoints[midIndex].Z);
                    //计算一个点积正负也可，角度0-180，计算一个可反推另一个,不可能为0（垂直）
                    if(plt.DotProduct(vetor1,vetor0) > 0 && plt.DotProduct(vetor2, vetor0) < 0)
                    {
                        if(maxIndex == assistIndex)
                        {
                            maxIndex = index2;
                            assistIndex = midIndex;
                        }
                        else
                        {
                            //只有这一种情况
                            if(maxIndex == midIndex)
                            {
                                maxIndex = index2;
                            }
                        }
                    }
                    else
                    {
                        if (maxIndex == assistIndex)
                        {
                            maxIndex = midIndex;
                            assistIndex = index2;
                        }
                        else
                        {
                            //只有这一种情况
                            if (maxIndex == index2)
                            {
                                maxIndex = midIndex;
                            }
                        }
                    }
                }
                maxLines.Add(Line.CreateLine(vertexs[i],vertexs[maxIndex]));
            }
            return maxLines.OrderBy(it => it.Start.DistanceTo(it.End)).Last();
        }
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
        /// 参数flag，0:使用点与线的位置计算，1:使用三点夹角计算
        /// </summary>
        /// <param name="flag">标志</param>
        /// <returns></returns>
        public List<XYZ> GetConcexShell(List<XYZ> points,int flag = 0)
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
                if(flag == 0)
                {
                    if (!IsSameSide(Line.CreateLine(result[result.Count - 2], result.Last()), result[result.Count - 3], ps[i + 1]))
                    {
                        result.Remove(ps[i]);
                    }
                }
                else
                {
                    double angle1 = plt.ThreePointAngle(result[result.Count - 3], result[result.Count - 2], result.Last());
                    double angle2 = plt.ThreePointAngle(result[result.Count - 3], result[result.Count - 2], ps[i + 1]);
                    if(angle1 <= angle2)
                    {
                        result.Remove(ps[i]);
                    }
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