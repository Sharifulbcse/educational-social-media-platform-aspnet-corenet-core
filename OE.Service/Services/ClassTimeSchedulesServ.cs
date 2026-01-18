using OE.Data;
using OE.Repo;
using OE.Service.CustomEntitiesServ;
using OE.Service.ServiceModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OE.Service
{
    public class ClassTimeSchedulesServ : CommonServ, IClassTimeSchedulesServ
    {

        #region "Variables"
        private readonly IOE_InstitutionsRepo<OE_Institutions> _oeInstitutionsRepo;
        private readonly IClassTimeSchedulesRepo<ClassTimeSchedules> _classTimeSchedulesRepo;
        private readonly IClassTimeScheduleActionDatesRepo<ClassTimeScheduleActionDates> _classTimeScheduleActionDatesRepo;

        #endregion "Variables"

        #region "Constructor"

        public ClassTimeSchedulesServ(
            IClassTimeSchedulesRepo<ClassTimeSchedules> classTimeSchedulesRepo,
             IClassTimeScheduleActionDatesRepo<ClassTimeScheduleActionDates> classTimeScheduleActionDatesRepo,
            IOE_InstitutionsRepo<OE_Institutions> oeInstitutionsRepo
            )
        {
            _classTimeSchedulesRepo = classTimeSchedulesRepo;
            _classTimeScheduleActionDatesRepo = classTimeScheduleActionDatesRepo;
            _oeInstitutionsRepo = oeInstitutionsRepo;
        }

        #endregion "Constructor"

        #region "Get Methods"   

        public GetClassTimeScheduleList GetClassTimeScheduleList(long institutionId, long ctsADId)
        {
            var result = (dynamic)null;
            var institution = _oeInstitutionsRepo.Get(institutionId);
            var classTimeSchedule = _classTimeSchedulesRepo.GetAll();

            var query = from cts in classTimeSchedule
                        where cts.ClassTimeScheduleActionDateId == ctsADId
                        orderby cts.Sorting ascending
                        select cts;

            if (query != null)
            {
                result = new List<C_ClassTimeSchedules>();
                foreach (var item in query)
                {
                    var temp = new C_ClassTimeSchedules()
                    {
                        Id = item.Id,                        
                        Sorting = item.Sorting,
                        ClassStartTimeSlot = DateTime.Today.Add(item.ClassStartTime).ToString("hh:mm:ss tt"),
                        ClassEndTimeSlot = DateTime.Today.Add(item.ClassEndTime).ToString("hh:mm:ss tt"),
                        ClassTimeScheduleActionDateId = item.ClassTimeScheduleActionDateId
                    };
                    result.Add(temp);
                }
            }

            var model = new GetClassTimeScheduleList()
            {
                _C_ClassTimeSchedule = result,
                InstitutionName = institution != null ? institution.Name : ""
            };
            return model;
        }
        public GetValidTimeSlots GetValidTimeSlots(long institutionId)
        {
            var getAll = _classTimeSchedulesRepo.GetAll();
            var clsTSAD = _classTimeScheduleActionDatesRepo.GetAll();

            var filter = from ctsad in clsTSAD
                         join e in getAll on ctsad.Id equals e.ClassTimeScheduleActionDateId
                         where ctsad.EffectiveStartDate <= CommDate_ConvertToLocalDate(DateTime.Now.Date)
                         && (ctsad.EffectiveEndDate >= CommDate_ConvertToLocalDate(DateTime.Now.Date) || ctsad.EffectiveEndDate == null)
                         && e.InstitutionId == institutionId
                         && e.IsActive == true
                         orderby e.Sorting ascending
                         select e;

            var list = new List<C_ClassTimeSchedules>();
            foreach (var item in filter)
            {
                var temp = new C_ClassTimeSchedules()
                {
                    Id = item.Id,
                    TimeStot = DateTime.Today.Add(item.ClassStartTime).ToString("hh:mm:ss tt") + " - " + DateTime.Today.Add(item.ClassEndTime).ToString("hh:mm:ss tt")
                };
                list.Add(temp);
            }
            var model = new GetValidTimeSlots()
            {
                _TimeSlots = list
            };
            return model;
        }
        public IEnumerable<ClassTimeSchedules> GetClassTimeSchedules(long institutionId)
        {
            //[NOTE: get all ClassTimeSchedule]
            var getAll = _classTimeSchedulesRepo.GetAll();
            var returnQry = from e in getAll
                            where e.InstitutionId == institutionId
                            orderby e.Sorting ascending
                            select e;
            return returnQry;
        }

        public ClassTimeSchedules GetClassTimeScheduleById(long id)
        {
            var getAll = _classTimeSchedulesRepo.GetAll();
            var returnQry = (from e in getAll
                             where e.Id == id
                             select e).SingleOrDefault();
            return returnQry;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"

        public string InsertClassTimeSchedules(InsertClassTimeSchedules obj)
        {

            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    if (obj.ClassTimeSchedules != null)
                    {
                        var d = new ClassTimeSchedules()
                        {
                            ClassTimeScheduleActionDateId = obj.ClassTimeSchedules.ClassTimeScheduleActionDateId,
                            Sorting = obj.ClassTimeSchedules.Sorting,
                            InstitutionId = obj.ClassTimeSchedules.InstitutionId,
                            ClassStartTime = obj.ClassTimeSchedules.ClassStartTime,
                            ClassEndTime = obj.ClassTimeSchedules.ClassEndTime,
                            IsActive = obj.ClassTimeSchedules.IsActive,
                            AddedBy = obj.ClassTimeSchedules.AddedBy,
                            AddedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.ClassTimeSchedules.AddedDate))
                        };

                        _classTimeSchedulesRepo.Insert(d);
                        returnResult = "Saved";
                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:ClassTimeSchedulesServ/InsertClassTimeSchedules - " + ex.Message;
            }
            return returnResult;
        }
        public string UpdateClassTimeSchedules(UpdateClassTimeSchedules obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj.ClassTimeSchedules != null)
                {
                    var cts = _classTimeSchedulesRepo.Get(obj.ClassTimeSchedules.Id);
                    cts.ClassStartTime = obj.ClassTimeSchedules.ClassStartTime;                   
                    cts.Sorting = obj.ClassTimeSchedules.Sorting;                    
                    cts.ClassEndTime = obj.ClassTimeSchedules.ClassEndTime;
                    cts.ModifiedBy = obj.ClassTimeSchedules.ModifiedBy;
                    cts.ModifiedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.ClassTimeSchedules.ModifiedDate));
                    _classTimeSchedulesRepo.Update(cts);
                    returnResult = "Updated";
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:ClassTimeSchedulesServ/UpdateClassTimeSchedules - " + ex.Message;
            }
            return returnResult;
        }
        
        public DeleteClassTimeSchedule DeleteClassTimeSchedules(DeleteClassTimeSchedule obj)       
        {
            var returnModel = new DeleteClassTimeSchedule();            
            try
            {                
                if (obj.Id > 0)
                
                {
                     var classTimeSchedule = _classTimeSchedulesRepo.Get(obj.Id);
                    if (classTimeSchedule != null)
                    {
                        _classTimeSchedulesRepo.Delete(classTimeSchedule);
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
                    returnModel.Message = "ERROR102:ClassTimeSchedulesServ/DeleteClassTimeSchedules- " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }
               
            }           
            return returnModel;           
        }

        #endregion "Insert Update Delete Methods"
    }
}

