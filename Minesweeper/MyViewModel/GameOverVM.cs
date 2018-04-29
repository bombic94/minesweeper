using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.MyViewModel
{
    public class GameOverVM
    {
        /// <summary>
        /// Collection of lost games
        /// </summary>
        public ObservableCollection<HRA> LostGames { get; set; }

        /// <summary>
        /// Collection of won games
        /// </summary>
        public ObservableCollection<HRA> WonGames { get; set; }

        public GameOverVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                LostGames = new ObservableCollection<HRA>();
                WonGames = new ObservableCollection<HRA>();
            }
            else
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            LostGames = new ObservableCollection<HRA>(
                DBHelper.LostGames()
            );
            WonGames = new ObservableCollection<HRA>(
                DBHelper.WonGames()
            );
        }
    }
}
