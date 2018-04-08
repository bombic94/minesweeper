using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public partial class STAV
    {
        public override string ToString()
        {
            string result = new StringBuilder().Append("STAV: ")
                .Append("ID - ").Append(this.stav_id)
                .Append(", Nazev - ").Append(this.nazev_stavu)
                .ToString();
            return result;
        }
    }
}
