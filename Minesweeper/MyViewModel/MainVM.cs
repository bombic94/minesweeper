using Minesweeper.MyView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Minesweeper.MyViewModel
{
    class MainVM
    {
        public int OblastID { get; set; }

        public int RemainingMines { get; set; }

        public int Time { get; set; }

        private ICommand startGameCommand;
        public ICommand StartGameCommand
        {
            get
            {
                return startGameCommand;
            }
            set
            {
                startGameCommand = value;
            }
        }

        private ICommand customGameCommand;
        public ICommand CustomGameCommand
        {
            get
            {
                return customGameCommand;
            }
            set
            {
                customGameCommand = value;
            }
        }

        private ICommand continueGameCommand;
        public ICommand ContinueGameCommand
        {
            get
            {
                return continueGameCommand;
            }
            set
            {
                continueGameCommand = value;
            }
        }

        private ICommand quitGameCommand;
        public ICommand QuitGameCommand
        {
            get
            {
                return quitGameCommand;
            }
            set
            {
                quitGameCommand = value;
            }
        }

        private ICommand howToCommand;
        public ICommand HowToCommand
        {
            get
            {
                return howToCommand;
            }
            set
            {
                howToCommand = value;
            }
        }

        private ICommand aboutCommand;
        public ICommand AboutCommand
        {
            get
            {
                return aboutCommand;
            }
            set
            {
                aboutCommand = value;
            }
        }

        public MainVM()
        {
            StartGameCommand = new RelayCommand(param => this.StartGame(param));
            CustomGameCommand = new RelayCommand(param => this.CustomGame());
            ContinueGameCommand = new RelayCommand(param => this.ContinueGame());
            QuitGameCommand = new RelayCommand(param => this.QuitGame());
            HowToCommand = new RelayCommand(param => this.HowTo());
            AboutCommand = new RelayCommand(param => this.About());
        }

        private void StartGame(object param)
        {
            int obtiznostID = (int) param;
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
            throw new NotImplementedException();
        }
    }
}
