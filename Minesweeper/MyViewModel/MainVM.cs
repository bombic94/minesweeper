using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Minesweeper.MyView;

namespace Minesweeper.MyViewModel
{
    /// <summary>
    /// ViewModel for MainWindow
    /// Controls for every move - showing fields and marking mines
    /// Takes care of drawing the game field
    /// </summary>
    public class MainVM : INotifyPropertyChanged
    {
        /// <summary>
        /// Number of columns
        /// </summary>
        private int numColumns;

        /// <summary>
        /// Number of rows
        /// </summary>
        private int numRows;

        /// <summary>
        /// List of fields
        /// </summary>
        private ObservableCollection<POLE> pole;

        /// <summary>
        /// Number of remaining mines to mark
        /// </summary>
        private int remainingMines;

        /// <summary>
        /// Time in seconds
        /// </summary>
        private int time;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainVM" /> class
        /// Sets all commands to functions
        /// </summary>
        public MainVM()
        {
            this.LeftButton = new RelayCommand(param => this.ShowField(param));
            this.RightButton = new RelayCommand(param => this.MarkUnmarkMine(param));
            this.StartGameCommand = new RelayCommand(param => this.StartGame(param));
            this.CustomGameCommand = new RelayCommand(param => this.CustomGame());
            this.ContinueGameCommand = new RelayCommand(param => this.ContinueGame());
            this.ListGameCommand = new RelayCommand(param => this.ListGames());
            this.QuitGameCommand = new RelayCommand(param => this.QuitGame());
            this.HowToCommand = new RelayCommand(param => this.HowTo());
            this.AboutCommand = new RelayCommand(param => this.About());
        }

        /// <summary>
        /// Property changed event handler
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets ID of game
        /// </summary>
        public int OblastID { get; set; }

        /// <summary>
        /// Gets or sets number of columns in game
        /// </summary> 
        public int NumColumns
        {
            get
            {
                return this.numColumns;
            }

            set
            {
                this.numColumns = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets number of rows in game
        /// </summary>
        public int NumRows
        {
            get
            {
                return this.numRows;
            }

            set
            {
                this.numRows = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets list of fields in game
        /// </summary>
        public ObservableCollection<POLE> Pole
        {
            get
            {
                return this.pole;
            }

            set
            {
                this.pole = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets number of remaining mines to mark
        /// </summary>
        public int RemainingMines
        {
            get
            {
                return this.remainingMines;
            }

            set
            {
                this.remainingMines = value;
                this.OnPropertyChanged();
            }
        }

        /// <summary>
        /// Gets or sets time of game
        /// </summary>
        public int Time
        {
            get
            {
                return this.time;
            }

            set
            {
                this.time = value;
                this.OnPropertyChanged();
            }
        }
        
        /// <summary>
        /// Gets or sets timer instance
        /// </summary>
        public Timer Timer { get; set; }

        /// <summary>
        /// Gets or sets Command to start game
        /// </summary>
        public ICommand StartGameCommand { get; set; }

        /// <summary>
        /// Gets or sets Command to create custom game
        /// </summary>
        public ICommand CustomGameCommand { get; set; }

        /// <summary>
        /// Gets or sets Command to continue game
        /// </summary>
        public ICommand ContinueGameCommand { get; set; }

        /// <summary>
        /// Gets or sets Command to list played games
        /// </summary>
        public ICommand ListGameCommand { get; set; }

        /// <summary>
        /// Gets or sets Command to quit game
        /// </summary>
        public ICommand QuitGameCommand { get; set; }

        /// <summary>
        /// Gets or sets Command to show howto
        /// </summary>
        public ICommand HowToCommand { get; set; }

        /// <summary>
        /// Gets or sets Command to show about
        /// </summary>
        public ICommand AboutCommand { get; set; }

        /// <summary>
        /// Gets or sets command for left click
        /// </summary>
        public ICommand LeftButton { get; set; }

        /// <summary>
        /// Gets or sets command for right click
        /// </summary>
        public ICommand RightButton { get; set; }

        /// <summary>
        /// At start of game generate grid for selected game
        /// Specifies number of rows and columns to draw
        /// Starts Timer which shows Time of game
        /// </summary>
        public void GenerateGrid()
        {
            var level = DBHelper.GetLevelInfo(this.OblastID);
            this.NumColumns = level.sirka;
            this.NumRows = level.vyska;

            this.Timer = new Timer();
            this.Timer.Elapsed += new ElapsedEventHandler(this.OnTimedEvent);
            this.Timer.Interval = 500;
            this.Timer.Enabled = true;

            this.RefreshGame();
        }

        /// <summary>
        /// OnPropertyChange handling
        /// </summary>
        /// <param name="name">Property to change</param>
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Reaction to left click of button.
        /// Show field on selected button and check from database, if game still continues.
        /// If yes, refresh field.
        /// If lost, call GameLost method
        /// If won, call GameWon method
        /// </summary>
        /// <param name="param">Button representing one field in game</param>
        private void ShowField(object param)
        {
            int gameState = (int)DBHelper.GetGame(this.OblastID).stav; // check if you can play
            if (gameState == (int)DBHelper.State.Playing)
            {
                Button b = param as Button;
                POLE p = b.DataContext as POLE;

                DBHelper.ShowField(this.OblastID, p.souradnice_x, p.souradnice_y);

                gameState = (int)DBHelper.GetGame(this.OblastID).stav; // check state after move
                if (gameState != (int)DBHelper.State.Playing)
                {
                    this.Timer.Enabled = false; // stop counting time

                    if (gameState == (int)DBHelper.State.Won)
                    {
                        this.GameWon();
                        GameOverWindow gameWin = new GameOverWindow();
                        gameWin.ShowDialog();
                    }
                    else if (gameState == (int)DBHelper.State.Lost)
                    {
                        this.GameLost(p);
                        GameOverWindow gameWin = new GameOverWindow();
                        gameWin.ShowDialog();
                    }
                }
                else
                {
                    this.RefreshGame();
                }
            }
        }

        /// <summary>
        /// Reaction to right click of button.
        /// If is not marked, mark it as mine.
        /// If marked, unmark mine.
        /// </summary>
        /// <param name="param">Button representing one field in game</param>
        private void MarkUnmarkMine(object param)
        {
            int gameState = (int)DBHelper.GetGame(this.OblastID).stav; // check if you can play
            if (gameState == (int)DBHelper.State.Playing)
            {
                Button b = param as Button;
                POLE p = b.DataContext as POLE;

                if (p.Flag == false)
                {
                    DBHelper.MarkMine(this.OblastID, p.souradnice_x, p.souradnice_y);
                    p.Flag = true;
                }
                else
                {
                    DBHelper.UnmarkMine(this.OblastID, p.souradnice_x, p.souradnice_y);
                    p.Flag = false;
                }

                this.RefreshGame();
            }        
        }
            
        /// <summary>
        /// Start game with selected difficulty level
        /// </summary>
        /// <param name="param">Difficulty level</param>
        private void StartGame(object param)
        {
            int obtiznostID = Convert.ToInt32(param);
            this.OblastID = DBHelper.AddGame(obtiznostID);

            this.GenerateGrid();
        }

        /// <summary>
        /// Show CustomWindow to create custom difficulty level and start game
        /// </summary>
        private void CustomGame()
        {
            CustomWindow cusWin = new CustomWindow();
            cusWin.ShowDialog();
        }

        /// <summary>
        /// Show ContinueWindow to select one of not finished games to play
        /// </summary>
        private void ContinueGame()
        {
            ContinueWindow conWin = new ContinueWindow();
            conWin.ShowDialog();
        }

        /// <summary>
        /// Show GameOverWindow containing list of played games
        /// </summary>
        private void ListGames()
        {
            GameOverWindow gameWin = new GameOverWindow();
            gameWin.ShowDialog();
        }

        /// <summary>
        /// Close window, end of program
        /// </summary>
        private void QuitGame()
        {
            Application.Current.MainWindow.Close();
        }    

        /// <summary>
        /// Show HowToWindow with instructions to play
        /// </summary>
        private void HowTo()
        {
            HowToWindow howWin = new HowToWindow();
            howWin.ShowDialog();
        }

        /// <summary>
        /// Show AboutWindow with info about program
        /// </summary>
        private void About()
        {
            AboutWindow aboutWin = new AboutWindow();
            aboutWin.ShowDialog();
        }

        /// <summary>
        /// After each move in playing game refresh game field
        /// Based on data from tables POLE and MINA show marked mines
        /// Also updates info about remaining mines
        /// </summary>
        private void RefreshGame()
        {          
            List<POLE> listPoli = DBHelper.GetListPoli(this.OblastID);
            List<MINA> listMin = DBHelper.GetListMin(this.OblastID);

            this.RemainingMines = DBHelper.MaxMinesToMark(this.OblastID);                    

            foreach (POLE p in listPoli)              
            {
                foreach (MINA m in listMin)
                {
                    if (m.souradnice_x == p.souradnice_x && m.souradnice_y == p.souradnice_y)
                    {
                        p.Flag = true;
                    }
                }
            }

            this.Pole = new ObservableCollection<POLE>(listPoli);
         }

        /// <summary>
        /// When game is lost, show all unmarked mines and wrongly marked mines.
        /// Show the field that contains mine which was stepped on as red mine
        /// </summary>
        /// <param name="clicked">Field which was stepped on - red mine</param>
        private void GameLost(POLE clicked)
        {
            List<POLE> listPoli = DBHelper.GetListPoli(this.OblastID);
            List<MINA> listMin = DBHelper.GetListMin(this.OblastID);

            foreach (POLE p in listPoli)
            {
                foreach (MINA m in listMin)
                {  
                    if (m.souradnice_x == p.souradnice_x && m.souradnice_y == p.souradnice_y)
                    {
                        p.Flag = true;
                    }
                }

                if (p.souradnice_x == clicked.souradnice_x && p.souradnice_y == clicked.souradnice_y)
                {
                    p.SteppedMine = true;
                }
                else if (p.je_mina == true && p.Flag == false)
                {
                    p.NotRevealed = true;
                }
                else if (p.je_mina == false && p.Flag == true)
                {
                    p.Flag = false;
                    p.WrongFlag = true;
                }
            }

            this.Pole = new ObservableCollection<POLE>(listPoli);
        }

        /// <summary>
        /// After game is won, show all mines that were not marked
        /// </summary>
        private void GameWon()
        {
            List<POLE> listPoli = DBHelper.GetListPoli(this.OblastID);
            foreach (POLE p in listPoli)
            {
                if (p.je_mina)
                {
                    p.Flag = true;
                }
            }

            this.Pole = new ObservableCollection<POLE>(listPoli);
        }

        /// <summary>
        /// Refresh Time information about game
        /// </summary>
        /// <param name="source">Source of event</param>
        /// <param name="e">Param of event</param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            HRA hra = DBHelper.GetGame(this.OblastID);
            this.Time = hra.Time;
        }
    }
}
