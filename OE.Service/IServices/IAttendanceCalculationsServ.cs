using OE.Data;
using System.Collections.Generic;

namespace OE.Service
{
    public interface IAttendanceCalculationsServ
    {
        #region "Get Function Definitions"    
        AttendanceCalculations GetAttendanceCalculationById(long id);
        IEnumerable<AttendanceCalculations> GetAttendanceCalculations(long institutionId);

        #endregion "Get Function Definitions"

        #region "Insert, Update and Delete Function Definitions"
        void InsertAttendanceCalculation(AttendanceCalculations attendanceCalculations);
        void UpdateAttendanceCalculation(AttendanceCalculations attendanceCalculations);
        void DeleteAttendanceCalculation(AttendanceCalculations attendanceCalculations);
        #endregion "Insert, Update and Delete Function Definitions"
    }
}


