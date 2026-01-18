using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{

    public interface IAssignedStudentsServ
    {
        #region "Get Function Definitions"
        AssignedStudents GetAssignedStudentById(long id);
        GetAssignedStudentsForAttendance GetAssignedStudentsForAttendance(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId);

        GetSubjectList GetSubjectList(int year, long userId, long institutionId);

        GetRespectedCoursesByStudent GetRespectedCoursesByStudent(int year, long userId, long institutionId);
        GetAssignedOrUnassignedStudents GetAssignedOrUnassignedStudents(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId);

        IEnumerable<AssignedStudents> AllAssignedStudents();
        IEnumerable<GetAssignedStudents> GetAssignedStudents(long institutionId, long year, long classId, long subjectId, long markTypeId, long examTypeId, long employeeId, long assignedCourseId = 0, long assignedSectionId = 0);

        #endregion "Get Function Definitions"

        #region "Insert, Update and Delete Function Definitions"
        void InsertAssignedStudents(AssignedStudents stu);

        void UpdateAssignedStudents(AssignedStudents stu);
        UpdateAssignedStudents UpdateAssignedStudents(UpdateAssignedStudents obj);
        UpdateAssignedStudent UpdateAssignedStudent(UpdateAssignedStudent obj);

        void DeleteAssignedStudents(AssignedStudents stu);
        #endregion "Insert, Update and Delete Function Definitions"       
    }
}




