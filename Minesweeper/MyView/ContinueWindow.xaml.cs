using Minesweeper.MyViewModel;
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
        public ContinueWindow()
        {
            InitializeComponent();
        }

        private void ListView_SelectionChanged(object sender, EventArgs e)
        {
            if (lst.SelectedIndex != -1)
            {                
                btn.IsEnabled = true;
            }
        }
    }
}
