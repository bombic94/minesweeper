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
    public class AboutVM
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AboutVM"/> class
        /// Read values from config file app.config
        /// </summary>
        public AboutVM()
        {
            this.Author = ConfigurationManager.AppSettings["about.author"];
            this.Year = ConfigurationManager.AppSettings["about.year"];
            this.Version = ConfigurationManager.AppSettings["about.version"];
        }

        /// <summary>
        /// Gets or sets info about author
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets year of creation
        /// </summary>
        public string Year { get; set; }

        /// <summary>
        /// Gets or sets version of app
        /// </summary>
        public string Version { get; set; }
    }
}
