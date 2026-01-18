using Microsoft.AspNetCore.Http;
using OE.Data;


namespace OE.Service.ServiceModels
{
    public class InsertOE_Institution
    {
        public OE_Institutions oe_Institution { get; set; }
        //[NOTE: Extra fields]
        public IFormFile logo { get; set; }
        public IFormFile favicon { get; set; }
    }
}
