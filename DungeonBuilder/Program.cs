using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonBuilder {
    class Program {
        static int rooms = 5;
        static int gridWidth = 80;
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

            grid = new int[gridWidth, gridHeight];
            random = new Random();
            
            AddPoints();
            DrawGrid();

            Console.ReadLine();
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
