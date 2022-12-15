using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class GrahamScan : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // Sorting Points 
            points.Sort(CoordinatesComparer);
            // Adding Unique Points
            List<Point> HullPoints = new List<Point>();
            HullPoints.Add(points[0]);
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].Equals(points[i - 1]))
                {
                    continue;
                }
                HullPoints.Add(points[i]);
            }
            points = HullPoints;
            // Determining Minimum Point
            double YMin = double.MaxValue;
            double XMin = 0;
            int Index = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (YMin > points[i].Y)
                {
                    YMin = points[i].Y;
                    XMin = points[i].X;
                    Index = i;
                }
            }
            //Making a Horizontal Line to Measure Angles
            Line HorizontalLine = new Line(new Point(XMin, YMin), new Point(XMin + 1000.0, YMin));
            List<KeyValuePair<double, int>> list = new List<KeyValuePair<double, int>>();
            for (int i = 0; i < points.Count; i++)
            {
                if (i == Index)
                {
                    continue;
                }
                // Identifying Angle between Vectors
                Point Vec1 = VectorOfPoint(HorizontalLine.Start, HorizontalLine.End);
                Point Vec2 = VectorOfPoint(HorizontalLine.Start, points[i]);
                double CrossResult = HelperMethods.CrossProduct(Vec1, Vec2);
                double DotResult = Dot_Product(Vec1, Vec2);
                double Angle = Math.Atan2(CrossResult, DotResult) * (180.00 / Math.PI); ;
                if (Angle < 0)
                {
                    Angle += 360;
                }
                list.Add(new KeyValuePair<double, int>(Angle, i));
            }
            // Sorting The List of KeyValuePair
            list.Sort(KeyComparer);
            Stack<int> L = new Stack<int>();
            L.Push(Index);
            if (list.Count > 0)
            {
                L.Push(list[0].Value);
            }
            // Checking Convex Hull Points
            for (int i = 1; i < points.Count - 1 && L.Count >= 2; i++)
            {
                int Top = L.Peek();
                Point P1 = points[Top];
                L.Pop();
                int SecondTop = L.Peek();
                Point P2 = points[SecondTop];
                L.Push(Top);
                HorizontalLine = new Line(P2, P1);
                if (HelperMethods.CheckTurn(HorizontalLine, points[list[i].Value]) == Enums.TurnType.Left)
                {
                    L.Push(list[i].Value);
                }
                else if (HelperMethods.CheckTurn(HorizontalLine, points[list[i].Value]) == Enums.TurnType.Colinear)
                {
                    L.Pop();
                    L.Push(list[i].Value);
                }
                else
                {
                    L.Pop();
                    i--;
                }
            }
            // Adding Final Points 
            while (L.Count > 0)
            {
                outPoints.Add(points[L.Peek()]);
                L.Pop();
            }
        }
        // Calculating Dot Product
        public double Dot_Product(Point P1, Point P2)
        {
            return P1.X * P2.X + P1.Y * P2.Y;
        }
        // Making a Vector of Two Points
        Point VectorOfPoint(Point a, Point b)
        {
            return new Point(b.X - a.X, b.Y - a.Y);
        }
        // A Function that compares Coordinates
        static int CoordinatesComparer(Point a, Point b)
        {
            if (a.X == b.X) return a.Y.CompareTo(b.Y);
            return a.X.CompareTo(b.X);
        }
        public override string ToString()
        {
            return "Convex Hull - Graham Scan";
        }
        // A Function that Compares Values and Keys of Lists of Pairs
        static int KeyComparer(KeyValuePair<double, int> a, KeyValuePair<double, int> b)
        {
            if (a.Key == b.Key) return a.Value.CompareTo(b.Value);
            return a.Key.CompareTo(b.Key);
        }
    }
}