
using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;
using OE.Service.ServiceModels;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class DistributionMarksController : Controller
    {
        #region "Variables"
        private readonly ICommonServ _commonServ;
        private readonly IDistributionMarksServ _distributionMarksServ;
        private readonly IDistributionMarkActionDatesServ _distributionMarkActionDatesServ;
        private readonly IMarkTypesServ _markTypesServ;
        private readonly ISubjectsServ _subjectsServ;
        private readonly IOE_InstitutionsServ _oeInstitutionServ;
        private readonly IClassesServ _classesServ;

        public DateTime Datetime { get; private set; }
        #endregion "Variables"

        #region "Constructor"
        public DistributionMarksController(
            IDistributionMarksServ distributionMarksServ,
            IDistributionMarkActionDatesServ distributionMarkActionDatesServ,
            IMarkTypesServ markTypesServ, 
            ISubjectsServ subjectsServ,
            IOE_InstitutionsServ oeInstitutionServ,
            ICommonServ commonServ,
            IClassesServ classesServ
            )
        {
            _distributionMarksServ = distributionMarksServ;
            _distributionMarkActionDatesServ = distributionMarkActionDatesServ;
            _markTypesServ = markTypesServ;
            _subjectsServ = subjectsServ;
            _commonServ = commonServ;
            _oeInstitutionServ = oeInstitutionServ;
            _classesServ = classesServ;
        }
        #endregion "Constructor"

        #region "Get_Methods"
        public IActionResult DistributionMarks(long classId = 0)
        {
            ViewBag.ddlClasses = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            ViewBag.ddlMarkType = _markTypesServ.Dropdown_MarkTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            var listDistributionMark = _distributionMarksServ.getDisMarkSubMarks(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), classId).ToList();
            var currentInstitutionDetails = _oeInstitutionServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

            var distributionMarkList = new List<DistributionMarksListVM>();
            ViewBag.msg = TempData["error"];

            foreach (var item in listDistributionMark)
            {
                var temp = new DistributionMarksListVM()
                {
                    Id = item.DistributionMarks.Id,
                    SubjectId = item.DistributionMarks.SubjectId,
                    SubjectName = item.Subjects.Name,
                    MarkTypeId = item.DistributionMarks.MarkTypeId,
                    MarkTypeName = item.MarkTypes.Name,
                    BreakDownInP = item.DistributionMarks.BreakDownInP,                    
                    InstitutionId = item.DistributionMarks.InstitutionId,
                    ClassId = item.DistributionMarks.ClassId,
                    ClassName = item.Classes.Name

                };
                distributionMarkList.Add(temp);
            }

            var model = new IndexDistributionMarksVM()
            {
                InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                distributionMarks = distributionMarkList,
                SelectedClassId = classId
            };
            return View("DistributionMarks", model);
        }
        public IActionResult DistributionMarkActionDates()
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString
                ("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {                        
                        var distributionMarkActionDates = _distributionMarkActionDatesServ.GetDistributionMarkActionDates(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        ViewBag.msg = TempData["error"];
                        var list = new List<DistributionMarkActionDatesListVM>();
                        foreach (var item in distributionMarkActionDates.DistributionMarkActionDates)
                        {
                            var temp = new DistributionMarkActionDatesListVM()
                            {
                                Id = item.Id,
                                EffectiveStartDate = item.EffectiveStartDate,
                                EffectiveEndDate = item.EffectiveEndDate,
                                IsActive = item.IsActive
                            };
                            list.Add(temp);
                        }

                        var model = new IndexDistributionMarkActionDatesVM()
                        {
                            distributionMarkActionDates = list,
                            InstitutionName = distributionMarkActionDates.InstitutionName                           
                        };
                        return View("DistributionMarkActionDates", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:DistributionMarks/DistributionMarkActionDates -unauthorized access is not permitted";
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:DistributionMarks/DistributionMarkActionDates - " + ex.Message;
                return View("DistributionMarkActionDates");
            }

        }
        public IActionResult DistributionMarkList(long distributionMarkActionDateId, long classId = 0)
        {

            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString
                ("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var distributionMarks = _distributionMarksServ.GetDistributionMarkList(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), distributionMarkActionDateId, classId);
                        ViewBag.classId = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        var list = (dynamic)null;
                        var subjectList = (dynamic)null;
                        if (classId != 0)
                        {
                            list = new List<DistributionMarksListVM>();
                            foreach (var item in distributionMarks.DistributionMark)
                            {
                                var temp = new DistributionMarksListVM()
                                {
                                    Id = item.Id,
                                    SubjectId = item.SubjectId,
                                    InstitutionId = item.InstitutionId,
                                    DistributionMarkActionDateId = item.DistributionMarkActionDateId,
                                    MarkTypeId = item.MarkTypeId,
                                    ClassId = item.ClassId,
                                    BreakDownInP = item.BreakDownInP,
                                    SubjectName = item.SubjectName,
                                    MarkTypeName = item.MarkTypeName
                                };
                                list.Add(temp);
                            }

                            subjectList = new List<SubjectsListVM>();
                            foreach (var item in distributionMarks.Subject)
                            {
                                var temp = new SubjectsListVM()
                                {
                                    Id = item.Id,
                                    Name = item.Name
                                };
                                subjectList.Add(temp);
                            }

                        }
                        
                        var markTypeList = new List<MarkTypesListVM>();
                        foreach (var item in distributionMarks.MarkType)
                        {
                            var temp = new MarkTypesListVM()
                            {
                                Id = item.Id,
                                Name = item.Name
                            };
                            markTypeList.Add(temp);
                        }
                        
                        var model = new IndexDistributionMarksVM()
                        {
                            distributionMarks = list,
                            subjects = subjectList,                            
                            MarkType = markTypeList,
                            SelectedClassId = classId,                            
                            InstitutionName = distributionMarks.InstitutionName,
                            ClassName = distributionMarks.ClassName,
                            DistributionMarkActionDateId = distributionMarkActionDateId
                        };
                        return View("DistributionMarkList", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:DistributionMarks/DistributionMarkList -unauthorized access is not permitted";
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:DistributionMarks/DistributionMarkList - " + ex.Message;
                return View("DistributionMarkList");
            }

        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public JsonResult InsertDistributionMark(IndexDistributionMarksVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;

            try
            {
                if (obj.DistributionMark != null)
                {
                    var distributionMark = new DistributionMarks()
                    {
                        SubjectId = obj.DistributionMark.SubjectId,
                        MarkTypeId = obj.DistributionMark.MarkTypeId,
                        BreakDownInP = obj.DistributionMark.BreakDownInP,
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        ClassId = obj.DistributionMark.ClassId,
                        IsActive = true,
                        InsId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                    };

                    var model = new InsertDistributionMark()
                    {
                        DistributionMarks = distributionMark
                    };
                    message = _distributionMarksServ.InsertDistributionMarks(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:DistributionMarks/InsertDistributionMark - " + ex.Message });

            }
            return result;
        }
        [HttpPost]
        public JsonResult InsertDistributionMarkActionDate(IndexDistributionMarkActionDatesVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;

            try
            {
                if (obj.DistributionMarkActionDate != null)
                {
                    var distributionMarkActionDate = new DistributionMarkActionDates()
                    {
                        EffectiveStartDate = obj.DistributionMarkActionDate.EffectiveStartDate,
                        IsActive = true,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),                        
                        AddedDate = DateTime.Now                       
                    };

                    var model = new InsertDistributionMarkActionDates()
                    {
                        DistributionMarkActionDates = distributionMarkActionDate
                    };
                    message = _distributionMarkActionDatesServ.InsertDistributionMarkActionDates(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:DistributionMarks/InsertDistributionMarkActionDate - " + ex.Message });

            }
            return result;
        }
        [HttpPost]
        public JsonResult InsertUpdateDistributionMarks(IndexDistributionMarksVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;

            try
            {
                if (obj.distributionMarks != null)
                {
                    var distributionMarkList = new List<DistributionMarks>();
                    foreach (var item in obj.distributionMarks)
                    {
                        var temp = new DistributionMarks()
                        {
                            SubjectId = obj.SelectedSubjectId,
                            MarkTypeId = item.MarkTypeId,
                            ClassId = item.ClassId,
                            InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                            DistributionMarkActionDateId = item.DistributionMarkActionDateId,
                            BreakDownInP = item.BreakDownInP,
                            IsActive = true,
                            AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                            AddedDate = DateTime.Now,
                            ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                            ModifiedDate = DateTime.Now
                        };
                        distributionMarkList.Add(temp);
                    }

                    var model = new InsertUpdateDistributionMark()
                    {
                        _DistributionMarks = distributionMarkList
                    };
                    message = _distributionMarksServ.InsertUpdateDistributionMarks(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:DistributionMarks/InsertUpdateDistributionMarks - " + ex.Message });

            }
            return result;
        }
        [HttpPost]
        public IActionResult Edit(Int64 editId, Int64 ddlEditClassId, Int64 ddlEditSubjectId, Int64 ddlEditMarkTypeId, long txtEditDisMrkMax, DateTime txtEditDisMrkESD, DateTime txtEditDisMrkEED)
        {
            var dm = _distributionMarksServ.GetDistributionMarkById(editId);
            dm.SubjectId = ddlEditSubjectId;
            dm.MarkTypeId = ddlEditMarkTypeId;
            dm.BreakDownInP = txtEditDisMrkMax;
            dm.ClassId = ddlEditClassId;
            dm.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            dm.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);
            _distributionMarksServ.UpdateDistributionMarks(dm);


            return RedirectToAction("DistributionMarks");
        }
        [HttpPost]
        public IActionResult EditDistributionMarkActionDate(IndexDistributionMarkActionDatesVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var disMAD = new DistributionMarkActionDates()
                        {
                            Id = obj.DistributionMarkActionDate.Id,
                            EffectiveStartDate = obj.DistributionMarkActionDate.EffectiveStartDate,
                            EffectiveEndDate = obj.DistributionMarkActionDate.EffectiveEndDate,
                            ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                            ModifiedDate = DateTime.Now                            
                        };
                        var model = new UpdateDistributionMarkActionDates()
                        {
                            DistributionMarkActionDates = disMAD
                        };
                        _distributionMarkActionDatesServ.UpdateDistributionMarkActionDates(model);
                        return RedirectToAction("DistributionMarkActionDates");
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:DistributionMarks/DistributionMarkActionDates -unauthorized access is not permitted";
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:DistributionMarks/DistributionMarkActionDates - " + ex.Message;
                return RedirectToAction("DistributionMarkActionDates");
            }
        }

        [HttpPost]
        public IActionResult Delete(Int64 deleteId)
        {
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var dm = _distributionMarksServ.GetDistributionMarkById(deleteId);
                        _distributionMarksServ.DeleteDistributionMarks(dm);
                        return RedirectToAction("DistributionMarks");
                    }
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    TempData["error"] = "Distribution Mark is not possible to delete because it is used another place";
                    return RedirectToAction("DistributionMarks");
                }
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }
       
        [HttpPost]
        public IActionResult DeleteDistributionMarkActionDate(IndexDistributionMarkActionDatesVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var disMAD = new DistributionMarkActionDates()
                        {
                            Id = obj.DistributionMarkActionDate.Id
                        };
                        var model = new DeleteDistributionMarkActionDates()
                        {
                            DistributionMarkActionDates = disMAD
                        };
                        _distributionMarkActionDatesServ.DeleteDistributionMarkActionDates(model);

                        return RedirectToAction("DistributionMarkActionDates");
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:DistributionMarks/DistributionMarkActionDates -unauthorized access is not permitted";
                return RedirectToAction("Login", "Home", new { area = "" });
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    TempData["error"] = "Distribution Mark Action Date is not possible to delete because it is used another place";
                    return RedirectToAction("DistributionMarkActionDates");
                }
                else
                    ViewBag.OeErrorMessage = "ERROR101:DistributionMarks/DistributionMarkActionDates - " + ex.Message;
                return RedirectToAction("DistributionMarkActionDates");
            }
        }
        
        #endregion "Post_Methods"

        #region "Post Methods- dropdown"

        public JsonResult DropDown_Classes(long ddlClassId)
        {
            var result = (dynamic)null;
            var ddlSubject = _subjectsServ.dropdown_Subjects(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), 0, ddlClassId);
            result = Json(new SelectList(ddlSubject, "Id", "Name"));
            return result;
        }
        #endregion "Post Methods- dropdown"
        
    }
}
