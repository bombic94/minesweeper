using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public partial class OMEZENI
    {
        public override string ToString()
        {
            string result = new StringBuilder().Append("OMEZENI: ")
                .Append("ID - ").Append(this.omezeni_id)
                .Append(", Sirka min - ").Append(this.sirka_min)
                .Append(", Sirka max- ").Append(this.sirka_max)
                .Append(", Vyska min - ").Append(this.vyska_min)
                .Append(", Vyska max - ").Append(this.vyska_max)
                .Append(", Pocet min min - ").Append(this.pocet_min_min)
                .Append(", Pocet min max - ").Append(this.pocet_min_max)
                .ToString();
            return result;
        }
    }
}
