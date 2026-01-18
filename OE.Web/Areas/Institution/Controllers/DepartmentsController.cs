
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
    public class DepartmentsController : Controller
    {
        #region "Variables"
        private readonly IDepartmentsServ _departmentsServ;
        private readonly IOE_UserAuthenticationsServ _userAuthenticationsServ;
        private readonly IOE_InstitutionsServ _institutionServ;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public DepartmentsController(
            IDepartmentsServ departmentsServ, 
            IOE_UserAuthenticationsServ userAuthenticationsServ,
            IOE_InstitutionsServ institutionServ,
            ICommonServ commonServ
            )
        {
            _userAuthenticationsServ =userAuthenticationsServ;
            _departmentsServ = departmentsServ;
            _institutionServ = institutionServ;
            _commonServ = commonServ;
        }
        #endregion "Constructor"
        
        #region "Get_Methods"
        [HttpGet]
        public IActionResult Departments()
        {           
            var listDept = _departmentsServ.GetDepartments(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).ToList();
            var currentInstitutionDetails = _institutionServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            ViewBag.msg = TempData["error"];
           
            var list = from e in listDept
                       select new
                       {
                           e.Id,
                           e.Name,
                           e.InstitutionId
                       };

            var deptv = new List<DepartmentListVM>();

            foreach (var item in list.ToList())
            {
                var e = new DepartmentListVM();               
                e.Id = item.Id;
                e.Name = item.Name;
                e.InstitutionId = item.InstitutionId;
                deptv.Add(e);
            }

            //[NOTE: integrate all records for the specific page. ]
            var model = new IndexDepartmentVM()
            {
               InstitutionName = currentInstitutionDetails!=null? currentInstitutionDetails.Name: "",
               _DepartmentList = deptv
                
            };
            return View("Departments", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public JsonResult InsertDepartment(IndexDepartmentVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;

            try
            {
                if (obj.DepartmentList != null)
                {
                    var department = new Departments()
                    {
                        Name = obj.DepartmentList.Name,
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        IsActive = true,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                    };

                    var model = new InsertDepartments()
                    {
                        Departments = department
                    };
                    message = _departmentsServ.InsertDepartments(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:Departments/InsertDepartments - " + ex.Message });

            }
            return result;            
        }

        [HttpPost]        
        public IActionResult Edit(int editDepartmentId, string txtEditDepartmentName)
        {
            var departments = _departmentsServ.GetDepartmentById(editDepartmentId);
            departments.Name = txtEditDepartmentName;
            departments.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            departments.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

            _departmentsServ.UpdateDepartments(departments);

            return RedirectToAction("Departments");
        }
       
        [HttpPost]
        public JsonResult DeleteDepartments(int departmentId)
        {
            var result = (dynamic)null;           
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        if (departmentId > 0)
                        {                           
                            var model = new DeleteDepartments()
                            {
                                Id = departmentId
                            };
                            var returnResult = _departmentsServ.DeleteDepartments(model);
                            result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                            return result;                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:Departments/DeleteDepartments - " + ex.Message });
            }
            return result;
        }
        #endregion "Post_Methods"

    }
}
