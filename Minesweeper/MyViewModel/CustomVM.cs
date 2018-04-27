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
    class CustomVM : INotifyPropertyChanged
    {
        public ICollectionView CustomWindow { get; set; }
        public ObservableCollection<int> Heights { get; set; }
        public ObservableCollection<int> Widths { get; set; }
        public ObservableCollection<int> Mines { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private ICommand startCommand;
        public ICommand StartCommand
        {
            get
            {
                return startCommand;
            }
            set
            {
                startCommand = value;
            }
        }

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
                computeMines();
                OnPropertyChanged("Mines");
            }
        }

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
                computeMines();
                OnPropertyChanged("Mines");
            }
        }

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

        public CustomVM()
        {
            StartCommand = new RelayCommand(param => this.StartGame());

            Heights = new ObservableCollection<int>(
                Enumerable.Range(9, 90 + 1)
            );

            Widths = new ObservableCollection<int>(
                Enumerable.Range(9, 90 + 1)
            );

            SelectedHeight = Heights.Min();
            SelectedWidth = Widths.Min();

            computeMines();
        }

        private void StartGame()
        {
            int obtiznostID = DBHelper.AddObtiznost(SelectedHeight, SelectedWidth, SelectedMine);
            int oblastID = DBHelper.AddGame(obtiznostID);
            MainVM main = Application.Current.MainWindow.DataContext as MainVM;
            main.OblastID = oblastID;
            main.generateGrid();
            Application.Current.Windows.OfType<CustomWindow>().First().Close();
            
        }

        private void computeMines()
        {
            int min = (int) Math.Ceiling(SelectedHeight * SelectedWidth * 0.1);
            int max = (int) Math.Floor(SelectedHeight * SelectedWidth * 0.4);
            Mines = new ObservableCollection<int>(
                Enumerable.Range(min, max - min + 1)
            );
            SelectedMine = Mines.Min();
        }

        protected void OnPropertyChanged(string name)
        {          
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
