using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    /// <summary>
    /// Additional properties of Object HRA
    /// </summary>
    public partial class HRA
    {
        /// <summary>
        /// Gets time from start of game until end of game if game is finished.
        /// Or time from start until now, if game is still played.
        /// </summary>
        public int Time
        {
            get
            {
                if (this.cas_posledni_tah.HasValue && this.cas_prvni_tah.HasValue)
                {
                    TimeSpan t = this.cas_posledni_tah.Value - this.cas_prvni_tah.Value;

                    if (this.stav == (int)DBHelper.State.Playing)
                    {
                        t = DateTimeOffset.Now - this.cas_prvni_tah.Value;
                    }

                    return (int)Math.Round(t.TotalSeconds);
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets number of correctly found mines
        /// </summary>
        public int FoundMines
        {
            get
            {
                return DBHelper.GetFoundMines((int)this.oblast);
            }
        }
    }
}
