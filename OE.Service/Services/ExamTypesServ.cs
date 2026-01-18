
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class ExamTypesServ : IExamTypesServ
    {
        
        #region "Variables"
        private readonly IExamTypesRepo<ExamTypes> _examTypesRepo;
        private readonly IClassesRepo<Classes> _classesRepo;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public ExamTypesServ(
            IExamTypesRepo<ExamTypes> examTypesRepo,
            IClassesRepo<Classes> classesRepo,
            ICommonServ commonServ
         )
        {
            _examTypesRepo = examTypesRepo;            
           _classesRepo = classesRepo;
           _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get Methods"        
        public IEnumerable<GetExamTypes> GetExamTypes(long institutionId, long classId)        
        {            
            var queryExamTypes = (dynamic)null;
            var getExmType = _examTypesRepo.GetAll();
            var getClasses = _classesRepo.GetAll();
            if (classId != 0)
            {
                queryExamTypes = from e in getExmType
                                 join c in getClasses on e.ClassId equals c.Id
                                 where e.InstitutionId == institutionId && e.ClassId == classId
                                 orderby e.Sorting ascending
                                 select new { e, c };
            }
            else
            {
                queryExamTypes = from e in getExmType
                                 join c in getClasses on e.ClassId equals c.Id
                                 where e.InstitutionId == institutionId
                                 orderby e.Sorting ascending
                                 select new { e, c };
            }

            var returnQry = new List<GetExamTypes>();
            foreach (var item in queryExamTypes)
            {
                var temp = new GetExamTypes()
                {
                    Classes = item.c,
                    ExamTypes = item.e
                };
                returnQry.Add(temp);
            }
            return returnQry;           

        }

        public ExamTypes GetExamTypesById(Int64 id)
        {
            //[NOTE: get all ExamTypes]
            var queryAll = _examTypesRepo.GetAll();
            var query = (from e in queryAll                             
                         where e.Id == id                         
                         select e).SingleOrDefault();
            return query;
        }

        #endregion "Get Methods"

        #region "Insert update and delete Methods"
        public string InsertExamTypes(InsertExamTypes obj)
        {
           string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {

                    if (obj.ExamTypes != null)
                    {
                        var exmType = new ExamTypes()
                        {
                            Name = obj.ExamTypes.Name,
                            ClassId = obj.ExamTypes.ClassId,
                            Sorting = obj.ExamTypes.Sorting,
                            BreakDownInP = obj.ExamTypes.BreakDownInP,
                            IsLastExam = obj.ExamTypes.IsLastExam,
                            InstitutionId = obj.ExamTypes.InstitutionId,
                            IsActive = obj.ExamTypes.IsActive,
                            AddedBy = obj.ExamTypes.AddedBy,
                            AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                        };
                        _examTypesRepo.Insert(exmType);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:ExamTypesServ/InsertExamTypes- " + ex.Message;
            }
            return returnResult;           
        }
        public void UpdateExamTypes(UpdateExamTypes obj)
        {
            if (obj.ExamTypes != null)
            {
                var exmType = _examTypesRepo.Get(obj.ExamTypes.Id);
                exmType.Name = obj.ExamTypes.Name;
                exmType.ClassId = obj.ExamTypes.ClassId;
                exmType.Sorting = obj.ExamTypes.Sorting;
                exmType.BreakDownInP = obj.ExamTypes.BreakDownInP;
                exmType.IsLastExam = obj.ExamTypes.IsLastExam;
                exmType.IsActive = obj.ExamTypes.IsActive;
                exmType.ModifiedBy = obj.ExamTypes.ModifiedBy;
                exmType.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);
                _examTypesRepo.Update(exmType);
            }

        }
        public DeleteExamTypes DeleteExamTypes(DeleteExamTypes obj)        
        {            
            var returnModel = new DeleteExamTypes();
            try
            {
                if (obj.Id > 0)
                {
                    var exmType = _examTypesRepo.Get(obj.Id);                    
                    if (exmType != null)
                    {
                        _examTypesRepo.Delete(exmType);                        
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
                    returnModel.Message = "ERROR102:ExamTypesServ/DeleteExamTypes - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }                
            }            
            return returnModel;           
        }
        #endregion "nsert update and delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_ExamTypes> Dropdown_ExamTypes(long institutionId)
        {

            var getAll = _examTypesRepo.GetAll().ToList();
            var getXmTp = (from et in getAll
                           where et.InstitutionId == institutionId
                           group et by new { et.Id, et.Name } into ex
                           select ex).Select(r => r.First());

            var queryResult = new List<dropdown_ExamTypes>();

            foreach (var item in getXmTp)
            {
                var d = new dropdown_ExamTypes()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                queryResult.Add(d);
            }

            return queryResult;

        }
        #endregion "Dropdown Methods"

    }
}
