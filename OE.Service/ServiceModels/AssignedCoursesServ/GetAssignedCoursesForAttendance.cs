using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetAssignedCoursesForAttendance
    {
        //[RIAZ-1/5/19 - not correct coding]
        public List<C_AssignTeachers> _CourseList { get; set; }
        //[~RIAZ-1/5/19 - not correct coding]
        //[NOTE: Extra Fields from 'Employees' Table]
        public long EmployeeId { get; set; }
    }
}

