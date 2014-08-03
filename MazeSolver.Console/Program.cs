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
            String inputImagePath = args[0];
            String outputImagePath = args[1];
            SolverController.TrySolveAndSaveToFile(inputImagePath, outputImagePath);
        }
    }
}
