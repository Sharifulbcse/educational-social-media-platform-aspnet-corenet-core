using OE.Data;
using System.Collections.Generic;
using OE.Service.ServiceModels;
namespace OE.Service
{
    public interface IClassTimeSchedulesServ
    {
        #region "Get Function Definitions"
        GetClassTimeScheduleList GetClassTimeScheduleList(long institutionId, long ctsADId);        
        IEnumerable<ClassTimeSchedules> GetClassTimeSchedules(long institutionId);
        ClassTimeSchedules GetClassTimeScheduleById(long id);
        GetValidTimeSlots GetValidTimeSlots(long institutionId);
        #endregion "Get Function Definitions"

        #region "Insert, Update and Delete Function Definitions"       
        string InsertClassTimeSchedules(InsertClassTimeSchedules obj);
        string UpdateClassTimeSchedules(UpdateClassTimeSchedules obj);
        DeleteClassTimeSchedule DeleteClassTimeSchedules(DeleteClassTimeSchedule obj);

        #endregion "Insert, Update and Delete Function Definitions"
    }
}

