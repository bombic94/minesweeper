using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper.MyViewModel
{
    class HowToVM
    {
        public String startGame
        { get
            {
                return "To start a new game select difficulty (Beginner, Advanced, Expert), or create custom difficulty.";
            }
        }
        public String continueGame
        {
            get
            {
                return "To continue started game click on 'Continue' and select game from the list";
            }
        }
        public String play
        {
            get
            {
                return "To show field click on empty field with left mouse-button. To mark mine click on empty field with right mouse-button. "
                + "To unmark mine click on marked mine with right mouse-button.";
            }
        }
        public String toWin
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
