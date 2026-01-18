using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetStudentLicenses
    {
        public List<C_Students> Students { get; set; }
        //[NOTE: Extra Fields]
        public string InstitutionName { get; set; }
    }
}

