using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class Tringles
    {
        public List<List<Point>> curTringles;
        public Tringles()
        {
            curTringles = new List<List<Point>>();
        }
        public void addTringle(Point a, Point b, Point c)
        {
            var tringle = new List<Point>();
            tringle.Add(a); tringle.Add(b); tringle.Add(c);
            curTringles.Add(tringle);
        }
        public void addTringle(List<Point> l)
        {
            var tringle = new List<Point>();
            tringle.Add(l[0]); tringle.Add(l[1]); tringle.Add(l[2]);
            curTringles.Add(tringle);
        }
    }
    public class Incremental : Algorithm
    {

        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            if (points.Count < 3)
            {
                outPoints = points;
                return;
            }
            Tringles tringles = new Tringles();
            List<Point> newPoints = new List<Point>();
            int ex = 0;
            for (int i = 0; i < 3 + ex; i++)
            {
                if (!newPoints.Contains(points[i]))
                    newPoints.Add(points[i]);
                else
                    ex++;
            }

            for (int i = 0; i < points.Count; i++)
            {
                if (newPoints.Contains(points[i]))
                {
                    continue;
                }
                if (IsInsidePolygon(points[i], newPoints) == Enums.PointInPolygon.Outside)
                {
                    var Neighbours = newEdgeNeighbours(newPoints, points[i]);
                    if (Neighbours.Count == 3)
                    {
                        newPoints.Add(points[i]);
                        newPoints = removeInside(newPoints, Neighbours);

                    }
                }

            }
            var l = new List<Point>();
            try
            {
                for (int i = 0; i < 5; i++)
                {
                    l.Add(newPoints[newPoints.Count - 1 - i]);
                }
                for (int i = 0; i < newPoints.Count; i++)
                {
                    if (IsInsidePolygon(newPoints[i], l) == Enums.PointInPolygon.Inside)
                    {
                        newPoints.RemoveAt(i);
                        i--;
                    }
                    else if (IsInsidePolygon(newPoints[i], l) == Enums.PointInPolygon.Inside)
                    {
                        newPoints.RemoveAt(i);
                        i--;
                    }
                }
            }
            catch (Exception)
            {
            }
            outPoints = newPoints;
        }
        public List<Point> newEdgeNibor(Point point, List<Point> polygon)
        {
            List<Line> lines = new List<Line>();
            for (int i = 0; i < polygon.Count - 1; i++)
            {
                lines.Add(new Line(polygon[i], polygon[i + 1]));
            }
            lines.Add(new Line(polygon.Last(), polygon[0]));
            var firstPoint = lines.First().End;
            bool toSecond = false;
            for (int i = 0; i < lines.Count - 1; i++)
            {
                if (HelperMethods.CheckTurn(lines[i], point) != HelperMethods.CheckTurn(lines[i + 1], point))
                {
                    if (toSecond)
                    {
                        return new List<Point>() { lines[i].End, point, firstPoint };
                    }
                    firstPoint = lines[i].End;
                    toSecond = true;
                }
            }
            return null;
        }
        public List<Point> newEdgeNeighbours(List<Point> pts, Point np)
        {
            List<Point> Neighbours = new List<Point> { np };
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
                    else if (HelperMethods.CheckTurn(ab, pts[j]) == Enums.TurnType.Right)
                    {
                        isRight++;
                    }
                    else
                    {
                        var d1 = Math.Sqrt(Math.Pow((ab.Start.X - ab.End.X), 2) + Math.Pow((ab.Start.Y - ab.End.Y), 2));
                        var d2 = Math.Sqrt(Math.Pow((ab.Start.X - pts[j].X), 2) + Math.Pow((ab.Start.Y - pts[j].Y), 2));

                        if (d2 >= d1)
                        {
                            found = false;
                            break;
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
                    Neighbours.Add(pts[i]);
                    if (Neighbours.Count == 3)
                    {
                        break;
                    }
                }
            }
            return Neighbours;
        }
        public List<Point> addNewPoint(List<Point> pts, List<Point> targets)
        {
            int ind1 = pts.IndexOf(targets[1]);
            int ind2 = pts.IndexOf(targets[2]);
            if (ind1 < ind2)
            {
                var tmp = ind1;
                ind1 = ind2;
                ind2 = tmp;

            }
            pts.Insert(ind2, targets[0]);

            for (int i = ind2 + 2; i < ind1; i++)
            {
                pts.RemoveAt(i);
            }
            return pts;
        }
        public Enums.PointInPolygon IsInsidePolygon(Point point, List<Point> polygon)
        {
            List<Line> lines = new List<Line>();
            for (int i = 0; i < polygon.Count - 1; i++)
            {
                lines.Add(new Line(polygon[i], polygon[i + 1]));
            }
            lines.Add(new Line(polygon.Last(), polygon[0]));
            foreach (var line in lines)
            {
                if (HelperMethods.GetVector(line).Equals(Point.Identity)) return (HelperMethods.PointOnSegment(point, line.Start, line.End)) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
                if (HelperMethods.CheckTurn(line, point) == Enums.TurnType.Colinear)
                    return HelperMethods.PointOnSegment(point, line.Start, line.End) ? Enums.PointInPolygon.OnEdge : Enums.PointInPolygon.Outside;
            }
            var InFlag = HelperMethods.CheckTurn(lines[0], point);
            foreach (var line in lines)
            {
                if (HelperMethods.CheckTurn(line, point) != InFlag)
                {
                    return Enums.PointInPolygon.Outside;
                }

            }
            return Enums.PointInPolygon.Inside;
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
                else if (HelperMethods.PointInTriangle(tmpP, a, b, c) == Enums.PointInPolygon.OnEdge)
                {
                    pts.RemoveAt(i);
                }
            }
            return pts;
        }
        public List<Point> removeInside(List<Point> pts, List<Point> target)
        {
            for (int i = 0; i < pts.Count; i++)
            {
                Point tmpP = new Point(pts[i].X, pts[i].Y);

                if (tmpP.Equals(target[0]) || tmpP.Equals(target[1]) || tmpP.Equals(target[2]))
                {
                    continue;
                }
                if (HelperMethods.PointInTriangle(tmpP, target[0], target[1], target[2]) == Enums.PointInPolygon.Inside)
                {
                    pts.RemoveAt(i);
                }
                if (HelperMethods.PointInTriangle(tmpP, target[0], target[1], target[2]) == Enums.PointInPolygon.OnEdge)
                {
                    pts.RemoveAt(i);
                }
            }
            return pts;
        }
        public override string ToString()
        {
            return "Convex Hull - Incremental";
        }
    }
}
