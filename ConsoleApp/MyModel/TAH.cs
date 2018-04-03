using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public partial class TAH
    {
        public override string ToString()
        {
            string result = new StringBuilder().Append("TAH: ")
                .Append("ID - ").Append(this.tah_id)
                .Append(", OblastID - ").Append(this.oblast)
                .Append(", X - ").Append(this.souradnice_x)
                .Append(", Y - ").Append(this.souradnice_y)
                .Append(", Cas tahu - ").Append(this.cas_tahu)
                .ToString();
            return result;
        }
    }
}
