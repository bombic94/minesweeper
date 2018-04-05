using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    /// <summary>
    /// Static class - takes care of printing information during playing.
    /// </summary>
    static class PrintHelper
    {

        /// <summary>
        /// Print info at the start of the game.
        /// </summary>
        public static void PrintWelcome()
        {
            Console.WriteLine("Welcome to Minesweeper game");
            Console.WriteLine("You can select to continue one of running games or start a new game");
            Console.WriteLine("Game has three levels: 1 - beginner, 2 - advanced, 3 - expert");
            Console.WriteLine("To choose running game input: 'R [id]'");
            Console.WriteLine("To start new game input: 'N [level]'");
        }

        /// <summary>
        /// Print info for each move.
        /// </summary>
        public static void PrintMoveInfo()
        {
            Console.WriteLine("Show new field by instruction 'F [x] [y]'");
            Console.WriteLine("Mark new mine by instruction 'M [x] [y]");
            Console.WriteLine("Unmark mine by instruction 'U [x] [x]'");
            Console.WriteLine("Exit game by insturction 'EXIT'");
        }
    }

}
