using OE.Data;
using OE.Service.CustomEntitiesServ;

using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class InsertStudents
    {
        public Students Students { get; set; }
        public OE_Users OeUsers { get; set; }
        public List<C_StudentDetails> StudentDetails { get; set; }

        //[NOTE: Extra field from StudentPromotion]
        public long RollNo { get; set; }

        //[NOTE: Extra]
        public bool EnableAuthentic { get; set; }
        public string Message { get; set; }
        public long CurrentInstitutionId { get; set; }

    }   
}
