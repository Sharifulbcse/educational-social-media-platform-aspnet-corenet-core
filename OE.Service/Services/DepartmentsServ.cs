
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class DepartmentsServ : IDepartmentsServ
    {
        
        #region "Variables"
        private IDepartmentsRepo<Departments> _departmentsRepo;
        #endregion "Variables"

        #region "Constructor"
        public DepartmentsServ(IDepartmentsRepo<Departments> departmentsRepo)
        {
            _departmentsRepo = departmentsRepo;

        }
        #endregion "Constructor"

        #region "Get Methods"        
        public IEnumerable<Departments> GetDepartments(long institutionId)       
        {
            //[NOTE: get all departments]
            var queryAll = _departmentsRepo.GetAll();
            var queryDepartments = from e in queryAll
                                   where e.InstitutionId == institutionId
                                   orderby e.Name
                                   select e;
            return queryDepartments;
        }

        public Departments GetDepartmentById(Int64 id)
        {
            //[NOTE: get all departments]
            var queryAll = _departmentsRepo.GetAll();
            var query = (from e in queryAll
                             where e.Id == id                         
                         select e).SingleOrDefault();
            return query;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertDepartments(InsertDepartments obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'Departments' table]
                    if (obj.Departments != null)
                    {
                        var dept = new Departments()
                        {
                            InstitutionId = obj.Departments.InstitutionId,
                            Name = obj.Departments.Name,
                            IsActive = obj.Departments.IsActive,

                            AddedBy = obj.Departments.AddedBy,
                            AddedDate = obj.Departments.AddedDate
                        };

                        _departmentsRepo.Insert(dept);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:DepartmentsServ/InsertDepartments - " + ex.Message;
            }

            return returnResult;

        }
        public void UpdateDepartments(Departments departments)
        {
            _departmentsRepo.Update(departments);
        }
        public DeleteDepartments DeleteDepartments(DeleteDepartments obj)        
        {
            var returnModel = new DeleteDepartments();            
            try
            {
               
                if (obj.Id > 0)
               
                {
                    var department = _departmentsRepo.Get(obj.Id);                   
                    if (department != null)
                    {

                        _departmentsRepo.Delete(department);                      
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
                    returnModel.Message = "ERROR102:DepartmentsServ/DeleteDepartments - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }
                
            }
            return returnModel;           

        }
        #endregion "Insert Update Delete Methods"

        #region "Implementation of Dropdown Methods"
        public IEnumerable<dropdown_InsCategories> Dropdown_Departments()
        {

            var getDept = _departmentsRepo.GetAll().ToList();

            var queryResult = new List<dropdown_InsCategories>();

            foreach (var item in getDept)
            {
                var d = new dropdown_InsCategories()
                {
                    Id = item.Id,
                    Name = item.Name                
                };
                queryResult.Add(d);
            }

            return queryResult;

        }
        #endregion "Implementation of Dropdown Methods"
        
    }
}
