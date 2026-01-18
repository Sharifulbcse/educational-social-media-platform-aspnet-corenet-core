
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface ISubjectsServ
    {
        #region "Get Function Definitions"
        IEnumerable<Subjects> GetSubjects();        
        Subjects GetSubjectsById(long Id);
        IEnumerable<GetSubClassSubTypes> GetSubClassSubTypes();
        IEnumerable<GetSubClassSubTypes> ClassWiseSubjectView(long classId, long institutionId);
        #endregion "Get Function Definitions"

        #region "Insert Update and Delete Function Definitions"
        string InsertSubjects(InsertSubjects obj);
        void UpdateSubjects(Subjects subjects);
        DeleteSubjects DeleteSubjects(DeleteSubjects obj);
        #endregion "Insert Update and Delete Function Definitions"

        #region "Dropdown Function Definitions"        
        IEnumerable<dropdown_Subjects> dropdown_Subjects(long institutionId, long year = 0, long classId = 0);
        IEnumerable<dropdown_Subjects> dropdown_Subjects(long institutionId, long year, long classId, long empId, long sectionId);

        #endregion "Dropdown Function Definitions"
    }
}
