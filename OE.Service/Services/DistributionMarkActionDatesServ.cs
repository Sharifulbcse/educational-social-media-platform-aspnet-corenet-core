using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;
using OE.Service.CustomEntitiesServ;

namespace OE.Service
{
    public class DistributionMarkActionDatesServ : CommonServ, IDistributionMarkActionDatesServ
    {
        #region "Variables"
        private readonly IDistributionMarkActionDatesRepo<DistributionMarkActionDates> _distributionMarkActionDatesRepo;
        private readonly IOE_InstitutionsRepo<OE_Institutions> _oeInstitutionsRepo;
        #endregion "Variables"

        #region "Constructor"
        public DistributionMarkActionDatesServ(
            IDistributionMarkActionDatesRepo<DistributionMarkActionDates> distributionMarkActionDatesRepo,
            IOE_InstitutionsRepo<OE_Institutions> oeInstitutionsRepo
            )
        {
            _distributionMarkActionDatesRepo = distributionMarkActionDatesRepo;
            _oeInstitutionsRepo = oeInstitutionsRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public GetDistributionMarkActionDates GetDistributionMarkActionDates(long institutionId)
        {
            var institution = _oeInstitutionsRepo.Get(institutionId);
            var queryAll = _distributionMarkActionDatesRepo.GetAll();
            var queryResult = from e in queryAll
                              select e;
            var list = new List<C_DistributionMarkActionDates>();
            foreach (var item in queryResult)
            {
                var temp = new C_DistributionMarkActionDates()
                {
                    Id = item.Id,
                    EffectiveStartDate = Convert.ToDateTime(CommDate_ConvertToLocalDate(item.EffectiveStartDate)),
                    EffectiveEndDate = item.EffectiveEndDate != null ? Convert.ToDateTime(CommDate_ConvertToLocalDate((DateTime)item?.EffectiveEndDate)) : item.EffectiveEndDate,
                    IsActive = item.IsActive
                };
                list.Add(temp);
            }

            var returnQuery = new GetDistributionMarkActionDates()
            {
                DistributionMarkActionDates = list,
                InstitutionName = institution != null ? institution.Name : ""
            };
            return returnQuery;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertDistributionMarkActionDates(InsertDistributionMarkActionDates obj)
        {

            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'DistributionMarks' table]
                    if (obj.DistributionMarkActionDates != null)
                    {
                        var d = new DistributionMarkActionDates()
                        {                           
                            EffectiveStartDate = Convert.ToDateTime(CommDate_ConvertToUtcDate(obj.DistributionMarkActionDates.EffectiveStartDate)),
                            IsActive = obj.DistributionMarkActionDates.IsActive,
                            AddedBy = obj.DistributionMarkActionDates.AddedBy,                           
                            AddedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.DistributionMarkActionDates.AddedDate))
                           
                        };

                        _distributionMarkActionDatesRepo.Insert(d);
                        returnResult = "Saved";
                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:DistributionMarkActionDatesServ/InsertDistributionMarkActionDates - " + ex.Message;
            }
            return returnResult;
        }
        public void UpdateDistributionMarkActionDates(UpdateDistributionMarkActionDates obj)
        {
            if (obj.DistributionMarkActionDates != null)
            {
                var disMAD = _distributionMarkActionDatesRepo.Get(obj.DistributionMarkActionDates.Id);
                disMAD.EffectiveStartDate = Convert.ToDateTime(CommDate_ConvertToUtcDate(obj.DistributionMarkActionDates.EffectiveStartDate));
                disMAD.EffectiveEndDate = obj.DistributionMarkActionDates?.EffectiveEndDate != null ? Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.DistributionMarkActionDates?.EffectiveEndDate)) : obj.DistributionMarkActionDates.EffectiveEndDate;
                disMAD.ModifiedBy = obj.DistributionMarkActionDates.ModifiedBy;
                disMAD.ModifiedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.DistributionMarkActionDates.ModifiedDate));
                _distributionMarkActionDatesRepo.Update(disMAD);
            }
        }
        public void DeleteDistributionMarkActionDates(DeleteDistributionMarkActionDates obj)
        {
            if (obj.DistributionMarkActionDates != null)
            {
                var distributionMarkActionDates = _distributionMarkActionDatesRepo.Get(obj.DistributionMarkActionDates.Id);
                _distributionMarkActionDatesRepo.Delete(distributionMarkActionDates);
            }
        }
        #endregion "Insert Update Delete Methods"
    }
}
