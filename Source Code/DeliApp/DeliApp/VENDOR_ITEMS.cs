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
    
    public partial class VENDOR_ITEMS
    {
        public int INV_ID { get; set; }
        public int VEND_ID { get; set; }
        public System.DateTime ITEM_DELIVERY_DATE { get; set; }
        public decimal ITEM_PRICE { get; set; }
    
        public virtual INVENTORY_ITEMS INVENTORY_ITEMS { get; set; }
        public virtual VENDOR VENDOR { get; set; }
    }
}
