
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;
using OE.Service.ServiceModels;
namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class ExamTypesController : Controller
    {
        #region "Variables"
        private readonly IClassesServ _classesServ;
        private readonly ICommonServ _commonServ;

        private readonly IExamTypesServ _examTypesServ;

        private readonly IOE_InstitutionsServ _oeInstitutionsServ;        
        #endregion "Variables"

        #region "Constructor"
        public ExamTypesController(
            IExamTypesServ examTypesServ, 
            IOE_InstitutionsServ oeInstitutionsServ,
            IClassesServ classesServ,
            ICommonServ commonServ
        )
        {
            _examTypesServ = examTypesServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _classesServ = classesServ;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get_Methods"
        public IActionResult ExamTypes(long ddlClassId = 0)
        {
            try
            {

                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var listExam = _examTypesServ.GetExamTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), ddlClassId).ToList();
                        ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        ViewBag.msg = TempData["error"];

                        var examv = new List<ExamTypeListVM>();
                        foreach (var item in listExam)
                        {
                            var e = new ExamTypeListVM()
                            {
                                Id = item.ExamTypes.Id,
                                Name = item.ExamTypes.Name,
                                InstitutionId = item.ExamTypes.InstitutionId,
                                ClassId = item.ExamTypes.ClassId,
                                ClassName = item.Classes.Name,
                                BreakDownInP = item.ExamTypes.BreakDownInP,
                                IsLastExam = item.ExamTypes.IsLastExam,
                                Sorting = item.ExamTypes.Sorting

                            }; examv.Add(e);
                        }
                       


                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexExamTypeVM()
                        {
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            SelectedClassId = ddlClassId,
                            _ExamTypesList = examv
                        };
                        return View("ExamTypes", model);
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
        public JsonResult InsertExamTypes(IndexExamTypeVM obj)
        {            
            var result = (dynamic)null;
            string message = (dynamic)null;           
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (obj.ExamType != null)
                        {
                            var examType = new ExamTypes()
                            {
                                Name = obj.ExamType.Name,
                                Sorting = obj.ExamType.Sorting,
                                BreakDownInP = obj.ExamType.BreakDownInP,
                                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                ClassId = obj.ExamType.ClassId,
                                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now),
                                InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                IsActive = true,
                                IsLastExam = obj.ExamType.IsLastExam

                            };

                            var model = new InsertExamTypes()
                            {
                                ExamTypes = examType
                            };
                            message = _examTypesServ.InsertExamTypes(model);
                            result = Json(new { success = true, Message = message });

                        }
                    }
                }               
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:ExamTypes/InsertExamTypes - " + ex.Message });
            }
            return result;            
        }
        [HttpPost]
        public IActionResult Edit(IndexExamTypeVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var xmTyp = new ExamTypes()
                        {
                            Id = obj.ExamType.Id,
                            ClassId = obj.ExamType.ClassId,
                            Name = obj.ExamType.Name,
                            Sorting = obj.ExamType.Sorting,
                            BreakDownInP = obj.ExamType.BreakDownInP,
                            IsLastExam = obj.ExamType.IsLastExam,
                            IsActive = obj.ExamType.IsActive,
                            ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                            ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                        };
                        var model = new UpdateExamTypes()
                        {
                            ExamTypes = xmTyp
                        };
                        _examTypesServ.UpdateExamTypes(model);
                        return RedirectToAction("ExamTypes");
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:ExamTypes/Edit -unauthorized access is not permitted";
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:ExamTypes/Edit - " + ex.Message;
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }

        [HttpPost]
        public JsonResult DeleteExamTypes(int examTypesId)
        {            
            var result = (dynamic)null;            
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (examTypesId > 0)
                        {
                           
                            var model = new DeleteExamTypes()
                            {
                                Id = examTypesId
                            };
                            var returnResult = _examTypesServ.DeleteExamTypes(model);
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
                    result = Json(new { success = false, Message = "Exam Type is not possible to delete because it is used another place" });
                }
                else
                {
                    result = Json(new { success = false, Message = "ERROR101:ExamTypes/DeleteExamTypes - " + ex.Message });
                }
            }
            return result;
        }
        #endregion "Post_Methods"
    }
}
