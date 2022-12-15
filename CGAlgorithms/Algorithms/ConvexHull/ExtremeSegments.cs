using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class ExtremeSegments : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> Result_Points, ref List<Line> Result_Lines, ref List<Polygon> outPolygons)
        {
            // Making a List of Visited Points
            bool[] VisitedPoints = new bool[points.Count];
            // Initializing Them In The Begining with False
            for (int i = 0; i < points.Count; i++)
            {
                VisitedPoints[i] = false;
            }
            // Sorting Points based on X and if X is Equal then based on Y
            points.Sort(Comparable);
            // Making a List of Unique(Unequal) Points 
            List<Point> UniquePoints = new List<Point>();
            // Begining with The First Point
            UniquePoints.Add(points[0]);
            // Looping over The Points and Filling The Unique Points 
            for (int i = 1; i < points.Count; i++)
            {
                // if The Points are Equal then pass
                if (points[i].Equals(points[i - 1]))
                {
                    continue;
                }
                UniquePoints.Add(points[i]);
            }
            // The Points are The Unique Points Now
            points = UniquePoints;
            // Comparing One Point with The Rest 
            for (int i = 0; i < points.Count; i++)
            {
                for (int j = 0; j < points.Count; j++)
                {
                    // If The Points are Equal then Pass
                    if (points[i].Equals(points[j]))
                    {
                        continue;
                    }
                    // Making Lines of Unequal Points 
                    Line Line_Made = new Line(points[i], points[j]);
                    // Counting Directions of The Points
                    int Left_Count = 0, Right_Count = 0, Colinear_Count = 0;
                    // Incrementing The Counts of Every Direction
                    for (int k = 0; k < points.Count; k++)
                    {
                        if (points[i].Equals(points[k]) || points[k].Equals(points[j]))
                        {
                            continue;
                        }
                        if (HelperMethods.CheckTurn(Line_Made, points[k]) == Enums.TurnType.Left)
                        {
                            Left_Count++;
                        }
                        else if (HelperMethods.CheckTurn(Line_Made, points[k]) == Enums.TurnType.Right)
                        {
                            Right_Count++;
                        }
                        // Checking if It's Colinear or on The Same Extension of Points
                        else if (HelperMethods.CheckTurn(Line_Made, points[k]) == Enums.TurnType.Colinear && points[k].X >= Line_Made.Start.X && points[k].X <= Line_Made.End.X && points[k].Y >= Line_Made.Start.Y && points[k].Y <= Line_Made.End.Y)
                        {
                            Colinear_Count++;
                        }
                    }
                    // Checking Extreme Segmants and Assigning Result Lines
                    if (Left_Count == points.Count - Colinear_Count - 2 || Right_Count == points.Count - Colinear_Count - 2)
                    {
                        Result_Lines.Add(Line_Made);
                        // Assigning Visited Points
                        for (int ii = 0; ii < Result_Points.Count; ii++)
                        {
                            if (points[i].X == Result_Points[ii].X && points[i].Y == Result_Points[ii].Y)
                            {
                                VisitedPoints[i] = true;
                                break;
                            }
                        }
                        // Assigning Result Points (Points that make Extreme Segmants)
                        if (VisitedPoints[i] != true)
                        {
                            Result_Points.Add(points[i]);
                            VisitedPoints[i] = true;
                        }
                        if (VisitedPoints[j] != true)
                        {
                            Result_Points.Add(points[j]);
                            VisitedPoints[j] = true;
                        }
                    }
                }
            }
            // Checking Points
            if (Result_Points.Count == 0 && points.Count > 0)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    for (int j = 0; j < points.Count; j++)
                    {
                        // Checking Equality of Points
                        if (points[i].Equals(points[j]))
                        {
                            continue;
                        }
                        // Making Lines of Points 
                        Line Line_Made = new Line(points[i], points[j]);
                        // Checking Colinear Points
                        int Colinear_Count = 0;
                        for (int k = 0; k < points.Count; k++)
                        {
                            if (j == k || k == i)
                                continue;
                            if (HelperMethods.CheckTurn(Line_Made, points[k]) == Enums.TurnType.Colinear)
                            {
                                if(points[k].X >= Line_Made.Start.X && points[k].X <= Line_Made.End.X)
                                {
                                    if(points[k].Y >= Line_Made.Start.Y)
                                    {
                                        if(points[k].Y <= Line_Made.End.Y)
                                        {
                                            Colinear_Count++;
                                        }
                                    }
                                }
                                    
                                
                            }
                        }
                        // Checking The Number of Colinear Points,Assigning Extreme Segmants and Extreme Points
                        if (Colinear_Count == points.Count - 2)
                        {

                            Result_Lines.Add(Line_Made);
                            for (int ii = 0; ii < Result_Points.Count; ii++)
                            {
                                if (points[i].X == Result_Points[ii].X && points[i].Y == Result_Points[ii].Y)
                                {
                                    VisitedPoints[i] = true;
                                    break;
                                }
                            }
                            if (VisitedPoints[i] == true)
                                continue;
                            Result_Points.Add(points[i]);
                            Result_Points.Add(points[j]);
                            VisitedPoints[i] = true;
                            VisitedPoints[j] = true;
                        }


                    }

                }
            }


            if (Result_Points.Count == 0 && points.Count > 0)
                Result_Points.Add(points[0]);
        }
        // Comparing Points based on X and Y
        int Comparable(Point a, Point b)
        {
            if (a.X == b.X)
            {
                return a.Y.CompareTo(b.Y);
            }
            else
            {
                return a.X.CompareTo(b.X);
            }
        }
        public override string ToString()
        {
            return "Convex Hull - Extreme Segments";
        }
    }
}