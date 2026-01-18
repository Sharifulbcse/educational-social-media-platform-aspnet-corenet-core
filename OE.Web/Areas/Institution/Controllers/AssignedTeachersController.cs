using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class AssignedTeachersController : Controller
    {
        #region "Variables"
        
        private readonly IAssignedTeachersServ _assignedTeachersServ;
        private readonly IAssignedSectionsServ _assignedSectionsServ;
        private readonly IAssignedCoursesServ _assignedCoursesServ;

        private readonly IClassesServ _classesServ;
        private readonly ICommonServ _commonServ;

        private readonly IEmployeesServ _employeesServ;

        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
       
        private readonly ISubjectsServ _subjectsServ; 
        #endregion "Variables"

        #region "Constructor"
        public AssignedTeachersController
        (
        IAssignedCoursesServ assignedCoursesServ,
        IOE_InstitutionsServ oeInstitutionsServ,
            IAssignedTeachersServ assignedTeachersServ,
            IAssignedSectionsServ assignedSectionsServ,
            IClassesServ classesServ,
            ISubjectsServ subjectsServ,
            IEmployeesServ employeesServ,
            ICommonServ commonServ
            )
        {
            _assignedCoursesServ = assignedCoursesServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _assignedSectionsServ = assignedSectionsServ;
            _assignedTeachersServ = assignedTeachersServ;

            _classesServ = classesServ;
            _subjectsServ = subjectsServ;
            _employeesServ = employeesServ;
            _commonServ = commonServ;

        }
        #endregion "Constructor"

        #region "Get_Methods"
        [HttpGet]
        public IActionResult AssignedTeachers(long ddlClass, long ddlSubject, long ddlSection, long ddlTeacher, int year)
        {
            var fetchAssignedTeachers = _assignedTeachersServ.GetDetailsAssignedTeachers(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), ddlClass, ddlSubject, ddlSection, ddlTeacher, year);
            var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

            ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            ViewBag.ddlEmployee = _employeesServ.Dropdown_Employees(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            
            var courses = new List<AssignedTeachersListVM>();
            foreach (var item in fetchAssignedTeachers)
            {
                var e = new AssignedTeachersListVM()
                {
                    Id = item.assignedTeachers.Id,
                    Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(item.assignedTeachers.Year)),

                    ClassId = item.assignedTeachers.ClassId,
                    ClassName = item.classes.Name,
                    AssignedCourseId = item.assignedTeachers.AssignedCourseId,
                    AssignedCourseName = item.subjects.Name,
                    AssignedSectionId = item.assignedTeachers.AssignedSectionId,
                    AssignedSectionName = item.sections.Name,
                    EmployeeId = item.assignedTeachers.EmployeeId,
                    EmployeeName = item.employees.FirstName + item.employees.LastName,
                    InstitutionId = item.assignedTeachers.InstitutionId,
                    IP300X200 = item.employees.IP300X200
                };
                courses.Add(e);
            }

            //[NOTE: find search year]                   
            int currentSearchYear = year != 0 ? year : _commonServ.CommDate_CurrentYear();

            var model = new IndexAssignedTeachersVM()
            {
                InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                assignedTeachers = courses,
                CurrentSearchYear = currentSearchYear
            };
            return View("AssignedTeachers", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"       

        [HttpPost]
        public IActionResult Add(int addYear, long addClassId, long addSubjectId, long addSectionId, long addTecherId)
        {
            long lastId = (_assignedTeachersServ.AllAssignedTeachers().Count() == 0) ? 1 : _assignedTeachersServ.AllAssignedTeachers().Last().Id + 1;
            var temp = new AssignedTeachers()
            {
                Id = lastId,
                Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(new DateTime(addYear, 1, 1))),

                ClassId = addClassId,
                AssignedCourseId = addSubjectId,
                AssignedSectionId = addSectionId,
                EmployeeId = addTecherId,
                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now),

                InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))
            };
            _assignedTeachersServ.InsertAssignedTeachers(temp);
            return RedirectToAction("AssignedTeachers");
        }
        [HttpPost]
        public IActionResult Edit(long editId, int editYear, long editddlClassId, long editddlSubjectId, long editddlSectionId, long editddlTeacherId)
        {
            var assTeacher = _assignedTeachersServ.GetAssignedTeacherById(editId);
            assTeacher.Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(new DateTime(editYear, 1, 1)));

            assTeacher.ClassId = editddlClassId;
            assTeacher.AssignedCourseId = editddlSubjectId;
            assTeacher.AssignedSectionId = editddlSectionId;
            assTeacher.EmployeeId = editddlTeacherId;
            assTeacher.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            assTeacher.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

            _assignedTeachersServ.UpdateAssignedTeachers(assTeacher);
            return RedirectToAction("AssignedTeachers");
        }
        [HttpPost]
        public IActionResult Delete(long delId)
        {
            var assTeacher = _assignedTeachersServ.GetAssignedTeacherById(delId);
            _assignedTeachersServ.DeleteAssignedTeachers(assTeacher);
            return RedirectToAction("AssignedTeachers");
        }
        #endregion "Post_Methods"

        
        #region "Post Methods- dropdown"

        public JsonResult DropDown_AssignedSections(int year, long ddlClassId)
        {
            var result = (dynamic)null;
            if (year != 0 && ddlClassId != 0)
            {
                var ddlSection = _assignedSectionsServ.dropdown_AssignedSections(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), year, ddlClassId);
                result = Json(new SelectList(ddlSection, "Id", "Name"));
            }
            return result;
        }

        public JsonResult DropDown_AssignedCourses(int year, long ddlClassId, long ddlSectionId)
        {
            var result = (dynamic)null;
            if (year != 0 && ddlClassId != 0)
            {
                var ddlCourse = _assignedCoursesServ.dropdown_AssignedCourses(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), year, ddlClassId, ddlSectionId);
                result = Json(new SelectList(ddlCourse, "Id", "Name"));
            }
            return result;
        }

        #endregion "Post Methods- dropdown"
       

    }
}
