//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Katalog
{
    using System;
    using System.Collections.Generic;
    
    public partial class Dorf
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dorf()
        {
            this.Objekt = new HashSet<Objekt>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual Departement Departement { get; set; }
        public virtual Sprachgruppe Sprachgruppe { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Objekt> Objekt { get; set; }
    }
}