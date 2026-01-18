using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetLicenses
    {
        public OE_Institutions OE_Institutions { get; set; }
        public Countries Countries { get; set; }
        public OE_Licenses Licenses { get; set; }
    }
}
