
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
    public class SubjectsController : Controller
    {
        #region "Variables"
        private readonly IClassesServ _classesServ;
        private readonly ICommonServ _commonServ;
        private readonly ISubjectsServ _subjectsServ;        
        private readonly ISubjectTypesServ _subjectTypesServ;
        private readonly IOE_InstitutionsServ _oeInstitutionServ;
        
        #endregion "Variables"

        #region "Constructor"
        public SubjectsController(
            IClassesServ classesServ,
            ICommonServ commonServ,
            ISubjectsServ subjectsServ,
            ISubjectTypesServ subjectTypesServ,
            IOE_InstitutionsServ oeInstitutionServ

            )
        {
            _classesServ = classesServ;
            _commonServ = commonServ;
            _subjectsServ = subjectsServ;
            _subjectTypesServ = subjectTypesServ;
            _oeInstitutionServ = oeInstitutionServ;
           
        }
        #endregion "Constructor"

        #region "Get_Methods"
        

        public IActionResult Subjects(long ddlClassId = 0)
        {
            ViewBag.ddlClasses = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            ViewBag.ddlSubjectTypes = _subjectTypesServ.Dropdown_SubjectTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            var listSubjects = _subjectsServ.ClassWiseSubjectView(ddlClassId, Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).ToList();
            var currentInstitutionDetails = _oeInstitutionServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

            ViewBag.msg = TempData["error"];
            
            var subjectList = new List<SubjectsListVM>();
            foreach (var item in listSubjects)
            {
                var e = new SubjectsListVM()
                {
                    Id = item.Subjects.Id,
                    Name = item.Subjects.Name,
                    ClassId = item.Subjects.ClassId,
                    ClassName = item.Classes.Name,
                    SubjectTypeId = item.Subjects.SubjectTypeId,
                    SubjectTypeName = item.SubjectTypes.Name
                };
               subjectList.Add(e);
            }
            var model = new IndexSubjectsVM()
            {
                InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                _subjectsVM = subjectList,
                SelectedClassId = ddlClassId
            };
            return View("Subjects", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public JsonResult InsertSubjects(IndexSubjectsVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            try
            {               
                if (obj.subjectsVM != null)
                {
                    var subjects = new Subjects()
                    {
                        Name = obj.subjectsVM.Name,
                        ClassId = obj.subjectsVM.ClassId,
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        IsActive = true,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now),
                        SubjectTypeId = obj.subjectsVM.SubjectTypeId

                    };

                    var model = new InsertSubjects()
                    {
                        Subjects = subjects
                    };
                    message = _subjectsServ.InsertSubjects(model);
                    result = Json(new { success = true, Message = message });
                }                
            }            
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:Subjects/InsertSubjects - " + ex.Message });               
            }
            return result;
        }

        [HttpPost]
        public IActionResult Edit(Int64 editSubjectsId, string editSubName, Int64 ddlEditClassesId, Int64 ddlEditSubjectTypesId)
        {
            var subjects = _subjectsServ.GetSubjectsById(editSubjectsId);
            subjects.Name = editSubName;
            subjects.ClassId = ddlEditClassesId;
            subjects.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            subjects.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);
            subjects.SubjectTypeId = ddlEditSubjectTypesId;

            _subjectsServ.UpdateSubjects(subjects);

            return RedirectToAction("Subjects");
        }

        [HttpPost]
        public JsonResult DeleteSubjects(int subjectsId)
        {           
            var result = (dynamic)null;           
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (subjectsId > 0)
                        {                           
                            var model = new DeleteSubjects()
                            {
                                Id = subjectsId
                            };
                            var returnResult = _subjectsServ.DeleteSubjects(model);
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
                    result = Json(new { success = false, Message = "Subjects is not possible to delete because it is used another place" });
                }
                else
                {
                    result = Json(new { success = false, Message = "ERROR101:Subjects/DeleteSubjects - " + ex.Message });
                }
            }
            return result;
        }

        #endregion "Post_Methods"

    }
}
