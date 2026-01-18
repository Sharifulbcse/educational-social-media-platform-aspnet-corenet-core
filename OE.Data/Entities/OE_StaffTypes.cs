
using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class OE_StaffTypes : BaseEntity
    {        
        public string Name { get; set; }
        public bool? IsActive { get; set; }

    }
}
