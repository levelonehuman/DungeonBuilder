using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonBuilder {
    class Program {
        static int rooms = 6;
        static int gridWidth = 100;
        static int gridHeight = 48;
        static int minDistance = 5;

        static int maxTries = 100;
        
        static int consoleWidth = (int) (Console.LargestWindowWidth * 0.9);
        static int consoleHeight = (int) (Console.LargestWindowHeight * 0.9);

        static int[,] grid;
        static Random random;

        static List<Point> points;

        static void Main(string[] args) {
            points = new List<Point>();
            
            Console.SetWindowSize(consoleWidth, consoleHeight);
            
            random = new Random();            
            
            ConsoleKeyInfo input;
            do {
                Console.WriteLine("Press 'G' to generate a new map. Press 'Q' to quit.");
                input = Console.ReadKey();
                if (input.Key == ConsoleKey.G) {
                    Console.Clear();
                    grid = new int[gridWidth, gridHeight];
                    AddPoints();
                    GrowRooms();
                    DrawGrid();
                }

            } while (input.Key != ConsoleKey.Q);

            
        }

        /// <summary>
        /// Draws each cell in a 2D grid.
        /// </summary>
        private static void DrawGrid() {
            for (int y = 0; y < gridHeight; y++) {
                for (int x = 0; x < gridWidth; x++) {                
                    Console.Write(grid[x, y] == 0 ? " " : grid[x, y].ToString());
                }
                Console.WriteLine();
            }
        }

        /// <summary>
        /// Picks random points for the rooms.
        /// </summary>
        private static void AddPoints() {            
            for (int i = 0; i < rooms; i++) {
                int x = 0;
                int y = 0;
                int tries = 0;

                do {
                    tries++;
                    x = random.Next(0, gridWidth);
                    y = random.Next(0, gridHeight);

                    if (tries > maxTries) {
                        break;
                    }

                } while (!TryAddPoint(x, y));

                grid[x, y] = i + 1;
            }
        }

        /// <summary>
        /// Loop through each of the rooms as long as there's space to grow.
        /// </summary>
        private static void GrowRooms() {
            int gridStep = 0; //How many spaces out we look from the initial point.

            while (points.Count > 0) {
                gridStep++;
                List<Point> pointsToRemove = new List<Point>();
                for (int i = 0; i < points.Count; i++) {                    
                    Point p = points[i];
                    int pointNumber = grid[p.X, p.Y];
                    Dictionary<Point, int> surroundingSpaces = GetSurroundingSpaces(p.X, p.Y, gridStep);

                    if (surroundingSpaces.Count == 0 || surroundingSpaces.Any(s => s.Value != 0 && s.Value != pointNumber)) { //this room has run out of space
                        pointsToRemove.Add(p);
                        continue;
                    }

                    foreach (KeyValuePair<Point, int> space in surroundingSpaces) {
                        grid[space.Key.X, space.Key.Y] = pointNumber;
                    }
                }

                foreach (Point point in pointsToRemove) {
                    points.Remove(point);
                }
            }

        }

        /// <summary>
        /// Checks the grid and returns a Dictionary where the Key is the location and Value is the current value at that location.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="step"></param>
        /// <returns>A dictionary with the Point as the key and the value representing the grid content at that space.</returns>
        private static Dictionary<Point, int> GetSurroundingSpaces(int x, int y, int step) {
            Dictionary<Point, int> spaces = new Dictionary<Point, int>();
            List<Point> possiblePoints = new List<Point>();

            for (int i = x - step; i <= x + step; i++) {
                for (int j = y - step; j <= y + step; j++) {
                    if (i == x && j == y) { //skip the target point
                        continue;
                    }
                    possiblePoints.Add(new Point(i, j));
                }
            }

            foreach (Point point in possiblePoints) {
                if (PointInBounds(point)) {
                    spaces.Add(point, grid[point.X, point.Y]);
                }
            }

            return spaces;
        }

        /// <summary>
        /// Checks whether a given point is within the bounds of the grid.
        /// </summary>
        /// <param name="p">The point to check.</param>
        /// <returns>True if the point is in bounds.</returns>
        private static bool PointInBounds(Point p) {
            return p.X > 0 && p.Y > 0 && p.X < gridWidth && p.Y < gridHeight;
        }

        /// <summary>
        /// Adds a point to the list if it meets minimum distance criteria.
        /// </summary>
        /// <param name="x">The x value of the point.</param>
        /// <param name="y">The y value of the point.</param>
        /// <returns>A boolean indicating success.</returns>
        private static bool TryAddPoint(int x, int y) {
            Point p1 = new Point(x, y);
            for (int p = 0; p < points.Count; p++) {
                Point p2 = points[p];

                int minX = Math.Min(minDistance, gridWidth / rooms);
                int minY = Math.Min(minDistance, gridHeight / rooms);

                if (Math.Abs(p2.X - p1.X) < minX) {
                    return false;
                }
                if (Math.Abs(p2.Y - p1.Y) < minY) {
                    return false;
                }
            }

            points.Add(p1);
            return true;
        }
    }
}
