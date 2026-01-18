
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using OE.Data;
using OE.Service;
using OE.Service.CustomEntitiesServ;
using OE.Service.ServiceModels;
using OE.Web.Areas.Institution.Models;

using System;
using System.Collections.Generic;
using System.Linq;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class RegistrationsController : Controller 
    {
        #region "Variables"
        private readonly IClassesServ _classesServ;
        private readonly ICOM_GendersServ _comGendersServ;
        private readonly ICOM_RegistrationUserTypesServ _comRegistrationUserTypesServ;

        private readonly IEmployeesServ _employeesServ;
        private readonly IEmployeeDetailsServ _employeeDetailsServ;
        private readonly IEmployeeDesignationsServ _employeeDesignationsServ;
        private readonly IEmployeeTypesServ _employeeTypesServ;
        private readonly IEmployeeTypeCategoriesServ _employeeTypeCategoriesServ;

        private readonly IHostingEnvironment _he;

       private readonly IOE_InstitutionsServ _oeInstitutionsServ;

        private readonly IRegistrationGroupsServ _registrationGroupsServ;
        private readonly IRegistrationItemTypesServ _registrationItemTypesServ;
        private readonly IRegistrationItemsServ _registrationItemsServ;

        private readonly IStudentsServ _studentsServ;      
        private readonly IStudentDetailsServ _studentDetailsServ;
        private readonly ICommonServ _commonServ;

        #endregion "Variables"

        #region "Constructor"
        public RegistrationsController(
            IRegistrationGroupsServ registrationGroupsServ, 
            IRegistrationItemTypesServ registrationItemTypesServ, 
            IRegistrationItemsServ registrationItemsServ, 
            ICOM_GendersServ comGendersServ, 
            
            IClassesServ classesServ, 
            IStudentsServ studentsServ,
            IStudentDetailsServ studentDetailsServ,
            IOE_InstitutionsServ oeInstitutionsServ,
            ICOM_RegistrationUserTypesServ comRegistrationUserTypesServ,
            IEmployeesServ employeesServ,
            IEmployeeDetailsServ employeeDetailsServ,
            IHostingEnvironment e,
            IEmployeeTypesServ employeeTypesServ,
            IEmployeeTypeCategoriesServ employeeTypeCategoriesServ,
            IEmployeeDesignationsServ employeeDesignationsServ,
            ICommonServ commonServ
            )
        {
            _registrationGroupsServ = registrationGroupsServ;
            _registrationItemTypesServ = registrationItemTypesServ;
            _registrationItemsServ = registrationItemsServ;
            _comGendersServ = comGendersServ;
            
            _classesServ = classesServ;
            _studentsServ = studentsServ;           
            _studentDetailsServ = studentDetailsServ;

            _oeInstitutionsServ = oeInstitutionsServ;
            _comRegistrationUserTypesServ = comRegistrationUserTypesServ;
            _employeesServ = employeesServ;
            _employeeDetailsServ = employeeDetailsServ;
            _he = e;
            _employeeTypesServ = employeeTypesServ;
            _employeeTypeCategoriesServ = employeeTypeCategoriesServ;
            _employeeDesignationsServ = employeeDesignationsServ;
            _commonServ = commonServ;

        }
        #endregion "Constructor"

        #region "Get_Methods"
        public IActionResult EmployeeRegistrationForm(long EmployeeTypeId, IndexRegistrationItemVM obj)

        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        ViewBag.ddlGender = _comGendersServ.Dropdown_COM_Genders();

                        ViewBag.ddlEmpType = _employeeTypesServ.Dropdown_EmployeeTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        var listRegistrationItem = _registrationItemsServ.GetRegItems(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 2).ToList();
                        var listRegistrationGroup = _registrationGroupsServ.GetRegistrationGroups(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 2).ToList();
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        var registrationItemv = new List<RegistrationItemListVM>();
                        var registrationGroupv = new List<RegistrationGroupListVM>();

                        foreach (var item in listRegistrationItem)
                        {
                            var ri = new RegistrationItemListVM();
                            ri.Id = item.RegistrationItemId;
                            ri.Name = item.RegistrationItemName;
                            ri.InstitutionId = item.InstitutionId;
                            ri.RegistrationGroupId = item.RegistrationGroupId;
                            ri.RegistrationGroupName = item.RegistrationGroupsName;
                            ri.RegistrationItemTypeId = item.RegistrationItemTypeId;
                            ri.RegistrationItemTypeName = item.RegistrationItemTypeName;
                            registrationItemv.Add(ri);
                        }

                        foreach (var item in listRegistrationGroup)
                        {
                            var rg = new RegistrationGroupListVM();
                            rg.Id = item.RegistrationGroups.Id;
                            rg.Name = item.RegistrationGroups.Name;
                            rg.InstitutionId = item.RegistrationGroups.InstitutionId;
                            rg.RegistrationUserTypeId = item.RegistrationGroups.RegistrationUserTypeId;
                            rg.RegistrationUserTypeName = item.COM_RegistrationUserTypes.Name;
                            registrationGroupv.Add(rg);
                        }

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexRegistrationItemVM()
                        {
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            _RegistrationItemList = registrationItemv,
                            _RegistrationGroupList = registrationGroupv,
                           // _GenderList = new SelectList(ddlGender, "Id", "Value"),
                            Employees = obj.Employees
                          
                        };
                        //[Note: Viewing Message to User on registration page.]
                        ViewBag.successMsg = TempData["successMsg"];
                        ViewBag.errorMsg = TempData["errorMsg"];
                        return View("RegistrationForm/EmployeeRegistrationForm", model);
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        public IActionResult RegistrationFormByAdmin(long EmployeeTypeId, IndexRegistrationItemVM obj)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11" )
                    {                        
                        ViewBag.ddlGender = _comGendersServ.Dropdown_COM_Genders();
                        ViewBag.ddlEmpType = _employeeTypesServ.Dropdown_EmployeeTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                                         

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexRegistrationFormByAdminVM()
                        {
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            //DdlGenders = new SelectList(ddlGender, "Id", "Value"),
                            Employees = obj.Employees

                        };
                        //[Note: Viewing Message to User on registration page.]
                        ViewBag.successMsg = TempData["successMsg"];
                        ViewBag.errorMsg = TempData["errorMsg"];
                        return View("RegistrationForm/RegistrationFormByAdmin", model);
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        public IActionResult RegistrationGroups(long registrationUserTypeId)        
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var listRegistrationGroup = _registrationGroupsServ.GetRegistrationGroups(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), registrationUserTypeId).ToList();
                        var registrationUserType = _comRegistrationUserTypesServ.GetRegistrationUserTypeById(registrationUserTypeId);
                        
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        var registrationGroupv = new List<RegistrationGroupListVM>();
                        foreach (var item in listRegistrationGroup)
                        {
                            var rg = new RegistrationGroupListVM()
                            {
                                Id = item.RegistrationGroups.Id,
                                InstitutionId = item.RegistrationGroups.InstitutionId,
                                Name = item.RegistrationGroups.Name,
                                RegistrationUserTypeId = item.RegistrationGroups.RegistrationUserTypeId,
                                RegistrationUserTypeName = item.COM_RegistrationUserTypes.Name
                            }; registrationGroupv.Add(rg);
                        }
                        
                        var regUserTypeVM = new COM_RegistrationUserTypesVM()
                        {
                            Id = registrationUserType.Id,
                            Name = registrationUserType.Name
                        };                                         

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexRegistrationGroupVM()
                        {
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            _RegistrationGroupList = registrationGroupv,
                            COM_RegistrationUserTypesVM = regUserTypeVM,                           

                        };
                        return View("RegistrationGroups/RegistrationGroups", model);
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        
        public IActionResult RegistrationItems(long registrationUserTypeId)        
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {                        
                        var registrationUserType = _comRegistrationUserTypesServ.GetRegistrationUserTypeById(registrationUserTypeId);
                        var listRegistrationItem = _registrationItemsServ.GetRegItems(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), registrationUserTypeId).ToList();
                        ViewBag.ddlComRegistrationItemTypes = _registrationItemTypesServ.Dropdown_RegistrationItemTypes();
                        ViewBag.ddlRegistrationGroups = _registrationGroupsServ.Dropdown_RegistrationGroups(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), registrationUserTypeId);
                       
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        var registrationItemv = new List<RegistrationItemListVM>();
                        foreach (var item in listRegistrationItem)
                        {
                            var ri = new RegistrationItemListVM();
                            ri.Id = item.RegistrationItemId;
                            ri.Name = item.RegistrationItemName;
                            ri.InstitutionId = item.InstitutionId;
                            ri.RegistrationGroupId = item.RegistrationGroupId;
                            ri.RegistrationGroupName = item.RegistrationGroupsName;
                            ri.RegistrationItemTypeId = item.RegistrationItemTypeId;
                            ri.RegistrationItemTypeName = item.RegistrationItemTypeName;

                            ri.RegistrationUserTypeId = item.RegistrationUserTypeId;

                            registrationItemv.Add(ri);
                        }

                        
                        var regUserTypeVM = new COM_RegistrationUserTypesVM()
                        {
                            Id = registrationUserType.Id,
                            Name = registrationUserType.Name
                        };
                        

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexRegistrationItemVM()
                        {
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            _RegistrationItemList = registrationItemv,

                            COM_RegistrationUserTypesVM = regUserTypeVM
                           
                        };
                        return View("RegistrationItems/RegistrationItems", model);
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        public IActionResult StudentRegistrationForm()
        {

            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        ViewBag.ddlGender = _comGendersServ.Dropdown_COM_Genders();
                        ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        
                        var listRegistrationItem = _registrationItemsServ.GetRegItems(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 1).ToList();
                        var listRegistrationGroup = _registrationGroupsServ.GetRegistrationGroups(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 1).ToList();


                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        var registrationItemv = new List<RegistrationItemListVM>();
                        var registrationGroupv = new List<RegistrationGroupListVM>();

                        foreach (var item in listRegistrationItem)
                        {
                            var ri = new RegistrationItemListVM();
                            ri.Id = item.RegistrationItemId;
                            ri.Name = item.RegistrationItemName;
                            ri.InstitutionId = item.InstitutionId;
                            ri.RegistrationGroupId = item.RegistrationGroupId;
                            ri.RegistrationGroupName = item.RegistrationGroupsName;
                            ri.RegistrationItemTypeId = item.RegistrationItemTypeId;
                            ri.RegistrationItemTypeName = item.RegistrationItemTypeName;
                            registrationItemv.Add(ri);
                        }


                        foreach (var item in listRegistrationGroup)
                        {
                            var rg = new RegistrationGroupListVM();
                            rg.Id = item.RegistrationGroups.Id;
                            rg.Name = item.RegistrationGroups.Name;
                            rg.InstitutionId = item.RegistrationGroups.InstitutionId;
                            rg.RegistrationUserTypeId = item.RegistrationGroups.RegistrationUserTypeId;
                            rg.RegistrationUserTypeName = item.COM_RegistrationUserTypes.Name;
                            registrationGroupv.Add(rg);
                        }



                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexRegistrationItemVM()
                        {
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            _RegistrationItemList = registrationItemv,                            
                            _RegistrationGroupList = registrationGroupv                            
                        };

                        //[Note: Viewing Message to User on registration page.]
                        ViewBag.successMsg = TempData["successMsg"];
                        ViewBag.errorMsg = TempData["errorMsg"];
                        return View("RegistrationForm/StudentRegistrationForm", model);

                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public JsonResult InsertRegistrationItem(IndexRegistrationItemVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (obj.RegistrationItemList != null)
                        {
                            var regItem = new RegistrationItems()
                            {
                                Name = obj.RegistrationItemList.Name,
                                RegistrationGroupId = obj.RegistrationItemList.RegistrationGroupId,
                                RegistrationUserTypeId = obj.RegistrationItemList.RegistrationUserTypeId,
                                RegistrationItemTypeId = obj.RegistrationItemList.RegistrationItemTypeId,
                                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                IsActive = true,
                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                AddedDate = DateTime.Now

                            };

                            var model = new InsertRegistrationItem()
                            {
                                RegistrationItems = regItem
                            };

                            message = _registrationItemsServ.InsertRegistrationItem(model);
                            result = Json(new { success = true, Message = message });
                        } 
                    }
                }                
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:Registrations/InsertRegistrationItem - " + ex.Message });                
            }

            return result;
           
        }
        [HttpPost]
        public JsonResult InsertRegistrationGroup(IndexRegistrationGroupVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (obj.RegistrationGroupList != null)
                        {
                            var regGroup = new RegistrationGroups()
                            {
                                Name = obj.RegistrationGroupList.Name,
                                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),

                                RegistrationUserTypeId = obj.RegistrationGroupList.RegistrationUserTypeId,
                                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),

                                IsActive = true,
                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                ModifiedBy = 0

                            };

                            var model = new InsertRegistrationGroup()
                            {
                                RegistrationGroups = regGroup
                            };
                            message = _registrationGroupsServ.InsertRegistrationGroup(model);
                            result = Json(new { success = true, Message = message });
                        }                      
                    }
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:Registrations/InsertRegistrationGroup - " + ex.Message });                
            }
            return result;
        }

        [HttpPost]
        public IActionResult EditRegistrationItem(long editRegistrationItemId, string txtEditRegistrationItemName, long ddlEditRegITypeId, long ddlEditRegGroupId, long ddlregusertype)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var fetchRecord = _registrationItemsServ.GetRegistrationItemById(editRegistrationItemId);
                        fetchRecord.Name = txtEditRegistrationItemName;
                        fetchRecord.RegistrationUserTypeId = ddlregusertype;
                        fetchRecord.RegistrationItemTypeId = ddlEditRegITypeId;
                        fetchRecord.RegistrationGroupId = ddlEditRegGroupId;
                        fetchRecord.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                       
                        fetchRecord.ModifiedDate = DateTime.Now;
                        
                        _registrationItemsServ.UpdateRegistrationItem(fetchRecord);
                        return RedirectToAction("RegistrationItems", "Registrations", new { area = "Institution", registrationUserTypeId = ddlregusertype });
                        
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
        public IActionResult EditRegistrationGroup(long editRegistrationGroupId, string txtEditRegistrationGroupName, long editRegUserType)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var fetchRecord = _registrationGroupsServ.GetRegistrationGroupById(editRegistrationGroupId);
                        fetchRecord.RegistrationUserTypeId = editRegUserType;
                         fetchRecord.Name = txtEditRegistrationGroupName;
                        fetchRecord.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        fetchRecord.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);


                        _registrationGroupsServ.UpdateRegistrationGroup(fetchRecord);
                        return RedirectToAction("RegistrationGroups", "Registrations", new { area = "Institution", registrationUserTypeId = editRegUserType });

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
        public IActionResult DeleteRegistrationItem(long deleteRegistrationItemId, long ddlregusertype)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var fetchRecord = _registrationItemsServ.GetRegistrationItemById(deleteRegistrationItemId);
                        _registrationItemsServ.DeleteRegistrationItem(fetchRecord);
                       return RedirectToAction("RegistrationItems", "Registrations", new { area = "Institution", registrationUserTypeId = ddlregusertype });
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
        public IActionResult DeleteRegistrationGroup(long deleteRegistrationGroupId, long deleteRegUserTypeId)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var fetchRecord = _registrationGroupsServ.GetRegistrationGroupById(deleteRegistrationGroupId);
                        _registrationGroupsServ.DeleteRegistrationGroup(fetchRecord);
                        return RedirectToAction("RegistrationGroups", "Registrations", new { area = "Institution", registrationUserTypeId = deleteRegUserTypeId });
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        #endregion "Post_Methods"

        #region "Post Methods-Students"
        [HttpPost]
        public JsonResult AddStudentRegistrationForm(IndexRegistrationItemVM obj)
        {
            var user = (dynamic)null;
            var msg = (dynamic)null;
            var result = (dynamic)null;

            try
            {
                var stu = new Students()
                {
                    InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                    UserId = (obj.Students.UserId != null) ? obj.Students.UserId : (dynamic)null,
                    ClassId = obj.Students.ClassId,
                    GenderId = obj.Students.GenderId,
                    Name = obj.Students.Name,

                    AdmittedYear = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(obj.Students.AdmittedYear)),

                    PresentAddress = obj.Students.PresentAddress,
                    PermanentAddress = obj.Students.PermanentAddress,
                    DOB = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(obj.Students.DOB)),

                    IsActive = true,
                    AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                    AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),

                    InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))
                };

                user = new OE_Users()
                {
                    UserLoginId = obj.Students.UserLoginId
                };

                var stuDetails = new List<C_StudentDetails>();
                if (obj.studentsDetailsLists != null)
                {
                    foreach (var item in obj.studentsDetailsLists)
                    {
                        var studentDetails = new C_StudentDetails();
                        studentDetails.InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"));
                        studentDetails.StudentId = _studentsServ.GetStudents().LastOrDefault().Id;
                        studentDetails.RegistrationItemId = item.RegistrationItemId;
                        studentDetails.RegistrationItemTypeId = item.RegistrationItemTypeId;
                        studentDetails.AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        studentDetails.InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"));

                        switch (item.RegistrationItemTypeId)
                        {
                            case 1:
                                {
                                    studentDetails.ActualFile = item.ActualFile;
                                    break;
                                }
                            case 2:
                                {
                                    studentDetails.ActualImage = item.ActualImage;
                                    break;
                                }
                            case 3:
                                {
                                    studentDetails.BitValue = item.BitValue;
                                    break;
                                }
                            case 4:
                                {
                                    studentDetails.StringValue = item.StringValue;
                                    break;
                                }
                            case 5:
                                {
                                    studentDetails.WholeValue = item.WholeValue;
                                    break;
                                }
                            case 6:
                                {
                                    studentDetails.FloatValue = item.FloatValue;
                                    break;
                                }
                            case 7:
                                {
                                    studentDetails.TextAreaValue = item.TextAreaValue;
                                    break;
                                }
                            case 8:
                                {
                                    studentDetails.DateValue = _commonServ.CommDate_ConvertToUtcDate(Convert.ToDateTime(item.DateValue));
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }

                        stuDetails.Add(studentDetails);
                    }
                }

                var model = new InsertStudents()
                {
                    RollNo = obj.RollNo,
                    CurrentInstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                    EnableAuthentic = obj.Students.EnableAuthentic,
                    Students = stu,
                    OeUsers = user,
                    StudentDetails = stuDetails
                };

                msg = _studentsServ.InsertStudents(model, obj.Students.ActualFile, _he.WebRootPath);
                if (msg == null)
                {
                    TempData["successMsg"] = "Registration successfully Completed.";
                    result = Json(new { success = true, successMessage = "" });
                }
                else
                {
                    result = Json(new { success = true, errorMessage = msg });
                }


            }
            catch (Exception EX)
            {
                TempData["errorMsg"] = "Sorry, registration is not completed.";
            }

            return result;

        }
        #endregion "Post Methods - Students"

        #region "Post Methods - Teacher"
        [HttpPost]
        public JsonResult AddEmployeeRegistrationForm(IndexRegistrationItemVM obj)

        {
            var temp = (dynamic)null;
            var user = (dynamic)null;
            var empDetails = (dynamic)null;
            var msg = (dynamic)null;
            var result = (dynamic)null;

            try
            {
                if (obj.Employees != null)
                {
                    //[Note: Creating new Employee registration]
                    temp = new Employees()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        UserId = (obj.Employees.UserId != null) ? obj.Employees.UserId : (dynamic)null,
                        GenderId = obj.Employees.GenderId,
                        FirstName = obj.Employees.FirstName,
                        LastName = obj.Employees.LastName,
                        EmailAddress = obj.Employees.EmailAddress,
                        ContactNo = obj.Employees.ContactNo,
                        JoiningDate = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(DateTime.Now)),
                        PresentAddress = obj.Employees.PresentAddress,
                        PermanentAddress = obj.Employees.PermanentAddress,
                        DOB = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(obj.Employees.DOB)),
                        IsActive = true,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),
                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))
                    };
                }
                user = new OE_Users()
                {
                    UserLoginId = obj.Employees.UserLoginId
                };

                empDetails = new List<C_EmployeeDetails>();

                if (obj.EmpDetailsList != null)
                {
                    
                    foreach (var item in obj.EmpDetailsList)
                    {
                        var newEmp = new C_EmployeeDetails();
                        newEmp.InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"));
                        newEmp.RegistrationItemId = item.RegistrationItemId;
                        newEmp.RegistrationItemTypeId = item.RegistrationItemTypeId;
                        newEmp.AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        newEmp.InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"));

                        switch (item.RegistrationItemTypeId)
                        {
                            case 1:
                                {
                                    newEmp.ActualFile = item.ActualFile;
                                };
                                break;
                            case 2:
                                {
                                    newEmp.ActualImage = item.ActualImage;
                                };
                                break;
                            case 3:
                                {
                                    newEmp.BitValue = item.BitValue;
                                };
                                break;
                            case 4:

                                {
                                    newEmp.StringValue = item.StringValue;
                                };
                                break;
                            case 5:
                                {
                                    newEmp.WholeValue = item.WholeValue;

                                };
                                break;
                            case 6:
                                {
                                    newEmp.FloatValue = item.FloatValue;

                                };
                                break;
                            case 7:
                                {
                                    newEmp.TextAreaValue = item.TextAreaValue;

                                };
                                break;
                            case 8:
                                {
                                    newEmp.DateValue = item.DateValue;
                                };

                                break;
                            default:
                                break;
                        }
                        empDetails.Add(newEmp);
                    }
                   
                }
                var model = new InsertEmployee()
                {
                    OeUsers = user,
                    CurrentInstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                    Employees = temp,
                    EmployeeDetails = empDetails,
                    EmployeeImagefile = obj.Employees.EmployeeImage,
                    EmployeeTypeId = obj.EmployeeTypeId,
                    EmployeeCategoryTypeId = obj.EmployeeCategoryTypeId
                };

                msg = _employeesServ.InsertEmployee(model, _he.WebRootPath);
                if (msg == null)
                {
                    TempData["successMsg"] = "Registration successfully Completed.";
                    result = Json(new { success = true, successMessage = "" });
                }
                else
                {
                    result = Json(new { success = true, errorMessage = msg });
                }
            }
            catch (Exception)
            {
                TempData["errorMsg"] = "Sorry, registration is not completed.";
            }
            return result;
        }
        [HttpPost]        
        public JsonResult AddRegistrationFormByAdmin(IndexRegistrationFormByAdminVM obj)
        {
            var user = (dynamic)null;
            var msg = (dynamic)null;
            var result = (dynamic)null;
           try
            {
                if (obj.Employees != null)
                {
                    //[Note: Creating new Employee registration]
                    var emp = new Employees()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        UserId = (obj.Employees.UserId != null) ? obj.Employees.UserId : (dynamic)null,
                        GenderId = obj.Employees.GenderId,
                        FirstName = obj.Employees.FirstName,
                        LastName = obj.Employees.LastName,
                        EmailAddress = obj.Employees.EmailAddress,
                        ContactNo = obj.Employees.ContactNo,
                        JoiningDate = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(DateTime.Now)),
                        PresentAddress = obj.Employees.PresentAddress,
                        PermanentAddress = obj.Employees.PermanentAddress,
                        DOB = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(obj.Employees.DOB)),
                        IsActive = true,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date),
                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))
                    };

                    user = new OE_Users()
                    {
                        UserLoginId = obj.Employees.UserLoginId
                    };
                    

                    var model = new InsertEmployeeByAdmin()
                    {                       
                        OeUsers = user,
                        CurrentInstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        Employees = emp,
                        EmployeeImagefile = obj.Employees.EmployeeImage,
                        EmployeeTypeId = obj.EmployeeTypeId,
                        EmployeeCategoryTypeId = obj.EmployeeCategoryTypeId

                    };                    
                    msg = _employeesServ.InsertEmployeeByAdmin(model, _he.WebRootPath);                    
                }
               
                if (msg == null)
                {
                    TempData["successMsg"] = "Registration successfully Completed.";
                    result = Json(new { success = true, successMessage = "" });
                    //return RedirectToPage("RegistrationFormByAdmin");
                }
                else
                {
                    result = Json(new { success = true, errorMessage = msg });
                }                
            }
            catch (Exception)
            {
                TempData["errorMsg"] = "Sorry, registration is not completed.";
            }            
            return result;
        }
        #endregion "Post Methods - Teacher"

        #region "Post Methods- dropdown"
        public JsonResult DropDown_EmployeeCategoryTypes(long id)
        {
            var ddlEmpCategoryType = _employeeTypeCategoriesServ.Dropdown_EmployeeTypeCategories(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), id).ToList();
            return Json(new SelectList(ddlEmpCategoryType, "Id", "Name"));
        }
        #endregion "Post Methods- dropdown"
    }
}
