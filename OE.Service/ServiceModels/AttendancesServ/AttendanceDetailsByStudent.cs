using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class AttendanceDetailsByStudent : MessageModel
    {
        public List<C_Attendances> _Attendances { get; set; }

        //[NOTE:Extra field from 'Students' table]
        public string StudentName { get; set; } //[NOTE:Orginal name is 'Name']
        public string IP300X200 { get; set; }

        //[NOTE:Extra field from 'Classes' table]
        public string ClassName { get; set; } //[NOTE:Orginal name is 'Name']

        //[NOTE:Extra field from 'Subjects' table]
        public string SubjectName { get; set; } //[NOTE:Orginal name is 'Name']

        //[NOTE:Extra field from 'AssignedSections' table]
        public string AssignedSectionName { get; set; } //[NOTE:Orginal name is 'Name']

        //[NOTE:Extra field from 'AttendanceCalculations' table]
        public int TotalClasses { get; set; }
        public int TotalPresents { get; set; }

        //[NOTE:Extra field]
        public int TotalAbsents { get; set; }
    }
}

