using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Minesweeper.MyViewModel
{
    public class ContinueVM
    {
        public ObservableCollection<HRA> Hry { get; set; }

        public ICollectionView ContinueWindow { get; set; }

        private ICommand selectCommand;
        public ICommand SelectCommand
        {
            get
            {
                return selectCommand;
            }
            set
            {
                selectCommand = value;
            }
        }

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

        protected void Refresh()
        {
            Hry = new ObservableCollection<HRA>(
                DBHelper.getListOfRunnningGames()
            );
        }

        public void SelectGame(object param)
        {
            HRA hra = (HRA)param;
            int oblast = (int) hra.oblast;
        }
    }
}
