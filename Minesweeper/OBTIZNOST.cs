//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Minesweeper
{
    using System;
    using System.Collections.Generic;
    
    public partial class OBTIZNOST
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OBTIZNOST()
        {
            this.OBLAST = new HashSet<OBLAST>();
        }
    
        public int obtiznost_id { get; set; }
        public string nazev { get; set; }
        public int omezeni { get; set; }
        public int sirka { get; set; }
        public int vyska { get; set; }
        public int pocet_min { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OBLAST> OBLAST { get; set; }
        public virtual OMEZENI OMEZENI1 { get; set; }
    }
}
