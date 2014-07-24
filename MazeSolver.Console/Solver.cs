using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;
using Priority_Queue;

namespace MazeSolver.Console
{
    class Solver
    {

        private Node[,] mazeMap;
        private Point centre;
        private HeapPriorityQueue<Node> unvisited;
        private int mazeMaxX, mazeMaxY;
        private Node target;

        internal Image Execute(Image inputImage)
        {
            var inputBitmap = new Bitmap(inputImage);
            InitImageCentre(inputBitmap);
            InitMazeMap(inputBitmap);
            RunDijkstra();
            DrawMazePath(inputBitmap);
            //if (target == null) System.Console.WriteLine("No target found!");
            //else System.Console.WriteLine("Found path with cost: " + target.distance);
            return inputBitmap;
        }

        private void InitImageCentre(Bitmap image)
        {
            centre = new Point((int)(image.Width / 2), (int)(image.Height / 2));
        }

        private void InitMazeMap(Bitmap image)
        {
            mazeMaxX = image.Width;
            mazeMaxY = image.Height;
            mazeMap = new Node[mazeMaxX, mazeMaxY];
            unvisited = new HeapPriorityQueue<Node>(mazeMaxX * mazeMaxY);

            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    bool isTraverseable = pixel.R == 255 && pixel.G == 255 && pixel.B == 255; // Only white pixels
                    mazeMap[i, j] = new Node(new Point(i, j), isTraverseable);
                    if (isTraverseable)
                    {
                        unvisited.Enqueue(mazeMap[i, j], int.MaxValue);
                    }
                }
            }

            Node start = mazeMap[image.Width / 2, image.Height - 1];
            start.distance = 0;
            unvisited.UpdatePriority(start, 0);

        }

        private void RunDijkstra()
        {
            while (unvisited.Count != 0)
            {
                Node current = unvisited.Dequeue();
                if (current.position.X == centre.X && current.position.Y == centre.Y)
                {
                    target = current;
                    System.Console.WriteLine(current.distance);
                    break;
                    //return current;
                }

                current.visited = true;

                foreach (Node neighbour in GetUnvisitedNeighbours(current))
                {
                    int altDistance = current.distance + 1;
                    if (altDistance < neighbour.distance)
                    {
                        neighbour.distance = altDistance;
                        neighbour.previous = current;
                        unvisited.UpdatePriority(neighbour, altDistance);
                    }
                }
            }

            //return null; // No path found
        }

        private void DrawMazePath(Bitmap mazeImage)
        {
            if (target == null)
                return;

            //mazeImage.GetPixel(target.position.X, target.position.Y)
            mazeImage.SetPixel(target.position.X, target.position.Y, Color.Magenta);

            Node previous = target.previous;
            do
            {
                mazeImage.SetPixel(previous.position.X, previous.position.Y, Color.Magenta);
            } while ((previous = previous.previous) != null);
        }

        private List<Node> GetUnvisitedNeighbours(Node current)
        {
            List<Node> neighbours = new List<Node>();

            AddNeighbourAtXYOffset(current, neighbours, 0, 1);
            AddNeighbourAtXYOffset(current, neighbours, 0, -1);

            AddNeighbourAtXYOffset(current, neighbours, 1, 0);
            AddNeighbourAtXYOffset(current, neighbours, 1, 1);
            AddNeighbourAtXYOffset(current, neighbours, 1, -1);

            AddNeighbourAtXYOffset(current, neighbours, -1, 0);
            AddNeighbourAtXYOffset(current, neighbours, -1, 1);
            AddNeighbourAtXYOffset(current, neighbours, -1, -1);

            return neighbours;
        }

        private void AddNeighbourAtXYOffset(Node current, List<Node> neighbours, int XOffset, int YOffset)
        {
            Node neighbour;
            bool validPos =
                current.position.X + XOffset > 0 &&
                current.position.X + XOffset < mazeMaxX &&
                current.position.Y + YOffset > 0 &&
                current.position.Y + YOffset < mazeMaxY;

            if (validPos)
            {
                neighbour = mazeMap[current.position.X + XOffset, current.position.Y + YOffset];
                if (neighbour.traverseable && !neighbour.visited)
                    neighbours.Add(neighbour);
            }
        }

        private int GetPointDistance(Point a, Point b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
    }
}
