//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Portfolio_Manager_Main
{
    using System;
    using System.Collections.Generic;
    
    public partial class DigitalOption : Instrument
    {
        public double Strike { get; set; }
        public int Tenor { get; set; }
        public bool IsPut { get; set; }
        public double Rebate { get; set; }
    }
}