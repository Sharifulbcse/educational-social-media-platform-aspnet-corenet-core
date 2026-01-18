
using System;
using System.ComponentModel.DataAnnotations;

namespace OE.Data
{
    public class BaseEntity
    {
        [Key]
        public Int64 Id { get; set; }       
       
        public Int64 AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public string DataType { get; set; }
    }
}
