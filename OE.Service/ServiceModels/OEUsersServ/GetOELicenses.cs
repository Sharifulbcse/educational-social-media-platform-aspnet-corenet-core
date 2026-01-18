
using System;
using System.Collections.Generic;
using System.Text;
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetOELicenses
    {
        public OE_Users OEUsers { get; set; }
        public OE_UserAuthentications OEUserAuthentications { get; set; }
        public OE_Actors OEActors { get; set; }
    }
}

