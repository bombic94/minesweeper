using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    static class DBHelper
    {
        public enum Level { Begginer=1, Advanced=2, Expert=3 }

        public enum State { Playing=1, Won=2, Lost=3 }

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
            }
            return runningGames;
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

                    Console.WriteLine(e.InnerException.Message);
                    return -1;
                }
               
            }
        }

        public static void ShowField(int obl_id, int x, int y)
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

                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.InnerException.Message);
                }
            }
        }

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

                    Console.WriteLine(e.InnerException.Message);
                }
            }
        }

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

                    Console.WriteLine(e.InnerException.Message);
                }
            }
        }

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
    }
}
