using OE.Service.ServiceModels;
namespace OE.Service
{
    public interface IDistributionMarkActionDatesServ
    {
        #region "Get Function Definitions"        
        GetDistributionMarkActionDates GetDistributionMarkActionDates(long institutionId);

        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertDistributionMarkActionDates(InsertDistributionMarkActionDates obj);
        void UpdateDistributionMarkActionDates(UpdateDistributionMarkActionDates distributionMarkActionDates);
        void DeleteDistributionMarkActionDates(DeleteDistributionMarkActionDates distributionMarkActionDates);
        #endregion "Insert Update Delete Function Definitions"
    }
}
