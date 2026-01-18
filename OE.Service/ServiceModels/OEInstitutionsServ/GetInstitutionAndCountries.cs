using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetInstitutionAndCountries
    {
        public OE_Institutions OE_Institutions { get; set; }
        public Countries Countries { get; set; }
    }
}
