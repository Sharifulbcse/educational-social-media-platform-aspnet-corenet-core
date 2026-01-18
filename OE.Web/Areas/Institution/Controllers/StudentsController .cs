using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Hosting;

using OE.Data;
using OE.Service;
using OE.Service.ServiceModels;
using OE.Web.Areas.Institution.Models;
using OE.Service.CustomEntitiesServ;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class StudentsController : Controller
    {
        #region "Variables"
        private readonly IStudentsServ _studentsServ;
        private readonly IClassesServ _classesServ;
        private readonly IOE_UserAuthenticationsServ _oeUserAuthenticationsServ;
        private readonly IRegistrationGroupsServ _registrationGroupsServ;
        private readonly IRegistrationItemsServ _registrationItemsServ;
        private readonly IStudentDetailsServ _studentDetailsServ;
        private readonly ICOM_GendersServ _comGendersServ;
        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        private readonly ICommonServ _commonServ;
        IHostingEnvironment _he;
        #endregion "Variables"

        #region "Constructor"
        public StudentsController(
            IStudentsServ studentsServ,
            IClassesServ classesServ,           
            IOE_UserAuthenticationsServ oeUserAuthenticationsServ,
            IRegistrationGroupsServ registrationGroupsServ,
            IRegistrationItemsServ registrationItemsServ,
            IStudentDetailsServ studentDetailsServ,
            ICOM_GendersServ comGendersServ,
            IOE_InstitutionsServ oeInstitutionsServ,
             ICommonServ commonServ,
            IHostingEnvironment he
            )
        {
            _studentsServ = studentsServ;
            _classesServ = classesServ;
            _oeUserAuthenticationsServ = oeUserAuthenticationsServ;
            _registrationGroupsServ = registrationGroupsServ;
            _registrationItemsServ = registrationItemsServ;
            _studentDetailsServ = studentDetailsServ;
            _comGendersServ = comGendersServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _commonServ = commonServ;
            _he = he;
        }
        #endregion "Constructor"

        #region "Get Methods - students"  
        public JsonResult CheckValidUserLoginIdAndRoll(string userLoginId, long institutionId, string oldUserLoginId, long admittedYear, long admittedPYear, long admittedClassId, long ddlClassId, long rollNo, long oldRollNo)
        {
            var result = (dynamic)null;
            var msg = _studentsServ.CheckValidUserLoginIdAndRoll(userLoginId, institutionId, oldUserLoginId, admittedYear, admittedPYear, admittedClassId, ddlClassId, rollNo, oldRollNo);
            if (msg == null)
            {
                result = Json(new { success = true, successMessage = "" });
            }
            else
            {
                result = Json(new { success = true, errorMessage = msg });
            }

            return result;
        }

        public IActionResult Index()
        {
            ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            return View("Index");
        }

        public IActionResult StudentDetails(long studentId)
        {           
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        ViewData["Title"] = "Student Details";
                        var obj = _studentsServ.GetStudentDetails(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), studentId);
                        if (String.IsNullOrEmpty(obj.Message))
                        {
                            ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                            ViewBag.ddlGender = _comGendersServ.Dropdown_COM_Genders();
                            var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                            var lstRegGroup = new List<RegistrationGroupListVM>();
                            foreach (var item in obj._RegistrationGroups)
                            {
                                var rg = new RegistrationGroupListVM()
                                {
                                    Id = item.Id,
                                    Name = item.Name
                                };
                                lstRegGroup.Add(rg);
                            }

                            var lstRegItem = new List<RegistrationItemListVM>();
                            foreach (var item in obj._RegistrationItems)
                            {
                                var ri = new RegistrationItemListVM()
                                {
                                    Id = item.Id,
                                    Name = item.Name,
                                    RegistrationGroupId = item.RegistrationGroupId,
                                    RegistrationItemTypeId = item.RegistrationItemTypeId
                                };
                                lstRegItem.Add(ri);
                            }

                            var student = new StudentsListVM()
                            {
                                Id = obj.Students.Id,
                                Name = obj.Students.Name,
                                ClassId = obj.Students.ClassId,
                                ClassName = obj.Classes.Name,
                                RollNo = obj.StudentPromotions.RollNo,
                                InstitutionId = obj.Students.InstitutionId,
                                GenderId = obj.Students.GenderId,
                                GenderName = obj.Genders.Name,
                                IP300X200 = obj.Students.IP300X200,
                                DOB = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(obj.Students.DOB)),
                                UserId = obj.Students?.UserId,
                                UserName = obj.Users?.UserLoginId,
                                AdmittedYear = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(obj.Students.AdmittedYear)),
                                PermanentAddress = obj.Students.PermanentAddress,
                                PresentAddress = obj.Students.PresentAddress,
                                IsActive = obj.Students.IsActive
                            };

                            var lstStudentDetails = new List<StudentsDetailsListVM>();
                            foreach (var item in obj._StudentDetails)
                            {
                                var sd = new StudentsDetailsListVM()
                                {
                                    Id = item.Id,
                                    StudentId = item.StudentId,
                                    InstitutionId = item.InstitutionId,
                                    ImagePathValue = item.ImagePathValue,
                                    BitValue = item.BitValue,
                                    DateValue = _commonServ.CommDate_ConvertToLocalDate(Convert.ToDateTime(item.DateValue)),
                                    FilePathValue = item.FilePathValue,
                                    FloatValue = item.FloatValue,
                                    StringValue = item.StringValue,
                                    TextAreaValue = item.TextAreaValue,
                                    WholeValue = item.WholeValue,

                                    RegistrationGroupId = item.RegistrationGroupId,
                                    RegistrationItemTypeId = item.RegistrationItemTypeId,
                                    RegistrationItemId = item.RegistrationItemId,
                                    RegistrationItemName = item.RegistrationItemName,
                                };
                                lstStudentDetails.Add(sd);
                            }

                            var model = new IndexStudentsVM()
                            {
                                _RegistrationGroups = lstRegGroup,
                                _RegistrationItems = lstRegItem,
                                Student = student,
                                _StudentDetails = lstStudentDetails,
                                InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                            };
                            return View("StudentsDetails", model);
                        }
                        else
                        {
                            ViewBag.OeErrorMessage = "ERROR101:Students/StudentDetails -" + obj.Message;
                            return View("StudentsDetails");
                        }
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:Students/StudentDetails -unauthorized access is not permitted";
                return View("StudentsDetails");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Students/StudentDetails - " + ex.Message;
                return View("StudentsDetails");
            } 
        }

        public IActionResult StudentLicenses(int year, long ddlClass, int SelectedFilterSearchId)
        {          
           ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
           var studentLicenses = _studentsServ.GetStudentLicenses(year, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), SelectedFilterSearchId, ddlClass);
           ViewBag.FilterSearch = new List<SelectListItem>() {
                new SelectListItem{Text="All", Value="0"},
                new SelectListItem{Text="Non License", Value="1"},
                new SelectListItem{Text="License", Value="2"}
            };

            var students = (dynamic)null;
            if (year != 0 || ddlClass != 0 || SelectedFilterSearchId != 0)
            {
                students = new List<StudentsListVM>();
                foreach (var item in studentLicenses.Students)
                {
                    var student = new StudentsListVM()
                    {
                        Id = item.Id,
                        Name = item.Name,
                        RollNo = item.RollNo,
                        UserId = item.UserId,
                        UserLoginId = item.UserLoginId,
                        StudentPromotionClassId = item.StudentPromotionClassId,
                        ClassName = item.ClassName,
                        IsActive = item.IsActive,
                        StudentPromotionYear = item.StudentPromotionYear
                    }; students.Add(student);
                }
            }            

            var model = new IndexStudentLicensesVM()
            {
                CurrentYear = year == 0 ? _commonServ.CommDate_CurrentYear() : year,
                InstitutionName = studentLicenses.InstitutionName,
                SelectedFilterSearchId = SelectedFilterSearchId,
                SelectedClassId = ddlClass,
                StudentsListVM = students
            };
            return View("StudentLicenses", model);
        }
        #endregion "Get Methods - students"

        #region "Get Methods - StudentPromotions"
        public IActionResult PromotionSearch()
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {                        
                        ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        return View("PromotionSearch/PromotionSearch");
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        public IActionResult Promotion(long fromYear, long toYear, long ddlFromClass, long ddlToClass)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var listPromotionStudents = _studentsServ.GetStudentPromotions(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), fromYear, ddlFromClass).ToList();
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        //[Note:Get promoted class name by Id]
                        var className = _classesServ.ClassName(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), ddlToClass);
                        

                        var studentPromotion = new List<StudentPromotionsListVM>();

                        foreach (var item in listPromotionStudents)
                        {
                            var sp = new StudentPromotionsListVM();
                            sp.Id = item.StudentPromotions.Id;
                            sp.InstitutionId = item.StudentPromotions.InstitutionId;
                            sp.RollNo = item.StudentPromotions.RollNo;
                            sp.PromotionYear = toYear;
                            sp.PromotionClassId = ddlToClass;
                            sp.PromotionClassName = className;
                            sp.StudentId = item.StudentPromotions.StudentId;
                            sp.StudentName = item.Students.Name;
                            sp.StudentPhoto = item.Students.IP300X200;
                            studentPromotion.Add(sp);
                        }

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexStudentPromotionsVM()
                        {
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                             _StudentPromotionsListVM = studentPromotion
                        };
                        return View("Promotion/Promotion", model);
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        public IActionResult PromotedStudents(int year)
        {
            ViewBag.OeErrorMessage = TempData["OeErrorMessage"];
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var listPromotedStudents = _studentsServ.PromotedStudents(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), year).ToList();
                        ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        var studentPromotion = new List<StudentPromotionsListVM>();

                        foreach (var item in listPromotedStudents)
                        {
                            var sp = new StudentPromotionsListVM();
                            sp.Id = item.StudentPromotions.Id;
                            sp.InstitutionId = item.StudentPromotions.InstitutionId;
                            sp.RollNo = item.StudentPromotions.RollNo;
                            sp.Year = item.StudentPromotions.Year;
                            sp.ClassId = item.StudentPromotions.ClassId;
                            sp.ClassName = item.Classes.Name;
                            sp.StudentId = item.StudentPromotions.StudentId;
                            sp.StudentName = item.Students.Name;
                            sp.StudentPhoto = item.Students.IP300X200;
                            studentPromotion.Add(sp);
                        }

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexStudentPromotionsVM()
                        {
                            _StudentPromotionsListVM = studentPromotion  
                        };
                        return View("PromotedStudents/PromotedStudents", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:Students/PromotedStudents -unauthorized access is not permitted";
                return RedirectToAction("PromotedStudents", "Students", new { area = "Institution" });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Students/PromotedStudents - " + ex.Message;
                return RedirectToAction("PromotedStudents", "Students", new { area = "Institution" });
            }
        }
        #endregion "Get Methods - StudentPromotions"

        #region "Post Methods - students"
        [HttpPost]
        public IActionResult Students(int year, long ddlClass)
        {
            var institutionName = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            var stu = _studentsServ.GetStudentList(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), year, ddlClass);
            var list = new List<StudentsListVM>();
            foreach (var item in stu._Students)
            {
                var temp = new StudentsListVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    RollNo = item.RollNo,
                    ClassName = item.ClassName,
                    GenderId = item.GenderId,
                    GenderName = item.GenderName,
                    IP300X200 = item.IP300X200,
                    DOB = item.DOB,
                    UserId = item.UserId,
                    AdmittedYear = item.AdmittedYear,
                    UserLoginId = item.UserLoginId,
                    UsersIP300X200 = item.UsersIP300X200
                };
                list.Add(temp);
            }

            var model = new IndexStudentsVM()
            {
                _Students = list,
                InstitutionName = institutionName.Name
            };
            return View("StudentList", model);

        }
        [HttpPost]
        public IActionResult AddStudentDetails(long studentId, IFormFile filePath, IFormFile imagePath, bool bitValue, string stringValue, long wholeValue, double floatValue, string txtAreaValue, DateTime dateValue, long regItemId, long regItemTypeId)
        {
            long StudentUserId = _studentsServ.GetStudentsById(studentId, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0).UserId == null ? 0 : Convert.ToInt64(_studentsServ.GetStudentsById(studentId, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0).UserId);
            var authorizeStatus = _oeUserAuthenticationsServ.IsAuthorized(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), StudentUserId, 10) ? 1 : 0;
            switch (regItemTypeId)
            {
                case 1:
                    var studentDetails1 = new StudentDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        StudentId = studentId,
                        RegistrationItemId = regItemId,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                    }; _studentDetailsServ.InsertStudentDetails(studentDetails1, filePath, _he.WebRootPath, regItemTypeId);
                    break;
                case 2:
                    var studentDetails2 = new StudentDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        StudentId = studentId,
                        RegistrationItemId = regItemId,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                    }; _studentDetailsServ.InsertStudentDetails(studentDetails2, imagePath, _he.WebRootPath, regItemTypeId);
                    break;
                case 3:
                    var studentDetails3 = new StudentDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        StudentId = studentId,
                        RegistrationItemId = regItemId,
                        BitValue = bitValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                    }; _studentDetailsServ.InsertStudentDetails(studentDetails3, null, null, null);
                    break;
                case 4:
                    var studentDetails4 = new StudentDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        StudentId = studentId,
                        RegistrationItemId = regItemId,
                        StringValue = stringValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                    }; _studentDetailsServ.InsertStudentDetails(studentDetails4, null, null, null);
                    break;
                case 5:
                    var studentDetails5 = new StudentDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        StudentId = studentId,
                        RegistrationItemId = regItemId,
                        WholeValue = wholeValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                    }; _studentDetailsServ.InsertStudentDetails(studentDetails5, null, null, null);
                    break;
                case 6:
                    var studentDetails6 = new StudentDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        StudentId = studentId,
                        RegistrationItemId = regItemId,
                        FloatValue = floatValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                    }; _studentDetailsServ.InsertStudentDetails(studentDetails6, null, null, null);
                    break;
                case 7:
                    var studentDetails7 = new StudentDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        StudentId = studentId,
                        RegistrationItemId = regItemId,
                        TextAreaValue = txtAreaValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                    }; _studentDetailsServ.InsertStudentDetails(studentDetails7, null, null, null);
                    break;
                case 8:
                    var studentDetails8 = new StudentDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        StudentId = studentId,
                        RegistrationItemId = regItemId,
                        DateValue = dateValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                    }; _studentDetailsServ.InsertStudentDetails(studentDetails8, null, null, null);
                    break;
                default:
                    break;
            }
            return RedirectToAction("StudentDetails", new { studentId });
        }
        [HttpPost]
        public JsonResult InsertUpdateStudentLicenses(IndexStudentLicensesVM obj)
        {            
            var result = new InsertUpdateStudentLicenses();            
            if (obj.StudentsListVM != null)
            {
                var studentLicense = new List<C_StudentLicenses>();
                foreach (var item in obj.StudentsListVM)
                {
                    var temp = new C_StudentLicenses()
                    {
                        Id = item.Id,
                        UserLoginId = item.UserLoginId,
                        oldUserLoginId = item.oldUserLoginId,
                        UserId = item.UserId,
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),

                        IsActive = item.IsActive,
                        oldIsActive = item.oldIsActive
                    }; studentLicense.Add(temp);
                }

                var model = new InsertUpdateStudentLicenses()
                {
                    _StudentLicenses = studentLicense
                };

                result = _studentsServ.InsertUpdateStudentLicenses(model);
            }
            return Json(result._StudentLicenses.ToList());
        }
        [HttpPost]
        public JsonResult StudentLicensesCheck(IndexStudentLicensesVM obj)
        {
            var result = (dynamic)null;
            if (obj.StudentsListVM != null)
            {
                var studentLicensesValidation = new List<StudentLicensesValidation>();
                foreach (var item in obj.StudentsListVM)
                {
                    var temp = new StudentLicensesValidation()
                    {
                        StudentId = item.Id,
                        SelectedUserLoginId = item.UserLoginId,
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                    };
                    studentLicensesValidation.Add(temp);
                }
                result = _studentsServ.StudentLicensesValidation(studentLicensesValidation);
            }
            return result;
        }
        [HttpPost]
        public IActionResult UpdateStudentDetails(long id, long studentId, string userLoginId, string oldUserLoginId, IFormFile filePath, IFormFile imagePath, bool bitValue, string stringValue, long wholeValue, double floatValue, string txtAreaValue, DateTime dateValue, long regItemId, long regItemTypeId, int flagValue, string name, long ddlClassId, long rollNo, long admittedClassId, long admittedPYear, long ddlGenderId, DateTime dob, string presentAddress, string permanentAddress, bool enableAuthentic, int admittedYear, IFormFile ip300x200)
        {
            long StudentUserId = _studentsServ.GetStudentsById(studentId, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0).UserId == null ? 0 : Convert.ToInt64(_studentsServ.GetStudentsById(studentId, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0).UserId);
            var authorizeStatus = _oeUserAuthenticationsServ.IsAuthorized(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), StudentUserId, 10) ? 1 : 0;
            if (flagValue == 2)
            {
                switch (regItemTypeId)
                {
                    case 1:
                        var fetchStuDetails1 = _studentDetailsServ.GetStudentDetailsById(id);
                        fetchStuDetails1.RegistrationItemId = regItemId;
                        fetchStuDetails1.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        fetchStuDetails1.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _studentDetailsServ.UpdateStudentDetails(fetchStuDetails1, filePath, _he.WebRootPath, regItemTypeId);
                        break;
                    case 2:
                        var fetchStuDetails2 = _studentDetailsServ.GetStudentDetailsById(id);
                        fetchStuDetails2.RegistrationItemId = regItemId;
                        fetchStuDetails2.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        fetchStuDetails2.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _studentDetailsServ.UpdateStudentDetails(fetchStuDetails2, imagePath, _he.WebRootPath, regItemTypeId);
                        break;
                    case 3:
                        var fetchStuDetails3 = _studentDetailsServ.GetStudentDetailsById(id);
                        fetchStuDetails3.BitValue = bitValue;
                        fetchStuDetails3.RegistrationItemId = regItemId;
                        fetchStuDetails3.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        fetchStuDetails3.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _studentDetailsServ.UpdateStudentDetails(fetchStuDetails3, null, null, null);
                        break;
                    case 4:
                        var fetchStuDetails4 = _studentDetailsServ.GetStudentDetailsById(id);
                        fetchStuDetails4.StringValue = stringValue;
                        fetchStuDetails4.RegistrationItemId = regItemId;
                        fetchStuDetails4.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        fetchStuDetails4.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _studentDetailsServ.UpdateStudentDetails(fetchStuDetails4, null, null, null);
                        break;
                    case 5:
                        var fetchStuDetails5 = _studentDetailsServ.GetStudentDetailsById(id);
                        fetchStuDetails5.WholeValue = wholeValue;
                        fetchStuDetails5.RegistrationItemId = regItemId;
                        fetchStuDetails5.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        fetchStuDetails5.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _studentDetailsServ.UpdateStudentDetails(fetchStuDetails5, null, null, null);
                        break;
                    case 6:
                        var fetchStuDetails6 = _studentDetailsServ.GetStudentDetailsById(id);
                        fetchStuDetails6.FloatValue = floatValue;
                        fetchStuDetails6.RegistrationItemId = regItemId;
                        fetchStuDetails6.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        fetchStuDetails6.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _studentDetailsServ.UpdateStudentDetails(fetchStuDetails6, null, null, null);
                        break;
                    case 7:
                        var fetchStuDetails7 = _studentDetailsServ.GetStudentDetailsById(id);
                        fetchStuDetails7.TextAreaValue = txtAreaValue;
                        fetchStuDetails7.RegistrationItemId = regItemId;

                        fetchStuDetails7.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        fetchStuDetails7.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _studentDetailsServ.UpdateStudentDetails(fetchStuDetails7, null, null, null);
                        break;
                    case 8:
                        var fetchStuDetails8 = _studentDetailsServ.GetStudentDetailsById(id);
                        fetchStuDetails8.DateValue = dateValue;
                        fetchStuDetails8.RegistrationItemId = regItemId;
                        fetchStuDetails8.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        fetchStuDetails8.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _studentDetailsServ.UpdateStudentDetails(fetchStuDetails8, null, null, null);
                        break;
                    default:
                        break;
                }
            }
            if (flagValue == 1)
            {
                var student = new C_Students()
                {
                    Id = id,
                    Name = name,
                    ClassId = ddlClassId,
                    GenderId = ddlGenderId,                   
                    UserLoginId = userLoginId,
                    OldUserLoginId = oldUserLoginId,
                    InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                    DOB = dob,
                    PermanentAddress = permanentAddress,
                    PresentAddress = presentAddress,
                    AdmittedYear = new DateTime(admittedYear, 1, 1).Date,
                    IsActive = enableAuthentic,
                    RollNo = rollNo,
                    AdmittedClassId = admittedClassId,
                    AdmittedPYear = admittedPYear
                };
                var model = new UpdateStudents()
                {
                    _Student = student
                };
                _studentsServ.UpdateStudents(model, ip300x200, _he.WebRootPath);

            }
            return RedirectToAction("StudentDetails", new { studentId });
        }
        [HttpPost]
        public IActionResult DeleteStaticFile(long id, long studentId, int flagValue, long regItemTypeId)
        {
            if (flagValue == 1)
            {
                var student = _studentsServ.GetStudentsById(id, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0);
                _studentsServ.DeleteStaticFile(student, _he.WebRootPath);
            }
            if (flagValue == 2)
            {
                var student = _studentDetailsServ.GetStudentDetailsById(id);
                _studentDetailsServ.DeleteStaticFile(student, _he.WebRootPath, regItemTypeId);
            }
            return RedirectToAction("StudentDetails", new { studentId });
        }

        #endregion "Post Methods - students"

        #region "Post Methods- StudentPromotions"       
        [HttpPost]
        public IActionResult AddPromotion(IndexStudentPromotionsVM studentPromotionList)
        {            
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var newPromotions = new List<StudentPromotions>();
                        foreach (var item in studentPromotionList._StudentPromotionsListVM)
                        {
                            if (item.IsAllChecked == true)
                            {
                                var newPromotion = new StudentPromotions()
                                {
                                    InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                    StudentId = item.StudentId,
                                    ClassId = item.PromotionClassId,
                                    RollNo = item.RollNo,
                                    Year = new DateTime(Convert.ToInt32(item.PromotionYear), 1, 1),
                                    IsActive = true,
                                    AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                    AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now),
                                    ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                    ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now),

                                };
                                newPromotions.Add(newPromotion);
                            }
                        }
                        var model = new InsertStudentPromotions()
                        {
                            StudentPromotions = newPromotions
                        };
                        _studentsServ.InsertStudentPromotions(model);
                        return RedirectToAction("PromotedStudents");
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:Students/AddPromotion -unauthorized access is not permitted";
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Students/AddPromotion - " + ex.Message;
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        [HttpPost]
        public IActionResult UpdatePromotion(IndexStudentPromotionsVM obj)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var updatePromotions = new StudentPromotions()
                        {
                            Id = obj.StudentPromotions.Id,
                            ClassId = obj.StudentPromotions.ClassId,
                            Year = new DateTime(obj.SelectedYear, 1, 1),
                            RollNo = obj.StudentPromotions.RollNo
                        };

                        var model = new UpdateStudentPromotions()
                        {
                            StudentPromotions = updatePromotions
                        };
                        _studentsServ.UpdateStudentPromotions(model);
                        return Ok();
                    }
                }
                TempData["OeErrorMessage"] = "ERROR101:Students/UpdatePromotion -unauthorized access is not permitted";
                return RedirectToAction("PromotedStudents", "Students", new { area = "Institution" });
            }
            catch (Exception ex)
            {
                TempData["OeErrorMessage"] = "ERROR101:Students/UpdatePromotion - " + ex.Message;
                return RedirectToAction("PromotedStudents", "Students", new { area = "Institution" });
            }
        }               
        [HttpPost]
        public IActionResult DeletePromotion(IndexStudentPromotionsVM obj)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var deletePromotions = new StudentPromotions()
                        {
                            Id = obj.StudentPromotions.Id,
                        };

                        var model = new DeleteStudentPromotions()
                        {
                            StudentPromotions = deletePromotions
                        };
                        _studentsServ.DeleteStudentPromotions(model);
                        return Ok();
                    }
                }
                TempData["OeErrorMessage"] = "ERROR101:Students/DeletePromotion -unauthorized access is not permitted";
                return RedirectToAction("PromotedStudents", "Students", new { area = "" });
            }
            catch (Exception ex)
            {
                TempData["OeErrorMessage"] = "ERROR101:Students/DeletePromotion - " + ex.Message;
                return RedirectToAction("PromotedStudents", "Students", new { area = "" });
            }
        }
        #endregion "Post Methods- StudentPromotions"
    }
}
