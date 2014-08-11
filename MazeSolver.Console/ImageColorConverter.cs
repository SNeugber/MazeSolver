using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;


namespace MazeSolver.Console
{
    public class ImageColorAccess
    {

        int imageHeight;
        int imageWidth;
        int widthInByte;
        byte[] pixelData;
        double dpiX, dpiY;
        PixelFormat pixForm;
        BitmapPalette bmpPal;

        public ImageColorAccess(WriteableBitmap inputImage)
        {
            imageHeight = inputImage.PixelHeight;
            imageWidth = inputImage.PixelWidth;
            widthInByte = 4 * imageWidth;
            pixelData = new byte[widthInByte * imageHeight];
            inputImage.CopyPixels(pixelData, widthInByte, 0);
            dpiX = inputImage.DpiX;
            dpiY = inputImage.DpiY;
            pixForm = inputImage.Format;
            bmpPal = inputImage.Palette;
        }

        public WriteableBitmap ConvertAnyNotWhitePixelsToBlack()
        {
            for (int y = 0; y < imageHeight; y++)
            {
                for (int x = 0; x < imageWidth; x++)
                {
                    int index = DataIndexFromXY(x, y);
                    if (!IsPixelPureWhite(index))
                        SetPixelColor(index, 0, 0, 0, 255);
                }
            }

            var output = new WriteableBitmap(imageWidth, imageHeight, dpiX, dpiY, pixForm, bmpPal);
            output.WritePixels(new Int32Rect(0, 0, imageWidth, imageHeight), pixelData, widthInByte, 0);

            return output;
        }

        public bool IsPixelPureWhite(int x, int y)
        {
            return IsPixelPureWhite(DataIndexFromXY(x, y));
        }

        private bool IsPixelPureWhite(int index)
        {
            byte red = pixelData[index];
            byte green = pixelData[index + 1];
            byte blue = pixelData[index + 2];
            byte alpha = pixelData[index + 3];
            return red == 255 && green == 255 && blue == 255 && alpha == 255;
        }

        private void SetPixelColor(int index, byte R, byte G, byte B, byte A)
        {
            pixelData[index] = R;
            pixelData[index + 1] = G;
            pixelData[index + 2] = B;
            pixelData[index + 3] = A;
        }

        private int DataIndexFromXY(int x, int y)
        {
            return y * widthInByte + 4 * x;
        }
    }
}
