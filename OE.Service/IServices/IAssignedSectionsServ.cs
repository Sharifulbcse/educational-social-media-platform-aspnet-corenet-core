using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IAssignedSectionsServ
    {
        #region "Get Function Definitions"
        AssignedSections GetAssignedSectionsById(long id);
        IEnumerable<AssignedSections> GetAllAssignSections();
        IEnumerable<GetSections> GetSections(long instituteId, long ddlClass, int year);
        #endregion "Get Function Definitions"

        #region "Insert, Update and Delete Function Definitions"
        void InsertAssignSections(AssignedSections assSec);
        void UpdateAssignSections(AssignedSections assSec);
        void DeleteAssignSections(AssignedSections assSec);
        #endregion "Insert, Update and Delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_AssignedSections> dropdown_AssignedSections(long institutionId, long year = 0, long classId = 0);
        IEnumerable<dropdown_AssignedSections> dropdown_AssignedSections(long institutionId, long year = 0, long employeeId = 0, long classId = 0);

        #endregion "Dropdown Function Definitions"
    }
}


