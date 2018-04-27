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

        public ICommand ClickedButton { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public MainVM()
        {
            ClickedButton = new RelayCommand(param => this.Move(param));
            StartGameCommand = new RelayCommand(param => this.StartGame(param));
            CustomGameCommand = new RelayCommand(param => this.CustomGame());
            ContinueGameCommand = new RelayCommand(param => this.ContinueGame());
            QuitGameCommand = new RelayCommand(param => this.QuitGame());
            HowToCommand = new RelayCommand(param => this.HowTo());
            AboutCommand = new RelayCommand(param => this.About());
        }

        private void Move(object param)
        {
            POLE p = param as POLE;
            if (p.je_mina)
            {

            }
            else if (p.odkryto){

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
            refreshGame();
        }

        private void refreshGame()
        {
            var level = DBHelper.getLevelInfo(OblastID);
            NumColumns = level.sirka;
            NumRows = level.vyska;
            List<POLE> listPoli = DBHelper.getListPoli(OblastID);
            List<MINA> listMin = DBHelper.getListMin(OblastID);

            Pole = new ObservableCollection<POLE>(
                listPoli
            );

            OnPropertyChanged("Pole");
            OnPropertyChanged("NumColumns");
            OnPropertyChanged("NumRows");
        }
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }
}
