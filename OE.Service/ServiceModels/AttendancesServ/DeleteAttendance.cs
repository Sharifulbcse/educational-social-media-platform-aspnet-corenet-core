using OE.Data;
using System;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class DeleteAttendance : MessageModel
    {
        public long ClassId { get; set; }
        public long AssignedCourseId { get; set; }
        public long AssignedSectionId { get; set; }
        public long EmployeeId { get; set; }
        public DateTime AttendanceDate { get; set; }
        public long ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
       
    }

}

