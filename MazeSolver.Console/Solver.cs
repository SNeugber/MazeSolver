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

        internal Image Execute(Image inputImage)
        {
            var inputBitmap = new Bitmap(inputImage);
            InitImageCentre(inputBitmap);
            InitMazeMap(inputBitmap);
            Node target = RunDijkstra();
            if (target == null) System.Console.WriteLine("No target found!");
            else System.Console.WriteLine("Found path with cost: " + target.distance);
            return null;
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

            Node start = mazeMap[image.Width / 2, 0];
            start.distance = 0;
            unvisited.UpdatePriority(start, 0);

        }

        private Node RunDijkstra()
        {
            while (unvisited.Count != 0)
            {
                Node current = unvisited.Dequeue();
                if (current.position.X == centre.X && current.position.Y == centre.Y)
                {
                    return current;
                }

                foreach (Node neighbour in getNeighbours(current))
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

            return null; // No path found
        }

        private List<Node> getNeighbours(Node current)
        {
            List<Node> neighbours = new List<Node>();
            Node neighbour;

            if (current.position.X > 0 && current.position.Y > 0)
            {
                neighbour = mazeMap[current.position.X - 1, current.position.Y - 1];
                if(neighbour.traverseable)
                    neighbours.Add(neighbour);
            }

            if (current.position.X < mazeMaxX - 1 && current.position.Y > 0)
            {
                neighbour = mazeMap[current.position.X + 1, current.position.Y - 1];
                if (neighbour.traverseable)
                    neighbours.Add(neighbour);
            }

            if (current.position.X < mazeMaxX - 1 && current.position.Y < mazeMaxY - 1)
            {
                neighbour = mazeMap[current.position.X + 1, current.position.Y + 1];
                if (neighbour.traverseable)
                    neighbours.Add(neighbour);
            }


            if (current.position.X > 0 && current.position.Y < mazeMaxY - 1)
            {
                neighbour = mazeMap[current.position.X - 1, current.position.Y + 1];
                if (neighbour.traverseable)
                    neighbours.Add(neighbour);
            }

            return neighbours;
        }

        private int GetPointDistance(Point a, Point b)
        {
            return a.X * b.X + a.Y * b.Y;
        }
    }
}
