using OE.Data;
using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetCourseListForTeacher
    {
        public List<C_AssignTeachers> _CourseList { get; set; }
        public long _ExamTypeId { get; set; }
        public string _ExamTypeName { get; set; }
        public long _EmployeeId { get; set; }
        public string _EmployeeName { get; set; }
        public long _ResultSearchYear { get; set; }
    }
}
