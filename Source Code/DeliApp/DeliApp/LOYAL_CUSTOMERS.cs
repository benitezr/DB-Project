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
    
    public partial class LOYAL_CUSTOMERS
    {
        public int CUS_ID { get; set; }
        public string LCUS_PHONE { get; set; }
        public string LCUS_EMAIL { get; set; }
        public string LCUS_ADDRESS { get; set; }
        public string LCUS_ZIPCODE { get; set; }
        public int LCUS_POINTS { get; set; }
    
        public virtual CUSTOMER CUSTOMER { get; set; }
    }
}
