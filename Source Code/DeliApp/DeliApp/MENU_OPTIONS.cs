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
    
    public partial class MENU_OPTIONS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MENU_OPTIONS()
        {
            this.ORDER_LINES = new HashSet<ORDER_LINES>();
            this.SANDWICHES = new HashSet<SANDWICH>();
            this.INVENTORY_ITEMS = new HashSet<INVENTORY_ITEMS>();
        }
    
        public int MENU_ID { get; set; }
        public string MENU_NAME { get; set; }
        public decimal MENU_PRICE { get; set; }
        public decimal MENU_COST { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ORDER_LINES> ORDER_LINES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SANDWICH> SANDWICHES { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<INVENTORY_ITEMS> INVENTORY_ITEMS { get; set; }
    }
}
