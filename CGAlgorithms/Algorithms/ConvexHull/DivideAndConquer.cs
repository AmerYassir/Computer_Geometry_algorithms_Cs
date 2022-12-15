using CGUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CGAlgorithms.Algorithms.ConvexHull
{
    public class DivideAndConquer : Algorithm
    {

        public List<Point> Merging_Function(List<Point> Points_List1, List<Point> Points_List2)
        {
            // Initailizing Two Lists of Points
            List<Point> P1 = new List<Point>();
            List<Point> P2 = new List<Point>();
            // Adding Points of The First Convex to P1
            for (int i = 0; i < Points_List1.Count; ++i)
            {
                if (P1.Contains(Points_List1[i]) == false)

                    P1.Add(Points_List1[i]);
            }
            // Adding Points of The Second Convex to P2 
            for (int i = 0; i < Points_List2.Count; ++i)
            {
                if (P2.Contains(Points_List2[i]) == false)
                {
                    P2.Add(Points_List2[i]);
                }
            }
            // Assigning Number of Points  of Each One and Indices
            int Num1 = P1.Count;
            int Num2 = P2.Count;
            int Index1 = 0;
            int Index2 = 0;
            // Getting Maximum Point of Lists by X then by Y of First List to get Upper Tangent of The First List
            for (int i = 1; i < Num1; i++)
            {
                if (P1[i].X > P1[Index1].X)
                {
                    Index1 = i;
                }
                else if (P1[i].X == P1[Index1].X)
                {
                    if (P1[i].Y > P1[Index1].Y)
                        Index1 = i;
                }
            }
            // Getting Maximum Point of Lists by X then by Y of Second List to get Upper Tangent of The Second List
            for (int j = 1; j < Num2; j++)
            {
                if (P2[j].X < P2[Index2].X)
                {
                    Index2 = j;
                }
                else if (P2[j].X == P2[Index2].X)
                {
                    if (P2[j].Y < P2[Index2].Y)
                        Index2 = j;
                }
            }
            // Upper Tangent Part
            int Upper1 = Index1;
            int Upper2 = Index2;
            bool Found_Flag = true;
            do
            {
                Found_Flag = true;
                // If On The Right Proceed to The Next Upper and Flag is False
                while (HelperMethods.CheckTurn(new Line(P2[Upper2].X, P2[Upper2].Y, P1[Upper1].X, P1[Upper1].Y), P1[(Upper1 + 1) % Num1]) == Enums.TurnType.Right)
                {
                    Upper1 = (Upper1 + 1) % Num1;
                    Found_Flag = false;
                }
                // If Collinear procced  with The First Upper
                if (Found_Flag == true && (HelperMethods.CheckTurn(new Line(P2[Upper2].X, P2[Upper2].Y, P1[Upper1].X, P1[Upper1].Y), P1[(Upper1 + 1) % Num1]) == Enums.TurnType.Colinear))
                {
                    Upper1 = (Upper1 + 1) % Num1;
                }
                // If Left Proceed with The Second Upper and Flag is False
                while (HelperMethods.CheckTurn(new Line(P1[Upper1].X, P1[Upper1].Y, P2[Upper2].X, P2[Upper2].Y), P2[(Num2 + Upper2 - 1) % Num2]) == Enums.TurnType.Left)
                {
                    Upper2 = (Num2 + Upper2 - 1) % Num2;
                    Found_Flag = false;
                }
                // If Colinear Proceed with The Next Upper 2
                if (Found_Flag == true && (HelperMethods.CheckTurn(new Line(P1[Upper1].X, P1[Upper1].Y, P2[Upper2].X, P2[Upper2].Y), P2[(Upper2 + Num2 - 1) % Num2]) == Enums.TurnType.Colinear))
                {
                    Upper2 = (Upper2 + Num2 - 1) % Num2;
                }
            } while (Found_Flag == false);
            // Assigning Lowers to Find Lower Tangent 
            int Lower1 = Index1;
            int Lower2 = Index2;
            Found_Flag = true;
            //lower tangent 
            // The Same as The Above
            do
            {
                Found_Flag = true;
                while (HelperMethods.CheckTurn(new Line(P2[Lower2].X, P2[Lower2].Y, P1[Lower1].X, P1[Lower1].Y), P1[(Lower1 + Num1 - 1) % Num1]) == Enums.TurnType.Left)
                {
                    Lower1 = (Lower1 + Num1 - 1) % Num1;
                    Found_Flag = false;
                }
                if (Found_Flag == true && (HelperMethods.CheckTurn(new Line(P2[Lower2].X, P2[Lower2].Y, P1[Lower1].X, P1[Lower1].Y), P1[(Lower1 + Num1 - 1) % Num1]) == Enums.TurnType.Colinear))
                {
                    Lower1 = (Lower1 + Num1 - 1) % Num1;
                }
                while (HelperMethods.CheckTurn(new Line(P1[Lower1].X, P1[Lower1].Y, P2[Lower2].X, P2[Lower2].Y), P2[(Lower2 + 1) % Num2]) == Enums.TurnType.Right)
                {
                    Lower2 = (Lower2 + 1) % Num2;
                    Found_Flag = false;
                }
                if (Found_Flag == true && (HelperMethods.CheckTurn(new Line(P1[Lower1].X, P1[Lower1].Y, P2[Lower2].X, P2[Lower2].Y), P2[(Lower2 + 1) % Num2]) == Enums.TurnType.Colinear))
                {
                    Lower2 = (Lower2 + 1) % Num2;
                }
            } while (Found_Flag == false);
            // Assigning Final Points to be Added and Sent 
            // First Upper assigning Points
            List<Point> out_points = new List<Point>();
            int Index = Upper1;
            if (out_points.Contains(P1[Upper1]) == false)
            {
                out_points.Add(P1[Upper1]);
            }
            // First Lower Assigning Points
            while (!(Index == Lower1))
            {
                Index = (Index + 1) % Num1;
                if (out_points.Contains(P1[Index]) == false)
                {
                    out_points.Add(P1[Index]);

                }
            }
            // Second Lower assigning Points 
            Index = Lower2;
            if (out_points.Contains(P2[Lower2]) == false)
            {
                out_points.Add(P2[Lower2]);
            }
            while (!(Index == Upper2))
            {
                Index = (Index + 1) % Num2;
                if (out_points.Contains(P2[Index]) == false)
                {
                    out_points.Add(P2[Index]);
                }
            }
            return out_points;
        }
        // A Function to divide Points to Leftmost and Rightmost
        public List<Point> Dividing_Points(List<Point> Point_List)
        {
            // If List consists of One then send it as it is
            if (Point_List.Count <= 1)
            {
                return Point_List;
            }
            // Making Two Lists of Left_Most and Right_Most Points
            List<Point> Left_Most_Points = new List<Point>();
            List<Point> Right_Most_Points = new List<Point>();
            // Getting The First Half on The Left
            for (int i = 0; i < (int)(0.5 * Point_List.Count); i++)
            {
                Left_Most_Points.Add(Point_List[i]);
            }
            // Getting The Second Half on The Right
            for (int i = (int)(0.5 * Point_List.Count); i < Point_List.Count; i++)
            {
                Right_Most_Points.Add(Point_List[i]);
            }
            // Recursivly Going Through Points to Assign them
            List<Point> New_Left_Points = Dividing_Points(Left_Most_Points);
            List<Point> New_Right_Points = Dividing_Points(Right_Most_Points);
            // Sending Merged Points to Determing Convex Hull Points
            return Merging_Function(New_Left_Points, New_Right_Points);
        }
        public override void Run(List<Point> points, List<Line> lines, List<Polygon> polygons, ref List<Point> outPoints, ref List<Line> outLines, ref List<Polygon> outPolygons)
        {
            // Sorting The Points by Their X Coordinates then by Their Y Coordinate
            points = points.OrderBy(Point => Point.X).ThenBy(Point => Point.Y).ToList();
            // Dividing The Points into Leftmost,Rightmost and Merging Them
            List<Point> New_Points = Dividing_Points(points);
            // Initializing New Points
            outPoints = new List<Point>();
            // Assigning Result Points (Convex Hull Points)
            for (int i = 0; i < New_Points.Count; ++i)
            {
                if (outPoints.Contains(New_Points[i]) == false)
                {
                    outPoints.Add(New_Points[i]);
                }
            }
        }
        public override string ToString()
        {
            return "Convex Hull - Divide & Conquer";
        }

    }
}