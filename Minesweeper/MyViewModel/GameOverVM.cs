using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.MyViewModel
{
    /// <summary>
    /// ViewModel for GameOVerWindow
    /// Shows lists of won and lost games
    /// </summary>
    public class GameOverVM
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameOverVM" /> class
        /// </summary>
        public GameOverVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                this.LostGames = new ObservableCollection<HRA>();
                this.WonGames = new ObservableCollection<HRA>();
            }
            else
            {
                this.Refresh();
            }
        }

        /// <summary>
        /// Gets or sets collection of lost games
        /// </summary>
        public ObservableCollection<HRA> LostGames { get; set; }

        /// <summary>
        /// Gets or sets collection of won games
        /// </summary>
        public ObservableCollection<HRA> WonGames { get; set; }

        /// <summary>
        /// Set lists of won and lost games with data from DB
        /// </summary>
        private void Refresh()
        {
            this.LostGames = new ObservableCollection<HRA>(DBHelper.LostGames());
            this.WonGames = new ObservableCollection<HRA>(DBHelper.WonGames());
        }
    }
}
