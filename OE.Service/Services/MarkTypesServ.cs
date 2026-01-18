using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class MarkTypesServ : IMarkTypesServ
    {        
        #region "Variables"
        private IMarkTypesRepo<MarkTypes> _markTypesRepo;
        //private IInsCategoriesRepo<InsCategories> _insCategoriesRepo;
        #endregion "Variables"

        #region "Constructor"
        public MarkTypesServ(
            IMarkTypesRepo<MarkTypes> markTypesRepo
            //IInsCategoriesRepo<InsCategories> insCategoriesRepo
            )
        {
            _markTypesRepo = markTypesRepo;
            //_insCategoriesRepo = insCategoriesRepo;
        }
        #endregion "Constructor"

        #region "Get Methods" 
        public IEnumerable<MarkTypes> GetMarkTypes(long institutionId)        
        {
            //[NOTE: get all markTypes]
            var queryAll = _markTypesRepo.GetAll();
            var queryMarkTypes = from e in queryAll                                 
                                 where e.InstitutionId == institutionId  
                                 orderby e.Name ascending
                                 select e;
            return queryMarkTypes;
        }

        public MarkTypes GetMarkTypesById(long Id)
        {
            //[NOTE: get all markTypes]
            var queryAll = _markTypesRepo.GetAll();
            var queryMarkTypes = (from e in queryAll
                                  where e.Id == Id
                                  select e).SingleOrDefault();
            return queryMarkTypes;
        }

        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertMarkTypes(InsertMarkTypes obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'Departments' table]
                    if (obj.MarkTypes != null)
                    {
                        var markTypes = new MarkTypes()
                        {
                            InstitutionId = obj.MarkTypes.InstitutionId,
                            Name = obj.MarkTypes.Name,
                            IsActive = obj.MarkTypes.IsActive,

                            AddedBy = obj.MarkTypes.AddedBy,
                            AddedDate = obj.MarkTypes.AddedDate
                        };

                        _markTypesRepo.Insert(markTypes);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:MarkTypesServ/InsertMarkTypes - " + ex.Message;
            }

            return returnResult;

        }
        public void UpdateMarkTypes(MarkTypes markTypes)
        {
            _markTypesRepo.Update(markTypes);
        }
        public DeleteMarkTypes DeleteMarkTypes(DeleteMarkTypes obj)
        {
            var returnModel = new DeleteMarkTypes();
            try
            {
                if (obj.Id > 0)
                {
                    var markTypes = _markTypesRepo.Get(obj.Id);
                    if (markTypes != null)
                    {
                        _markTypesRepo.Delete(markTypes);
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
                    returnModel.Message = "ERROR102:MarkTypesServ/DeleteMarkTypes - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }                
            }            
            return returnModel;           
        }
        #endregion "Insert Update Delete Methods"

        #region "Dropdown Methods"        
        public IEnumerable<dropdown_MarkTypes> Dropdown_MarkTypes(long institutionId)        
        {
            
            var getMarkTypeList = _markTypesRepo.GetAll().ToList();
            var getMarkTypes = from mt in getMarkTypeList
                               where mt.InstitutionId == institutionId
                               select mt;
            var queryResult = new List<dropdown_MarkTypes>()
            {
                //new dropdown_MarkTypes{Id=0,Name="All" }
            };

            foreach (var item in getMarkTypes)
            {
                var d = new dropdown_MarkTypes()
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
