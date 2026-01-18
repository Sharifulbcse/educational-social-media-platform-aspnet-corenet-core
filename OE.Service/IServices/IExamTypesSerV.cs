
using System;
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IExamTypesServ
    {
        #region "Get Function Definitions"        
        IEnumerable<GetExamTypes> GetExamTypes(long institutionId, long classId);
        ExamTypes GetExamTypesById(Int64 id);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        string InsertExamTypes(InsertExamTypes obj);
        void UpdateExamTypes(UpdateExamTypes obj);
        DeleteExamTypes DeleteExamTypes(DeleteExamTypes obj);
        #endregion "Insert Update Delete Function Definitions"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_ExamTypes> Dropdown_ExamTypes(long institutionId);
        #endregion "Dropdown Function Definitions"

    }
}
