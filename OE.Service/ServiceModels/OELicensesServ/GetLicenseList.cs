
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetLicensesList
    {
        public OE_Licenses Licenses { get; set; }
        public OE_Institutions OEInstitutions { get; set; }
    }
}
