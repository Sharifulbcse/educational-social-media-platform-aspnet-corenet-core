
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class ClassesController : Controller
    {
        #region "Variables"
        private readonly IClassesServ _classesServ;
        private readonly ICommonServ _commonServ;      
           
        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        
        #endregion "Variables"

        #region "Constructor"
        public ClassesController(            
            IClassesServ classesServ,           
            IOE_InstitutionsServ oeInstitutionsServ,
            ICommonServ commonServ
            )
        {
            _classesServ = classesServ;
            _oeInstitutionsServ = oeInstitutionsServ; 
            _commonServ = commonServ;
        }
        #endregion "Constructor"
       
        #region "Get_Methods"
        [HttpGet]
        public IActionResult Classes()
        {            
            var listClasses = _classesServ.GetClasses(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).ToList();
            var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));


            ViewBag.msg = TempData["error"];
            var classList = new List<ClassesListVM>();

            foreach (var item in listClasses)
            {
                var e = new ClassesListVM();
                e.Id = item.Id;
                e.Name = item.Name;
                e.Sorting = item.Sorting;
                classList.Add(e);
            }

            //[NOTE: integrate all records for the specific page. ]
            var model = new IndexClassesVM()
            {
                InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                _classesVM = classList
            };


            return View("Classes", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public JsonResult InsertClasses(IndexClassesVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            try
            {
                if (obj.classesVM != null)
                {
                    var temp = new Classes()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        Name = obj.classesVM.Name,
                        Sorting = obj.classesVM.Sorting,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                    };

                    var model = new InsertClasses()
                    {
                        classes = temp
                    };
                    message = _classesServ.InsertClasses(model);
                    result = Json(new { success = true, Message = message });
                    
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:Classes/InsertClasses - " + ex.Message });
                
            }
            return result;            
        }
        [HttpPost]
         public IActionResult Edit(Int64 editClassesId, string txtEditClassesName, long editSortingNo)
        {
            var classes = _classesServ.GetClassById(editClassesId);
            classes.Name = txtEditClassesName;            
            classes.Sorting = editSortingNo;
            classes.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            classes.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);
            _classesServ.UpdateClasses(classes);
            return RedirectToAction("Classes");
        }
        [HttpPost]
        public JsonResult DeleteClasses(int classesId)
        {
            var result = (dynamic)null;            
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (classesId > 0)
                        {
                            var model = new DeleteClasses()
                            {
                                Id = classesId
                            };
                            var returnResult = _classesServ.DeleteClasses(model);
                            result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                            return result;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:Classes/DeleteClasses - " + ex.Message });
            }
            return result;
        }

        #endregion "Post_Methods"
    }
}
