
using System;
using System.ComponentModel.DataAnnotations;
namespace OE.Data
{
    public class COM_Genders : BaseEntity
    {
        public string Name { get; set; }
        public bool? IsActive { get; set; }
    }
}
