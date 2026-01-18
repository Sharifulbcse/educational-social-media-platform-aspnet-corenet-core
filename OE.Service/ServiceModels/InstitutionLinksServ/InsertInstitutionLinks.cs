using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class InsertInstitutionLinks
    {
        public InstitutionLinks institutionLinks { get; set; }
        //[NOTE: Extra fields]
        public IFormFile socialimage { get; set; }
    }
}
