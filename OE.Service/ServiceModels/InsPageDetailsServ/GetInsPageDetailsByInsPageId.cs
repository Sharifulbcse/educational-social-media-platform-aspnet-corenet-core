
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetInsPageDetailsByInsPageId
    {
        public List<InsPageDetails> _InsPageDatailsList { get; set; }
        public long InsPageId { get; set; }
        public string InsPageTitle { get; set; }
        public string IP300X200 { get; set; }
        public string IP600X400 { get; set; }
    }
}
