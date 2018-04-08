using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    /// <summary>
    /// Static class - takes care of communication with database
    /// </summary>
    static class DBHelper
    {
        /// <summary>
        /// Enum representing state of the game
        /// 1 - Playing: game is not finished
        /// 2 - Won: game is finished, player had won
        /// 3 - Lost: game is finished, player had lost
        /// </summary>
        public enum State { Playing=1, Won=2, Lost=3 }

        /// <summary>
        /// Method for obtaining list of games, which have not been finished
        /// </summary>
        /// <returns>List of IDs of non-finished games</returns>
        public static List<int> getListOfRunnningGames()
        {
            List<int> runningGames = new List<int>();
            using (var db = new postgresEntities())
            {
                Console.WriteLine("Running games:");
                var results = from hra in db.HRA
                              join obl in db.OBLAST on hra.oblast equals obl.oblast_id
                              where hra.stav == (int) State.Playing
                              orderby hra.hra_id
                              select new { hra, obl };

                foreach (var result in results)
                {
                    runningGames.Add(result.obl.oblast_id);
                    Console.WriteLine("Game: [id=" + result.obl.oblast_id + "], [level=" + result.obl.obtiznost + "] + [mines selected=" + result.hra.pocet_oznacenych_min + "]");
                }
                if (runningGames.Count == 0)
                {
                    Console.WriteLine("No games running, start a new game.");
                }
            }
            return runningGames;
        }

        /// <summary>
        /// Create new OBLAST with selected level of game (OBTIZNOST)
        /// </summary>
        /// <param name="obt">
        /// ID of level of game:
        /// 1 - beginner
        /// 2 - advanced
        /// 3 - expert
        /// </param>
        /// <returns>ID of OBLAST of created game, which will be used for playing</returns>
        public static int AddGame(int obt)
        {
            using (var db = new postgresEntities())
            {
                var oblast = new OBLAST
                {
                    obtiznost = obt
                };

                db.OBLAST.Add(oblast);
                try
                {
                    db.SaveChanges();
                    db.Entry(oblast).GetDatabaseValues();

                    Console.WriteLine("New game created");
                    return oblast.oblast_id;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.InnerException.Message);
                    return -1;
                }
               
            }
        }

        /// <summary>
        /// Show selected field (represents left click of the mouse)
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        public static void ShowField(int obl_id, int x, int y)
        {
            using (var db = new postgresEntities())
            {
                var tah = new TAH
                {
                    oblast = obl_id,
                    souradnice_x = x,
                    souradnice_y = y,
                    cas_tahu = DateTimeOffset.Now
                };

                db.TAH.Add(tah);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.InnerException.Message);
                }
            }
        }

        /// <summary>
        /// Mark selected field as a mine (represents right click of the mouse)
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>
        public static void MarkMine(int obl_id, int x, int y)
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

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.InnerException.Message);
                }
            }
        }

        /// <summary>
        /// Unmark selected field as a mine (represents right click of the mouse on marked mine)
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <param name="x">x coordinate</param>
        /// <param name="y">y coordinate</param>ID of OBLAST of played game
        public static void UnmarkMine(int obl_id, int x, int y)
        {
            using (var db = new postgresEntities())
            {
                var mina = (from m in db.MINA
                            where m.oblast == obl_id
                            where m.souradnice_x == x
                            where m.souradnice_y == y
                            select m).Single();

                db.MINA.Remove(mina);

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.InnerException.InnerException.Message);
                }
            }
        }

        /// <summary>
        /// Check state of game (HRA) and return its value
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <returns>Value of game state</returns>
        public static int CheckEndOfGame(int obl_id)
        {
            using (var db = new postgresEntities())
            {
                var hra = (from h in db.HRA
                           where h.oblast == obl_id
                           select h).Single();
                Console.WriteLine(hra.ToString());
                return (int)hra.stav;
            }
        }

        public static OBTIZNOST getLevelInfo(int obl_id)
        {
            using (var db = new postgresEntities())
            {
                var obtiznost = (from o in db.OBTIZNOST
                                 join obl in db.OBLAST on o.obtiznost_id equals obl.obtiznost
                                 where obl.oblast_id == obl_id
                                 select o).Single();
                return obtiznost;
            }
        }
        /// <summary>
        /// Get list of all fields in played game, order by rows and columns
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game<</param>
        /// <returns>Ordered list of fields in played game</returns>
        public static List<POLE> getListPoli(int obl_id)
        {
            using (var db = new postgresEntities())
            {
                var listPoli = (from p in db.POLE
                                where p.oblast == obl_id
                                orderby p.souradnice_y, p.souradnice_x
                                select p).ToList();
                return listPoli;
            }
        }

        /// <summary>
        /// Get list of all mines in played game
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <returns>List of mines in played game</returns>
        public static List<MINA> getListMin(int obl_id)
        {
            using (var db = new postgresEntities())
            {
                var listMin = (from m in db.MINA
                               where m.oblast == obl_id
                               select m).ToList();
                return listMin;
            }
        }

        /// <summary>
        /// Get maximum value of mines, which can be marked in selected game.
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <returns>Value of mines to be marked</returns>
        public static int MaxMinesToMark(int obl_id)
        {
            using (var db = new postgresEntities())
            {
                var obtiznost = (from o in db.OBTIZNOST
                                 join obl in db.OBLAST on o.obtiznost_id equals obl.obtiznost
                                 where obl.oblast_id == obl_id
                                 select o).Single();
                var hra = (from h in db.HRA
                           join obl in db.OBLAST on h.oblast equals obl.oblast_id
                           where obl.oblast_id == obl_id
                           select h).Single();
                return obtiznost.pocet_min - hra.pocet_oznacenych_min;
            }
        }

        /// <summary>
        /// Get list of wrongly marked mines in lost game
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <returns>List of wrongly marked mines</returns>
        public static List<POLE> falselyMarkedMines(int obl_id)
        {
            using (var db = new postgresEntities())
            {
                var wrongMines = (from p in db.POLE
                                  join m in db.MINA on p.oblast equals m.oblast
                                  where p.oblast == obl_id
                                  where p.souradnice_x == m.souradnice_x
                                  where p.souradnice_y == m.souradnice_y
                                  where p.je_mina == false         
                                  orderby p.souradnice_y, p.souradnice_x
                                  select p).ToList();
                return wrongMines;
            }
        }

        /// <summary>
        /// Get list of lost games ordered by time of creation
        /// </summary>
        /// <returns>List of lost games ordered by creation time</returns>
        public static List<HRA> lostGames()
        {
            using (var db = new postgresEntities())
            {
                var lostGames = (from h in db.HRA
                             where h.stav == (int)State.Lost
                             orderby h.cas_prvni_tah 
                             select h).ToList();
                return lostGames;
            }
        }

        /// <summary>
        /// Get list of won games ordered by time of creation
        /// </summary>
        /// <returns>List of lost games ordered by creation time</returns>
        public static List<HRA> wonGames()
        {
            using (var db = new postgresEntities())
            {
                var games = (from h in db.HRA
                             where h.stav == (int)State.Won
                             orderby h.cas_prvni_tah
                             select h).ToList();
                return games;
            }
        }
    }
}
