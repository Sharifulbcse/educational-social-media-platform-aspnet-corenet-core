
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;
using OE.Service.ServiceModels;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class SubjectTypesController : Controller
    {
        #region "Variables"
        private readonly ICommonServ _commonServ;
        private readonly ISubjectTypesServ _subjectTypesServ;
        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        
        #endregion "Variables"

        #region "Constructor"
        public SubjectTypesController(
            ICommonServ commonServ,
            IOE_InstitutionsServ oeInstitutionsServ,
            ISubjectTypesServ subjectTypesServ
            
            
            )
        {
            _commonServ = commonServ;
            _subjectTypesServ = subjectTypesServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            
        }
        #endregion "Constructor"
               
        #region "Get_Methods"
        [HttpGet]        
        public IActionResult SubjectTypes()
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var getSubjectTypes = _subjectTypesServ.GetSubjectTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).ToList();
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        ViewBag.msg = TempData["error"];

                        var subjecttv = new List<SubjectTypesListVM>();
                        foreach (var item in getSubjectTypes)
                        {
                            var e = new SubjectTypesListVM()
                            {
                                Id = item.Id,
                                Name = item.Name
                            };
                            subjecttv.Add(e);
                        }
                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexSubjectTypesVM()
                        {
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            _SubjectTypesList = subjecttv
                        };
                        return View("SubjectTypes", model);
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
        public JsonResult InsertSubjectTypes(IndexSubjectTypesVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
             try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (obj.SubjectTypesList != null)
                        {
                            var temp = new SubjectTypes()
                            {
                                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                Name = obj.SubjectTypesList.Name,

                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                            };

                            var model = new InsertSubjectTypes()
                            {
                                subjectTypes = temp
                            };
                            message = _subjectTypesServ.InsertSubjectTypes(model);
                            result = Json(new { success = true, Message = message });
                            
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:SubjectTypes/InsertSubjectTypes - " + ex.Message });
               
            }
            return result;            
        }

        [HttpPost]        
        public IActionResult Edit(long editSubjectTypesId, string txtEditSubjectTypesName)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var fetchRecord = _subjectTypesServ.GetSubjectTypesById(editSubjectTypesId);
                        fetchRecord.Name = txtEditSubjectTypesName;
                        fetchRecord.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        fetchRecord.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _subjectTypesServ.UpdateSubjectTypes(fetchRecord);

                        return RedirectToAction("SubjectTypes");
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
        public JsonResult DeleteSubjectTypes(int subTypeId)
        {            
            var result = (dynamic)null;            
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (subTypeId > 0)
                        {
                            
                            var model = new DeleteSubjectTypes()
                            {
                                Id = subTypeId
                            };
                            var returnResult = _subjectTypesServ.DeleteSubjectTypes(model);
                            result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                            return result;                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    result = Json(new { success = false, Message = "Subject Type is not possible to delete because it is used another place" });
                }
                else
                {
                    result = Json(new { success = false, Message = "ERROR101:SubjectTypes/DeleteSubjectTypes - " + ex.Message });
                }
            }
            return result;
        }
        #endregion "Post_Methods"       

    }
}
