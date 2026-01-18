using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class AttendanceListForDelete : MessageModel
    {
        public List<C_Attendances> _Attendances { get; set; }
    }
}

