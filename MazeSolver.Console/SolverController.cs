using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver.Console
{
    //Bad Name I know. Feel free to rename
    public class SolverController
    {
        public static void TrySolveAndSaveToFile(string inputImageFilePath, string outputImageFilePath)
        {
            Image inputImage;
            if (!InputValidator.TryGetInputImageFromFile(inputImageFilePath, out inputImage)) throw new ArgumentException("Input Image Loading failed. Check that file is a valid image");
            //TODO Add further validation information
            if (!InputValidator.IsOutputImageValid(outputImageFilePath)) throw new ArgumentException("Output Image File path check failed. ");
            var solver = new Solver();
            var outputImage = solver.Execute(inputImage);
            outputImage.Save(outputImageFilePath);
        }
    }
}
