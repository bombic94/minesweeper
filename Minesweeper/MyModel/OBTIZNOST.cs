using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public partial class OBTIZNOST
    {
        public override string ToString()
        {
            string result = new StringBuilder().Append("OBTIZNOST: ")
                .Append("ID - ").Append(this.obtiznost_id)
                .Append(", OmezeniID - ").Append(this.omezeni)
                .Append(", Nazev - ").Append(this.nazev)
                .Append(", Sirka - ").Append(this.sirka)
                .Append(", Vyska - ").Append(this.vyska)
                .Append(", Pocet min - ").Append(this.pocet_min)
                .ToString();
            return result;
        }
    }
}
