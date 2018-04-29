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

namespace Minesweeper.MyView
{
    /// <summary>
    /// Interaction logic for ContinueWindow.xaml
    /// </summary>
    public partial class ContinueWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueWindow" /> class
        /// </summary>
        public ContinueWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Enables button when some item is selected
        /// </summary>
        /// <param name="sender">Sender object param</param>
        /// <param name="e">EventArgs param</param>
        private void ListView_SelectionChanged(object sender, EventArgs e)
        {
            if (lst.SelectedIndex != -1)
            {                
                btn.IsEnabled = true;
            }
        }
    }
}
