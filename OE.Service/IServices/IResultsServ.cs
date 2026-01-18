
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IResultsServ
    {
        #region "Get Function Definitions"
        Gradings Gradings(long year, long institutionId, long employeeId, long classId, long subjectId, long examTypeId);
        GetStudentResultSheet GetStudentResultSheet(long institutionId, long year, long studentId, long examTypeId);
        IEnumerable<Results> GetResults();        
        Results GetResultById(long Id);        
        IEnumerable<GetStuClsXmTypSubMrkTyp> GetStuClsXmTypSubMrkTyps(long institutionId);
        IEnumerable<ResultSheet> ResultSheet(long institutionId, int year, long studentId, long classId, long examTypeId);
        IEnumerable<ResultByTeacher> ResultByTeacher(long institutionId, long year, long employeeId, long classId, long sectionId, long subjectId, long markTypeId, long examTypeId);

        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        void InsertResults(Results results);
        void InsertGradings(InsertGradings insertGradings);
        void UpdateResults(Results results);
        void DeleteResults(Results results);
        #endregion "Insert Update Delete Function Definitions"
    }
}
