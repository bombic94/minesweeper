using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.MyViewModel
{
    /// <summary>
    /// ViewModel for AboutWindow
    /// Contains information which will be shown in AboutWindow
    /// </summary>
    class AboutVM
    {
        public String Author1
        {
            get
            {
                return "Author";
            }
        }
        public String Author2
        {
            get
            {
                return "David Bohmann";
            }
        }

        public String Year1
        {
            get
            {
                return "Year";
            }
        }

        public String Year2
        {
            get
            {
                return "2018";
            }
        }

        public String Version1
        {
            get
            {
                return "Version";
            }
        }

        public String Version2
        {
            get
            {
                return "1.0.0";
            }
        }

        public AboutVM()
        {

        }
    }
}
