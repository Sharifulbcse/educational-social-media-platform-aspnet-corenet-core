
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class GradeTypesServ : IGradeTypesServ
    {
        #region "Variables"
        private IGradeTypesRepo<GradeTypes> _gradeTypesRepo;
        private IInsCategoriesRepo<InsCategories> insCategoryRepo;
        private IClassesRepo<Classes> _classesRepo;
        #endregion "Variables"

        #region "Constructor"
        public GradeTypesServ(
            IGradeTypesRepo<GradeTypes> gradeTypesRepo,
            IInsCategoriesRepo<InsCategories> insCategoryRepo,
            IClassesRepo<Classes> classesRepo
            )
        {
            _gradeTypesRepo = gradeTypesRepo;
            this.insCategoryRepo = insCategoryRepo;
            _classesRepo = classesRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public GradeTypes GetGradeTypesById(Int64 id)
        {
            //[NOTE: get all gradetypes]
            var queryAll = _gradeTypesRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }

        public IEnumerable<GetGradeTypes> GetGradeTypes(long institutionId, long classId)
        {
            var getGrades = _gradeTypesRepo.GetAll();
            var getClasses = _classesRepo.GetAll();
            var queryGradeTypes = (dynamic)null;

            if (classId != 0)
            {
                queryGradeTypes = from g in getGrades
                                  join c in getClasses on g.ClassId equals c.Id
                                  where g.InstitutionId == institutionId
                                  orderby g.Grade ascending                                  
                                  where g.ClassId == classId                                  
                                  select new { g, c };
               
            }
            else
            {
                queryGradeTypes = from g in getGrades
                                  join c in getClasses on g.ClassId equals c.Id
                                  where g.InstitutionId == institutionId
                                  orderby g.Grade ascending
                                  select new { g, c };
            }
            
            var returnQry = new List<GetGradeTypes>();
            foreach (var item in queryGradeTypes)
            {
                var gradeType = new GetGradeTypes()
                {
                    Classes = item.c,
                    GradeTypes = item.g
                };
                returnQry.Add(gradeType);
            }
            return returnQry;

        }
        #endregion "Get Methods"

        #region "Insert Update and Delete Methods"
        public string InsertGradeTypes(GetGradeTypes obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {

                    if (obj.GradeTypes != null)
                    {
                        var gradeTypes = new GradeTypes()
                        {
                            StartMark = obj.GradeTypes.StartMark,
                            InstitutionId = obj.GradeTypes.InstitutionId,
                            EndMark = obj.GradeTypes.EndMark,
                            Grade = obj.GradeTypes.Grade,
                            GPA = obj.GradeTypes.GPA,
                            GPAOutOf = obj.GradeTypes.GPAOutOf,
                            AddedBy = obj.GradeTypes.AddedBy,

                            IsActive = true,
                            ClassId = obj.GradeTypes.ClassId,
                            AddedDate = obj.GradeTypes.AddedDate,
                            InsId = obj.GradeTypes.InsId

                        };
                        _gradeTypesRepo.Insert(gradeTypes);
                        returnResult = "Saved";
                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:GradeTypesServ/InsertGradeTypes- " + ex.Message;
            }

            return returnResult;

        }
        public void UpdateGradeTypes(GradeTypes gradeTypes)
        {
            _gradeTypesRepo.Update(gradeTypes);
        }       
        public DeleteGradeTypes DeleteGradeTypes(DeleteGradeTypes obj)       
        {            
            var returnModel = new DeleteGradeTypes();            
            try
            {                
                if (obj.Id > 0)                
                {
                    var gradeTypes = _gradeTypesRepo.Get(obj.Id);
                    if (gradeTypes != null)
                    {
                        _gradeTypesRepo.Delete(gradeTypes);
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
                    returnModel.Message = "ERROR102:GradeTypesServ/DeleteGradeTypes - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }                
            }            
            return returnModel;           
        }
        #endregion "Insert Update and Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_GradeTypes> dropdown_GradeTypes()
        {

            var getGradeType = _gradeTypesRepo.GetAll().ToList();

            var queryResult = new List<dropdown_GradeTypes>();

            foreach (var item in getGradeType)
            {
                var d = new dropdown_GradeTypes()
                {
                    Id = item.Id,
                    Name = item.Grade
                };
                queryResult.Add(d);
            }

            return queryResult;
        }
        #endregion "Dropdown Methods"
        
    }
}
