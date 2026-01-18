using System;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexInsPageDetailsVM
    {
        public string InstitutionName { get; set; }
        public InsPageDetailsListVM InsPageDetails { get; set; }
        public List<InsPageDetailsListVM> InsPageDetailsList { get; set; }
        //Extra field from InsPages
        public long InsPageId { get; set; }
    }
}
