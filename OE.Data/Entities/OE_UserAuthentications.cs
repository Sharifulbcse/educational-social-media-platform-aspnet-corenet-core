
using System;

namespace OE.Data
{
    public class OE_UserAuthentications : BaseEntity
    {
        public Int64 ActorId { get; set; }
        public Int64 InstitutionId { get; set; }
        public Int64 UserId { get; set; }
        public bool? IsActive { get; set; }
    }
}
