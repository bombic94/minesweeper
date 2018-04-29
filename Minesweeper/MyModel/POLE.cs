using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Minesweeper
{
    /// <summary>
    /// Additional properties for Object POLE
    /// </summary>
    public partial class POLE
    {
        /// <summary>
        /// Gets or sets a value indicating whether is the field marked as mine
        /// </summary>
        public bool Flag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is the field wrongly marked as mine
        /// </summary>
        public bool WrongFlag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is the field mine which was not marked
        /// </summary>
        public bool NotRevealed { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether is the field mine which was stepped on
        /// </summary>
        public bool SteppedMine { get; set; }
    }
}
