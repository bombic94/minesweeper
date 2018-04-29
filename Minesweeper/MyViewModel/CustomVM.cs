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
    /// ViewModel for CustomWindow
    /// Allows to create new difficulty level and start a new game with it.
    /// All controls are in combobox for safe input
    /// </summary>
    class CustomVM : INotifyPropertyChanged
    {
        /// <summary>
        /// List of Heights for combobox
        /// </summary>
        public ObservableCollection<int> Heights { get; set; }

        /// <summary>
        /// List of Widths for combobox
        /// </summary>
        public ObservableCollection<int> Widths { get; set; }

        /// <summary>
        /// List of Mines for combobox
        /// </summary>
        public ObservableCollection<int> Mines { get; set; }

        /// <summary>
        /// Selected height from combobox
        /// On every change compute range of mines
        /// </summary>
        private int selectedHeight;
        public int SelectedHeight
        {
            get
            {
                return selectedHeight;
            }
            set
            {
                selectedHeight = value;
                ComputeMines();
            }
        }

        /// <summary>
        /// Selected width from combobox
        /// On every change compute range of mines
        /// </summary>
        private int selectedWidth;
        public int SelectedWidth
        {
            get
            {
                return selectedWidth;
            }
            set
            {
                selectedWidth = value;
                ComputeMines();
            }
        }

        /// <summary>
        /// Selected number of mines from combobox
        /// </summary>
        private int selectedMine;
        public int SelectedMine {
            get
            {
                return selectedMine;
            }
            set
            {
                selectedMine = value;
                OnPropertyChanged("Mines");
            }
        }

        /// <summary>
        /// Restrictions of game
        /// </summary>
        public OMEZENI Restriction { get; set; }

        public ICommand StartCommand { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// When window is opened, set range of heights and widths by restriction of game
        /// </summary>
        public CustomVM()
        {
            StartCommand = new RelayCommand(param => this.StartGame());

            Restriction = DBHelper.GetOmezeni();

            Heights = new ObservableCollection<int>(
                Enumerable.Range(
                    (int)Restriction.vyska_min, 
                    (int)(Restriction.vyska_max - Restriction.vyska_min) + 1
                )
            );
            Widths = new ObservableCollection<int>(
                Enumerable.Range(
                    (int)Restriction.sirka_min,
                    (int)(Restriction.sirka_max - Restriction.sirka_min) + 1
                )
            );

            SelectedHeight = Heights.Min();
            SelectedWidth = Widths.Min();
        }

        /// <summary>
        /// On change of height or width compute range of number of mines by given percentage from restriction of game
        /// </summary>
        private void ComputeMines()
        {
            int min = (int) Math.Ceiling(SelectedHeight * SelectedWidth * ((int)Restriction.pocet_min_min / 100.0));
            int max = (int) Math.Floor(SelectedHeight * SelectedWidth * ((int)Restriction.pocet_min_max / 100.0));

            Mines = new ObservableCollection<int>(
                Enumerable.Range(
                    min, 
                    (max - min) + 1
                )
            );
            SelectedMine = Mines.Min();
        }

        /// <summary>
        /// On press of button add new difficulty level with selected height, width and number of mines
        /// With this difficulty create new game and get its ID
        /// Pass ID of game to Main ViewModel and close this window
        /// </summary>
        private void StartGame()
        {
            int obtiznostID = DBHelper.AddObtiznost(SelectedHeight, SelectedWidth, SelectedMine);
            int oblastID = DBHelper.AddGame(obtiznostID);

            MainVM main = Application.Current.MainWindow.DataContext as MainVM;
            main.OblastID = oblastID;
            main.GenerateGrid();
            Application.Current.Windows.OfType<CustomWindow>().First().Close();
        }

        /// <summary>
        /// OnPropertyChange handling
        /// </summary>
        /// <param name="name">Property to change</param>
        protected void OnPropertyChanged(string name)
        {          
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
