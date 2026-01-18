using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class SubjectTypesServ : ISubjectTypesServ
    {       
        #region "Variables"
        private readonly ISubjectTypesRepo<SubjectTypes> _subjectTypesRepo;
        #endregion "Variables"

        #region "Constructor"
        public SubjectTypesServ(
            ISubjectTypesRepo<SubjectTypes> subjectTypesRepo
            //IInsCategoriesRepo<InsCategories> InsCategoryRepo
            )
        {
            _subjectTypesRepo = subjectTypesRepo;
            //this.InsCategoryRepo = InsCategoryRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"        
        public IEnumerable<SubjectTypes> GetSubjectTypes(long institutionId)
        {
            var queryAll = _subjectTypesRepo.GetAll();
            var querySubjectTypes = from e in queryAll
                                    where e.InstitutionId == institutionId
                                    select e;
            return querySubjectTypes;
        }        
        public SubjectTypes GetSubjectTypesById(long Id)
        {
            //[NOTE: get all SubjectTypes]
            var queryAll = _subjectTypesRepo.GetAll();
            var querySubjectTypes = (from e in queryAll
                                  where e.Id == Id
                                  select e).SingleOrDefault();
            return querySubjectTypes;
        }
        #endregion "Get Methods"

        #region "Insert Update and Delete Methods"
        public string InsertSubjectTypes(InsertSubjectTypes obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'Departments' table]
                    if (obj.subjectTypes != null)
                    {
                        var subType = new SubjectTypes()
                        {
                            InstitutionId = obj.subjectTypes.InstitutionId,
                            Name = obj.subjectTypes.Name,

                            AddedBy = obj.subjectTypes.AddedBy,
                            AddedDate = obj.subjectTypes.AddedDate
                        };

                        _subjectTypesRepo.Insert(subType);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:SubjectTypesServ/InsertSubjectTypes - " + ex.Message;
            }

            return returnResult;

        }
        public void UpdateSubjectTypes(SubjectTypes subjectTypes)
        {
            _subjectTypesRepo.Update(subjectTypes);
        }
        public DeleteSubjectTypes DeleteSubjectTypes(DeleteSubjectTypes obj)
        {
           var returnModel = new DeleteSubjectTypes();
            try
            {
                if (obj.Id > 0)
                
                {
                    var subTypes = _subjectTypesRepo.Get(obj.Id);
                    
                    if (subTypes != null)
                    {
                        _subjectTypesRepo.Delete(subTypes);                        
                        returnModel.Message = "Delete Successful.";
                        returnModel.SuccessIndicator = true;                       
                    }
                }
            }
            catch (Exception ex)
            {
                // returnResult = "ERROR102:SubjectTypesServ/DeleteSubjectTypes - " + ex.Message;
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    returnModel.Message = "Record is not possible to delete, because it used in other places.";
                    returnModel.SuccessIndicator = false;
                }
                else
                {
                    returnModel.Message = "ERROR102:SubjectTypesServ/DeleteSubjectTypes - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }                
            }           
            return returnModel;
         }
        #endregion "Insert Update and Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_SubjectTypes> Dropdown_SubjectTypes(long institutionId)
        {
            var queryAll = _subjectTypesRepo.GetAll().ToList();
            var getSubjectTypes = from st in queryAll
                                  where st.InstitutionId == institutionId
                                  select st;

            var queryResult = new List<dropdown_SubjectTypes>();

            foreach (var item in getSubjectTypes)
            {
                var d = new dropdown_SubjectTypes()
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
