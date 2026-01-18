using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using OE.Data;
using OE.Service;
using OE.Service.CustomEntitiesServ;
using OE.Service.ServiceModels;
using OE.Web.Areas.Institution.Models;

using Rotativa.AspNetCore;

using System;
using System.Collections.Generic;
using System.Linq;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class AssignedStudentsController : Controller
    {
        #region "Variables"
        private readonly IAssignedStudentsServ _assignedStudentsServ;        
        private readonly IClassesServ _classesServ;
        private readonly IAssignedSectionsServ _assignedSectionsServ;
        private readonly IAssignedCoursesServ _assignedCoursesServ;
        private readonly IStudentsServ _studentsServ;
        private readonly ISubjectsServ _subjectsServ;
        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public AssignedStudentsController
            (
            IAssignedStudentsServ assignedStudentsServ,
           IClassesServ classesServ,           
           IAssignedSectionsServ assignedSectionsServ,
           IAssignedCoursesServ assignedCoursesServ,
            IStudentsServ studentsServ,
            ISubjectsServ subjectsServ,
            IOE_InstitutionsServ oeInstitutionsServ,
            ICommonServ commonServ
            )
        {
            _assignedStudentsServ = assignedStudentsServ;
            _classesServ = classesServ;            
            _assignedCoursesServ = assignedCoursesServ;
            _assignedSectionsServ = assignedSectionsServ;
            _studentsServ = studentsServ;
            _subjectsServ = subjectsServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get_Methods"
        public IActionResult Index()
        {
            ViewBag.dddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            return View("Index");
        }

        public IActionResult AssignedStudents(IndexAssignedStudentsSearchVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var assignedStudents = _assignedStudentsServ.GetAssignedOrUnassignedStudents(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), obj.Year, obj.ClassId, obj.AssignedCourseId, obj.AssignedSectionId);
                        var institutionName = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        var lstOfPromotionStudents = new List<StudentPromotionsListVM>();

                        if (assignedStudents._StudentPromotions != null)
                        {
                            foreach (var item in assignedStudents._StudentPromotions)
                            {
                                var sp = new StudentPromotionsListVM()
                                {
                                    Id = item.Id,
                                    InstitutionId = item.InstitutionId,
                                    StudentId = item.StudentId,
                                    StudentName = item.StudentName,
                                    ClassId = item.ClassId,
                                    RollNo = item.RollNo,
                                    Year = item.Year,
                                    IsAssigned = item.IsAssigned
                                };
                                lstOfPromotionStudents.Add(sp);
                            }
                        }
                        var model = new IndexAssignedOrUnassignedStudentsListVM()
                        {
                            Year = assignedStudents.Year,
                            ClassId = assignedStudents.Classes.Id,
                            AssignedCourseId = assignedStudents.AssignedCourses.Id,
                            AssignedSectionId = assignedStudents.AssignedSections.Id,
                            SubjectTypeId = assignedStudents.Subjects.SubjectTypeId,
                           
                            ClassName = assignedStudents.Classes.Name,
                            AssignedCourseName = assignedStudents.AssignedCourses.SubjectName,
                            AssignedSectionName = assignedStudents.AssignedSections.Name,
                            InstitutionName = institutionName.Name,
                            _StudentPromotions = lstOfPromotionStudents
                        };
                        return View("AssignedStudents", model);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:AssignedStudents/AssignedStudents - " + ex.Message;
                return View("AssignedStudents", new { area = "Institution" });
            }
            return View("AssignedStudents");
        }

        public IActionResult GetSubjectList(int year = 0)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                if (HttpContext.Session.GetString("session_currentUserAuthentications") != null)
                {
                    foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                    {
                        if (cActorId != "")
                        {
                            if (cActorId.Substring(0, cActorId.IndexOf("_")) == "10")
                            {
                                var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var courseList = _assignedStudentsServ.GetSubjectList(year, Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")), Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                                var list = new List<AssignedStudentsListVM>();
                                foreach (var item in courseList._CourseList)
                                {
                                    var temp = new AssignedStudentsListVM()
                                    {
                                        Id = item.Id,
                                        ClassId = item.ClassId,
                                        ClassName = item.ClassName,
                                        AssignedCourseId = item.AssignedCourseId,
                                        AssignedCourseName = item.SubjectName,
                                        AssignedSectionId = item.AssignedSectionId,
                                        AssignedSectionName = item.SectionName,
                                        InstitutionId = item.InstitutionId
                                    };
                                    list.Add(temp);
                                }
                                var model = new IndexSubjectListVM()
                                {
                                    SelectedYear = year != 0 ? year : DateTime.Now.Year,
                                    _AssignStudents = list,
                                    StudentId = courseList.StudentId,
                                    StudentName=courseList.StudentName,
                                    InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                                };
                                return View("GetSubjectList", model);

                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:AssignedStudents/GetSubjectList -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:AssignedStudents/GetSubjectList -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });

            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:AssignedStudents/GetSubjectList -" + ex.Message;
                return View("GetSubjectList");
            }
        }

        public IActionResult PrintSubjectList(int year = 0)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                if (HttpContext.Session.GetString("session_currentUserAuthentications") != null)
                {
                    foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                    {
                        if (cActorId != "")
                        {
                            if (cActorId.Substring(0, cActorId.IndexOf("_")) == "10")
                            {
                                var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var courseList = _assignedStudentsServ.GetSubjectList(year, Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")), Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                                var list = new List<AssignedStudentsListVM>();
                                foreach (var item in courseList._CourseList)
                                {
                                    var temp = new AssignedStudentsListVM()
                                    {
                                        Id = item.Id,
                                        ClassId = item.ClassId,
                                        ClassName = item.ClassName,
                                        AssignedCourseId = item.AssignedCourseId,
                                        AssignedCourseName = item.SubjectName,
                                        AssignedSectionId = item.AssignedSectionId,
                                        AssignedSectionName = item.SectionName,
                                        InstitutionId = item.InstitutionId
                                    };
                                    list.Add(temp);
                                }
                                var model = new IndexPrintSubjectListVM()
                                {
                                    SelectedYear = year != 0 ? year : DateTime.Now.Year,
                                    _AssignStudents = list,
                                    StudentId = courseList.StudentId,
                                    StudentName=courseList.StudentName,
                                    InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                                };
                                return new ViewAsPdf("PrintSubjectList",model);
                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:AssignedStudents/GetSubjectList -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:AssignedStudents/GetSubjectList -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });

            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:AssignedStudents/GetSubjectList -" + ex.Message;
                return View("GetSubjectList");
            }
        }
        #endregion "Get_Methods"

        #region "Post_Methods"    
        [HttpPost]       
        public IActionResult AddRemoveAssignedStudents(IndexAssignedStudentsVM stu)
        {
            if (stu._StudentsPromotion != null)
            {
                var lastId = _assignedStudentsServ.AllAssignedStudents().Count() == 0 ? 1 : _assignedStudentsServ.AllAssignedStudents().Last().Id;
                foreach (var item in stu._StudentsPromotion)
                {
                    var assStu = new AssignedStudents
                    {
                        Id = lastId += 1,
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        StudentId = item.StudentId,
                        ClassId = item.ClassId,
                        AssignedCourseId = item.AssignedCourseId,
                        AssignedSectionId = item.AssignedSectionId,
                        Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(item.Year)),

                        IsActive = true,
                        SubjectTypeId = stu.AssignedCourses.SubjectTypeId,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now),

                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))
                    };                   
                    if (item.IsAssigned)
                    {
                        _assignedStudentsServ.InsertAssignedStudents(assStu);
                    }                   

                }
            }
                        
            if (stu._AssignedStudents != null)
            {
                foreach (var item in stu._AssignedStudents)
                {
                    var fetchData = _assignedStudentsServ.GetAssignedStudentById(item.Id);
                    if (!item.IsAssigned)
                    {
                        _assignedStudentsServ.DeleteAssignedStudents(fetchData);
                    }
                }
            }

            long Year = stu.Year, ddlClass = stu.ClassId, ddlCourse = stu.AssignedCourseId, ddlSection = stu.AssignedSectionId;

            return RedirectToAction("AssignedStudents", new { Year, ddlClass, ddlCourse, ddlSection });
            
        }

       [HttpPost]
        public IActionResult EditAssignedStudents(IndexAssignedOrUnassignedStudentsListVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (obj._AssignedStudents != null)
                        {
                            var lstOfAs = new List<C_AssignedStudents>();
                            foreach (var item in obj._AssignedStudents)
                            {
                                var aS = new C_AssignedStudents()
                                {
                                    StudentId = item.StudentId,
                                    ClassId = obj.ClassId,
                                    AssignedCourseId = obj.AssignedCourseId,
                                    AssignedSectionId = obj.AssignedSectionId,
                                    SubjectTypeId = obj.SubjectTypeId,
                                    InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                    AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                    Year = new DateTime(Convert.ToInt32(obj.Year), 1, 1),
                                    IsAssigned = item.IsAssigned
                                };
                                lstOfAs.Add(aS);
                            }

                            var model = new UpdateAssignedStudents()
                            {
                                _AssignedStudents = lstOfAs
                            };
                            _assignedStudentsServ.UpdateAssignedStudents(model);
                        }
                        return RedirectToAction("AssignedStudents", new { obj.Year, obj.ClassId, obj.AssignedSectionId, obj.AssignedCourseId });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:EditAssignedStudents/AssignedStudents - " + ex.Message;
                return View("AssignedStudents", new { obj.Year, obj.ClassId, obj.AssignedSectionId, obj.AssignedCourseId });
            }
            return RedirectToAction("AssignedStudents", new { obj.Year, obj.ClassId, obj.AssignedSectionId, obj.AssignedCourseId });

        }

        [HttpPost]
        public IActionResult EditAssignedStudent(IndexAssignedOrUnassignedStudentsListVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var aS = new C_AssignedStudents()
                        {
                            StudentId = obj.StudentId,
                            ClassId = obj.ClassId,
                            AssignedCourseId = obj.AssignedCourseId,
                            AssignedSectionId = obj.AssignedSectionId,
                            SubjectTypeId = obj.SubjectTypeId,
                            InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                            AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                            Year = new DateTime(Convert.ToInt32(obj.Year), 1, 1),
                            IsAssigned = obj.IsAssigned
                        };

                        var model = new UpdateAssignedStudent()
                        {
                            AssignedStudent = aS
                        };
                        _assignedStudentsServ.UpdateAssignedStudent(model);
                        return RedirectToAction("AssignedStudents", new { obj.Year, obj.ClassId, obj.AssignedSectionId, obj.AssignedCourseId });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:EditAssignedStudent/AssignedStudents - " + ex.Message;
                return View("AssignedStudents", new { obj.Year, obj.ClassId, obj.AssignedSectionId, obj.AssignedCourseId });
            }
            return RedirectToAction("AssignedStudents", new { obj.Year, obj.ClassId, obj.AssignedSectionId, obj.AssignedCourseId });
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

