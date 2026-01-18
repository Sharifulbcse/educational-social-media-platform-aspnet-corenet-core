
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using OE.Data;
using OE.Service;
using OE.Service.ServiceModels;
using OE.Web.Areas.Institution.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class ResultsController : Controller
    {
        #region "Variables"
        private readonly IResultsServ _resultsServ;

        private readonly IStudentsServ _studentsServ;
        private readonly IEmployeesServ _employeeServ;

        private readonly IClassesServ _classesServ;
        private readonly IExamTypesServ _examTypesServ;        
        private readonly ISubjectsServ _subjectsServ;
        private readonly IMarkTypesServ _markTypesServ;

        
        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        
        private readonly IAssignedStudentsServ _assignedStudentServ;
        private readonly IAssignedCoursesServ _assignedCoursesServ;
        private readonly IAssignedSectionsServ _assignedSectionsServ;
        private readonly IAssignedTeachersServ _assigneTeacherServ;
        
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public ResultsController(
            IResultsServ resultsServ,
            IStudentsServ studentsServ,
            IClassesServ classesServ,
            IExamTypesServ examTypesServ,
            ISubjectsServ subjectsServ,
            IMarkTypesServ markTypesServ,
            IOE_InstitutionsServ oeInstitutionsServ,
            IEmployeesServ employeeServ,
            IAssignedStudentsServ assignedStudentServ,
            IAssignedSectionsServ assignedSectionsServ,
            IAssignedCoursesServ assignedCoursesServ,
            IAssignedTeachersServ assignedTeacherServ,
            ICommonServ commonServ
            )
        {
            _resultsServ = resultsServ;
            _studentsServ = studentsServ;
            _classesServ = classesServ;
            _examTypesServ = examTypesServ;
            _subjectsServ = subjectsServ;
            _markTypesServ = markTypesServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _employeeServ = employeeServ;
            _assignedStudentServ = assignedStudentServ;
            _assignedSectionsServ = assignedSectionsServ;
            _assignedCoursesServ = assignedCoursesServ;
            _assigneTeacherServ = assignedTeacherServ;
            _commonServ = commonServ; ;
        }
        #endregion "Constructor"

        #region "Get_Methods"
        public IActionResult Results(long searchYear, long ddlClassId, long ddlSubjectId, long ddlMarkTypeId, long ddlExamTypeId, long ddlEmployeeId)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12" || cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                    {
                        var listResultStudent = _assignedStudentServ.GetAssignedStudents(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), searchYear, ddlClassId, ddlSubjectId, ddlMarkTypeId, ddlExamTypeId, ddlEmployeeId).ToList();
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        var results = new List<ResultsListVM>();

                        foreach (var item in listResultStudent)
                        {
                            var r = new ResultsListVM();
                            r.InstitutionId = item.Students.InstitutionId;
                            r.StudentId = item.Students.Id;
                            r.StudentName = item.Students.Name;
                            r.EmployeeId = item.Employees.Id;
                            r.EmployeeName = item.Employees.FirstName + " " + item.Employees.LastName;
                            r.ClassId = item.Classes.Id;
                            r.ClassName = item.Classes.Name;
                            r.ExamTypeId = item.ExamTypes.Id;
                            r.ExamTypeName = item.ExamTypes.Name;
                            r.SubjectId = item.Subjects.Id;
                            r.SubjectName = item.Subjects.Name;
                            r.MarkTypeId = item.MarkTypes.Id;
                            r.MarkTypeName = item.MarkTypes.Name;
                            r.Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(item.AssignedStudents.Year));

                            results.Add(r);
                        }

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexResultsVM()
                        {
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            resultsListVM = results
                        };
                        return View("Results/Results", model);
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        public IActionResult ResultSearchByRegister(long searchYear = 0, long ddlClassId = 0, long ddlSubjectId = 0, long ddlMarkTypeId = 0, long ddlExamTypeId = 0, long ddlEmployeeId = 0)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if ((searchYear == 0 && ddlClassId == 0 && ddlEmployeeId == 0 && ddlExamTypeId == 0 && ddlSubjectId == 0 && ddlMarkTypeId == 0) || (searchYear != 0 && ddlClassId == 0 && ddlEmployeeId == 0 && ddlExamTypeId == 0 && ddlSubjectId == 0 && ddlMarkTypeId == 0))
                        {
                            var ddlClasses = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0, 0).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            var model = new IndexResultsVM()
                            {
                                changeYear = searchYear,
                                _classes = new SelectList(ddlClasses, "Id", "Value")
                            };
                            return View("ResultSearchByRegister/ResultSearchByRegister", model);
                        }
                        if (searchYear != 0 && ddlClassId != 0 && ddlEmployeeId == 0 && ddlExamTypeId == 0 && ddlSubjectId == 0 && ddlMarkTypeId == 0)
                        {
                            var ddlClasses = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0, 0).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            
                            var ddlSubjects = _subjectsServ.dropdown_Subjects(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), searchYear, ddlClassId).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            var model = new IndexResultsVM()
                            {
                                changeYear = searchYear,
                                _classes = new SelectList(ddlClasses, "Id", "Value"),
                                _subject = new SelectList(ddlSubjects, "Id", "Value")
                            };
                            return View("ResultSearchByRegister/ResultSearchByRegister", model);
                        }
                        if (searchYear != 0 && ddlClassId != 0 && ddlSubjectId != 0 && ddlMarkTypeId == 0 && ddlExamTypeId == 0 && ddlEmployeeId == 0)
                        {
                            var ddlClasses = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0, 0).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            
                            var ddlSubjects = _subjectsServ.dropdown_Subjects(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), searchYear, ddlClassId).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            var ddlMrkTyp = _markTypesServ.Dropdown_MarkTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            var model = new IndexResultsVM()
                            {
                                changeYear = searchYear,
                                _classes = new SelectList(ddlClasses, "Id", "Value"),
                                _subject = new SelectList(ddlSubjects, "Id", "Value"),
                                _mrkTyp = new SelectList(ddlMrkTyp, "Id", "Value")
                            };
                            return View("ResultSearchByRegister/ResultSearchByRegister", model);
                        }
                        if (searchYear != 0 && ddlClassId != 0 && ddlSubjectId != 0 && ddlMarkTypeId != 0 && ddlExamTypeId == 0 && ddlEmployeeId == 0)
                        {
                            var ddlClasses = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0, 0).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            
                            var ddlSubjects = _subjectsServ.dropdown_Subjects(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), searchYear, ddlClassId).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            var ddlMrkTyp = _markTypesServ.Dropdown_MarkTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            var ddlXmTyp = _examTypesServ.Dropdown_ExamTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            var model = new IndexResultsVM()
                            {
                                changeYear = searchYear,
                                _classes = new SelectList(ddlClasses, "Id", "Value"),
                                _subject = new SelectList(ddlSubjects, "Id", "Value"),
                                _mrkTyp = new SelectList(ddlMrkTyp, "Id", "Value"),
                                _xmTyp = new SelectList(ddlXmTyp, "Id", "Value")
                            };
                            return View("ResultSearchByRegister/ResultSearchByRegister", model);
                        }
                        if (searchYear != 0 && ddlClassId != 0 && ddlSubjectId != 0 && ddlMarkTypeId != 0 && ddlExamTypeId != 0 && ddlEmployeeId == 0)
                        {
                            var ddlClasses = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0, 0).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            
                            var ddlSubjects = _subjectsServ.dropdown_Subjects(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), searchYear, ddlClassId).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                          
                            var ddlMrkTyp = _markTypesServ.Dropdown_MarkTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            var ddlXmTyp = _examTypesServ.Dropdown_ExamTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            var ddlEmp = _employeeServ.dropdown_EmployeeWithAssignTeacher(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), searchYear, ddlClassId, ddlSubjectId).OrderBy(s => s.Name).Select(x => new { Id = x.Id, Value = x.Name });
                            var model = new IndexResultsVM()
                            {
                                changeYear = searchYear,
                                _classes = new SelectList(ddlClasses, "Id", "Value"),
                                _subject = new SelectList(ddlSubjects, "Id", "Value"),
                                _mrkTyp = new SelectList(ddlMrkTyp, "Id", "Value"),
                                _xmTyp = new SelectList(ddlXmTyp, "Id", "Value"),
                                _employee = new SelectList(ddlEmp, "Id", "Value")
                            };
                            return View("ResultSearchByRegister/ResultSearchByRegister", model);
                        }
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                return RedirectToAction("Login", "Home", new { area = "" });
            }

        }
        public IActionResult ResultSheet(int year)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12" || cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                    {
                        var listResultSheet = _resultsServ.ResultSheet(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), year, 0, 0, 0).ToList();
                        var resultSheet = new List<ResultsListVM>();

                        foreach (var item in listResultSheet)
                        {
                            var r = new ResultsListVM();
                            r.Id = item.results.Id;
                            r.InstitutionId = item.results.InstitutionId;
                            r.ClassId = item.results.ClassId;
                            r.ClassName = item.classes.Name;
                            r.StudentId = item.results.StudentId;
                            r.StudentName = item.students.Name;
                            r.EmployeeId = item.results.EmployeeId;
                            r.EmployeeName = item.employees.FirstName;
                            r.SubjectId = item.results.SubjectId;
                            r.SubjectName = item.subjects.Name;
                            r.MarkTypeId = item.results.MarkTypeId;
                            r.MarkTypeName = item.markTypes.Name;
                            r.ExamTypeId = item.results.ExamTypeId;
                            r.ExamTypeName = item.examTypes.Name;
                            r.Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(item.results.Year));

                            r.Mark = item.results.Mark;
                            resultSheet.Add(r);
                        }

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexResultsVM()
                        {
                            resultsListVM = resultSheet
                        };
                        return View("ResultSheet/ResultSheet", model);
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        public IActionResult Grading(IndexResultsVM indexResult)
        {
            var getGrading = _resultsServ.Gradings(indexResult.SelectedYear, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), indexResult.EmployeeId, indexResult.ClassId, indexResult.SubjectId, indexResult.ExamTypeId);

            var xmTyp = new List<ExamTypeListVM>();
            foreach (var item in getGrading._ExamTypes)
            {
                var temp = new ExamTypeListVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    BreakDownInP = item.BreakDownInP,
                    IsLastExam = item.IsLastExam
                };
                xmTyp.Add(temp);
            }

            var mrkTyp = new List<MarkTypesListVM>();
            foreach (var item in getGrading._MarkTypes)
            {
                var temp = new MarkTypesListVM()
                {
                    Id = item.Id,
                    Name = item.Name,                   
                    BreakDownInP = item.BreakDownInP                    

                };
                mrkTyp.Add(temp);
            }

            var mrkDis = new List<DistributionMarksListVM>();
            foreach (var item in getGrading._DsitributionMarks)
            {
                var temp = new DistributionMarksListVM()
                {
                    MarkTypeName = item.MarkTypeName,
                    BreakDownInP = item.BreakDownInP
                };
                mrkDis.Add(temp);
            }

            var grd = new List<GradeTypesListVM>();
            foreach (var item in getGrading._GradeTypes)
            {
                var temp = new GradeTypesListVM()
                {
                    StartMark = item.StartMark,
                    EndMark = item.EndMark,
                    Grade = item.Grade,
                    GPA = item.GPA,
                    GPAOutOf = item.GPAOutOf
                };
                grd.Add(temp);
            }

            var results = new List<ResultsListVM>();
            foreach (var item in getGrading._Results)
            {
                var temp = new ResultsListVM()
                {
                    Id = item.Id,
                    StudentId = item.StudentId,
                    EmployeeId = item.EmployeeId,
                    ExamTypeId = item.ExamTypeId,
                    SubjectId = item.SubjectId,
                    MarkTypeId = item.MarkTypeId,
                    Year = item.Year,
                    Mark = item.Mark
                };
                results.Add(temp);
            }

            var assStu = new List<AssignedStudentsListVM>();
            foreach (var item in getGrading._AssignedStudents)
            {
                var temp = new AssignedStudentsListVM()
                {
                    Id = item.Id,
                    StudentId = item.StudentId,
                    StudentName = item.StudentName
                };
                assStu.Add(temp);
            }

            var model = new IndexResultsVM()
            {
                SelectedYear = indexResult.SelectedYear,
                EmployeeId = getGrading.Employees.Id,
                EmployeeName = getGrading.Employees.FirstName + " " + getGrading.Employees.LastName,
                ClassId = getGrading.Classes.Id,
                ClassName = getGrading.Classes.Name,
                SubjectId = getGrading.Subjects.Id,
                SubjectName = getGrading.Subjects.Name,
                ExamTypeId = getGrading.ExamTypes.Id,
                ExamTypeName = getGrading.ExamTypes.Name,               
                AssignedCourseId = getGrading.AssignedCourses.Id,
                _ExamTypes = xmTyp,
                _MarkTypes = mrkTyp,
                _DistributionMarks = mrkDis,
                _GradeTypes = grd,
                _AssignedStudents = assStu,
                _Results = results
            };



            return View("Grading/Grading", model);
        }
        #endregion "Get_Methods"

        #region "Get_Methods-student"

        public IActionResult ResultSearchByStudent()
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "10")
                    {
                        var student = _studentsServ.GetStudentsById(0, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")));
                        ViewBag.ddlExamTypeId = _examTypesServ.Dropdown_ExamTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        var model = new IndexResultsVM()
                        {
                            StudentId = Convert.ToInt64(student.Id),
                            StudentName = student.Name,
                            CurrentYear = DateTime.Now.Year
                        };
                        return View("ResultSearchByStudent/ResultSearchByStudent", model);
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex;
                return RedirectToAction("Login", "Home", new { area = "" });
            }

        }

        public IActionResult ResultForStudent(IndexResultsVM indexResultVM)
        {
            try
            {
                ViewBag.OeErrorMessage = null;
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "10")
                    {
                        var listResult = _resultsServ.GetStudentResultSheet(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), indexResultVM.CurrentYear, indexResultVM.StudentId, indexResultVM.ExamTypeId);
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        var resultSheet = new List<ResultsListVM>();
                        
                        foreach (var item in listResult._CourseResults)
                        {
                            var r = new ResultsListVM();
                            r.Mark = item.TotalMark;
                            r.Grade = item.Grade;
                            r.SubjectName = item.SubjectName;
                            resultSheet.Add(r);
                        }
                        

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexResultsVM()
                        {
                            resultsListVM = resultSheet,
                            StudentId = listResult._StudentId,
                            StudentName = listResult._StudentName,
                            ExamTypeId = listResult._ExamTypeId,
                            ExamTypeName = listResult._ExamTypeName,
                            ClassName = listResult._ClassName,
                            SelectedYear = listResult._ResultSearchYear,
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                        };
                        return View("ResultForStudent/ResultForStudent", model);
                    }

                }

                ViewBag.OeErrorMessage = "ERROR101:Results/ResultForStudent -unauthorized access is not permitted";
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Results/ResultSearchByStudent - " + ex.Message;
                return View("ResultForStudent/ResultForStudent");
            }

        }
        #endregion "Get_Methods-student"

        #region "Get_Methods_Teacher"        
        public IActionResult ResultSearchByTeacher()
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                    {
                        var emp = _employeeServ.GetEmployeeByTeacher(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")));
                        ViewBag.ddlExamTypeId = _examTypesServ.Dropdown_ExamTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        
                        var model = new IndexResultsVM();
                        if (emp != null)
                        {
                            model = new IndexResultsVM()
                            {
                                EmployeeId = emp.Id,
                                EmployeeName = emp.FirstName + " " + emp.LastName,
                                CurrentYear = DateTime.Now.Year
                            };
                        }
                        else
                        {
                            ViewBag.OeErrorMessage = "ERROR101:Results/ResultSearchByTeacher -teacher license is not valid.";
                        }

                        return View("ResultSearchByTeacher/ResultSearchByTeacher", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:Results/ResultSearchByTeacher -unauthorized access is not permitted";
                return View("ResultSearchByTeacher/ResultSearchByTeacher");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Results/ResultSearchByTeacher - " + ex.Message;
                return View("ResultSearchByTeacher/ResultSearchByTeacher");
            }

        }

        public IActionResult ResultByTeacher(IndexResultsVM indexResult)
        {           
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                    {
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        //[NOTE:Get courses for specific teacher from assign teacher]
                        var courseList = _assigneTeacherServ.GetCourseListForTeacher(indexResult.CurrentYear, indexResult.EmployeeId, indexResult.ExamTypeId);
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
                        var model = new IndexAssignedTeachersVM()
                        {
                            assignedTeachers = list,
                            EmployeeId = courseList._EmployeeId,
                            EmployeeName = courseList._EmployeeName,
                            ExamTypeId = courseList._ExamTypeId,
                            ExamTypeName = courseList._ExamTypeName,
                            ResultSeacrchYear = courseList._ResultSearchYear,
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                        };

                        return View("ResultByTeacher/ResultByTeacher", model);

                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:Results/ResultByTeacher -unauthorized access is not permitted";
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Results/ResultByTeacher -" + ex.Message;
                return RedirectToAction("Login", "Home", new { area = "" });
            }           
        }

        #endregion "Get_Method_Teacher"

        #region "Post_Methods"

        [HttpPost]
        public IActionResult AddResult(IndexResultsVM resultList)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12" || cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                    {
                        for (int i = 0; i < resultList.resultsListVM.Count; i++)
                        {
                            var temp = new Results()
                            {
                                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                StudentId = resultList.resultsListVM[i].StudentId,
                                EmployeeId = resultList.resultsListVM[i].EmployeeId,
                                ClassId = resultList.resultsListVM[i].ClassId,
                                ExamTypeId = resultList.resultsListVM[i].ExamTypeId,
                                SubjectId = resultList.resultsListVM[i].SubjectId,
                                MarkTypeId = resultList.resultsListVM[i].MarkTypeId,
                                Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(DateTime.Now)),
                                AddedDate = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(DateTime.Now)),

                                Mark = resultList.resultsListVM[i].Mark,
                                IsActive = true,
                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                ModifiedBy = 0
                            };
                            _resultsServ.InsertResults(temp);
                        }

                        return RedirectToAction("ResultSheet");
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        [HttpPost]
        public IActionResult Edit(Int64 editId, Int64 ddlEditStudentsId, Int64 ddlEditClassesId, Int64 ddlEditXmTypId, Int64 ddlEditSubjectsId, Int64 ddlEditMrkTypId, Int64 editMark, DateTime editYear)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12" || cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                    {
                        var fetchRecord = _resultsServ.GetResultById(editId);
                        fetchRecord.SubjectId = ddlEditStudentsId;
                        fetchRecord.ClassId = ddlEditClassesId;
                        fetchRecord.ExamTypeId = ddlEditXmTypId;
                        fetchRecord.SubjectId = ddlEditSubjectsId;
                        fetchRecord.MarkTypeId = ddlEditMrkTypId;
                        fetchRecord.Mark = editMark;
                        fetchRecord.Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(editYear));
                        fetchRecord.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        fetchRecord.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        _resultsServ.UpdateResults(fetchRecord);

                        return RedirectToAction("Results");
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        [HttpPost]
        public IActionResult Delete(Int64 deleteId)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12" || cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                    {
                        var fetchRecord = _resultsServ.GetResultById(deleteId);
                        _resultsServ.DeleteResults(fetchRecord);

                        return RedirectToAction("Results");
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    TempData["error"] = "Results is not possible to delete because it is used another place";
                    return RedirectToAction("Classes");
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }

        #endregion "Post_Methods"

        #region "Post_Mthod_Teacher"
        [HttpPost]
        public IActionResult AddResultByTeacher(IndexResultsVM resultList)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "14")
                    {
                        for (int i = 0; i < resultList.resultsListVM.Count; i++)
                        {
                            var temp = new Results()
                            {
                                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                StudentId = resultList.resultsListVM[i].StudentId,
                                EmployeeId = resultList.resultsListVM[i].EmployeeId,
                                ClassId = resultList.resultsListVM[i].ClassId,
                                ExamTypeId = resultList.resultsListVM[i].ExamTypeId,
                                SubjectId = resultList.resultsListVM[i].SubjectId,
                                MarkTypeId = resultList.resultsListVM[i].MarkTypeId,
                                Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(DateTime.Now)),
                                AddedDate = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(DateTime.Now)),

                                Mark = resultList.resultsListVM[i].Mark,
                                IsActive = true,
                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                ModifiedBy = 0
                            };
                            _resultsServ.InsertResults(temp);
                        }
                        TempData["success"] = "Results are successfully added";
                        return RedirectToAction("ResultByTeacher");
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
    
        [HttpPost]
        public IActionResult AddGrading(IndexResultsVM resultList)
        {
            if (resultList.resultsListVM != null)
            {
                var insertGradings = new List<Results>();
                foreach (var item in resultList.resultsListVM)
                {
                    var temp = new Results()
                    {
                        Id = item.Id,
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        StudentId = item.StudentId,
                        EmployeeId = item.EmployeeId,
                        ClassId = item.ClassId,
                        ExamTypeId = item.ExamTypeId,
                        SubjectId = item.SubjectId,
                        MarkTypeId = item.MarkTypeId,
                        Mark = item.Mark,
                        IsActive = true,
                        Year = DateTime.Now,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"))

                    };
                    insertGradings.Add(temp);
                }
                var model = new InsertGradings()
                {
                    ExampTypeId = resultList.ExamTypeId,
                    ResultList = insertGradings
                };
                _resultsServ.InsertGradings(model);
            }

            return RedirectToAction("Grading", new { resultList.EmployeeId, resultList.SubjectId, resultList.SectionId, resultList.ClassId, resultList.SelectedYear, resultList.ExamTypeId });
        }
        
        #endregion "Post_Method_Teacher"



    }
}
