
using System;
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IClassesServ
    {
        #region "Get Function Definitions"        
        Classes GetClassById(Int64 id);        
        string ClassName(long institutionId, long id);        
        IEnumerable<Classes> GetClasses(long institutionId);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertClasses(InsertClasses obj);
        void UpdateClasses(Classes classes);
        DeleteClasses DeleteClasses(DeleteClasses obj);
        #endregion "Insert Update Delete Function Definitions"

        #region "Dropdown Function Definitions"        
        IEnumerable<dropdown_Classes> Dropdown_Classes(long institutionId);
        IEnumerable<dropdown_Classes> Dropdown_Classes(long institutionId, long empId, long year);
        #endregion "Dropdown Function Definitions"
    }
}
