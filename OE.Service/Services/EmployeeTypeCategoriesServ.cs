using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class EmployeeTypeCategoriesServ : IEmployeeTypeCategoriesServ
    {

        #region "Variables"
        private IEmployeeTypeCategoriesRepo<EmployeeTypeCategories> _employeeCategoriesRepo;
        private IEmployeeTypesRepo<EmployeeTypes> _employeeTypesRepo;
        #endregion "Variables"

        #region "Constructor"
        public EmployeeTypeCategoriesServ(
            IEmployeeTypeCategoriesRepo<EmployeeTypeCategories> employeeCategoriesRepo, 
            IEmployeeTypesRepo<EmployeeTypes> employeeTypesRepo
        )
        {
            _employeeCategoriesRepo = employeeCategoriesRepo;
            _employeeTypesRepo = employeeTypesRepo;

        }
        #endregion "Constructor"

        #region "Get Methods"      
        public EmployeeTypeCategories GetEmployeeTypeCategoriesById(Int64 id)
        {
            //[NOTE: get all departments]
            var queryAll = _employeeCategoriesRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        public IEnumerable<EmployeeTypeCategories> GetEmployeeTypeCategories(long institutionId)
        {
            //[NOTE: get all departments]
            var queryAll = _employeeCategoriesRepo.GetAll();
            var queryEmployeeTypeCategories = from e in queryAll
                                              where e.InstitutionId == institutionId
                                              select e;
            return queryEmployeeTypeCategories;
        }
       public IEnumerable<GetEmployeeTypeCategories> GetEmployeeType(long institutionId)
        {
            var getEmpCategoty = _employeeCategoriesRepo.GetAll().ToList();

            var getEmpType = _employeeTypesRepo.GetAll().ToList();

            var query = from e in getEmpType
                        join ec in getEmpCategoty on e.Id equals ec.EmployeeTypeId
                        where e.InstitutionId == institutionId
                        select new { e, ec };

            var queryResult = new List<GetEmployeeTypeCategories>();
            foreach (var item in query)
            {
                var obj = new GetEmployeeTypeCategories()
                {
                    EmployeeTypeCategories = item.ec,
                    EmployeeTypes = item.e
                };
                queryResult.Add(obj);
            }


            return queryResult;

        }

        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertEmployeeTypeCategories(GetEmployeeTypeCategories obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'Departments' table]
                    if (obj.EmployeeTypeCategories != null)
                    {
                        var emptypeCate = new EmployeeTypeCategories()
                        {
                            InstitutionId = obj.EmployeeTypeCategories.InstitutionId,
                            Name = obj.EmployeeTypeCategories.Name,
                            IsActive = obj.EmployeeTypeCategories.IsActive,
                            EmployeeTypeId = obj.EmployeeTypeCategories.EmployeeTypeId,
                            AddedBy = obj.EmployeeTypeCategories.AddedBy,
                            AddedDate = obj.EmployeeTypeCategories.AddedDate
                        };

                        _employeeCategoriesRepo.Insert(emptypeCate);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:EmployeeTypeCategoriesServ/InsertEmployeeTypeCategories - " + ex.Message;
            }

            return returnResult;

        }
        public void UpdateEmployeeTypeCategories(EmployeeTypeCategories employeeTypeCategories)
        {
            _employeeCategoriesRepo.Update(employeeTypeCategories);
        }
        public DeleteEmployeeTypeCategories DeleteEmployeeTypeCategories(DeleteEmployeeTypeCategories obj)
        {
           var returnModel = new DeleteEmployeeTypeCategories();
            try
            {                
                if (obj.Id > 0)                
                {                   
                    var empTypeCategories = _employeeCategoriesRepo.Get(obj.Id);                    
                    if (empTypeCategories != null)
                    {
                        _employeeCategoriesRepo.Delete(empTypeCategories);                       
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
                    returnModel.Message = "ERROR102:EmployeeTypeCategoriesServ/DeleteEmployeeTypeCategories - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }                
            }         
            return returnModel;            
        }
        #endregion "Insert Update Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_EmployeeTypeCategories> Dropdown_EmployeeTypeCategories(long instituteId, long ddlEmpTypeId)
        {
            var getAll = _employeeCategoriesRepo.GetAll();
            var getEmpCategoryType = from ctg in getAll
                                     where ctg.InstitutionId == instituteId
                                     && ctg.EmployeeTypeId == ddlEmpTypeId
                                     orderby ctg.Id
                                     select ctg;

            
            var queryResult = new List<dropdown_EmployeeTypeCategories>();
            

            foreach (var item in getEmpCategoryType)
            {
                var d = new dropdown_EmployeeTypeCategories()
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

