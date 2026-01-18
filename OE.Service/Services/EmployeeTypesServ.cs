
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class EmployeeTypesServ : IEmployeeTypesServ
    {
        #region "Variables"
        private IEmployeeTypesRepo<EmployeeTypes> _employeeTypesRepo;
        #endregion "Variables"

        #region "Constructor"
        public EmployeeTypesServ(
            IEmployeeTypesRepo<EmployeeTypes> employeeTypesRepo
            )
        {
            _employeeTypesRepo = employeeTypesRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public EmployeeTypes GetEmployeeTypeById(Int64 id)
        {
            //[NOTE: get all EmployeeType]
            var queryAll = _employeeTypesRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        public IEnumerable<EmployeeTypes> GetEmployeeTypes(long institutionId)
        {
            //[NOTE: get all events]
            var queryAll = _employeeTypesRepo.GetAll();
            var queryEmployeeTypes = from e in queryAll
                                     where e.InstitutionId == institutionId //[NOTE: 1= Faculty]
                                     orderby e.Name
                                     select e;
            return queryEmployeeTypes;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertEmployeeTypes(InsertEmployeeTypes obj)
        {

            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'EmployeeTypes' table]
                    if (obj.EmployeeTypes != null)
                    {
                        var empType = new EmployeeTypes()
                        {
                            InstitutionId = obj.EmployeeTypes.InstitutionId,
                            Name = obj.EmployeeTypes.Name,
                            IsActive = obj.EmployeeTypes.IsActive,

                            AddedBy = obj.EmployeeTypes.AddedBy,
                            AddedDate = obj.EmployeeTypes.AddedDate
                        };

                        _employeeTypesRepo.Insert(empType);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:EmployeeTypesServ/InsertEmployeeTypes - " + ex.Message;
            }

            return returnResult;

        }

        public void UpdateEmployeeTypes(EmployeeTypes employeeTypes)
        {
            _employeeTypesRepo.Update(employeeTypes);
        }

        public DeleteEmployeeTypes DeleteEmployeeTypes(DeleteEmployeeTypes obj)
        {
            var returnModel = new DeleteEmployeeTypes();
            try
            {              
                if (obj.Id > 0)                
                {                    
                    var empTypes = _employeeTypesRepo.Get(obj.Id);
                    if (empTypes != null)
                    {

                        _employeeTypesRepo.Delete(empTypes);
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
                    returnModel.Message = "ERROR102:EmployeeTypesServ/DeleteEmployeeTypes - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }                
            }           
            return returnModel;            
        }
        #endregion "Insert Update Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_EmployeeTypes> Dropdown_EmployeeTypes(long institutionId)
        {
            var employeeTypeQuery = _employeeTypesRepo.GetAll().ToList();
            var getemp = from e in employeeTypeQuery
                         where e.InstitutionId == institutionId
                         orderby e.Name
                         select e;
            var queryResult = new List<dropdown_EmployeeTypes>();
            foreach (var item in getemp)
            {
                var e = new dropdown_EmployeeTypes()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                queryResult.Add(e);
            };
            return queryResult;

        }

        #endregion "Dropdown Methods"
    }
}
