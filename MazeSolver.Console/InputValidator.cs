using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeSolver
{
    class InputValidator
    {
        public static bool TryGetInputImageFromFile(string inputImagePath, out Image inputImage)
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



        public static bool IsOutputImageValid(String imagePath)
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
                    if (!imageFile.IsReadOnly)
                    {
                        return true;
                    }
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
