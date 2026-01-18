using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IEmployeeDesignationsServ
    {
        #region "Get Function Definitions"
        IEnumerable<EmployeeDesignations> EmployeeDesignations();
        EmployeeDesignations GetEmployeeDesignationsById(long id);
        #endregion "Get Function Definitions"

        #region "Insert, Update and Delete Function Definitions"
        void InsertEmployeeDesignations(EmployeeDesignations desingation);
        void UpdateEmployeeDesignations(EmployeeDesignations desingation);
        void DeleteEmployeeDesignations(EmployeeDesignations desingation);
        #endregion "Insert, Update and Delete Function Definitions"  
    }
}



