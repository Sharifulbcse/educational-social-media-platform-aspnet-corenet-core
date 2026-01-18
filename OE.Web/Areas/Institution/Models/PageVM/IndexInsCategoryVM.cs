
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexInsCategoryVM
    {
        public List<InsCategoryListVM> InsCategories{ get; set; }
        public SelectList Countries { get; set; }
    }
}
