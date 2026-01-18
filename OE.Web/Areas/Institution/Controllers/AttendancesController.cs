using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OE.Data;
using OE.Service;
using OE.Service.ServiceModels;
using OE.Web.Areas.Institution.Models;

using System;
using System.Collections.Generic;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class AttendancesController : Controller
    {
        #region "Variables"
        private readonly IAttendancesServ _attendancesServ;
        private readonly IAssignedTeachersServ _assignedTeachersServ;
        private readonly IAssignedStudentsServ _assignedStudentsServ;

        private readonly IClassTimeSchedulesServ _classTimeSchedulesServ;
        private readonly ICommonServ _commonServ;

        private readonly IEmployeesServ _employeesServ;

        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        #endregion "Variables"

        #region "Constructor"
        public AttendancesController(
           IAttendancesServ attendancesServ,
           IAssignedTeachersServ assignedTeachersServ,
           IAssignedStudentsServ assignedStudentsServ,
           IClassTimeSchedulesServ classTimeSchedulesServ,
           IOE_InstitutionsServ oeInstitutionsServ,
           IEmployeesServ employeesServ,
            ICommonServ commonServ
        )
        {
            _employeesServ = employeesServ;
            _assignedTeachersServ = assignedTeachersServ;
            _assignedStudentsServ = assignedStudentsServ;
            _classTimeSchedulesServ = classTimeSchedulesServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _attendancesServ = attendancesServ;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get_Methods- Teachers"
        public IActionResult RespectedCourses(int year = 0)
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
                            if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                            {
                                var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var courseList = _assignedTeachersServ.GetAssignedCoursesForAttendance(year, Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")), Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var list = new List<AssignedTeachersListVM>();
                                foreach (var item in courseList._CourseList)
                                {
                                    var temp = new AssignedTeachersListVM()
                                    {
                                        Id = item.Id,
                                        ClassId = item.ClassId,
                                        ClassName = item.ClassName,
                                        AssignedCourseId = item.AssignCourseId,
                                        CourseId = item.CourseId,
                                        AssignedCourseName = item.CourseName,
                                        AssignedSectionId = item.AssignSectionId,
                                        AssignedSectionName = item.AssignSectionName,
                                        InstitutionId = item.InstitutionId
                                    };
                                    list.Add(temp);
                                }
                                var model = new IndexAttendanceRespectedCoursesVM()
                                {
                                    SelectedYear = year != 0 ? year : DateTime.Now.Year,
                                    _AssignTeachers = list,
                                    EmployeeId = courseList.EmployeeId,
                                    InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                                };
                                return View("AttendanceRespectedCourses", model);

                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });

            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Results/RespectedCourses -" + ex.Message;
                return View("AttendanceRespectedCourses");
            }
        }
        public IActionResult ViewAttendanceCourses(int year = 0)
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
                            if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                            {
                                var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var courseList = _assignedTeachersServ.GetAssignedCoursesForAttendance(year, Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")), Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var list = new List<AssignedTeachersListVM>();
                                foreach (var item in courseList._CourseList)
                                {
                                    var temp = new AssignedTeachersListVM()
                                    {
                                        Id = item.Id,
                                        ClassId = item.ClassId,
                                        ClassName = item.ClassName,
                                        AssignedCourseId = item.AssignCourseId,
                                        CourseId = item.CourseId,
                                        AssignedCourseName = item.CourseName,
                                        AssignedSectionId = item.AssignSectionId,
                                        AssignedSectionName = item.AssignSectionName,
                                        InstitutionId = item.InstitutionId
                                    };
                                    list.Add(temp);
                                }
                                var model = new IndexViewAttendanceCoursesVM()
                                {
                                    SelectedYear = year != 0 ? year : DateTime.Now.Year,
                                    _AssignTeachers = list,
                                    EmployeeId = courseList.EmployeeId,
                                    InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                                };
                                return View("ViewAttendanceCourses", model);
                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Results/RespectedCourses -" + ex.Message;
                return View("ViewAttendanceCourses");
            }
        }
        public IActionResult RespectedCoursesForDelete(int year = 0)
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
                            if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                            {
                                var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var courseList = _assignedTeachersServ.GetAssignedCoursesForAttendance(year, Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")), Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var list = new List<AssignedTeachersListVM>();
                                foreach (var item in courseList._CourseList)
                                {
                                    var temp = new AssignedTeachersListVM()
                                    {
                                        Id = item.Id,
                                        ClassId = item.ClassId,
                                        ClassName = item.ClassName,
                                        AssignedCourseId = item.AssignCourseId,
                                        CourseId = item.CourseId,
                                        AssignedCourseName = item.CourseName,
                                        AssignedSectionId = item.AssignSectionId,
                                        AssignedSectionName = item.AssignSectionName,
                                        InstitutionId = item.InstitutionId
                                    };
                                    list.Add(temp);
                                }
                                var model = new IndexRespectedCoursesForDeleteVM()
                                {
                                    SelectedYear = year != 0 ? year : DateTime.Now.Year,
                                    _AssignTeachers = list,
                                    EmployeeId = courseList.EmployeeId,
                                    InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                                };
                                return View("RespectedCoursesForDelete", model);

                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCoursesForDelete -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });

            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Results/RespectedCoursesForDelete -" + ex.Message;
                return View("RespectedCoursesForDelete");
            }
        }
        public IActionResult AttendanceListForDelete(IndexRespectedCoursesForDeleteVM obj)
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
                            if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                            {
                                ViewData["Title"] = "Attendance List";
                                var attendanceList = _attendancesServ.AttendanceListForDelete(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), obj.SelectedYear, obj.ClassId, obj.AssignedCourseId, obj.AssignedSectionId, obj.EmployeeId);
                                var institution = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                                //[NOTE:Passing service Exception message]
                                if (attendanceList.Message != null)
                                {
                                    ViewBag.OeErrorMessage = attendanceList.Message;
                                }

                                var lst = new List<AttendancesListVM>();
                                foreach (var item in attendanceList._Attendances)
                                {
                                    var temp = new AttendancesListVM()
                                    {
                                        Id = item.Id,
                                        AttendanceDate = item.AttendanceDate,
                                        TimeSlot = item.TimeSlot,
                                        ClassTimeScheduleId = item.ClassTimeScheduleId,
                                        ClassId = item.ClassId,
                                        AssignedCourseId = item.AssignedCourseId,
                                        AssignedSectionId = item.AssignedSectionId,
                                        EmployeeId = item.EmployeeId
                                    };
                                    lst.Add(temp);
                                }
                                var model = new IndexAttendanceListForDeleteVM()
                                {
                                    InstitutionName = institution.Name,
                                    _Atendances = lst,
                                    ClassName = obj.ClassName,
                                    AssignedCourseName = obj.AssignedCourseName,
                                    AssignedSectionName = obj.AssignedSectionName
                                };

                                return View("AttendanceListForDelete", model);

                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:Attendances/AttendanceListForDelete -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Attendances/AttendanceListForDelete - " + ex.Message;
                return View("AttendanceListForDelete");
            }

        }
        public IActionResult AttendanceDetails(IndexViewAttendanceByTeacherVM obj)
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
                            if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                            {
                                ViewData["Title"] = "Attendance Details";
                                var studentAttendanceList = _attendancesServ.AttendanceDetails(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), obj.SelectedYear, obj.ClassId, obj.AssignedCourseId, obj.AssignedSectionId, obj.StudentId);
                                var institution = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var lst = new List<AttendancesListVM>();
                                foreach (var item in studentAttendanceList._Attendances)
                                {
                                    var temp = new AttendancesListVM()
                                    {

                                        Id = item.Id,

                                        StudentId = item.StudentId,
                                        StudentName = item.StudentName,
                                        IP300X200 = item.IP300X200,
                                        AttendanceDate = item.AttendanceDate,
                                        TimeSlot = item.TimeSlot,
                                        IsPresent = item.IsPresent
                                    };
                                    lst.Add(temp);
                                }
                                var model = new IndexAttendanceDetailsVM()
                                {
                                    InstitutionName = institution.Name,
                                    StudentId = obj.StudentId,
                                    ClassId = obj.ClassId,
                                    AssignedCourseId = obj.AssignedCourseId,
                                    AssignedSectionId = obj.AssignedSectionId,
                                    EmployeeId = obj.EmployeeId,
                                    SelectedYear = obj.SelectedYear,
                                    TotalClasses = studentAttendanceList.TotalClasses,
                                    TotalPresents = studentAttendanceList.TotalPresents,
                                    TotalAbsents = studentAttendanceList.TotalAbsents,
                                    StudentName = studentAttendanceList.StudentName,
                                    IP300X200 = studentAttendanceList.IP300X200,
                                    ClassName = studentAttendanceList.Class,
                                    AssignedCourseName = studentAttendanceList.AssignedCourse,
                                    AssignedSectionName = studentAttendanceList.AssignedSection,
                                    _Atendances = lst
                                };
                                return View("AttendanceDetails", model);
                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:Attendances/AttendanceDetails -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Attendances/AttendanceDetails - " + ex.Message;
                return View("AttendanceDetails");
            }
        }

        #endregion "Get_Methods-Teachers"

        #region "Get Methods-Students"     
        public IActionResult RespectedCoursesByStudent(int year = 0)
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
                                var courseList = _assignedStudentsServ.GetRespectedCoursesByStudent(year, Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")), Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

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
                                var model = new IndexRespectedCoursesByStudentVM()
                                {
                                    SelectedYear = year != 0 ? year : DateTime.Now.Year,
                                    _AssignStudents = list,
                                    StudentId = courseList.StudentId,
                                    InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                                };
                                return View("RespectedCoursesByStudent", model);

                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCoursesByStudent -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });

            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCoursesByStudent -" + ex.Message;
                return View("RespectedCoursesByStudent");
            }
        }
        public IActionResult StudentListForAttendance(IndexAttendanceRespectedCoursesVM obj)
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
                            if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                            {
                                ViewData["Title"] = "Student List";
                                ViewBag.Message = TempData["Message"];
                                var students = _assignedStudentsServ.GetAssignedStudentsForAttendance(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), obj.SelectedYear, obj.ClassId, obj.AssignedCourseId, obj.AssignedSectionId);
                                ViewBag.ddlTimeSchedules = _classTimeSchedulesServ.GetValidTimeSlots(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")))._TimeSlots;
                                var institution = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                                var lst = new List<AssignedStudentsListVM>();
                                foreach (var item in students._AssignedStudents)
                                {
                                    var temp = new AssignedStudentsListVM()
                                    {
                                        Id = item.Id,
                                        StudentId = item.StudentId,
                                        StudentName = item.StudentName,
                                        IP300X200 = item.IP300X200
                                    };
                                    lst.Add(temp);
                                }
                                var model = new IndexAttendanceStudentListVM()
                                {
                                    InstitutionName = institution.Name,
                                    SelectedYear = obj.SelectedYear,
                                    ClassId = obj.ClassId,
                                    AssignedCourseId = obj.AssignedCourseId,
                                    AssignedSectionId = obj.AssignedSectionId,
                                    _AssignedStudents = lst
                                };
                                return View("AttendanceStudentList", model);
                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:Attendances/StudentListForAttendance -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Attendances/StudentListForAttendance - " + ex.Message;
                return View("AttendanceStudentList");
            }
        }
        public IActionResult StudentListForViewAttendance(IndexViewAttendanceCoursesVM obj)
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
                            if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                            {
                                ViewData["Title"] = "Student Attendance List";
                                var studentAttendanceList = _attendancesServ.ViewAttendances(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), obj.SelectedYear, obj.ClassId, obj.AssignedCourseId, obj.AssignedSectionId, obj.EmployeeId);
                                var institution = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var lst = new List<AttendancesListVM>();
                                foreach (var item in studentAttendanceList._Attendances)
                                {
                                    var temp = new AttendancesListVM()
                                    {
                                        StudentId = item.StudentId,
                                        StudentName = item.StudentName,
                                        EmployeeId = item.EmployeeId,
                                        IP300X200 = item.IP300X200,
                                        ClassId = item.ClassId,
                                        AssignedCourseId = item.AssignedCourseId,
                                        AssignedSectionId = item.AssignedSectionId,
                                        TotalClasses = item.TotalClasses,
                                        TotalPresents = item.TotalPresents,
                                        TotalAbsents = item.TotalAbsents
                                    };
                                    lst.Add(temp);
                                }
                                var model = new IndexViewAttendanceByTeacherVM()
                                {
                                    SelectedYear = obj.SelectedYear,
                                    InstitutionName = institution.Name,
                                    ClassName = obj.ClassName,
                                    AssignedCourseName = obj.AssignedCourseName,
                                    AssignedSectionName = obj.AssignedSectionName,
                                    _Atendances = lst
                                };

                                return View("ViewAttendanceByTeacher", model);

                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:Attendances/ViewAttendances -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Attendances/ViewAttendances - " + ex.Message;
                return View("ViewAttendanceByTeacher");
            }

        }
        public IActionResult ViewAttendanceByStudent(IndexRespectedCoursesByStudentVM obj)
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
                                ViewData["Title"] = "Attendance List";
                                var studentAttendanceList = _attendancesServ.AttendanceDetailsByStudent(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), obj.SelectedYear, obj.ClassId, obj.AssignedCourseId, obj.AssignedSectionId, obj.StudentId);
                                if (studentAttendanceList.Message != null)
                                {
                                    ViewBag.OeErrorMessage = studentAttendanceList.Message;
                                }
                                var institution = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                var lst = new List<AttendancesListVM>();
                                foreach (var item in studentAttendanceList._Attendances)
                                {
                                    var temp = new AttendancesListVM()
                                    {
                                        Id = item.Id,
                                        StudentId = item.StudentId,
                                        StudentName = item.StudentName,
                                        IP300X200 = item.IP300X200,
                                        AttendanceDate = item.AttendanceDate,
                                        TimeSlot = item.TimeSlot,
                                        IsPresent = item.IsPresent
                                    };
                                    lst.Add(temp);
                                }
                                var model = new IndexAttendanceDetailsByStudentVM()
                                {
                                    InstitutionName = institution.Name,
                                    StudentId = obj.StudentId,
                                    ClassId = obj.ClassId,
                                    AssignedCourseId = obj.AssignedCourseId,
                                    AssignedSectionId = obj.AssignedSectionId,
                                    TotalClasses = studentAttendanceList.TotalClasses,
                                    TotalPresents = studentAttendanceList.TotalPresents,
                                    TotalAbsents = studentAttendanceList.TotalAbsents,
                                    StudentName = studentAttendanceList.StudentName,
                                    IP300X200 = studentAttendanceList.IP300X200,
                                    ClassName = studentAttendanceList.ClassName,
                                    AssignedCourseName = studentAttendanceList.SubjectName,
                                    AssignedSectionName = studentAttendanceList.AssignedSectionName,
                                    _Atendances = lst
                                };
                                return View("AttendanceDetailsByStudent", model);
                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:Attendances/AttendanceDetailsByStudent -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Attendances/AttendanceDetailsByStudent - " + ex.Message;
                return View("AttendanceDetailsByStudent");
            }
        }
        #endregion "Get Methods-Students"

        #region "Post_Methods - Teachers"
        [HttpPost]
        public IActionResult AddAttendance(IndexAttendanceStudentListVM obj)
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
                            if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                            {
                                var employeeId = _employeesServ.GetEmployeeByTeacher(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")));
                                if (obj._Atendances != null)
                                {
                                    var list = new List<Attendances>();
                                    foreach (var item in obj._Atendances)
                                    {
                                        var temp = new Attendances()
                                        {
                                            InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                            EmployeeId = employeeId.Id,
                                            StudentId = item.StudentId,
                                            ClassTimeScheduleId = obj.ClassTimeScheduleId,
                                            AssignedCourseId = obj.AssignedCourseId,
                                            ClassId = obj.ClassId,
                                            AssignedSectionId = obj.AssignedSectionId,
                                            AttendanceDate = obj.SelectedDate,
                                            IsPresent = item.IsPresent,
                                            AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                            AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                                        };
                                        list.Add(temp);
                                    }
                                    var model = new InsertAttendances()
                                    {
                                        _Attendances = list,
                                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                        ClassTimeScheduleId = obj.ClassTimeScheduleId,
                                        AssignedCourseId = obj.AssignedCourseId,
                                        ClassId = obj.ClassId,
                                        AssignedSectionId = obj.AssignedSectionId,
                                        SelectedDate = obj.SelectedDate
                                    };
                                    TempData["Message"] = _attendancesServ.InsertAttendance(model);
                                }
                                return RedirectToAction("StudentListForAttendance", new { employeeId.Id, obj.AssignedCourseId, obj.AssignedSectionId, obj.ClassId, obj.SelectedYear });
                            }
                        }
                    }
                    ViewBag.OeErrorMessage = "ERROR101:Attendances/RespectedCourses -unauthorized access is not permitted";
                    return RedirectToAction("RedirectMessage", "AllMessages", new { area = "Institution" });
                }
                ViewBag.OeErrorMessage = "ERROR101:Attendances/AddAttendances -unauthorized access is not permitted";
                return RedirectToAction("RedirectMessage", "Home", new { area = " " });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Attendances/StudentListForAttendance - " + ex.Message;
                return RedirectToAction("Attendances", "Attendances", new { area = "Institution" });
            }
        }
        [HttpPost]
        public JsonResult EditAttendance(IndexAttendanceDetailsVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            try
            {
                if (obj.Attendance != null)
                {
                    var attendance = new Attendances()
                    {
                        Id = obj.Attendance.Id,
                        IsPresent = obj.Attendance.IsPresent,
                        ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        ModifiedDate = DateTime.Now
                    };
                    var model = new UpdateAttendance()
                    {
                        Attendances = attendance
                    };
                    message = _attendancesServ.UpdateAttendance(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:AttendanceDetails/UpdateAttendance - " + ex.Message });

            }
            return result;
        }

        [HttpPost]
        public JsonResult DeleteAttendance(IndexAttendanceListForDeleteVM obj)
        {
            var result = (dynamic)null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                    {
                        if (obj.Id > 0)
                        {
                            var model = new DeleteAttendance()
                            {
                                ClassId = obj.ClassId,
                                AssignedCourseId = obj.AssignedCourseId,
                                AssignedSectionId = obj.AssignedSectionId,
                                EmployeeId = obj.EmployeeId,
                                ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                ModifiedDate = DateTime.Now,
                                AttendanceDate = obj.AttendanceDate
                            };
                            var returnResult = _attendancesServ.DeleteAttendance(model);
                            result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                            return result;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, message = "ERROR101:Attendances/Deleteattendance - " + ex.Message });
            }
            return result;
        }
        #endregion "Post_Methods-Teachers"
    }
}

