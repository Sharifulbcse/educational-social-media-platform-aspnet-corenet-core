
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{

    public interface IAssignedTeachersServ
    {
        #region "Get Function Definitions"
        GetCourseListForTeacher GetCourseListForTeacher(long year, long employeeId, long examTypeId);
        GetAssignedCoursesForAttendance GetAssignedCoursesForAttendance(int year, long userId, long institutionId);

        AssignedTeachers GetAssignedTeacherById(long id);
        IEnumerable<AssignedTeachers> AllAssignedTeachers();
        IEnumerable<GetAssignedTeachers> GetDetailsAssignedTeachers(long institutionId, long ddlClass, long ddlSubject, long ddlSection, long ddlTeacher, int year);
        #endregion "Get Function Definitions"

        #region "Insert, Update and Delete Function Definitions"
        void InsertAssignedTeachers(AssignedTeachers teacher);
        void UpdateAssignedTeachers(AssignedTeachers teacher);
        void DeleteAssignedTeachers(AssignedTeachers teacher);
        #endregion "Insert, Update and Delete Function Definitions"
      
    }
}


