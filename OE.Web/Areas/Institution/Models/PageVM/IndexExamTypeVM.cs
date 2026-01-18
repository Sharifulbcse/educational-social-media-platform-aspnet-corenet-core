
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexExamTypeVM
    {
        public string InstitutionName { get; set; }
        public ExamTypeListVM ExamType { get; set; }
        public List<ExamTypeListVM> _ExamTypesList { get; set; }
        public long SelectedClassId { get; set; }
    }
}
