
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;
using OE.Service.CustomEntitiesServ;

namespace OE.Service
{
    public class ClassTimeScheduleActionDatesServ : CommonServ, IClassTimeScheduleActionDatesServ
    {
        #region "Variables"
        private readonly IClassTimeScheduleActionDatesRepo<ClassTimeScheduleActionDates> _classTimeScheduleActionDatesRepo;
        private readonly IOE_InstitutionsRepo<OE_Institutions> _oeInstitutionsRepo;
        #endregion "Variables"

        #region "Constructor"
        public ClassTimeScheduleActionDatesServ(
            IClassTimeScheduleActionDatesRepo<ClassTimeScheduleActionDates> classTimeScheduleActionDatesRepo,
            IOE_InstitutionsRepo<OE_Institutions> oeInstitutionsRepo
            )
        {
            _classTimeScheduleActionDatesRepo = classTimeScheduleActionDatesRepo;
            _oeInstitutionsRepo = oeInstitutionsRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public GetClassTimeScheduleActionDates GetClassTimeScheduleActionDates(long institutionId)
        {
            var institution = _oeInstitutionsRepo.Get(institutionId);
            var queryAll = _classTimeScheduleActionDatesRepo.GetAll();
            var queryResult = from e in queryAll
                              orderby e.Sorting ascending
                              select e;
            var list = new List<C_ClassTimeScheduleActionDates>();
            foreach (var item in queryResult)
            {
                var temp = new C_ClassTimeScheduleActionDates()
                {
                    Id = item.Id,
                    Sorting = item.Sorting,
                    EffectiveStartDate = Convert.ToDateTime(CommDate_ConvertToLocalDate(item.EffectiveStartDate)),
                    EffectiveEndDate = item.EffectiveEndDate != null ? Convert.ToDateTime(CommDate_ConvertToLocalDate((DateTime)item?.EffectiveEndDate)) : item.EffectiveEndDate,
                    IsActive = item.IsActive
                };
                list.Add(temp);
            }

            var returnQuery = new GetClassTimeScheduleActionDates()
            {
                _C_ClassTimeScheduleActionDates = list,
                InstitutionName = institution != null ? institution.Name : ""
            };
            return returnQuery;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertClassTimeScheduleActionDates(InsertClassTimeScheduleActionDates obj)
        {

            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    if (obj.ClassTimeScheduleActionDates != null)
                    {
                        var d = new ClassTimeScheduleActionDates()
                        {
                            EffectiveStartDate = Convert.ToDateTime(CommDate_ConvertToUtcDate(obj.ClassTimeScheduleActionDates.EffectiveStartDate)),
                            Sorting = obj.ClassTimeScheduleActionDates.Sorting,
                            IsActive = obj.ClassTimeScheduleActionDates.IsActive,
                            AddedBy = obj.ClassTimeScheduleActionDates.AddedBy,
                            AddedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.ClassTimeScheduleActionDates.AddedDate))

                        };

                        _classTimeScheduleActionDatesRepo.Insert(d);
                        returnResult = "Saved";
                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:ClassTimeScheduleActionDatesServ/InsertClassTimeScheduleActionDates - " + ex.Message;
            }
            return returnResult;
        }
        public string UpdateClassTimeScheduleActionDates(UpdateClassTimeScheduleActionDates obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj.ClassTimeScheduleActionDates != null)
                {
                    var ctsAD = _classTimeScheduleActionDatesRepo.Get(obj.ClassTimeScheduleActionDates.Id);
                    ctsAD.Sorting = obj.ClassTimeScheduleActionDates.Sorting;
                    ctsAD.EffectiveStartDate = Convert.ToDateTime(CommDate_ConvertToUtcDate(obj.ClassTimeScheduleActionDates.EffectiveStartDate));
                    ctsAD.EffectiveEndDate = obj.ClassTimeScheduleActionDates?.EffectiveEndDate != null ? Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.ClassTimeScheduleActionDates?.EffectiveEndDate)) : obj.ClassTimeScheduleActionDates.EffectiveEndDate;
                    ctsAD.ModifiedBy = obj.ClassTimeScheduleActionDates.ModifiedBy;
                    ctsAD.ModifiedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.ClassTimeScheduleActionDates.ModifiedDate));
                    _classTimeScheduleActionDatesRepo.Update(ctsAD);
                    returnResult = "Updated";
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:ClassTimeScheduleActionDatesServ/UpdateClassTimeScheduleActionDates - " + ex.Message;
            }
            return returnResult;
        }       
        public DeleteClassTimeScheduleActionDate DeleteClassTimeScheduleActionDates(DeleteClassTimeScheduleActionDate obj)
        
        {
          
            var returnModel = new DeleteClassTimeScheduleActionDate();
           
            try
            {
                if (obj.Id > 0)
               
                {
                    
                    var classTimeScheduleAD = _classTimeScheduleActionDatesRepo.Get(obj.Id);
                  
                    if (classTimeScheduleAD != null)
                    {
                        _classTimeScheduleActionDatesRepo.Delete(classTimeScheduleAD);                      
                        returnModel.Message = "Delete Successful.";
                        returnModel.SuccessIndicator = true;
                       
                    }
                }
            }
            catch (Exception ex)
            {              
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    returnModel.Message = "Record is not possible to delete, because it used in other places.";
                    returnModel.SuccessIndicator = false;
                }
                else
                {
                    returnModel.Message = "ERROR102:ClassTimeSchedulesServ/DeleteClassTimeScheduleActionDates - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }
                
            }
          
            return returnModel;
            
        }
        #endregion "Insert Update Delete Methods"
    }
}
