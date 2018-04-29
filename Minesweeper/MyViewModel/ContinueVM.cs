using Minesweeper.MyView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Minesweeper.MyViewModel
{
    /// <summary>
    /// ViewModel for ContinueWindow
    /// Show table of games which are not finished and let player select one to continue
    /// </summary>
    public class ContinueVM
    {
        /// <summary>
        /// Collection of games which are not finished
        /// </summary>
        public ObservableCollection<HRA> Hry { get; set; }

        public ICommand SelectCommand { get; set; }

        /// <summary>
        /// When window is opened, show all games which are not finished
        /// </summary>
        public ContinueVM()
        {
            SelectCommand = new RelayCommand(param => this.SelectGame(param));

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                Hry = new ObservableCollection<HRA>();
            }
            else
            {
                Refresh();
            }
        }

        /// <summary>
        /// Get list of games from database
        /// </summary>
        protected void Refresh()
        {
            Hry = new ObservableCollection<HRA>(
                DBHelper.GetListOfRunnningGames()
            );
        }

        /// <summary>
        /// On click of button get selected Game from list and pass its params to Main ViewModel
        /// and close this window
        /// </summary>
        /// <param name="param">Selected game HRA</param>
        public void SelectGame(object param)
        {
            HRA hra = (HRA)param;
            int oblastID = (int) hra.oblast;

            MainVM main = Application.Current.MainWindow.DataContext as MainVM;
            main.OblastID = oblastID;
            main.GenerateGrid();
            Application.Current.Windows.OfType<ContinueWindow>().First().Close();           
        }
    }
}
