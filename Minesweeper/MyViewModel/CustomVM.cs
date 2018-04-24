using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.MyViewModel
{
    class CustomVM
    {
        public ICollectionView CustomWindow { get; set; }
        public ObservableCollection<int> Heights { get; set; }

        public ObservableCollection<int> Widths { get; set; }

        public ObservableCollection<int> Mines { get; set; }

        public CustomVM()
        {

            Heights = new ObservableCollection<int>(
                Enumerable.Range(9, 90)
            );

            Widths = new ObservableCollection<int>(
                Enumerable.Range(9, 90)
            );
        }
    }
}
