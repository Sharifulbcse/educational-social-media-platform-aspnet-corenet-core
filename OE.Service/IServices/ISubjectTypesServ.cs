
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface ISubjectTypesServ
    {

        #region "Get Function Definitions"
        SubjectTypes GetSubjectTypesById(long Id);
        IEnumerable<SubjectTypes> GetSubjectTypes(long institutionId);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertSubjectTypes(InsertSubjectTypes obj);
        void UpdateSubjectTypes(SubjectTypes subjectTypes);
        DeleteSubjectTypes DeleteSubjectTypes(DeleteSubjectTypes obj);
        #endregion "Insert Update Delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_SubjectTypes> Dropdown_SubjectTypes(long institutionId);
        #endregion "Dropdown Function Definitions"

    }
}
