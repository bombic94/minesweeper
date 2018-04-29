using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.MyViewModel
{
    /// <summary>
    /// ViewModel for HowToWindow
    /// Contains information which will be shown in HowToWindow
    /// </summary>
    public class HowToVM
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HowToVM" /> class
        /// Read values from config file app.config
        /// </summary>
        public HowToVM()
        {
            this.StartGame = ConfigurationManager.AppSettings["howto.start"];
            this.ContinueGame = ConfigurationManager.AppSettings["howto.continue"];
            this.Play = ConfigurationManager.AppSettings["howto.play"];
            this.ToWin = ConfigurationManager.AppSettings["howto.win"];
        }

        /// <summary>
        /// Gets or sets How to start game
        /// </summary>
        public string StartGame { get; set; }

        /// <summary>
        /// Gets or sets How to continue started game
        /// </summary>
        public string ContinueGame { get; set; }

        /// <summary>
        /// Gets or sets How to play game
        /// </summary>
        public string Play { get; set; }

        /// <summary>
        /// Gets or sets Goal of game
        /// </summary>
        public string ToWin { get; set; }
    }
}
