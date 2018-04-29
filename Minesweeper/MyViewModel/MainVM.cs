using Minesweeper.MyView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper.MyViewModel
{
    /// <summary>
    /// ViewModel for MainWindow
    /// Controls for every move - showing fields and marking mines
    /// Takes care of drawing the game field
    /// </summary>
    class MainVM : INotifyPropertyChanged
    {

        /// <summary>
        /// ID of game
        /// </summary>
        public int OblastID { get; set; }

        /// <summary>
        /// Number of columns in game
        /// </summary>
        private int numColumns;
        public int NumColumns
        {
            get
            {
                return numColumns;
            }
            set
            {
                numColumns = value;
                OnPropertyChanged("NumColumns");
            }
        }

        /// <summary>
        /// Number of rows in game
        /// </summary>
        private int numRows;
        public int NumRows
        {
            get
            {
                return numRows;
            }
            set
            {
                numRows = value;
                OnPropertyChanged("numRows");
            }
        }

        /// <summary>
        /// List of fields in game
        /// </summary>
        private ObservableCollection<POLE> pole;
        public ObservableCollection<POLE> Pole
        {
            get
            {
                return pole;
            }
            set
            {
                pole = value;
                OnPropertyChanged("Pole");
            }
        }

        /// <summary>
        /// Number of remaining mines to mark
        /// </summary>
        private int remainingMines;
        public int RemainingMines
        {
            get
            {
                return remainingMines;
            }
            set
            {
                remainingMines = value;
                OnPropertyChanged("remainingMines");
            }
        }

        /// <summary>
        /// Time of game
        /// </summary>
        private int time;
        public int Time
        {
            get
            {
                return time;
            }
            set
            {
                time = value;
                OnPropertyChanged("time");
            }
        }
        
        public Timer Timer { get; set; }
        public ICommand StartGameCommand { get; set; }
        public ICommand CustomGameCommand { get; set; }
        public ICommand ContinueGameCommand { get; set; }
        public ICommand ListGameCommand { get; set; }
        public ICommand QuitGameCommand { get; set; }
        public ICommand HowToCommand { get; set; }
        public ICommand AboutCommand { get; set; }
        public ICommand LeftButton { get; set; }
        public ICommand RightButton { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public MainVM()
        {
            LeftButton = new RelayCommand(param => this.ShowField(param));
            RightButton = new RelayCommand(param => this.MarkUnmarkMine(param));
            StartGameCommand = new RelayCommand(param => this.StartGame(param));
            CustomGameCommand = new RelayCommand(param => this.CustomGame());
            ContinueGameCommand = new RelayCommand(param => this.ContinueGame());
            ListGameCommand = new RelayCommand(param => this.ListGames());
            QuitGameCommand = new RelayCommand(param => this.QuitGame());
            HowToCommand = new RelayCommand(param => this.HowTo());
            AboutCommand = new RelayCommand(param => this.About());
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
            int gameState = (int)DBHelper.GetGame(OblastID).stav; //check if you can play
            if (gameState == (int)DBHelper.State.Playing)
            {
                Button b = param as Button;
                POLE p = b.DataContext as POLE;

                DBHelper.ShowField(OblastID, p.souradnice_x, p.souradnice_y);

                gameState = (int)DBHelper.GetGame(OblastID).stav; //check state after move
                if (gameState != (int)DBHelper.State.Playing)
                {
                    Timer.Enabled = false; //stop counting time

                    if (gameState == (int)DBHelper.State.Won)
                    {
                        RefreshGame();
                        GameOverWindow goWin = new GameOverWindow();
                        goWin.ShowDialog();

                    }
                    else if (gameState == (int)DBHelper.State.Lost)
                    {
                        GameLost(p);
                        GameOverWindow goWin = new GameOverWindow();
                        goWin.ShowDialog();
                    }
                }
                else
                {
                    RefreshGame();
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
            int gameState = (int)DBHelper.GetGame(OblastID).stav; //check if you can play
            if (gameState == (int)DBHelper.State.Playing)
            {
                Button b = param as Button;
                POLE p = b.DataContext as POLE;

                if (p.Flag == false)
                {
                    DBHelper.MarkMine(OblastID, p.souradnice_x, p.souradnice_y);
                    p.Flag = true;
                }
                else
                {
                    DBHelper.UnmarkMine(OblastID, p.souradnice_x, p.souradnice_y);
                    p.Flag = false;
                }
                RefreshGame();
            }        
        }
            
        /// <summary>
        /// Start game with selected difficulty level
        /// </summary>
        /// <param name="param">Difficulty level</param>
        private void StartGame(object param)
        {
            int obtiznostID = Convert.ToInt32(param);
            OblastID = DBHelper.AddGame(obtiznostID);

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
            GameOverWindow goWin = new GameOverWindow();
            goWin.ShowDialog();
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
            HowToWindow htWin = new HowToWindow();
            htWin.ShowDialog();
        }

        /// <summary>
        /// Show AboutWindow with info about program
        /// </summary>
        private void About()
        {
            AboutWindow abWin = new AboutWindow();
            abWin.ShowDialog();
        }

        /// <summary>
        /// At start of game generate grid for selected game
        /// Specifies number of rows and columns to draw
        /// Starts Timer which shows Time of game
        /// </summary>
        public void GenerateGrid()
        {
            var level = DBHelper.GetLevelInfo(OblastID);
            NumColumns = level.sirka;
            NumRows = level.vyska;
            
            Timer = new Timer();
            Timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            Timer.Interval = 500;
            Timer.Enabled = true;

            RefreshGame();
        }

        /// <summary>
        /// After each move in playing game refresh game field
        /// Based on data from tables POLE and MINA show marked mines
        /// Also updates info about remaining mines
        /// </summary>
        private void RefreshGame()
        {          
            List<POLE> listPoli = DBHelper.GetListPoli(OblastID);
            List<MINA> listMin = DBHelper.GetListMin(OblastID);

            RemainingMines = DBHelper.MaxMinesToMark(OblastID);                    

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
            Pole = new ObservableCollection<POLE>(listPoli);
         }

        /// <summary>
        /// When game is lost, show all unmarked mines and wrongly marked mines.
        /// Show the field that contains mine which was stepped on as red mine
        /// </summary>
        /// <param name="clicked">Field which was stepped on - red mine</param>
        private void GameLost(POLE clicked)
        {
            List<POLE> listPoli = DBHelper.GetListPoli(OblastID);
            List<MINA> listMin = DBHelper.GetListMin(OblastID);

            foreach (POLE p in listPoli)
            {
                foreach (MINA m in listMin)
                {  
                    if (m.souradnice_x == p.souradnice_x && m.souradnice_y == p.souradnice_y)
                    {
                        p.Flag = true;
                    }
                }
                if (p.souradnice_x == clicked.souradnice_x && p.souradnice_y == clicked.souradnice_y) //stepped on mine
                {
                    p.SteppedMine = true;
                }
                else if (p.je_mina == true && p.Flag == false) //unmarked mines
                {
                    p.NotRevealed = true;
                }
                else if (p.je_mina == false && p.Flag == true) //wrongly marked mines
                {
                    p.Flag = false;
                    p.WrongFlag = true;
                }
            }
            Pole = new ObservableCollection<POLE>(listPoli);
        }

        /// <summary>
        /// After game is won, show all mines that were not marked
        /// </summary>
        private void GameWon()
        {
            List<POLE> listPoli = DBHelper.GetListPoli(OblastID);
            foreach (POLE p in listPoli)
            {
                if (p.je_mina) //flag all remaining mines
                {
                    p.Flag = true;
                }
            }
            Pole = new ObservableCollection<POLE>(listPoli);
        }

        /// <summary>
        /// Refresh Time information about game
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            HRA Hra = DBHelper.GetGame(OblastID);
            Time = Hra.Time;
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
