using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public partial class POLE
    {
        public override string ToString()
        {
            string result = new StringBuilder().Append("POLE: ")
                .Append("ID - ").Append(this.pole_id)
                .Append(", OblastID - ").Append(this.oblast)
                .Append(", X - ").Append(this.souradnice_x)
                .Append(", Y - ").Append(this.souradnice_y)
                .Append(", Je mina - ").Append(this.je_mina)
                .Append(", Odkryto - ").Append(this.odkryto)
                .Append(", Sousedni miny - ").Append(this.sousedni_miny)
                .ToString();
            return result;
        }

        public bool flag { get; set; }
    }
}
