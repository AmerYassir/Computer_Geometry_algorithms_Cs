using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremePoints : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            List<bool> flags = new List<bool>();
            for (int i = 0; i < points.Count; i++)
            {
                flags.Add(true);
            }
            for (int i = 0; i < points.Count(); i++)
            {
                if (!flags[i])
                {
                    continue;
                }
                for (int j = 0; j < points.Count(); j++)
                {
                    if (j == i)
                    {
                        continue;
                    }
                    if (!flags[j])
                    {
                        continue;
                    }
                    for (int k = 0; k < points.Count(); k++)
                    {
                        if (!flags[k])
                        {
                            continue;
                        }
                        if (k == i)
                        {
                            continue;
                        }
                        if (j == k)
                        {
                            continue;
                        }
                        for (int z = 0; z < points.Count(); z++)
                        {
                            if (!flags[z])
                            {
                                continue;
                            }
                            if (z == i)
                            {
                                continue;
                            }
                            if (z == j)
                            {
                                continue;
                            }
                            if (z == k)
                            {
                                continue;
                            }
                            var pos = HelperMethods.PointInTriangle(points[z], points[i], points[j], points[k]);
                            if (pos == Enums.PointInPolygon.Inside || pos == Enums.PointInPolygon.OnEdge)
                            {
                                flags[z] = false;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < points.Count; i++)
            {
                if (flags[i])
                {
                    outPoints.Add(points[i]);
                }
            }
        }

        public override string ToString()
        {
            return "Convex Hull - Extreme Points";
        }
    }
}
