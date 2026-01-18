using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class COM_RegistrationItemTypes : BaseEntity
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }

    }
}