
using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetSubjectList : MessageModel
    {
        public List<C_AssignedStudents> _CourseList { get; set; }

        //[NOTE:Extra field from 'Students' table]
        public long StudentId { get; set; }
        public string StudentName { get; set; }//[NOTE:Orginal name is 'Name']
    }
}
