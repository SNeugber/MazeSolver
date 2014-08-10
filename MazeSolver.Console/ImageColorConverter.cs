using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows;


namespace MazeSolver.Console
{
    public class ImageColorConverter
    {
        public static WriteableBitmap ConvertAnyNotWhitePixelsToBlack(WriteableBitmap input)
        {
            int h = input.PixelHeight;
            int w = input.PixelWidth;
            int widthInByte = 4 * w;
            byte[] pixelData = new byte[widthInByte * h];
            input.CopyPixels(pixelData, widthInByte, 0);

            for (int y = 0; y < input.PixelHeight; y++)
            {
                for (int x = 0; x < input.PixelWidth; x++)
                {
                    int index = y * widthInByte + 4 * x;
                    byte red = pixelData[index];
                    byte green = pixelData[index + 1];
                    byte blue = pixelData[index + 2];
                    byte alpha = pixelData[index + 3];
                    if (red != 255 || green != 255 || blue != 255 || alpha != 255)
                    {
                        pixelData[index] = 0;
                        pixelData[index + 1] = 0;
                        pixelData[index + 2] = 0;
                        pixelData[index + 3] = 255;
                    }

                    //byte alpha = pixels[index + 3];
                }
            }

            //for (int i = 0; i < pixelData.Length; i++)
            //{
            //    if (pixelData[i] != 0x00ffffff)
            //        pixelData[i] = 0x00000000;
            //}

            input.WritePixels(new Int32Rect(0, 0, w, h), pixelData, widthInByte, 0);

            //for (int y = 0; y < input.Height; y++)
            //{
            //    for (int x = 0; x < input.Width; x++)
            //    {




            //        Color pixelColor = input.GetPixel(x, y);
            //        if (pixelColor.R != 255 || pixelColor.G != 255 || pixelColor.B != 255)
            //        {
            //            input.SetPixel(x, y, Color.Black);
            //        }
            //    }
            //}
            return input;
        }
    }
}
