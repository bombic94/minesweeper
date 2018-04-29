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
    /// ViewModel for CustomWindow
    /// Allows to create new difficulty level and start a new game with it.
    /// All controls are in combobox for safe input
    /// </summary>
    public class CustomVM : INotifyPropertyChanged
    {
        /// <summary>
        /// selected Height
        /// </summary>
        private int selectedHeight;

        /// <summary>
        /// selected Width
        /// </summary>
        private int selectedWidth;

        /// <summary>
        /// selected Mine
        /// </summary>
        private int selectedMine;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomVM" /> class
        /// When window is opened, set range of heights and widths by restriction of game
        /// </summary>
        public CustomVM()
        {
            this.StartCommand = new RelayCommand(param => this.StartGame());

            this.Restriction = DBHelper.GetOmezeni();

            this.Heights = new ObservableCollection<int>(Enumerable.Range((int)this.Restriction.vyska_min, (int)(this.Restriction.vyska_max - this.Restriction.vyska_min) + 1));
            this.Widths = new ObservableCollection<int>(Enumerable.Range((int)this.Restriction.sirka_min, (int)(this.Restriction.sirka_max - this.Restriction.sirka_min) + 1));

            this.SelectedHeight = this.Heights.Min();
            this.SelectedWidth = this.Widths.Min();
        }

        /// <summary>
        /// Property Changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets list of Heights for combobox
        /// </summary>
        public ObservableCollection<int> Heights { get; set; }

        /// <summary>
        /// Gets or sets list of Widths for combobox
        /// </summary>
        public ObservableCollection<int> Widths { get; set; }

        /// <summary>
        /// Gets or sets list of Mines for combobox
        /// </summary>
        public ObservableCollection<int> Mines { get; set; }

        /// <summary>
        /// Gets or sets selected height from combobox
        /// On every change compute range of mines
        /// </summary>
        public int SelectedHeight
        {
            get
            {
                return this.selectedHeight;
            }

            set
            {
                this.selectedHeight = value;
                this.ComputeMines();
            }
        }

        /// <summary>
        /// Gets or sets selected width from combobox
        /// On every change compute range of mines
        /// </summary>
        public int SelectedWidth
        {
            get
            {
                return this.selectedWidth;
            }

            set
            {
                this.selectedWidth = value;
                this.ComputeMines();
            }
        }

        /// <summary>
        /// Gets or sets selected number of mines from combobox
        /// </summary>
        public int SelectedMine
        {
            get
            {
                return this.selectedMine;
            }

            set
            {
                this.selectedMine = value;
                this.OnPropertyChanged("Mines");
            }
        }

        /// <summary>
        /// Gets or sets restrictions of game
        /// </summary>
        public OMEZENI Restriction { get; set; }

        /// <summary>
        /// Gets or sets command for Start
        /// </summary>
        public ICommand StartCommand { get; set; }

        /// <summary>
        /// OnPropertyChange handling
        /// </summary>
        /// <param name="name">Property to change</param>
        protected void OnPropertyChanged(string name)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// On change of height or width compute range of number of mines by given percentage from restriction of game
        /// </summary>
        private void ComputeMines()
        {
            int min = (int)Math.Ceiling(this.SelectedHeight * this.SelectedWidth * ((int)this.Restriction.pocet_min_min / 100.0));
            int max = (int)Math.Floor(this.SelectedHeight * this.SelectedWidth * ((int)this.Restriction.pocet_min_max / 100.0));

            this.Mines = new ObservableCollection<int>(Enumerable.Range(min, (max - min) + 1));
            this.SelectedMine = this.Mines.Min();
        }

        /// <summary>
        /// On press of button add new difficulty level with selected height, width and number of mines
        /// With this difficulty create new game and get its ID
        /// Pass ID of game to Main ViewModel and close this window
        /// </summary>
        private void StartGame()
        {
            int obtiznostID = DBHelper.AddObtiznost(this.SelectedHeight, this.SelectedWidth, this.SelectedMine);
            int oblastID = DBHelper.AddGame(obtiznostID);

            MainVM main = Application.Current.MainWindow.DataContext as MainVM;
            main.OblastID = oblastID;
            main.GenerateGrid();
            Application.Current.Windows.OfType<CustomWindow>().First().Close();
        }
    }
}
