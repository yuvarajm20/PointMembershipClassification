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
            new Point(1,1),
            new Point(1,10),
            new Point(10,10),
            new Point(10,1)
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

    static string ClassifyPoint(Point p, List<Point> outerPolygon, List<Point> hole1, List<Point> hole2)
    {
        if (!IsPointInPolygon(p, outerPolygon)) return "Outside polygon";
        if (IsPointInPolygon(p, hole1) || IsPointInPolygon(p, hole2)) return "Inside a hole";
        return "Inside polygon";
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
