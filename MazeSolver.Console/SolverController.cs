using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MazeSolver.Console
{
    //Bad Name I know. Feel free to rename
    public class SolverController
    {
        public event MazeSolvedHandler Solved;
        public EventArgs e = null;
        public delegate void MazeSolvedHandler(SolverController m, EventArgs e);

        public void TrySolveAndSaveToFile(string inputImageFilePath, string outputImageFilePath, System.Windows.Point start, System.Windows.Point end)
        {
            Image inputImage;
            if (!InputValidator.TryGetInputImageFromFile(inputImageFilePath, out inputImage)) throw new ArgumentException("Input Image Loading failed. Check that file is a valid image");
            //TODO Add further validation information
            if (!InputValidator.IsOutputImageValid(outputImageFilePath)) throw new ArgumentException("Output Image File path check failed. ");
            try
            {
                Image outputImage = new Solver().Execute(inputImage, start, end);
                outputImage.Save(outputImageFilePath);
            }
            catch (PathNotFoundException pnfe)
            {
                System.Diagnostics.Debug.WriteLine("NO PATH FOUND");
                inputImage.Save(outputImageFilePath);
            }
            if (Solved != null)
                Solved(this, e);
        }
    }
}
