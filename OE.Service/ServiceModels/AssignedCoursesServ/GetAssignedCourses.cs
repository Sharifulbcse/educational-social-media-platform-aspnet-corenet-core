using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetAssignedCourses
    {
        public AssignedCourses courses { get; set; }
        public Classes classes { get; set; }
        public AssignedSections sections { get; set; }
        public Subjects subjects { get; set; }
    }
}
