
using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
     public class OE_Actors : BaseEntity
    {        
        public string Name { get; set; }
        public Int64 OrderNo { get; set; }
        public bool? IsActive { get; set; }
    }   
}
