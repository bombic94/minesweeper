using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public partial class OBLAST
    {
        public override string ToString()
        {
            string result = new StringBuilder().Append("OBLAST: ")
                .Append("ID - ").Append(this.oblast_id)
                .Append(", ObtiznostID - ").Append(this.obtiznost)
                .ToString();
            return result;
        }
    }
}
