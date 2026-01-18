using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexStudentPromotionsVM
    {
        public string InstitutionName { get; set; }

        public StudentPromotionsListVM StudentPromotions { get; set; }
        public List<StudentPromotionsListVM> _StudentPromotionsListVM { get; set; }      
        
        //[NOTE: Extra Fields]
        public int SelectedYear { get; set; }
    }
}
