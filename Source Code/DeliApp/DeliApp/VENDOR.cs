//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeliApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class VENDOR
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public VENDOR()
        {
            this.VENDOR_ITEMS = new HashSet<VENDOR_ITEMS>();
        }
    
        public int VEND_ID { get; set; }
        public string VEND_NAME { get; set; }
        public string VEND_PHONE { get; set; }
        public string VEND_EMAIL { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VENDOR_ITEMS> VENDOR_ITEMS { get; set; }
    }
}
