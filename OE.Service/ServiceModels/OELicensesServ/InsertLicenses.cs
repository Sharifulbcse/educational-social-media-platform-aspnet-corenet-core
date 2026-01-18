using System;
using System.Collections.Generic;
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class InsertLicenses
    {
        public OE_Licenses Licenses { get; set; }
        public OE_UserAuthentications UserAuthentications { get; set; }

        //[NOTE: Extra]
        public bool EnableAuthentic { get; set; }
        public Int64 UserId { get; set; }
        
    }
}
