using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing;
using MazeSolver;

namespace MazeSolver.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string inputFileName;
        string tempFileName;
        string outputFileName;
        IndeterminateProgressBar progressBar;
        private SynchronizationContext uiContext = SynchronizationContext.Current;
        private Point mazeStart;
        private Point mazeEnd;
        private bool mazeStartSet = false;
        private bool mazeEndSet = false;
        private ImageColorAccess imageColor;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            SolveMazeButton.IsEnabled = false;
            SetMazeStartGoalButton.IsEnabled = false;
            mazeImage.Source = null;
        }

        private void SelectInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            SolveMazeButton.IsEnabled = false;
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.FileName = "InputFile";
            dialog.DefaultExt = ".jpeg";
            dialog.Filter = "JPEG Files (*.jpeg)|*.jpeg|JPG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|GIF Files (*.gif)|*.gif";

            Nullable<bool> result = dialog.ShowDialog();

            if (result.HasValue && result.Value)
            {
                inputFileName = dialog.FileName;
                BitmapImage inputImage = new BitmapImage();
                inputImage.CacheOption = BitmapCacheOption.OnLoad;
                inputImage.BeginInit();
                inputImage.UriSource = new Uri(inputFileName);
                inputImage.EndInit();
                imageColor = new ImageColorAccess(new WriteableBitmap(inputImage));
                //ImageColorConverter
                //MazeSolver.Console.ImageColorConverter.ConvertAnyNotWhitePixelsToBlack(inputImage);
                WriteableBitmap blackAndWhiteImage = imageColor.ConvertAnyNotWhitePixelsToBlack();
                mazeImage.Source = blackAndWhiteImage;
                //mazeImage.Source = inputImage;
                SetMazeStartGoalButton.IsEnabled = true;
            }
        }

        private void SaveOutputFileButton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = "SolvedFile";
            dialog.DefaultExt = ".jpg";
            dialog.Filter = "JPG Files (*.jpg)|*.jpg";

            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string saveFileName = dialog.FileName;
                File.Copy(tempFileName, saveFileName, true);
            }
        }

        private void SetMazeStartGoalButton_Click(object sender, RoutedEventArgs e)
        {
            mazeStartSet = false;
            mazeEndSet = false;
            SolveMazeButton.IsEnabled = false;
            ShowTextPopup("Click on the image to set the start and end points", 1000);
        }

        private void MazeImage_MouseDown(object sender, MouseEventArgs e)
        {

            Point ImagePosition = ImagePointFromClickInImageFrame(e.GetPosition(mazeImage));
            System.Diagnostics.Debug.WriteLine(
                "Mouse position: " +
                ImagePosition.X + ", " + ImagePosition.Y);

            if (!mazeStartSet && imageColor.IsPixelPureWhite((int)ImagePosition.X, (int)ImagePosition.Y))
            {
                mazeStartSet = true;
                mazeStart = ImagePosition;
                ShowTextPopup("Start Set", 500);
            }
            else if (!mazeEndSet && imageColor.IsPixelPureWhite((int)ImagePosition.X, (int)ImagePosition.Y))
            {

                mazeEndSet = true;
                mazeEnd = ImagePosition;
                SolveMazeButton.IsEnabled = true;
                ShowTextPopup("End set", 500);
            }
            else
            {
                // TODO: Show warning
            }
            //System.Diagnostics.Trace.WriteLine(
            //    "Mouse Down on image at position: " +
            //    MousePosition.X + ", " + MousePosition.Y);
        }

        private Point ImagePointFromClickInImageFrame(Point mousePosition)
        {
            int x = (int)(((BitmapSource)mazeImage.Source).PixelWidth * mousePosition.X / mazeImage.ActualWidth);
            int y = (int)(((BitmapSource)mazeImage.Source).PixelHeight * mousePosition.Y / mazeImage.ActualHeight);
            return new Point(x, y);
        }

        private void SolveMazeButton_Click(object sender, RoutedEventArgs e)
        {
            // This should never happen, because the button should only be enabled
            // after both tart & end have been specified by the user
            if (!(mazeStartSet && mazeEndSet)) return;
            tempFileName = Directory.GetCurrentDirectory() + @"\temp.jpg";
            try
            {
                new Thread(() =>
                {
                    //Thread.CurrentThread.IsBackground = true;
                    var solver = new MazeSolver.Program();
                    solver.Solved += new MazeSolver.Program.MazeSolvedHandler(MazeSolved);
                    try
                    {
                        solver.Run(inputFileName, tempFileName, mazeStart, mazeEnd);
                    }
                    catch (PathNotFoundException pnfe)
                    {
                        uiContext.Post(new SendOrPostCallback(new Action<object>(o =>
                        {
                            if (progressBar != null)
                            {
                                progressBar.Close();
                                progressBar = null;
                            }
                        })), null);
                        ShowTextPopup("Could not solve the maze!", 1000);
                    }
                }).Start();
                progressBar = new IndeterminateProgressBar();
                progressBar.Owner = this;
                progressBar.ShowDialog();

            }
            catch (ArgumentException exception)
            {
                //TODO Add exception display 
            }

        }

        private void MazeSolved(Program p, EventArgs e)
        {
            // Gets called from background worker thread, but need to access image in main UI thread
            uiContext.Post(new SendOrPostCallback(new Action<object>(o =>
            {
                BitmapImage outputImage = new BitmapImage();
                outputImage.CacheOption = BitmapCacheOption.OnLoad;
                outputImage.BeginInit();
                outputImage.UriSource = new Uri(tempFileName);
                outputImage.EndInit();
                mazeImage.Source = outputImage;
                progressBar.Close();
                progressBar = null;
            })), null);
        }

        private void ShowTextPopup(string text, int durationInMilliSec)
        {
            uiContext.Post(new SendOrPostCallback(new Action<object>(o =>
            {
                var textPopup = new TextPopup();
                textPopup.SetText(text);
                textPopup.Owner = this;
                textPopup.Show();
                Thread.Sleep(durationInMilliSec);
                textPopup.Close();
            })), null);
        }
    }
}
//