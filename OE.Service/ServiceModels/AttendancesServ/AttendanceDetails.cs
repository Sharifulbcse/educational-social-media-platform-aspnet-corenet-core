using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class AttendanceDetails
    {
        public List<C_Attendances> _Attendances { get; set; }
        public string StudentName { get; set; }
        public string Class { get; set; }
        public string AssignedCourse { get; set; }
        public string AssignedSection { get; set; }
        public string IP300X200 { get; set; }
        public int TotalClasses { get; set; }
        public int TotalPresents { get; set; }
        public int TotalAbsents { get; set; }
    }
}

