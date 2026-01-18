
using OE.Data;
using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class Gradings
    {
        public Employees Employees { get; set; }
        public Subjects Subjects { get; set; }
        public Classes Classes { get; set; }
        public ExamTypes ExamTypes { get; set; }
        public AssignedCourses AssignedCourses { get; set; }

        public List<Students> _Students { get; set; }
        public List<ExamTypes> _ExamTypes { get; set; }
       
        public List<GradeTypes> _GradeTypes { get; set; }
        public List<Results> _Results { get; set; }
        public List<C_DistributionMarks> _DsitributionMarks { get; set; }
        public List<C_AssignedStudents> _AssignedStudents { get; set; }
        public List<C_MarkTypes> _MarkTypes { get; set; }
    }
}

