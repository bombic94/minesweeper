using Minesweeper.MyView;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minesweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Grid grid1;
        public MainWindow()
        {
            InitializeComponent();

        }

     /*   private void MenuItem_Start(object sender, RoutedEventArgs e)
        {
            MenuItem item = e.Source as MenuItem;
            int oblast_id = 0;
            switch (item.Name)
            {
                case "Beginner":
                    {
                        oblast_id = DBHelper.AddGame(1);
                        break;
                    }
                case "Advanced":
                    {
                        oblast_id = DBHelper.AddGame(2);
                        break;
                    }
                case "Expert":
                    {
                        oblast_id = DBHelper.AddGame(3);
                        break;
                    }
            }
            createGrid(oblast_id);
            refreshGame(oblast_id);

            GameGrid = grid1;
        }*/

        private void createGrid(int oblast_id)
        {
            grid1 = new Grid();
            var level = DBHelper.getLevelInfo(oblast_id);
            for (int i = 0; i < level.sirka; i++)
            {
                RowDefinition rowdef = new RowDefinition();
                rowdef.Height = new System.Windows.GridLength(25);
                grid1.RowDefinitions.Add(rowdef);
            }

            for (int i = 0; i < level.sirka; i++)
            {
                ColumnDefinition coldef = new ColumnDefinition();
                coldef.Width = new System.Windows.GridLength(25);
                grid1.ColumnDefinitions.Add(coldef);
            }
        }

        private void refreshGame(int oblast_id)
        {
            List<POLE> listPoli = DBHelper.getListPoli(oblast_id);
            List<MINA> listMin = DBHelper.getListMin(oblast_id);
            var level = DBHelper.getLevelInfo(oblast_id);

            foreach (POLE p in listPoli)
            {
                Button b = new Button();
                grid1.Children.Add(b);
                Grid.SetRow(b, p.souradnice_y);
                Grid.SetColumn(b, p.souradnice_x);
            }
        }
    }
}
