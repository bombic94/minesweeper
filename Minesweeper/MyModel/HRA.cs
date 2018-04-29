using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public partial class HRA
    {
        public override string ToString()
        {
            string result = new StringBuilder().Append("HRA: ")
                .Append("ID - ").Append(this.hra_id)
                .Append(", OblastID - ").Append(this.oblast)
                .Append(", StavID - ").Append(this.stav)
                .Append(", Pocet oznacenych min - ").Append(this.pocet_oznacenych_min)
                .Append(", Cas prvni tah - ").Append(this.cas_prvni_tah)
                .Append(", Cas posledni tah - ").Append(this.cas_posledni_tah)             
                .ToString();
            return result;
        }

        public double Time
        {
            get
            {
                if (cas_posledni_tah.HasValue && cas_prvni_tah.HasValue)
                {
                    TimeSpan t = cas_posledni_tah.Value - cas_prvni_tah.Value;
                    return Math.Round(t.TotalSeconds);
                }
                return 0;
            }
        }

        public int FoundMines
        {
            get
            {
                return DBHelper.GetFoundMines((int) oblast);
            }
        }
    }
}
