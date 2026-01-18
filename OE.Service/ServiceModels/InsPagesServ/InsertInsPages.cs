
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class InsertInsPages
    {
        public InsPages insPages { get; set; }
        //[NOTE: Extra fields]
        public IFormFile image { get; set; }
    }
}
