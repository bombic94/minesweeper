using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Game
    {
        List<int> runningGames;

        int oblast_id;

        public Game()
        {          
            setGame();
        }

        private void setGame()
        {
            while (true)
            {
                PrintHelper.PrintWelcome();
                runningGames = DBHelper.getListOfRunnningGames();
                oblast_id = CreateOrSelectGame(runningGames);
                Hrej(oblast_id);
                Console.Read();
            }   
        }
        

        void Hrej(int oblast_id)
        {
            int stav = DBHelper.CheckEndOfGame(oblast_id);
            int minesToMark = DBHelper.MaxMinesToMark(oblast_id);
            while (stav == (int) DBHelper.State.Playing)
            {
                Console.WriteLine("Remaining mines to mark:" + minesToMark);
                PrintField(oblast_id);
                PrintHelper.PrintMoveInfo();
                String response = Console.ReadLine();
                String[] args = response.Split();

                if (args.Length == 3 && int.TryParse(args[1], out int x) && int.TryParse(args[2], out int y))
                {
                    switch (args[0])
                    {
                        case ("F"):
                            DBHelper.ShowField(oblast_id, x, y);
                            break;
                        case ("M"):
                            DBHelper.MarkMine(oblast_id, x, y);
                            minesToMark--;
                            break;
                        case ("U"):
                            DBHelper.UnmarkMine(oblast_id, x, y);
                            minesToMark++;
                            break;
                        default:
                            Console.WriteLine("Wrong param");
                            break;
                    }
                }
                else if (args[0].Equals("EXIT"))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Wrong param");
                }
                stav = DBHelper.CheckEndOfGame(oblast_id);
               
            }
            if (stav == (int) DBHelper.State.Won)
            {
                Console.WriteLine("Game over, You win!!!");
            }
            else if (stav == (int)DBHelper.State.Lost)
            {
                Console.WriteLine("Game over, You lose!!!");
            }
            Console.WriteLine("Press enter to start again");
        }

        

        int CreateOrSelectGame(List<int> runningGames)
        {
            bool selected = false;
            int oblast = 0;
            while (!selected)
            {
                String response = Console.ReadLine();
                String[] args = response.Split();

                if (args.Length == 2 && int.TryParse(args[1], out int parsed))
                {
                    switch (args[0])
                    {
                        case ("N"):
                            if (parsed >= 1 && parsed <= 3)
                            {
                                oblast = DBHelper.AddGame(parsed);
                                selected = true;
                            }
                            else
                            {
                                Console.WriteLine("Wrong param");
                            }
                            break;
                        case ("R"):
                            if (runningGames.Contains(parsed))
                            {
                                oblast = parsed;
                                selected = true;
                            }
                            else
                            {
                                Console.WriteLine("Wrong param");
                            }
                            break;
                        default:
                            Console.WriteLine("Wrong param");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Wrong param");
                }

            }
            Console.WriteLine("Selected game id: " + oblast);
            return oblast;
        }     

        void PrintField(int obl_id)
        {
            List<POLE> listPoli = DBHelper.getListPoli(obl_id);
            List<MINA> listMin = DBHelper.getListMin(obl_id);

            Console.Write("    ");
            for (int i = 0; i <= listPoli.Last().souradnice_x; i++)
            {
                Console.Write(i.ToString("00") + " ");
            }

            foreach (var pole in listPoli)
            {
                if (pole.souradnice_x == 0)
                {
                    Console.Write("\n " + pole.souradnice_y.ToString("00") + " ");
                }
                bool oznacena_mina = false;
                foreach (var mina in listMin)
                {
                    if (mina.souradnice_x == pole.souradnice_x && mina.souradnice_y == pole.souradnice_y)
                    {
                        Console.Write("?  ");
                        oznacena_mina = true;
                        break;
                    }
                }
                if (!oznacena_mina)
                {
                    if (!pole.odkryto)
                    {
                        Console.Write("_  ");
                    }
                    else
                    {
                        if (pole.sousedni_miny == 0)
                        {
                            Console.Write("X  ");
                        }
                        else
                        {
                            Console.Write(pole.sousedni_miny + "  ");
                        }
                    }
                }
            }
            Console.Write("\n");
        }
    }
}
