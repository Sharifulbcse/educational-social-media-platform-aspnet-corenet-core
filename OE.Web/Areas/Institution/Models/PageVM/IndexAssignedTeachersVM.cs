using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexAssignedTeachersVM
    {
        public string InstitutionName { get; set; }
        public List<AssignedTeachersListVM> assignedTeachers { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public long ExamTypeId { get; set; }
        public string ExamTypeName { get; set; }
        public long ResultSeacrchYear { get; set; }

        public long SubjectId { get; set; }
        public long SectionId { get; set; }
        public long ClassId { get; set; }


        //[NOTE: Extra Fields]
        public int CurrentSearchYear { get; set; }
    }
}
