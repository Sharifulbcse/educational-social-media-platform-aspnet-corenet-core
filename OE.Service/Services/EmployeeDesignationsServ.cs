using System.Linq;
using System.Collections.Generic;
using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class EmployeeDesignationsServ : CommonServ, IEmployeeDesignationsServ
    {
        #region "Variables"
        private readonly IEmployeeDesignationsRepo<EmployeeDesignations> _employeeDesignationsRepo;
        #endregion "Variables"

        #region "Constructor"
        public EmployeeDesignationsServ(
            IEmployeeDesignationsRepo<EmployeeDesignations> employeeDesignationsRepo
        )
        {
            _employeeDesignationsRepo = employeeDesignationsRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public IEnumerable<EmployeeDesignations> EmployeeDesignations()
        {
            var queryAll = _employeeDesignationsRepo.GetAll();
            var query = from e in queryAll
                        select e;
            return query;
        }
        public EmployeeDesignations GetEmployeeDesignationsById(long id)
        {
            var queryAll = _employeeDesignationsRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public void InsertEmployeeDesignations(EmployeeDesignations desingation)
        {
            _employeeDesignationsRepo.Insert(desingation);
        }
        public void UpdateEmployeeDesignations(EmployeeDesignations desingation)
        {
            _employeeDesignationsRepo.Update(desingation);
        }
        public void DeleteEmployeeDesignations(EmployeeDesignations desingation)
        {
            _employeeDesignationsRepo.Delete(desingation);
        }
        #endregion "Insert Update Delete Methods"        
    }
}


