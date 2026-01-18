
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IAssignedCoursesServ
    {
        #region "Get Function Definitions"
        AssignedCourses GetAssignedCoursesById(long id);
        GetAssignedCourses AssignedCoursesById(long id);
        IEnumerable<AssignedCourses> GetAllAssignCourses();
        IEnumerable<GetAssignedCourses> GetAssignedCourses(long instituteId, long ddlClass, long ddlSection, int year);
        #endregion "Get Function Definitions"

        #region "Insert, Update and Delete Function Definitions"
        void InsertAssignedCourses(AssignedCourses courses);
        void UpdateAssignedCourses(AssignedCourses courses);
        void DeleteAssignedCourses(AssignedCourses courses);
        #endregion "Insert, Update and Delete Methods"

        #region "Dropdown Function Definitions"
        IEnumerable<dropdown_AssignCourse> dropdown_AssignedCourses(long institutionId, long year, long classId, long assignedSectionId = 0);

        #endregion "Dropdown Function Definitions"
    }
}


