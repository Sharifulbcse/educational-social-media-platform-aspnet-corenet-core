using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;
using OE.Service.ServiceModels;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class EmployeeTypeCategoriesController : Controller
    {
        #region "Variables"
        private readonly ICommonServ _commonServ;

        private readonly IEmployeeTypeCategoriesServ _employeecategoriesServ;
        private readonly IEmployeeTypesServ _employeeTypesServ;

        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        #endregion "Variables"

        #region "Constructor"
        public EmployeeTypeCategoriesController(
            IEmployeeTypeCategoriesServ employeecategoriesServ,
            IOE_InstitutionsServ oeInstitutionsServ,
            IEmployeeTypesServ employeeTypesServ,
            ICommonServ commonServ
        )
        {
           
            _employeecategoriesServ = employeecategoriesServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _employeeTypesServ = employeeTypesServ;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get_Methods"
        [HttpGet]
        public IActionResult EmployeeTypeCategories()
        {
            var listempcategory = _employeecategoriesServ.GetEmployeeType(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).ToList();
            ViewBag.ddlempType = _employeeTypesServ.Dropdown_EmployeeTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

            var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                       
            var empcategory = new List<EmployeeTypeCategoriesListVM>();
            foreach (var item in listempcategory)
            {
                var e = new EmployeeTypeCategoriesListVM()
                {
                    Id = item.EmployeeTypeCategories.Id,
                    Name = item.EmployeeTypeCategories.Name,
                    InstitutionId = item.EmployeeTypeCategories.InstitutionId,
                    EmployeeType = item.EmployeeTypes.Name,
                    EmployeeTypeId = item.EmployeeTypeCategories.EmployeeTypeId
                };
                empcategory.Add(e);
            }

            var model = new IndexEmployeeTypeCategoriesVM()
            {
                InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                _EmpTypeCategoryList = empcategory
            };
            return View("EmployeeTypeCategories", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public JsonResult InsertEmployeeTypeCategories(IndexEmployeeTypeCategoriesVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                    {
                        if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                        {
                           if (obj.EmpTypeCategoryList != null)
                            {
                                var empTypeCate = new EmployeeTypeCategories()
                                {
                                    Name = obj.EmpTypeCategoryList.Name,
                                    EmployeeTypeId = obj.EmpTypeCategoryList.EmployeeTypeId,

                                    InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                    IsActive = true,
                                    AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                    AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                                };

                                var model = new GetEmployeeTypeCategories()
                                {
                                    EmployeeTypeCategories = empTypeCate,

                                };
                                message = _employeecategoriesServ.InsertEmployeeTypeCategories(model);
                                result = Json(new { success = true, Message = message });
                            }
                        }
                    }                    
                }
                else
                {
                    result = Json(new { success = false, Message = "ERROR101:EmployeeTypeCategories/InsertEmployeeTypeCategories - " + message });
                    
                }
            }
            catch (Exception ex)           
            {
                result = Json(new { success = false, Message = "ERROR101:EmployeeTypeCategories/InsertEmployeeTypeCategories - " + ex.Message });
                
            }
            return result;
        }

        [HttpPost]
        public IActionResult Edit(int editEmpCategoryId, string txtEditEmpCategoryName, long ddlEditEmpTypeId)
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                    {
                        if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                        {
                            var empcategory = _employeecategoriesServ.GetEmployeeTypeCategoriesById(editEmpCategoryId);
                            empcategory.Name = txtEditEmpCategoryName;
                            empcategory.EmployeeTypeId = ddlEditEmpTypeId;
                            empcategory.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                            empcategory.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                            _employeecategoriesServ.UpdateEmployeeTypeCategories(empcategory); 
                            return RedirectToAction("EmployeeTypeCategories");
                        }
                    }
                    return RedirectToAction("Login", "Home", new { area = "" });
                }
                else
                {
                    return RedirectToAction("Login", "Home", new { area = "" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }

        }

        [HttpPost]
        public JsonResult DeleteEmployeeTypeCategories(int empTypesCategoriesId)
        {           
            var result = (dynamic)null;            
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        if (empTypesCategoriesId > 0)
                        {
                            var model = new DeleteEmployeeTypeCategories()
                            {
                                Id = empTypesCategoriesId
                            };
                            var returnResult = _employeecategoriesServ.DeleteEmployeeTypeCategories(model);
                            result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                            return result;                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:EmployeeTypeCategories/DeleteEmployeeTypeCategories - " + ex.Message });
            }
            return result;
        }
        #endregion "Post_Methods"

    }
}

