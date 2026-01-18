using OE.Data;
using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetStudentResultSheet
    {
        public List<C_AssignedStudents> _CourseResults { get; set; }

        public long _StudentId { get; set; }
        public string _StudentName { get; set; }
        public string _ClassName { get; set; }
        public long _ExamTypeId { get; set; }
        public string _ExamTypeName { get; set; }
        public long _ResultSearchYear { get; set; }
    }
}

