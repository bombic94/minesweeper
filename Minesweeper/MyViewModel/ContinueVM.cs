using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Minesweeper.MyView;

namespace Minesweeper.MyViewModel
{
    /// <summary>
    /// ViewModel for ContinueWindow
    /// Show table of games which are not finished and let player select one to continue
    /// </summary>
    public class ContinueVM : INotifyPropertyChanged
    {
        /// <summary>
        /// selected Game
        /// </summary>
        private HRA selected;
        /// <summary>
        /// Initializes a new instance of the <see cref="ContinueVM" /> class
        /// When window is opened, show all games which are not finished
        /// </summary>
        public ContinueVM()
        {
            this.SelectCommand = new RelayCommand(param => this.SelectGame(), c => Selected != null);

            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                this.Hry = new ObservableCollection<HRA>();
            }
            else
            {
                this.Refresh();
            }
        }

        /// <summary>
        /// Property Changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets collection of games which are not finished
        /// </summary>
        public ObservableCollection<HRA> Hry { get; set; }

        /// <summary>
        /// Gets or sets selected game to continue
        /// </summary>
        public HRA Selected
        {
            get
            {
                return selected;
            }
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }

        /// <summary>
        /// Gets or sets command for Select
        /// </summary>
        public ICommand SelectCommand { get; set; }

        /// <summary>
        /// Get list of games from database
        /// </summary>
        private void Refresh()
        {
            this.Hry = new ObservableCollection<HRA>(DBHelper.GetListOfRunnningGames());
        }

        /// <summary>
        /// On click of button get selected Game from list and pass its params to Main ViewModel
        /// and close this window
        /// </summary>
        /// <param name="param">Selected game HRA</param>
        private void SelectGame()
        {
            HRA hra = Selected;
            int oblastID = (int)hra.oblast;

            MainVM main = Application.Current.MainWindow.DataContext as MainVM;
            main.OblastID = oblastID;
            main.GenerateGrid();
            Application.Current.Windows.OfType<ContinueWindow>().First().Close();
        }

        /// <summary>
        /// OnPropertyChange handling
        /// </summary>
        /// <param name="name">Property to change</param>
        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
