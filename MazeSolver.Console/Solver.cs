using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Drawing;

namespace MazeSolver.Console
{
    class Solver
    {

        int[,] mazeMap;

        internal Image Execute(Image inputImage)
        {
            var inputBitmap = new Bitmap(inputImage);
            setMazeMap(inputBitmap);
            return null;
        }

        private void setMazeMap(Bitmap image)
        {
            mazeMap = new int[image.Width, image.Height];

            //Enumerable.Range(0, image.Width)
            //    .Zip(Enumerable.Range(0, image.Height), (x, y) => new { x, y })
            //    .Select(coords => new { X = coords.x, Y = coords.y, Result = PixelTest(image.GetPixel(coords.x,coords.y)) })
            //    .ToList()
            //    .ForEach(pixel => mazeMap[pixel.X,pixel.Y] = pixel.Result);

            var centre = new Point((int)(image.Width / 2), (int)(image.Height / 2));
                
            for (int i = 0; i < image.Width; i++)
            {
                for (int j = 0; j < image.Height; j++)
                {
                    Color pixel = image.GetPixel(i, j);
                    mazeMap[i, j] = GetDistanceToCenter(pixel, new Point(i,j), centre);
                }
            }
            
        }

        private static int GetDistanceToCenter(Color pixel, Point xy, Point centre)
        {
            if (pixel.R == 255 && pixel.G == 255 && pixel.B == 255) return xy.X * centre.X + xy.Y * centre.Y;
            return -1;
        }
    }
}
