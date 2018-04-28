using Minesweeper.MyView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minesweeper.MyViewModel
{
    class MainVM : INotifyPropertyChanged
    {
        public int NumColumns { get; set; }
        public int NumRows { get; set; }
        public int OblastID { get; set; }
        public ObservableCollection<POLE> Pole { get; set; }
        public int RemainingMines { get; set; }

        public int Time { get; set; }
        public ICommand StartGameCommand { get; set; }
        public ICommand CustomGameCommand { get; set; }
        public ICommand ContinueGameCommand { get; set; }
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
            QuitGameCommand = new RelayCommand(param => this.QuitGame());
            HowToCommand = new RelayCommand(param => this.HowTo());
            AboutCommand = new RelayCommand(param => this.About());
        }

        private void MarkUnmarkMine(object param)
        {
            Button b = param as Button;
            POLE p = b.DataContext as POLE;

            if (p.flag == false)
            {
                DBHelper.MarkMine(OblastID, p.souradnice_x, p.souradnice_y);
                p.flag = true;
            }
            else
            {
                DBHelper.UnmarkMine(OblastID, p.souradnice_x, p.souradnice_y);
                p.flag = false;
            }
            refreshGame();
        }

        private void ShowField(object param)
        {
            Button b = param as Button;
            POLE p = b.DataContext as POLE;

            DBHelper.ShowField(OblastID, p.souradnice_x, p.souradnice_y);

            int endOfGame = DBHelper.CheckEndOfGame(OblastID);
            if (endOfGame != 1)
            {
                if (endOfGame == 2)
                {
                    //vyhra
                }
                else if (endOfGame == 3)
                {
                    //prohra
                }
            }
            else
            {
                refreshGame();
            }
        }

        private void StartGame(object param)
        {
            int obtiznostID = Convert.ToInt32(param);
            OblastID = DBHelper.AddGame(obtiznostID);

            this.generateGrid();
        }

        private void CustomGame()
        {
            CustomWindow cusWin = new CustomWindow();
            cusWin.ShowDialog();
        }
        private void ContinueGame()
        {
            ContinueWindow conWin = new ContinueWindow();
            conWin.ShowDialog();
        }

        private void QuitGame()
        {
            Application.Current.MainWindow.Close();
        }    
        private void HowTo()
        {
            HowToWindow htWin = new HowToWindow();
            htWin.ShowDialog();
        }
        private void About()
        {
            AboutWindow abWin = new AboutWindow();
            abWin.ShowDialog();
        }


        public void generateGrid()
        {
            var level = DBHelper.getLevelInfo(OblastID);
            NumColumns = level.sirka;
            NumRows = level.vyska;
            OnPropertyChanged("NumColumns");
            OnPropertyChanged("NumRows");

            refreshGame();
        }

        private void refreshGame()
        {          
            List<POLE> listPoli = DBHelper.getListPoli(OblastID);
            List<MINA> listMin = DBHelper.getListMin(OblastID);

            foreach (MINA m in listMin)
            {
                foreach (POLE p in listPoli)
                {
                    if (m.souradnice_x == p.souradnice_x && m.souradnice_y == p.souradnice_y)
                    {
                        p.flag = true;
                    }
                }
            }

            Pole = new ObservableCollection<POLE>(
                listPoli
            );

            OnPropertyChanged("Pole");
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
