using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using OE.Data;
using OE.Service;
using OE.Service.ServiceModels;
using OE.Web.Areas.Institution.Models;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class EmployeeTypesController : Controller
    {
        #region "Variables"
        private readonly IEmployeeTypesServ _employeeTypesServ;
        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public EmployeeTypesController(
            IEmployeeTypesServ employeeTypesServ,
            
            IOE_InstitutionsServ oeInstitutionsServ,
            ICommonServ commonServ
            )
        {
          
            _employeeTypesServ = employeeTypesServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get_Methods"
        [HttpGet]
        public IActionResult EmployeeTypes()
        {
            var listempType = _employeeTypesServ.GetEmployeeTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).ToList();


            var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));


            ViewBag.msg = TempData["error"];

            var list = from e in listempType
                       select new
                       {
                           e.Id,
                           e.Name,
                           e.InstitutionId
                       };

            var empType = new List<EmployeeTypeListVM>();

            foreach (var item in list.ToList())
            {
                var e = new EmployeeTypeListVM();
                e.Id = item.Id;
                e.Name = item.Name;
                e.InstitutionId = item.InstitutionId;
                empType.Add(e);
            }

            //[NOTE: integrate all records for the specific page. ]
            var model = new IndexEmployeeTypeVM()
            {
                InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                _EmployeeTypeList = empType

            };
            return View("EmployeeTypes", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public JsonResult InsertEmployeeTypes(IndexEmployeeTypeVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;

            try
            {
                if (obj.EmployeeTypeList != null)
                {
                    var empTypes = new EmployeeTypes()
                    {
                        Name = obj.EmployeeTypeList.Name,
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        IsActive = true,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                    };

                    var model = new InsertEmployeeTypes()
                    {
                        EmployeeTypes = empTypes
                    };
                    message = _employeeTypesServ.InsertEmployeeTypes(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:EmployeeTypes/InsertEmployeeTypes - " + ex.Message });

            }
            return result;
        }
        
        [HttpPost]
        public IActionResult Edit(int editEmployeeTypesId, string txtEditEmployeeTypesName)
        {
            var employeeType = _employeeTypesServ.GetEmployeeTypeById(editEmployeeTypesId);
            employeeType.Name = txtEditEmployeeTypesName;
            employeeType.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            employeeType.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

            _employeeTypesServ.UpdateEmployeeTypes(employeeType);

            return RedirectToAction("EmployeeTypes");
        }
        
        [HttpPost]
        public JsonResult DeleteEmployeeTypes(int employeeTypesId)
        {           
            var result = (dynamic)null;           
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        if (employeeTypesId > 0)
                        {                            
                            var model = new DeleteEmployeeTypes()
                            {
                                Id = employeeTypesId
                            };
                            var returnResult = _employeeTypesServ.DeleteEmployeeTypes(model);
                            result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                            return result;                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:EmployeeTypes/DeleteEmployeeTypes - " + ex.Message });
            }
            return result;
        }
        #endregion "Post_Methods"

    }
}

