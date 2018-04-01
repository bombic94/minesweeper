using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using Npgsql;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{ 
    class Program
    {
        
        /// <summary>
        /// Start programu - načtení rozehraných her, možnost přidat novou
        /// uložit do globální proměnné id oblasti
        /// možnosti hry, po každé tisknout oblast
        /// 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            int oblast_id;

            List<int> runningGames = afterStart();

            oblast_id = CreateOrSelectGame(runningGames);

            TiskInfo();
            Console.Read();
        }
        
        private static List<int> afterStart()
        {
            Console.WriteLine("Welcome to Minesweeper game");
            Console.WriteLine("You can select to continue one of running games or start a new game");
            Console.WriteLine("Game has three levels: 1 - beginner, 2 - advanced, 3 - expert");
            Console.WriteLine("To choose running game input: 'R [id]'");
            Console.WriteLine("To start new game input: 'N [level]'");

            List<int> runningGames = new List<int>();
            using (var db = new postgresEntities())
            {
                Console.WriteLine("Running games:");
                var results = from hra in db.HRA
                              join obl in db.OBLAST on hra.oblast equals obl.oblast_id
                              where hra.stav == 1
                              orderby hra.hra_id
                              select new { hra, obl };
                
                foreach (var result in results)
                {
                    runningGames.Add(result.obl.oblast_id);
                    Console.WriteLine("Game: [id=" + result.obl.oblast_id + "], [level=" + result.obl.obtiznost + "] + [mines selected=" + result.hra.pocet_oznacenych_min + "]");
                }
            }
            return runningGames;
        }

        private static int CreateOrSelectGame(List<int> runningGames)
        {
            bool selected = false;
            int oblast = 0;
            while (!selected)
            {
                String response = Console.ReadLine();
                String[] arr = response.Split();
                switch (arr[0])
                {
                    case ("N"):
                        int obt = int.Parse(arr[1]);
                        if (obt >= 1 && obt <= 3)
                        {
                            oblast = NovaHra(obt);
                            selected = true;
                        }
                        else
                        {
                            Console.WriteLine("Wrong param");
                        }
                        break;
                    case ("R"):
                        int obl = int.Parse(arr[1]);
                        if (runningGames.Contains(obl))
                        {
                            oblast = obl;
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
            Console.WriteLine("Selected game id: " + oblast);
            return oblast;
        }

        /// <summary>
        /// Game is created by inserting new row in table "oblast".
        /// This method creates new oblast and returns its id
        /// </summary>
        /// <param name="obt">
        /// ID of level of game:
        /// 1 - beginner
        /// 2 - advanced
        /// 3 - expert
        /// </param>
        /// <returns></returns>
        static int NovaHra(int obt)
        {
            using (var db = new postgresEntities())
            {
                var oblast = new OBLAST
                {
                    obtiznost = obt
                };

                db.OBLAST.Add(oblast);
                db.SaveChanges();
                db.Entry(oblast).GetDatabaseValues();

                Console.WriteLine("New game created");
                return oblast.oblast_id;
            }
        }

        static void TiskInfo()
        {
            Console.WriteLine("Show new field by instruction 'F [x] [y]'");
            Console.WriteLine("Mark new mine by instruction 'M [x] [y]");
            Console.WriteLine("Unmark mine by instruction 'U [x] [x]'");
        }
        static int TiskOblasti(int obl_id)
        {
            return 0;
        }

        static int OdkrytiPole(int x, int y, int obl_id)
        {
            return 0;
        }

        static int OznaceniMiny(int x, int y, int obl_id)
        {
            return 0;
        }

        static int ZruseniMiny(int x, int y, int obl_id)
        {
            return 0;
        }
    }
}
