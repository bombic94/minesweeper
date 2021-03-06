﻿using System;
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
    public static class DBHelper
    {
        /// <summary>
        /// Enum representing state of the game
        /// </summary>
        public enum State
        {
            /// <summary>
            /// 1 - Playing: game is not finished
            /// </summary>
            Playing = 1,

            /// <summary>
            /// 2 - Won: game is finished, player had won
            /// </summary>
            Won = 2,

            /// <summary>
            /// 3 - Lost: game is finished, player had lost
            /// </summary>
            Lost = 3
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
        /// Method for obtaining list of games, which have not been finished
        /// </summary>
        /// <returns>List of IDs of non-finished games</returns>
        public static List<HRA> GetListOfRunnningGames()
        {
            List<int> runningGames = new List<int>();
            using (var db = new postgresEntities())
            {
                var results = db.HRA
                    .Where(h => h.stav == (int)State.Playing)
                    .Include(h => h.OBLAST1)
                    .Include(h => h.OBLAST1.OBTIZNOST1)
                    .OrderBy(h => h.hra_id)
                    .ToList();

                return results;
            }
        }

        /// <summary>
        /// Get information about difficulty level (height, width, num of mines, etc.)
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <returns>Difficulty level OBTIZNOST</returns>
        public static OBTIZNOST GetLevelInfo(int obl_id)
        {
            using (var db = new postgresEntities())
            {
                var obtiznost = db.OBTIZNOST
                    .Join(db.OBLAST, obt => obt.obtiznost_id, obl => obl.obtiznost, (obt, obl) => new { obt, obl })
                    .Where(a => a.obl.oblast_id == obl_id)
                    .Select(a => a.obt)
                    .Single();

                return obtiznost;
            }
        }

        /// <summary>
        /// Get list of all fields in played game, order by rows and columns
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <returns>Ordered list of fields in played game</returns>
        public static List<POLE> GetListPoli(int obl_id)
        {
            using (var db = new postgresEntities())
            {
                var listPoli = db.POLE
                    .Where(p => p.oblast == obl_id)
                    .OrderBy(p => new { p.souradnice_y, p.souradnice_x })
                    .ToList();

                return listPoli;
            }
        }

        /// <summary>
        /// Get list of all mines in played game
        /// </summary>
        /// <param name="obl_id">ID of OBLAST of played game</param>
        /// <returns>List of mines in played game</returns>
        public static List<MINA> GetListMin(int obl_id)
        {
            using (var db = new postgresEntities())
            {
                var listMin = db.MINA
                    .Where(m => m.oblast == obl_id)
                    .OrderBy(m => new { m.souradnice_y, m.souradnice_x })
                    .ToList(); 

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
                var mines = db.OBTIZNOST
                    .Join(db.OBLAST, obt => obt.obtiznost_id, obl => obl.obtiznost, (obt, obl) => new { obt, obl })
                    .Join(db.HRA, a => a.obl.oblast_id, hra => hra.oblast, (a, hra) => new { a.obl, a.obt, hra })
                    .Where(a => a.obl.oblast_id == obl_id)
                    .Select(a => a.obt.pocet_min - a.hra.pocet_oznacenych_min)
                    .Single();

                return mines;
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
                var lostGames = db.HRA
                    .Where(h => h.stav == (int)State.Lost)
                    .Include(h => h.OBLAST1)
                    .Include(h => h.OBLAST1.OBTIZNOST1)
                    .OrderByDescending(h => h.cas_prvni_tah)
                    .ToList();

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
                var wonGames = db.HRA
                    .Where(h => h.stav == (int)State.Won)
                    .Include(h => h.OBLAST1)
                    .Include(h => h.OBLAST1.OBTIZNOST1)
                    .OrderByDescending(h => h.cas_prvni_tah)
                    .ToList();

                return wonGames;
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
                    Console.WriteLine(e.InnerException.InnerException.Message);
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
                var omezeni = db.OMEZENI
                    .Single();

                return omezeni;
            }
        }

        /// <summary>
        /// Get number of mines that were correctly marked in lost game
        /// </summary>
        /// <param name="oblastID">ID of field OBLAST</param>
        /// <returns>Number of correctly marked mines</returns>
        public static int GetFoundMines(int oblastID)
        {
            using (var db = new postgresEntities())
            {
                var found = db.POLE
                    .Join(db.MINA, p => p.oblast, m => m.oblast, (p, m) => new { p, m })
                    .Where(a => a.p.oblast == oblastID)
                    .Where(a => a.p.souradnice_x == a.m.souradnice_x)
                    .Where(a => a.p.souradnice_y == a.m.souradnice_y)
                    .Where(a => a.p.je_mina == true)
                    .Count();

                return found;
            }
        }

        /// <summary>
        /// Get actual game HRA
        /// </summary>
        /// <param name="oblastID">ID of field OBLAST</param>
        /// <returns>Game info HRA</returns>
        public static HRA GetGame(int oblastID)
        {
            using (var db = new postgresEntities())
            {
                var hra = db.HRA
                    .Where(h => h.oblast == oblastID)
                    .Single();

                return hra;
            }
        }
    }
}
