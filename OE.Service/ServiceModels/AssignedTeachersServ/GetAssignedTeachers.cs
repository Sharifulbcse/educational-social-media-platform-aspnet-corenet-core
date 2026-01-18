using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetAssignedTeachers
    {
        public AssignedTeachers assignedTeachers { get; set; }
        public AssignedCourses AssignedCourses { get; set; }
        public Classes classes { get; set; }
        public AssignedSections sections { get; set; }
        public Subjects subjects { get; set; }       
        public Employees employees { get; set; }
    }
}
