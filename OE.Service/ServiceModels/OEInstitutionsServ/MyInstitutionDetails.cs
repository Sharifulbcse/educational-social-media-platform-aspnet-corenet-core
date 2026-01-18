using System;
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class MyInstitutionDetails
    {
        public OE_Institutions OE_Institutions { get; set; }
        public OE_Licenses OELicenses { get; set; }
    }
}
