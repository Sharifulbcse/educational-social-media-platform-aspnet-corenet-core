
using System;
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IGradeTypesServ
    {
        #region "Get Function Definitions"
        GradeTypes GetGradeTypesById(Int64 id);
        IEnumerable<GetGradeTypes> GetGradeTypes(long institutionId, long classId);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertGradeTypes(GetGradeTypes obj);
        void UpdateGradeTypes(GradeTypes gradeTypes);
        DeleteGradeTypes DeleteGradeTypes(DeleteGradeTypes obj);
        #endregion "Insert Update Delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_GradeTypes> dropdown_GradeTypes();
        #endregion "Dropdown Function Definitions"

    }
}
