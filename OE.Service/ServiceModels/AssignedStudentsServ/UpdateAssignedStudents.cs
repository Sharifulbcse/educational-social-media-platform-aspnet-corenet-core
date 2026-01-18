using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class UpdateAssignedStudents : MessageModel
    {
        public List<C_AssignedStudents> _AssignedStudents { get; set; }
    }

}

