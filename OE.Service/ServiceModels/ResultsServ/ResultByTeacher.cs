using OE.Data;

namespace OE.Service.ServiceModels
{
    public class ResultByTeacher
    {

        public ExamTypes ExamTypes { get; set; }
        public MarkTypes MarkTypes { get; set; }
        public Employees Employees { get; set; }
        public Subjects Subjects { get; set; }
        public Students Students { get; set; }
        public Classes Classes { get; set; }

        public AssignedCourses AssignedCourses { get; set; }
        public AssignedSections AssignedSections { get; set; }
        public AssignedTeachers AssignedTeachers { get; set; }
        public AssignedStudents AssignedStudents { get; set; }

    }
}

