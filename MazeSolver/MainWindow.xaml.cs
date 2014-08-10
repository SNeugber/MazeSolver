﻿using System;
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
using MazeSolver.Console;

namespace MazeSolver
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
        private SynchronizationContext _uiContext = SynchronizationContext.Current;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ClearInputFileButton_Click(object sender, RoutedEventArgs e)
        {
            SolveMazeButton.IsEnabled = false;
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
                mazeImage.Source = inputImage;
                SolveMazeButton.IsEnabled = true;
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


        private void SolveMazeButton_Click(object sender, RoutedEventArgs e)
        {
            tempFileName = Directory.GetCurrentDirectory() + @"\temp.jpg";
            try
            {
                // TODO: display a progress bar here
              
                //progressBar.Activate();

                new Thread(() =>
                {
                    //Thread.CurrentThread.IsBackground = true;
                    var solver = new SolverController();
                    solver.Solved += new SolverController.MazeSolvedHandler(MazeSolved);
                    solver.TrySolveAndSaveToFile(inputFileName, tempFileName);
                }).Start();
                progressBar = new IndeterminateProgressBar();
                progressBar.Owner = this;
                progressBar.WindowStyle = WindowStyle.None;
                progressBar.ShowDialog();

            }
            catch (ArgumentException exception)
            {
                //TODO Add exception display 
            }

        }

        private void MazeSolved(SolverController sc, EventArgs e)
        {
            // Gets called from background worker thread, but need to access image in main UI thread
            _uiContext.Post(new SendOrPostCallback(new Action<object>(o =>
            {
                BitmapImage outputImage = new BitmapImage();
                outputImage.CacheOption = BitmapCacheOption.OnLoad;
                outputImage.BeginInit();
                outputImage.UriSource = new Uri(tempFileName);
                outputImage.EndInit();
                mazeImage.Source = outputImage;
                progressBar.Close();
            })), null);
   
            
        }




    }
}
//