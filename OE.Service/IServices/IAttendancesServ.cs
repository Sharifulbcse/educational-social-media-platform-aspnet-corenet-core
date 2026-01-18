
using OE.Data;
using OE.Service.ServiceModels;
using System;
using System.Collections.Generic;

namespace OE.Service
{
    public interface IAttendancesServ
    {
        #region "Get Function Definitions"    
        Attendances GetAttendanceById(long id);
        IEnumerable<Attendances> GetAttendances(long institutionId);       
        ViewAttendances ViewAttendances(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId, long employeeId);
        AttendanceDetails AttendanceDetails(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId, long studentId);
        AttendanceListForDelete AttendanceListForDelete(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId, long employeeId);
        AttendanceDetailsByStudent AttendanceDetailsByStudent(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId, long studentId);
        #endregion "Get Function Definitions"

        #region "Insert, Update and Delete Function Definitions"
        string InsertAttendance(InsertAttendances obj);
        void InsertAttendance(Attendances attendance);
        string UpdateAttendance(UpdateAttendance obj);
        DeleteAttendance DeleteAttendance(DeleteAttendance obj);
        #endregion "Insert, Update and Delete Function Definitions"
    }
}

