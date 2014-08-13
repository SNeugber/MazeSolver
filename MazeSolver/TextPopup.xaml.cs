using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MazeSolver.UI
{
    /// <summary>
    /// Interaction logic for IndeterminateProgressBar.xaml
    /// </summary>
    public partial class TextPopup : Window
    {
        public TextPopup()
        {
            InitializeComponent();
            this.WindowStyle = WindowStyle.None;
        }

        public void Show()
        {
            
            base.Show();
            this.Left = Owner.Left + Owner.ActualWidth - this.ActualWidth;
            this.Top = Owner.Top + Owner.ActualHeight - this.ActualHeight;
            
        }

        public void SetText(string text)
        {
            TextContent.Text = text;
        }
    }
}
