using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexDistributionMarksVM
    {
        
        public long DistributionMarkActionDateId { get; set; }
        public List<DistributionMarksListVM> distributionMarks { get; set; }
        public List<SubjectsListVM> subjects { get; set; }
        public List<MarkTypesListVM> MarkType { get; set; }
        public DistributionMarksListVM DistributionMark { get; set; }

        public SelectList _subjects { get; set; }
        public SelectList _markTypes { get; set; }

        //[NOTE: Extra properties]
        public string InstitutionName { get; set; }
        public long SelectedClassId { get; set; }
        public long SelectedSubjectId { get; set; }
        public string ClassName { get; set; }        
    }
}
