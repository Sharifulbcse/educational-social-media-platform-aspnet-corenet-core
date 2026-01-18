
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
    public class GradeTypesController : Controller
    {
        #region "Variables"
        private readonly ICommonServ _commonServ;
        private readonly IClassesServ _classesServ;

        private readonly IGradeTypesServ _gradeTypesServ;

        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        
        #endregion "Variables"

        #region "Constructor"
        public GradeTypesController
            (
            IGradeTypesServ gradeTypesServ,
            IOE_InstitutionsServ oeInstitutionsServ,
             ICommonServ commonServ,
            IClassesServ classesServ
            )
        {
            _gradeTypesServ = gradeTypesServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _commonServ = commonServ;
            _classesServ = classesServ;
        }
        #endregion "Constructor"

        #region "Get_Methods"
        public IActionResult GradeTypes(long classId = 0)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var listGradeTypes = _gradeTypesServ.GetGradeTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), classId).ToList();
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        ViewBag.msg = TempData["error"];
                        var gradeTypesList = new List<GradeTypesListVM>();
                        
                        foreach (var item in listGradeTypes)
                        {
                            var temp = new GradeTypesListVM()
                            {
                                Id = item.GradeTypes.Id,
                                StartMark = item.GradeTypes.StartMark,
                                EndMark = item.GradeTypes.EndMark,
                                Grade = item.GradeTypes.Grade,
                                GPA = item.GradeTypes.GPA,
                                GPAOutOf = item.GradeTypes.GPAOutOf,
                                InstitutionId = item.GradeTypes.InstitutionId,
                                ClassId = item.GradeTypes.ClassId,
                                ClassName = item.Classes.Name
                            };
                            gradeTypesList.Add(temp);
                        }
                        

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexGradeTypesVM()
                        {
                            SelectedClassId = classId,
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            _gradeTypesVM = gradeTypesList

                        };
                        return View("GradeTypes", model);
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception )
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        #endregion "Get_Methods"

        #region "Post_Methods"        
        [HttpPost]
        public JsonResult InsertGradeTypes(IndexGradeTypesVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (obj.gradeTypesVM != null)
                        {
                            var gradeTypes = new GradeTypes()
                            {
                                StartMark = obj.gradeTypesVM.StartMark,
                                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                EndMark = obj.gradeTypesVM.EndMark,
                                Grade = obj.gradeTypesVM.Grade,
                                GPA = obj.gradeTypesVM.GPA,
                                GPAOutOf = obj.gradeTypesVM.GPAOutOf,
                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),

                                IsActive = true,
                                ClassId = obj.gradeTypesVM.ClassId,
                                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now),
                                InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))
                             };

                            var model = new GetGradeTypes()
                            {
                                GradeTypes = gradeTypes
                            };
                            message = _gradeTypesServ.InsertGradeTypes(model);
                            result = Json(new { success = true, Message = message });
                        }
                    }
                }                
            }
            catch (Exception ex)
             {
                result = Json(new { success = false, Message = "ERROR101:GradeTypes/InsertGradeTypes - " + ex.Message });
            }
            return result;
        }
        [HttpPost]
        public IActionResult Edit(Int64 editId, Int64 editStartMark, Int64 editEndMark, string editGrade, float editPoint, float editOutOf, long ddlEditClass)
        
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var fetchRecord = _gradeTypesServ.GetGradeTypesById(editId);
                        fetchRecord.StartMark = editStartMark;
                        fetchRecord.EndMark = editEndMark;
                        fetchRecord.Grade = editGrade;
                        fetchRecord.GPA = editPoint;
                        fetchRecord.GPAOutOf = editOutOf;
                        fetchRecord.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        
                        fetchRecord.ClassId = ddlEditClass;
                        fetchRecord.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _gradeTypesServ.UpdateGradeTypes(fetchRecord);

                        return RedirectToAction("GradeTypes");
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception )
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
        [HttpPost]
        public JsonResult DeleteGradeTypes(int gradeTypesId)
        {           
            var result = (dynamic)null;            
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (gradeTypesId > 0)
                        {
                           
                            var model = new DeleteGradeTypes()
                            {
                                Id = gradeTypesId
                            };
                            var returnResult = _gradeTypesServ.DeleteGradeTypes(model);
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
                    result = Json(new { success = false, Message = "Grade Type is not possible to delete because it is used another place" });
                }
                else
                {
                    result = Json(new { success = false, Message = "ERROR101:GradeTypes/DeleteGradeTypes - " + ex.Message });
                }
            }
            return result;
        }
        #endregion "Post_Methods"
    }
}
