using System;
using System.Collections.Generic;
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
        public String StartGame
        { get
            {
                return "To start a new game select difficulty (Beginner, Advanced, Expert), or create custom difficulty.";
            }
        }
        public String ContinueGame
        {
            get
            {
                return "To continue started game click on 'Continue' and select game from the list";
            }
        }
        public String Play
        {
            get
            {
                return "To show field click on empty field with left mouse-button. To mark mine click on empty field with right mouse-button. "
                + "To unmark mine click on marked mine with right mouse-button.";
            }
        }
        public String ToWin
        {
            get
            {
                return "To win this game you must not 'step' on any mine and show all unmined fields.";
            }
        }

        public HowToVM()
        {

        }

    }
}
