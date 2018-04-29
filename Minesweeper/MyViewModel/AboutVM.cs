using System;
using System.Collections.Generic;
using System.Configuration;
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
        /// <summary>
        /// Info about author
        /// </summary>
        public String Author { get; set; }
 
        /// <summary>
        /// Year of creation
        /// </summary>
        public String Year { get; set; }

        /// <summary>
        /// Version of app
        /// </summary>
        public String Version { get; set; }

        /// <summary>
        /// Read values from config file app.config
        /// </summary>
        public AboutVM()
        {
            Author = ConfigurationManager.AppSettings["about.author"];
            Year = ConfigurationManager.AppSettings["about.year"];
            Version = ConfigurationManager.AppSettings["about.version"];
        }
    }
}
