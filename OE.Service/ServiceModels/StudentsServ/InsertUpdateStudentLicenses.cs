using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class InsertUpdateStudentLicenses
    {
        public List<C_StudentLicenses> _StudentLicenses { get; set; }
        //[NOTE: Extra ]
        public string errorMessage { get; set; }
    }


}