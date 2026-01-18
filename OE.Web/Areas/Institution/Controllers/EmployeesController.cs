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

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class EmployeesController : Controller
    {
        #region "Variables"
        private readonly ICommonServ _commonServ;
        private readonly ICOM_GendersServ _comGendersServ;

        private readonly IEmployeeDetailsServ _employeeDetailsServ;
        private readonly IEmployeesServ _employeesServ;
        private readonly IEmployeeTypesServ _employeeTypesServ;
        private readonly IEmployeeTypeCategoriesServ _employeeTypeCategoriesServ;

        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        private readonly IOE_ActorsServ _oeActorsServ;
        
        private readonly IRegistrationGroupsServ _regGroupServ;
        private readonly IRegistrationItemsServ _regItemsServ;              
        IHostingEnvironment _he;
        #endregion "Variables"

        #region "Constructor"
        public EmployeesController(
            IOE_ActorsServ oeActorsServ,
            IEmployeesServ employeesServ,
            IEmployeeTypesServ employeeTypesServ,
            IEmployeeTypeCategoriesServ employeeTypeCategoriesServ,
            IRegistrationGroupsServ regGroupServ,
            IRegistrationItemsServ regItemsServ,
            IEmployeeDetailsServ employeeDetailsServ,
            ICOM_GendersServ comGendersServ,
            IOE_InstitutionsServ oeInstitutionsServ,
            ICommonServ commonServ,
            IHostingEnvironment he
            )
        {
            _oeActorsServ = oeActorsServ;
            _employeesServ = employeesServ;
            _employeeTypesServ = employeeTypesServ;
            _employeeTypeCategoriesServ = employeeTypeCategoriesServ;
            _regGroupServ = regGroupServ;
            _regItemsServ = regItemsServ;
            _employeeDetailsServ = employeeDetailsServ;
            _comGendersServ = comGendersServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _commonServ = commonServ;
            _he = he;
        }
        #endregion "Constructor"

        #region "Get Methods"             
        public IActionResult Index()
        {
            var stu = _employeesServ.GetEmployees(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            var list = new List<EmployeeListVM>();
            foreach (var item in stu)
            {
                var temp = new EmployeeListVM()
                {
                    Id = item.Id,
                    InstitutionId = item.InstitutionId,
                    UserId = item.UserId,
                    GenderId = item.GenderId,
                    DOB = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(item.DOB)),
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    IP300X200 = item.IP300X200,
                    PresentAddress = item.PresentAddress,
                    PermanentAddress = item.PermanentAddress,
                    JoiningDate = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(item.JoiningDate)),

                    ContactNo = item.ContactNo,
                    EmailAddress = item.EmailAddress
                };
                list.Add(temp);
            }
            
            var model = new IndexEmployeeVM()
            {
                employeelist = list
            };
            return View("Index", model);
        }
        public IActionResult EmployeeListByRegister()
        {

            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString
                ("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        ViewBag.errorMsg = TempData["error"];
                        ViewBag.empId = TempData["SelectedEmployeeId"];
                        var employees = _employeesServ.GetEmployeeListByRegister(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        var list = new List<EmployeeListVM>();
                        foreach (var item in employees._Employees)
                        {
                            var temp = new EmployeeListVM()
                            {
                                Id = item.Id,
                                InstitutionId = item.InstitutionId,
                                UserId = item.UserId,
                                GenderId = item.GenderId,
                                UserLoginId = item?.UserLoginId,

                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                IP300X200 = item.IP300X200,
                                PresentAddress = item.PresentAddress,
                                PermanentAddress = item.PermanentAddress,
                                ContactNo = item?.ContactNo,
                                EmailAddress = item?.EmailAddress,
                                DOB = item.DOB,
                                JoiningDate = item.DOB,
                                GenderName = item?.GenderName,
                                UsersIP300X200 = item?.UsersIP300X200,


                                EmployeeTypeId = item?.EmployeeTypeId,
                                EmployeeTypeName = item.EmployeeTypeName,
                                EmployeeCategoryTypeId = item?.EmployeeCategoryTypeId,
                                EmployeeCategoryTypeName = item.EmployeeCategoryTypeName
                            };
                            list.Add(temp);
                        }

                        var model = new IndexEmployeeVM()
                        {
                            employeelist = list
                        };
                        return View("EmployeeListByRegister", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:Employees/EmployeeListByRegister -unauthorized access is not permitted";
                return View("Employees/EmployeeListByRegister");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Employees/EmployeeListByRegister - " + ex.Message;
                return View("Employees/EmployeeListByRegister");
            }

        }
        public IActionResult EmployeeListByAdmin(bool employeeStatus = true)
        {

            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString
                ("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {                        
                        var employees = _employeesServ.GetEmployeeListByAdmin(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), employeeStatus);
                        var list = new List<EmployeeListVM>();
                        foreach (var item in employees._Employees)
                        {
                            var temp = new EmployeeListVM()
                            {
                                Id = item.Id,
                                InstitutionId = item.InstitutionId,
                                UserId = item.UserId,
                                GenderId = item.GenderId,
                                UserLoginId = item?.UserLoginId,

                                FirstName = item.FirstName,
                                LastName = item.LastName,
                                IP300X200 = item.IP300X200,
                                PresentAddress = item.PresentAddress,
                                PermanentAddress = item.PermanentAddress,
                                ContactNo = item?.ContactNo,
                                EmailAddress = item?.EmailAddress,
                                DOB = item.DOB,
                                JoiningDate = item.DOB,
                                GenderName = item?.GenderName,
                                UsersIP300X200 = item?.UsersIP300X200,


                                EmployeeTypeId = item?.EmployeeTypeId,
                                EmployeeTypeName = item.EmployeeTypeName,
                                EmployeeCategoryTypeId = item?.EmployeeCategoryTypeId,
                                EmployeeCategoryTypeName = item.EmployeeCategoryTypeName
                            };
                            list.Add(temp);
                        }

                        var model = new IndexEmployeeVM()
                        {
                            SelectedEmployee = employeeStatus,
                            employeelist = list
                        };
                        return View("EmployeeListByAdmin", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:Employees/EmployeeListByAdmin -unauthorized access is not permitted";
                return View("Employees/EmployeeListByAdmin");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Employees/EmployeeListByAdmin - " + ex.Message;
                return View("Employees/EmployeeListByAdmin");
            }

        }
        public IActionResult EmployeeDetails(long empId)
        {
            try
            {
                ViewBag.ddlGender = _comGendersServ.Dropdown_COM_Genders();                
                ViewBag.ddlEmpType = _employeeTypesServ.Dropdown_EmployeeTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
               

                var empDetailsAllRecord = _employeesServ.GetEmployeeDetails(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), empId);
                var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                //[NOTE:Employee Details list]
                var emp = new EmployeeListVM()
                {
                    Id = empDetailsAllRecord.EmployeeDetails.employees.Id,
                    FirstName = empDetailsAllRecord.EmployeeDetails.employees.FirstName,
                    LastName = empDetailsAllRecord.EmployeeDetails.employees.LastName,
                    GenderId = empDetailsAllRecord.EmployeeDetails.employees.GenderId,
                    GenderName = empDetailsAllRecord.EmployeeDetails.genders.Name,
                    IP300X200 = empDetailsAllRecord.EmployeeDetails.employees.IP300X200,
                    DOB = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(empDetailsAllRecord.EmployeeDetails.employees.DOB)),
                    EmailAddress = empDetailsAllRecord.EmployeeDetails.employees.EmailAddress,
                    ContactNo = empDetailsAllRecord.EmployeeDetails.employees.ContactNo,
                    UserId = empDetailsAllRecord.EmployeeDetails.employees.UserId,

                    UserLoginId = (empDetailsAllRecord.EmployeeDetails.Users == null) ? "OurEdu Id is not there." : empDetailsAllRecord.EmployeeDetails.Users.UserLoginId,

                    JoiningDate = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(empDetailsAllRecord.EmployeeDetails.employees.JoiningDate)),

                    PermanentAddress = empDetailsAllRecord.EmployeeDetails.employees.PermanentAddress,
                    PresentAddress = empDetailsAllRecord.EmployeeDetails.employees.PresentAddress,
                    IsActive = empDetailsAllRecord.EmployeeDetails.employees.IsActive,


                    EmployeeTypeId = empDetailsAllRecord.EmployeeTypes?.Id,
                    EmployeeTypeName = empDetailsAllRecord.EmployeeTypes?.Name,
                    EmployeeCategoryTypeId = empDetailsAllRecord.EmployeeTypeCategories?.Id,
                    EmployeeCategoryTypeName = empDetailsAllRecord.EmployeeTypeCategories?.Name
                   
                };

                //[NOTE:Registration Group Item list]
                var regGroupItem = new List<RegistrationGroupListVM>();
                foreach (var item in empDetailsAllRecord.RegGroup)
                {
                    var tem = new RegistrationGroupListVM()
                    {
                        Id = item.RegistrationGroups.Id,
                        InstitutionId = item.RegistrationGroups.InstitutionId,
                        Name = item.RegistrationGroups.Name
                    }; regGroupItem.Add(tem);
                }

                //[NOTE:Registration Item Details list]
                var regItemDetails = new List<EmployeesDetailsListVM>();
                foreach (var item in empDetailsAllRecord.RegItemWithDetails)
                {
                    var temp = new EmployeesDetailsListVM()
                    {
                        BitValue = item.employeeDetails.BitValue,
                        DateValue = _commonServ.CommDate_ConvertToLocalDate(Convert.ToDateTime(item.employeeDetails.DateValue)),
                        FilePathValue = item.employeeDetails.FilePathValue,
                        FloatValue = item.employeeDetails.FloatValue,
                        Id = item.employeeDetails.Id,
                        EmployeeId = item.employeeDetails.EmployeeId,
                        ImagePathValue = item.employeeDetails.ImagePathValue,
                        InstitutionId = item.employeeDetails.InstitutionId,
                        RegistrationGroupId = item.registrationItems.RegistrationGroupId,
                        RegistrationItemTypeId = item.registrationItems.RegistrationItemTypeId,
                        RegistrationItemId = item.employeeDetails.RegistrationItemId,
                        RegistrationItemName = item.registrationItems.Name,
                        StringValue = item.employeeDetails.StringValue,
                        TextAreaValue = item.employeeDetails.TextAreaValue,
                        WholeValue = item.employeeDetails.WholeValue,
                    }; regItemDetails.Add(temp);
                }

                //[NOTE:Registration Item list]
                var regItemList = new List<RegistrationItemListVM>();
                foreach (var item in empDetailsAllRecord.RegItems)
                {
                    var temp = new RegistrationItemListVM()
                    {
                        Id = item.RegistrationItemId,
                        InstitutionId = item.InstitutionId,
                        Name = item.RegistrationItemName,
                        RegistrationGroupId = item.RegistrationGroupId,
                        RegistrationItemTypeId = item.RegistrationItemTypeId
                    }; regItemList.Add(temp);
                }
                var model = new IndexEmployeeVM()
                {
                    employees = emp,
                    regGroup = regGroupItem,

                    regItemDetails = regItemDetails,
                    regItems = regItemList,
                    InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                };
                return View("EmployeeDetails", model);
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
                return RedirectToAction("Index");
            }
        }
        public IActionResult TeacherLicenses()
        {
            var listOfEmployees = _employeesServ.GetTeacherLicenses(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            ViewBag.ddlActors = _oeActorsServ.Dropdown_Actors(14);
            var authenticationList = new List<OE_UserAuthenticationsListVM>();
            foreach (var item in listOfEmployees)
            {
                var temp = new OE_UserAuthenticationsListVM()
                {
                    Id = item.OE_UserAuthentications.Id,
                    EmployeeName = item.Employees?.FirstName + " " + item.Employees?.LastName,
                    UserLoginId = item.OE_Users.UserLoginId,
                    UserIP300X200 = item.Employees?.IP300X200,
                    ActorId = item.OE_UserAuthentications.ActorId,
                    ActorName = item.OE_Actors.Name,
                    IsActive = item.OE_UserAuthentications.IsActive

                }; authenticationList.Add(temp);
            }

            var model = new IndexTeacherLicensesVM()
            {
                OE_UserAuthenticationsListVM = authenticationList
            };

            return View("TeacherLicenses", model);
        }
        #endregion "Get Methods"

        #region "Post Methods"

        [HttpPost]
        public IActionResult AddEmployeeDetails(long empId, IFormFile filePath, IFormFile imagePath, bool bitValue, string stringValue, long wholeValue, double floatValue, string txtAreaValue, DateTime dateValue, long regItemId, long regItemTypeId)
        {
            long EmployeeUserId = _employeesServ.EmployeeById(empId, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0).UserId == null ? 0 : Convert.ToInt64(_employeesServ.EmployeeById(empId, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0).UserId);

            switch (regItemTypeId)
            {
                case 1:
                    var EmpDetails1 = new EmployeeDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        EmployeeId = empId,
                        RegistrationItemId = regItemId,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),
                       


                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))

                    }; _employeeDetailsServ.InsertEmployeeDetails(EmpDetails1, filePath, _he.WebRootPath, regItemTypeId);
                    break;
                case 2:
                    var EmpDetails2 = new EmployeeDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        EmployeeId = empId,
                        RegistrationItemId = regItemId,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),
                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))

                    }; _employeeDetailsServ.InsertEmployeeDetails(EmpDetails2, imagePath, _he.WebRootPath, regItemTypeId);
                    break;
                case 3:
                    var EmpDetails3 = new EmployeeDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        EmployeeId = empId,
                        RegistrationItemId = regItemId,
                        BitValue = bitValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),
                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))

                    }; _employeeDetailsServ.InsertEmployeeDetails(EmpDetails3, null, null, null);
                    break;
                case 4:
                    var EmpDetails4 = new EmployeeDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        EmployeeId = empId,
                        RegistrationItemId = regItemId,
                        StringValue = stringValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),
                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))

                    }; _employeeDetailsServ.InsertEmployeeDetails(EmpDetails4, null, null, null);
                    break;
                case 5:
                    var EmpDetails5 = new EmployeeDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        EmployeeId = empId,
                        RegistrationItemId = regItemId,
                        WholeValue = wholeValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),
                        
                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))

                    }; _employeeDetailsServ.InsertEmployeeDetails(EmpDetails5, null, null, null);
                    break;
                case 6:
                    var EmpDetails6 = new EmployeeDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        EmployeeId = empId,
                        RegistrationItemId = regItemId,
                        FloatValue = floatValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),
                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))

                    }; _employeeDetailsServ.InsertEmployeeDetails(EmpDetails6, null, null, null);
                    break;
                case 7:
                    var EmpDetails7 = new EmployeeDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        EmployeeId = empId,
                        RegistrationItemId = regItemId,
                        TextAreaValue = txtAreaValue,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),
                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))

                    }; _employeeDetailsServ.InsertEmployeeDetails(EmpDetails7, null, null, null);
                    break;
                case 8:
                    var EmpDetails8 = new EmployeeDetails()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        EmployeeId = empId,
                        RegistrationItemId = regItemId,                        
                        DateValue = _commonServ.CommDate_ConvertToUtcDate(dateValue),
                        
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),
                        
                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))

                    }; _employeeDetailsServ.InsertEmployeeDetails(EmpDetails8, null, null, null);
                    break;
                default:
                    break;
            }
            return RedirectToAction("EmployeeDetails", new { empId });
        }
        [HttpPost]
        public JsonResult AddTeacherLicenses(IndexTeacherLicensesVM obj)
        {
            var result = (dynamic)null;
            if (obj != null)
            {
                var model = new InsertTeacherLicenses()
                {
                    UserLoginId = obj.UserLoginId,
                    ActorId = obj.ActorId,
                    IsActive = obj.IsActive,
                    AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                    InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                };
                var msg = _employeesServ.InsertTeacherLicense(model);
                if (msg == null)
                {
                    result = Json(new { success = true, successMessage = "" });
                }
                else
                {
                    result = Json(new { success = true, errorMessage = msg });
                }
            }
            return result;
        }

        [HttpPost]
        public IActionResult UpdateEmployeeDetails(long id, long empId, IFormFile filePath, IFormFile imagePath, bool bitValue, string stringValue, long wholeValue, double floatValue, string txtAreaValue, DateTime dateValue, long regItemId, long regItemTypeId, int flagValue)
        {
            IFormFile employeeDetailsStaticFile = (dynamic)null;
            var employeeDetails = _employeeDetailsServ.GetEmployeeDetailsById(id);
            if (flagValue == 2)
            {
                employeeDetails.RegistrationItemId = regItemId;
                employeeDetails.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                employeeDetails.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);
                switch (regItemTypeId)
                {
                    case 1:
                        employeeDetailsStaticFile = filePath;
                        break;
                    case 2:
                        employeeDetailsStaticFile = imagePath;
                        break;
                    case 3:
                        employeeDetails.BitValue = bitValue;
                        break;
                    case 4:
                        employeeDetails.StringValue = stringValue;
                        break;
                    case 5:
                        employeeDetails.WholeValue = wholeValue;
                        break;
                    case 6:
                        employeeDetails.FloatValue = floatValue;
                        break;
                    case 7:
                        employeeDetails.TextAreaValue = txtAreaValue;
                        break;
                    case 8:
                        employeeDetails.DateValue = _commonServ.CommDate_ConvertToUtcDate(dateValue);
                        break;
                    default:
                        break;
                }
            }
            
            var model = new UpdateEmployees();
            if (flagValue == 2)
            {
                model.EmployeeDetails = employeeDetails;
                model.EmployeeDetailsStaticFile = employeeDetailsStaticFile;
                model.RegistrationItemTypeId = regItemTypeId;
            }
            _employeesServ.UpdateEmployees(model, _he.WebRootPath);
            return RedirectToAction("EmployeeDetails", new { empId });
        }
        [HttpPost]
        public JsonResult UpdateTeacherLicense(IndexTeacherLicensesVM obj)
        {

            var result = (dynamic)null;
            var model = new UpdateTeacherLicenses()
            {
                Id = obj.Id,
                ActorId = obj.ActorId,
                UserLoginId = obj.UserLoginId,
                IsActive = obj.IsActive,
                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
            };
            var msg = _employeesServ.UpdateTeacherLicense(model);
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
        [HttpPost]
        public JsonResult EditEmployeeBasicInfo(IndexEmployeeVM employee)
        {
            var emp = new Employees()
            {
                Id = employee.employees.Id,
                FirstName = employee.employees.FirstName,
                LastName = employee.employees.LastName,
                EmailAddress = employee.employees.EmailAddress,
                ContactNo = employee.employees.ContactNo,
                GenderId = employee.employees.GenderId,
                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                DOB = employee.employees.DOB,
                PresentAddress = employee.employees.PresentAddress,
                PermanentAddress = employee.employees.PermanentAddress,
                JoiningDate = employee.employees.JoiningDate,
                IsActive = employee.employees.IsActive,
                ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
            };
           
            var designation = new EmployeeDesignations()
            {
                EmployeeTypeId = employee.Designations.EmployeeTypeId,
                EmployeeTypeCategoryId = employee.Designations.EmployeeTypeCategoryId != null || employee.Designations.EmployeeTypeCategoryId != 0 ? employee.Designations.EmployeeTypeCategoryId : (dynamic)null,
                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"))
            };
           
            var model = new UpdateEmployees()
            {
               
                Designations = designation,
                Employees = emp,
                EmployeesStaticFile = employee.employees.ActualFile,
                SelectedUserLoginId = employee.employees.UserLoginId,
                CurrentInstituteId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),

            };
            var msg = _employeesServ.UpdateEmployees(model, _he.WebRootPath);

            var result = (dynamic)null;

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

        [HttpPost]
        public IActionResult DeleteStaticFile(long id, long empId, int flagValue, long regItemTypeId)
        {
            if (flagValue == 1)
            {
                var emp = _employeesServ.EmployeeById(empId, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0);
                _employeesServ.DeleteStaticFile(emp, _he.WebRootPath);
            }
            if (flagValue == 2)
            {
                var emp = _employeeDetailsServ.GetEmployeeDetailsById(id);
                _employeeDetailsServ.DeleteStaticFile(emp, _he.WebRootPath, regItemTypeId);
            }
            return RedirectToAction("EmployeeDetails", new { empId });
        }

        [HttpPost]
        public JsonResult DeleteTeacherLicenses(IndexTeacherLicensesVM obj)
        {            
            var result = (dynamic)null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        if (obj.Id > 0)
                        {
                            var model = new DeleteTeacherLicenses()
                            {
                                Id = obj.Id
                            };
                            var returnResult = _employeesServ.DeleteTeacherLicense(model);
                            result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, message = "ERROR101:Employees/DeleteTeacherLicenses - " + ex.Message });
            }
            return result;           
        }

        [HttpPost]
        public JsonResult DeleteEmpListByAdmin(int empId)
        {
            var result = (dynamic)null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        if (empId > 0)
                        {
                            var model = new DeleteEmployees()
                            {
                                Id = empId
                            };
                            var returnResult = _employeesServ.DeleteEmployees(model);
                            result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });                            
                            return result;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, message = "ERROR101:Employees/DeleteEmpListByAdmin - " + ex.Message });
            }
            return result;
        }

        #endregion "Post Methods"

        #region "Post Methods - By Admin"

        public IActionResult EmployeeDetailsByAdmin(long empId)
        {
            try
            {
                ViewBag.ddlGender = _comGendersServ.Dropdown_COM_Genders();
                var empDetailsAllRecord = _employeesServ.GetEmployeeDetailsByAdmin(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), empId);
                var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                ViewBag.ddlEmpType = _employeeTypesServ.Dropdown_EmployeeTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                //[NOTE:Employee Details list]
                var emp = new EmployeeListVM()
                {
                    Id = empDetailsAllRecord.Employees.Id,
                    FirstName = empDetailsAllRecord.Employees?.FirstName,
                    LastName = empDetailsAllRecord.Employees?.LastName,

                    IP300X200 = empDetailsAllRecord.Employees?.IP300X200,
                    DOB = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(empDetailsAllRecord.Employees.DOB)),
                    EmailAddress = empDetailsAllRecord.Employees?.EmailAddress,
                    ContactNo = empDetailsAllRecord.Employees.ContactNo,
                    UserId = empDetailsAllRecord?.Employees.UserId,
                    JoiningDate = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(empDetailsAllRecord.Employees.JoiningDate)),

                    PermanentAddress = empDetailsAllRecord.Employees.PermanentAddress,
                    PresentAddress = empDetailsAllRecord.Employees.PresentAddress,
                    IsActive = empDetailsAllRecord.Employees.IsActive,

                    GenderId = empDetailsAllRecord.Employees.GenderId,
                    GenderName = empDetailsAllRecord.ComGenders.Name,
                    
                    UserLoginId = (empDetailsAllRecord.OEUsers?.UserLoginId == null) ? "" : empDetailsAllRecord?.OEUsers?.UserLoginId,
                    EmployeeTypeId = empDetailsAllRecord.EmployeeTypes?.Id,
                    EmployeeTypeName = empDetailsAllRecord.EmployeeTypes?.Name,
                    EmployeeCategoryTypeId = empDetailsAllRecord.EmployeeTypeCategories?.Id,
                    EmployeeCategoryTypeName = empDetailsAllRecord.EmployeeTypeCategories?.Name

                };


                var model = new IndexEmployeeDetailsByAdminVM()
                {

                    InstitutionName = currentInstitutionDetails?.Name,
                    employees = emp
                };
                return View("EmployeeDetailsByAdmin", model);
            }
            catch (Exception ex)
            {
                ViewBag.msg = ex.Message;
                return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public JsonResult UpdateEmployeeDetailsByAdmin(IndexEmployeeDetailsByAdminVM empDetails)
        {
            var result = (dynamic)null;
            var empId = (dynamic)null;
            var msg = (dynamic)null;
            if (empDetails != null)
            {
                var emp = (dynamic)null;
                var user = (dynamic)null;                
                var designation = (dynamic)null;                

                if (empDetails.employees != null)
                {
                    empId = empDetails.employees.Id;

                    emp = new Employees()
                    {
                        Id = empDetails.employees.Id,
                        UserId = empDetails.employees.UserId,
                        FirstName = empDetails.employees?.FirstName,
                        LastName = empDetails.employees?.LastName,
                        GenderId = empDetails.employees.GenderId,
                        DOB = empDetails.employees.DOB,
                        EmailAddress = empDetails.employees.EmailAddress,
                        ContactNo = empDetails.employees.ContactNo,
                        PresentAddress = empDetails.employees.PresentAddress,
                        PermanentAddress = empDetails.employees.PermanentAddress,
                        JoiningDate = empDetails.employees.JoiningDate,
                        IsActive = empDetails.employees.IsActive != null ? empDetails.employees.IsActive : false
                    };

                    user = new OE_Users()
                    {
                        UserLoginId = empDetails.employees.UserLoginId
                    };

                    
                    designation = new EmployeeDesignations()
                    {
                        EmployeeTypeId = empDetails.DesignationsListVM.EmployeeTypeId,
                        EmployeeTypeCategoryId = empDetails.DesignationsListVM.EmployeeTypeCategoryId != null || empDetails.DesignationsListVM.EmployeeTypeCategoryId != 0 ? empDetails.DesignationsListVM.EmployeeTypeCategoryId : (dynamic)null,
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"))
                    };                   

                }

                var model = new UpdateEmployeeDetailsByAdmin()
                {

                    ProfileImage = empDetails.ProfileImage,
                    WebRootPath = _he.WebRootPath,

                    Employees = emp,
                    OeUsers = user,                   
                    Designations = designation,   
                    CurrentInstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))
                };
                msg = _employeeDetailsServ.UpdateEmployeeDetailsByAdmin(model);
            }
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

        #endregion "Post Methods - By Admin"

        #region "Post Methods- dropdown"
        public JsonResult DropDown_EmployeeCategoryTypes(long id)
        {
            var ddlEmpCategoryType = _employeeTypeCategoriesServ.Dropdown_EmployeeTypeCategories(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), id).ToList();
            return Json(new SelectList(ddlEmpCategoryType, "Id", "Name"));
        }
        #endregion "Post Methods- dropdown"
    }
}
