
using OE.Service.ServiceModels;
namespace OE.Service
{
    public interface IClassTimeScheduleActionDatesServ
    {
        #region "Get Function Definitions"        
        GetClassTimeScheduleActionDates GetClassTimeScheduleActionDates(long institutionId);

        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertClassTimeScheduleActionDates(InsertClassTimeScheduleActionDates obj);
        string UpdateClassTimeScheduleActionDates(UpdateClassTimeScheduleActionDates obj);
        DeleteClassTimeScheduleActionDate DeleteClassTimeScheduleActionDates(DeleteClassTimeScheduleActionDate obj);
        #endregion "Insert Update Delete Function Definitions"
    }
}
