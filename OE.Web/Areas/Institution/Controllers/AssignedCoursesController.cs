
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class AssignedCoursesController : Controller
    {
        #region "Variables"
        private readonly IAssignedCoursesServ _assignedCoursesServ;
        private readonly IAssignedSectionsServ _assignedSectionsServ;
        private readonly IClassesServ _classesServ;
        private readonly ICommonServ _commonServ;
        private readonly IOE_InstitutionsServ _institutionServ;
        private readonly ISubjectsServ _subjectsServ;
        #endregion "Variables"

        #region "Constructor"
        public AssignedCoursesController(
           IClassesServ classesServ,             
            ISubjectsServ subjectsServ,
            IOE_InstitutionsServ institutionServ,
            IAssignedSectionsServ assignedSectionsServ,
            IAssignedCoursesServ assignedCoursesServ,
            ICommonServ commonServ
            )
        {
           
            _classesServ = classesServ;            
            _assignedCoursesServ = assignedCoursesServ;
            _assignedSectionsServ = assignedSectionsServ;
            _subjectsServ = subjectsServ;
            _institutionServ = institutionServ;
            _commonServ = commonServ;

        }
        #endregion "Constructor"

        #region "Get_Methods"
        public IActionResult AssignedCourses(int year, long ddlClass, long ddlSection, long ddlSubject)        
        {
            ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

            var fetchCourses = _assignedCoursesServ.GetAssignedCourses(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), ddlClass, ddlSection, year);
            var currentInstitutionDetails = _institutionServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

            var courses = new List<AssignedCoursesListVM>();
            foreach (var item in fetchCourses)
            {
                var e = new AssignedCoursesListVM()
                {
                    Id = item.courses.Id,
                    Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(item.courses.Year)),
                    ClassId = item.courses.ClassId,
                    ClassName = item.classes.Name,
                    AssignedSectionId = item.courses.AssignedSectionId,
                    SectionName = item.sections.Name,
                    SubjectId = item.courses.SubjectId,
                    CourseName = item.subjects.Name,
                    InstitutionId = item.courses.InstitutionId
                };
                courses.Add(e);
            }
            var model = new IndexAssignedCoursesVM()
            {
                courseVM = courses,
                InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
            };
            return View("AssignedCourses", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public IActionResult Add(int addYear, long addClassId, long addSectionId, long addSubjectId)
        {
            long lastId = (_assignedCoursesServ.GetAllAssignCourses().Count() == 0) ? 1 : _assignedCoursesServ.GetAllAssignCourses().Last().Id + 1;
            var temp = new AssignedCourses()
            {
                Id = lastId,
                Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(new DateTime(addYear, 1, 1))),
                ClassId = addClassId,
                AssignedSectionId = addSectionId,
                SubjectId = addSubjectId,
                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

            };
            _assignedCoursesServ.InsertAssignedCourses(temp);
            return RedirectToAction("AssignedCourses");
        }

        [HttpPost]
        public IActionResult Edit(long editId, int editYear, long editddlClassId, long editddlSectionId, long editddlCourseId)
        {
            var course = _assignedCoursesServ.GetAssignedCoursesById(editId);
            course.Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(new DateTime(editYear, 1, 1)));
            course.ClassId = editddlClassId;
            course.AssignedSectionId = editddlSectionId;
            course.SubjectId = editddlCourseId;
            course.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            course.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);
            _assignedCoursesServ.UpdateAssignedCourses(course);
            
            return RedirectToAction("AssignedCourses");
        }

        [HttpPost]
        public IActionResult Delete(long delId)
        {            
            var course = _assignedCoursesServ.GetAssignedCoursesById(delId);
            _assignedCoursesServ.DeleteAssignedCourses(course);
            
            return RedirectToAction("AssignedCourses");
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

        public JsonResult DropDown_Subjects(long ddlClassId)
        {
            var result = (dynamic)null;
            if (ddlClassId != 0)
            {
                var ddlCourse = _subjectsServ.dropdown_Subjects(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0, ddlClassId);
                result = Json(new SelectList(ddlCourse, "Id", "Name"));
            }
            return result;
        }
      
        #endregion "Post Methods- dropdown"
    }
}
