using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{

    public class QuickHull : Algorithm
    {
        public List<Point> getBoundries(List<Point> pts)
        {

            double maxx = -100000;
            double maxy = -100000;
            double minx = 100000;
            double miny = 100000;
            int maxxind = 0;
            int maxyind = 0;
            int minxind = 0;
            int minyind = 0;
            for (int i = 0; i < pts.Count; i++)
            {
                if (pts[i].X > maxx)
                {
                    maxxind = i;
                    maxx = pts[i].X;
                }
                if (pts[i].X < minx)
                {
                    minxind = i;
                    minx = pts[i].X;
                }
                if (pts[i].Y > maxy)
                {
                    maxyind = i;
                    maxy = pts[i].Y;
                }
                if (pts[i].Y < miny)
                {
                    minyind = i;
                    miny = pts[i].Y;
                }
            }
            List<Point> tmplist = new List<Point>() { pts[maxxind], pts[maxyind], pts[minxind], pts[minyind] };
            return tmplist;
        }
        public int pointRegion(Point p, List<Line> boundries)
        {
            for (int i = 0; i < boundries.Count; i++)
            {
                if (HelperMethods.CheckTurn(boundries[0], p) == Enums.TurnType.Right)
                {
                    return i;
                }
            }
            return -1;
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count < 4)
            {
                outPoints = points;
                return;
            }
            Tringles tringles = new Tringles();
            List<Point> _4pts = new List<Point>();
            _4pts = getBoundries(points);
            tringles.addTringle(_4pts[0], _4pts[1], _4pts[2]);
            tringles.addTringle(_4pts[1], _4pts[2], _4pts[3]);
            List<Point> outSiders = new List<Point>();
            outSiders = points;
            for (int i = 0; i < tringles.curTringles.Count; i++)
            {
                var tra = tringles.curTringles[i];
                outSiders = removeInside(outSiders, tra[0], tra[1], tra[2]);
            }

            List<Point> extreme = new List<Point>();
            extreme.Add(_4pts[0]);
            for (int i = 0; i < outSiders.Count; i++)
            {
                var newExtreme = calcNewExtreme(outSiders, extreme.Last(), extreme);
                if (newExtreme == null)
                {
                    break;
                }
                extreme.Add(newExtreme);
            }

            outPoints = extreme;
        }

        public Point calcNewExtreme(List<Point> pts, Point np, List<Point> exist)
        {
            for (int i = 0; i < pts.Count; i++)
            {
                Line ab = new Line(np, pts[i]);
                int isLeft = 0;
                int isRight = 0;
                bool found = true;
                for (int j = 0; j < pts.Count; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    if (HelperMethods.CheckTurn(ab, pts[j]) == Enums.TurnType.Left)
                    {
                        isLeft++;
                    }
                    if (HelperMethods.CheckTurn(ab, pts[j]) == Enums.TurnType.Right)
                    {
                        isRight++;
                    }
                    if (HelperMethods.CheckTurn(ab, pts[j]) == Enums.TurnType.Colinear)
                    {

                        var d1 = Math.Sqrt(Math.Pow((ab.Start.X - ab.End.X), 2) + Math.Pow((ab.Start.Y - ab.End.Y), 2));
                        var d2 = Math.Sqrt(Math.Pow((ab.Start.X - pts[j].X), 2) + Math.Pow((ab.Start.Y - pts[j].Y), 2));
                        if (d2 > d1)
                        {
                            found = false;
                        }
                    }
                    if (isLeft != 0 && isRight != 0)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    if (!exist.Contains(pts[i]))
                    {
                        return pts[i];
                    }
                }
            }
            return null;
        }
        public List<Point> calcNewTriangle(List<Point> pts, Point np)
        {
            List<Point> tra = new List<Point> { np };
            for (int i = 0; i < pts.Count - 1; i++)
            {
                Line ab = new Line(np, pts[i]);
                int isLeft = 0;
                int isRight = 0;
                bool found = true;
                for (int j = 0; j < pts.Count - 1; j++)
                {
                    if (i == j)
                    {
                        continue;
                    }
                    if (HelperMethods.CheckTurn(ab, pts[j]) == Enums.TurnType.Left)
                    {
                        isLeft++;
                    }
                    if (HelperMethods.CheckTurn(ab, pts[j]) == Enums.TurnType.Right)
                    {
                        isRight++;
                    }
                    if (HelperMethods.CheckTurn(ab, pts[j]) == Enums.TurnType.Colinear)
                    {

                        var d1 = Math.Sqrt(Math.Pow((ab.Start.X - ab.End.X), 2) + Math.Pow((ab.Start.Y - ab.End.Y), 2));
                        var d2 = Math.Sqrt(Math.Pow((ab.Start.X - pts[j].X), 2) + Math.Pow((ab.Start.Y - pts[j].Y), 2));
                        if (d2 >= d1)
                        {
                            found = false;
                        }
                    }
                    if (isLeft != 0 && isRight != 0)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    tra.Add(pts[i]);
                    if (tra.Count == 3)
                    {
                        break;
                    }
                }
            }
            return tra;
        }
        public List<Point> removeInside(List<Point> pts, Point a, Point b, Point c)
        {
            for (int i = 0; i < pts.Count; i++)
            {
                Point tmpP = new Point(pts[i].X, pts[i].Y);

                if (tmpP.Equals(a) || tmpP.Equals(b) || tmpP.Equals(c))
                {
                    continue;
                }
                if (HelperMethods.PointInTriangle(tmpP, a, b, c) == Enums.PointInPolygon.Inside)
                {
                    pts.RemoveAt(i);
                }
                if (HelperMethods.PointInTriangle(tmpP, a, b, c) == Enums.PointInPolygon.OnEdge)
                {
                    pts.RemoveAt(i);
                }
            }
            return pts;
        }
        public override string ToString()
        {
            return "Convex Hull - Quick Hull";
        }
    }
}
