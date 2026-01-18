using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexInsPagesVM
    {
        public string InstitutionName { get; set; }
        public InsPagesListVM InsPage { get; set; }       
        public List<InsPagesListVM> InsPages { get; set; }

        //[NOTE: Extra fields]
        public IFormFile image { get; set; }
    }
}
