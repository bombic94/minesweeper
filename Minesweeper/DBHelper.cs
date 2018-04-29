using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public static List<HRA> getListOfRunnningGames()
        {
            List<int> runningGames = new List<int>();
            using (var db = new postgresEntities())
            {
                var results = db.HRA
                    .Where(h => h.stav == (int)State.Playing)
                    .Include(h => h.OBLAST1)
                    .OrderBy(h => h.hra_id)
                    .ToList();
                return results;
            }
            
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
                    
                    return oblast.oblast_id;
                }
                catch (Exception e)
                {
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
                var mina = db.MINA
                    .Where(m => m.oblast == obl_id)
                    .Where(m => m.souradnice_x == x)
                    .Where(m => m.souradnice_y == y)
                    .Single();

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

        /// <summary>
        /// Get information about difficulty level (height, width, num of mines, etc.)
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <returns>Difficulty level OBTIZNOST</returns>
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
        /// Get list of lost games ordered by time of creation
        /// </summary>
        /// <returns>List of lost games ordered by creation time</returns>
        public static List<HRA> LostGames()
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
        public static List<HRA> WonGames()
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

        /// <summary>
        /// Add new Difficulty level for new game. Create level with custom height, width and num of mines
        /// </summary>
        /// <param name="selectedHeight">Custom height</param>
        /// <param name="selectedWidth">Custom width</param>
        /// <param name="selectedMine">Custom number of mines</param>
        /// <returns>ID of newly created difficulty OBTIZNOST</returns>
        public static int AddObtiznost(int selectedHeight, int selectedWidth, int selectedMine)
        {
            using (var db = new postgresEntities())
            {
                int id = db.OBTIZNOST.Count() + 1;
                var obtiznost = new OBTIZNOST
                {
                    nazev = "custom" + id,
                    omezeni = 1,
                    vyska = selectedHeight,
                    sirka = selectedWidth,
                    pocet_min = selectedMine,
                    obtiznost_id = id
                };

                db.OBTIZNOST.Add(obtiznost);
                try
                {
                    db.SaveChanges();
                    db.Entry(obtiznost).GetDatabaseValues();

                    return obtiznost.obtiznost_id;
                }
                catch (Exception e)
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Get info about restrictions OMEZENI for new difficulty level.
        /// Contains info about min/max width, height, num.of mines, etc.
        /// </summary>
        /// <returns>Restriction OMEZENI</returns>
        public static OMEZENI GetOmezeni()
        {
            using (var db = new postgresEntities())
            {
                var omezeni = db.OMEZENI.Single();
                return omezeni;
            }
        }
    }
}
