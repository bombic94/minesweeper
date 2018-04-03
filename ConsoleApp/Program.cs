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

            Hrej(oblast_id); 
        }

        private static void Hrej(int oblast_id)
        {
            bool finished = false;
            while (!finished)
            {
                TiskOblasti(oblast_id);
                TiskInfo();
                String response = Console.ReadLine();
                String[] arr = response.Split();
                int x = int.Parse(arr[1]);
                int y = int.Parse(arr[2]);
                switch (arr[0])
                {
                    case ("F"):
                        OdkrytiPole(oblast_id, x, y);
                        break;
                    case ("M"):
                        OznaceniMiny(oblast_id, x, y);
                        break;
                    case ("U"):
                        ZruseniMiny(oblast_id, x, y);
                        break;
                    default:
                        Console.WriteLine("Wrong param");
                        break;
                }
            }
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
        static void TiskOblasti(int obl_id)
        {
            using (var db = new postgresEntities())
            {
                var listPoli = (from p in db.POLE
                            where p.oblast == obl_id
                            orderby p.souradnice_y, p.souradnice_x
                            select p).ToList();
                var listMin = (from m in db.MINA
                            where m.oblast == obl_id
                            select m).ToList();

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
                    if (!oznacena_mina) { 
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
                Console.Write("\n ");
            }
        }

        static void OdkrytiPole(int obl_id, int x, int y)
        {
            using (var db = new postgresEntities())
            {
                var tah = new TAH
                {
                    oblast = obl_id,
                    souradnice_x = x,
                    souradnice_y = y                    
                };

                db.TAH.Add(tah);
                db.SaveChanges();
            }
        }

        static void OznaceniMiny(int obl_id, int x, int y)
        {
            using (var db = new postgresEntities())
            {
                var mina = new MINA
                {
                    oblast = obl_id,
                    souradnice_x = x,
                    souradnice_y = y
                };

                db.MINA.Add(mina);
                db.SaveChanges();
            }
        }

        static void ZruseniMiny(int obl_id, int x, int y)
        {
            using (var db = new postgresEntities())
            {
                var mina = (from m in db.MINA
                           where m.oblast == obl_id
                           where m.souradnice_x == x
                           where m.souradnice_y == y
                           select m).Single();

                db.MINA.Remove(mina);
                db.SaveChanges();
            }
        }
    }
}
