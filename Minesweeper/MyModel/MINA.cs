using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    public partial class MINA
    {
        public override string ToString()
        {
            string result = new StringBuilder().Append("MINA: ")
                .Append("ID - ").Append(this.mina_id)
                .Append(", OblastID - ").Append(this.oblast)
                .Append(", X - ").Append(this.souradnice_x)
                .Append(", Y - ").Append(this.souradnice_y)
                .ToString();
            return result;
        }
    }
}
