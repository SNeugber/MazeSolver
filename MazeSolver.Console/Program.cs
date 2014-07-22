using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MazeSolver.Console
{

    class Program
    {

        static void Main(string[] args)
        {
            Image inputImage;
            String inputImagePath = args[0];
            String outputImagePath = args[1];
            if (!TryGetInputImageFromFile(inputImagePath, out inputImage) || !IsOutputImageValid(outputImagePath)) return;
            var solver = new Solver();
            var outputImage = solver.Execute(inputImage);

        }

        private static bool TryGetInputImageFromFile(string inputImagePath, out Image inputImage)
        {
            try
            {
                inputImage = Image.FromFile(inputImagePath);
            }
            catch
            {
                inputImage = null;
                return false;
            }

            return true;
        }
        


        private static bool IsOutputImageValid(String imagePath)
        {
       
            try
            {
                var imageFile = new FileInfo(imagePath);
                if (!imageFile.Exists)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}
