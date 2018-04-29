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
    class HowToVM
    {
        /// <summary>
        /// How to start game
        /// </summary>
        public String StartGame { get; set; }

        /// <summary>
        /// How to continue started game
        /// </summary>
        public String ContinueGame { get; set; }

        /// <summary>
        /// How to play game
        /// </summary>
        public String Play { get; set; }

        /// <summary>
        /// Goal of game
        /// </summary>
        public String ToWin { get; set; }

        /// <summary>
        /// Read values from config file app.config
        /// </summary>
        public HowToVM()
        {
            StartGame = ConfigurationManager.AppSettings["howto.start"];
            ContinueGame = ConfigurationManager.AppSettings["howto.continue"];
            Play = ConfigurationManager.AppSettings["howto.play"];
            ToWin = ConfigurationManager.AppSettings["howto.win"];
        }
    }
}
