using System;
using System.Collections.Generic;
using System.Drawing; 

class Program
{
    static void Main(string[] args)
    {
        // Define outer polygon
        var outerPolygon = new List<Point>
        {
            new Point(-1,1),
            new Point(-1,10),
            new Point(-10,10),
            new Point(-10,1)
        };
        // Define holes
        var hole1 = new List<Point>
        {
            new Point(2,6),
            new Point(2,9),
            new Point(5,9),
            new Point(5,6)
        };
        var hole2 = new List<Point>
        {
            new Point(6,2),
            new Point(6,5),
            new Point(9,5),
            new Point(9,2)
        };

        Console.WriteLine("Orientation of outerPolygon: " + GetPolygonOrientation(outerPolygon));
        Console.WriteLine("Orientation of hole1: " + GetPolygonOrientation(hole1));
        Console.WriteLine("Orientation of hole2: " + GetPolygonOrientation(hole2));
        Console.WriteLine("Point in Polygon Checker");
        Console.WriteLine("Type 'exit' at any time to quit.\n");

        while (true)
        {
            Console.Write("Enter X coordinate: ");
            string xInput = Console.ReadLine();
            if (xInput?.ToLower() == "exit") break;

            Console.Write("Enter Y coordinate: ");
            string yInput = Console.ReadLine();
            if (yInput?.ToLower() == "exit") break;

            if (double.TryParse(xInput, out double x) && double.TryParse(yInput, out double y))
            {
                var point = new Point((int)x, (int)y);
                string result = ClassifyPoint(point, outerPolygon, hole1, hole2);
                Console.WriteLine($"Point ({x},{y}) is: {result}\n");
            }
            else
            {
                Console.WriteLine("Invalid number. Please try again.\n");
            }
        }
    }
    static string GetPolygonOrientation(List<Point> polygon)
    {
        double sum = 0;
        int n = polygon.Count;

        for (int i = 0; i < n; i++)
        {
            Point current = polygon[i];
            Point next = polygon[(i + 1) % n];

            sum += (next.X - current.X) * (next.Y + current.Y);
        }

        return sum < 0 ? "Counter-Clockwise" : "Clockwise";
    }

    static string ClassifyPoint(Point p, List<Point> outerPolygon, List<Point> hole1, List<Point> hole2)
    {
        if (IsPointOnPolygonBoundary(p, outerPolygon)) return "On outer boundary";
        if (IsPointOnPolygonBoundary(p, hole1)) return "On hole1 boundary";
        if (IsPointOnPolygonBoundary(p, hole2)) return "On hole2 boundary";

        if (!IsPointInPolygon(p, outerPolygon)) return "Outside polygon";
        if (IsPointInPolygon(p, hole1) || IsPointInPolygon(p, hole2)) return "Inside a hole";

        return "Inside polygon";
    }
    static bool IsPointOnPolygonBoundary(Point p, List<Point> polygon)
    {
        int n = polygon.Count;

        for (int i = 0; i < n; i++)
        {
            Point a = polygon[i];
            Point b = polygon[(i + 1) % n];

            if (IsPointOnLineSegment(p, a, b))
                return true;
        }

        return false;
    }
    static bool IsPointOnLineSegment(Point p, Point a, Point b)
    {
        // Bounding box check
        if (p.X < Math.Min(a.X, b.X) || p.X > Math.Max(a.X, b.X) ||
            p.Y < Math.Min(a.Y, b.Y) || p.Y > Math.Max(a.Y, b.Y))
            return false;

        // Collinearity check using cross product
        int cross = (b.X - a.X) * (p.Y - a.Y) - (b.Y - a.Y) * (p.X - a.X);
        return cross == 0;
    }
    static bool IsPointInPolygon(Point p, List<Point> polygon)
    {
        int numPoints = polygon.Count;
        bool isInside = false;

        for (int i = 0, j = numPoints - 1; i < numPoints; j = i++)
        {
            var pi = polygon[i];
            var pj = polygon[j];

            bool intersect = ((pi.Y > p.Y) != (pj.Y > p.Y)) &&
                             (p.X < (pj.X - pi.X) * (p.Y - pi.Y) / (double)(pj.Y - pi.Y) + pi.X);

            if (intersect)
                isInside = !isInside;
        }

        return isInside;
    }
}
