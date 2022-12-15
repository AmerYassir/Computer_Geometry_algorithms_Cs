using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class JarvisMarch : Algorithm
    {
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // All The Work is done in Counter Clock Wise
            // If The Points List is Empty, Return Nothing
            if (points.Count == 0)
            {
                return;
            }
            // Sorting based on Coordinates to catch the case of many Similar Points
            points.Sort(Comparable);
            // Finding Minimum Point
            int MinPoint = FindingMinIdx(points);
            // Beginning with The Minimum Point 
            Point Current_Point = points[MinPoint];
            // Getting Index of Minimum Index
            int Current_Point_Index = MinPoint;
            // Initializing Convex Hull Points
            List<Point> ConvexHull = new List<Point>();
            // Beginning with The First Point
            ConvexHull.Add(Current_Point);
            while (true)
            {
                
                // Getting Next Point Index
                int Next_Point_Index = (Current_Point_Index + 1) % points.Count;
                while (Compare_Points(Current_Point, points[Next_Point_Index]) && Next_Point_Index != Current_Point_Index)
                    Next_Point_Index = (Next_Point_Index + 1) % points.Count;
                if (Next_Point_Index == Current_Point_Index)
                    break;
                // Assigning Next Point
                Point Next_Point = points[Next_Point_Index];  
                // Getting Initial Extreme Segmant
                Line Extreme_Segmant = new Line(Current_Point, Next_Point);
                // Looping Over Points
                for (int i = 0; i < points.Count; i++)
                {
                    Point Comp_Point = points[i];
                    // If The Direction on The Right, Assigning Segmant and Proceeding as The Next Point
                    Enums.TurnType Direction = HelperMethods.CheckTurn(Extreme_Segmant, Comp_Point);
                    if (Direction == Enums.TurnType.Right)
                    {
                        Extreme_Segmant.End = Comp_Point;
                        Next_Point_Index = i;
                        Next_Point = Comp_Point;
                    }
                    // in case of Colinear,Getting The Furthest One in The Counter Clock Wise Direction
                    else if (Direction == Enums.TurnType.Colinear)
                    {
                        double Current_Distance = Get_Distance(Current_Point, Next_Point);
                        double New_Distance = Get_Distance(Current_Point, Comp_Point);
                        // Checking Furthest Distance
                        if (New_Distance > Current_Distance)
                        {
                            Extreme_Segmant.End = Comp_Point;
                            Next_Point_Index = i;
                            Next_Point = Comp_Point;
                        }
                    }
                }
                // Stopping Condition , In case The Next Point is The Start Point
                if (!(Next_Point_Index != MinPoint))
                {
                    break;
                }

                // Adding Point and Getting into Next Step
                ConvexHull.Add(Next_Point);
                Current_Point = Next_Point;
                Current_Point_Index = Next_Point_Index;
            }
            outPoints = ConvexHull;
            return;
        }
        // A Function made to compare Points 
        public bool Compare_Points(Point P1, Point P2)
        {
            // if Two Points are basically The Same (Or have a very little Difference) then return True else False 
            if (Math.Abs(P1.X - P2.X) <= Constants.Epsilon && Math.Abs(P1.Y - P2.Y) <= Constants.Epsilon)
            {
                return true;
            }
            return false;
        }
        // A Function made to find The Distance and It's Squared to avoid Precision Errors
        public double Get_Distance(Point P1, Point P2)
        {
            double Distance = (P1.X - P2.X) * (P1.X - P2.X) + (P1.Y - P2.Y) * (P1.Y - P2.Y);
            return Distance;
        }
        // A Function made to Find Minimum Point despite The Coordinates
        public int FindingMinIdx(List<Point> points)
        {
            Point MinPoint = points[0];
            int MinIdx = 0;
            for (int i = 0; i < points.Count; i++)
            {
                if (points[i].Y == MinPoint.Y)
                {
                    if (points[i].X < MinPoint.X)
                    {
                        MinPoint = points[i];
                        MinIdx = i;
                    }
                }
                else if (points[i].Y < MinPoint.Y)
                {
                    MinPoint = points[i];
                    MinIdx = i;
                }

            }
            return MinIdx;
        }
        // A Function to Sort Points based on their Coordinates
        public int Comparable(Point P1, Point P2)
        {
            if (P1.X == P2.X)
            {
                if (P1.Y < P2.Y)
                {
                    return -1;
                }
                return 1;
            }
            else if (P1.X < P2.X)
            {
                return -1;
            }
            return 1;
        }

        public override string ToString()
        {
            return "Convex Hull - Jarvis March";
        }
    }
}