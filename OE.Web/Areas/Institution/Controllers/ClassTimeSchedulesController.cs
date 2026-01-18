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
    public class ClassTimeSchedulesController : Controller
    {
        #region "Variables"
        private readonly ICommonServ _commonServ;
        private readonly IClassTimeSchedulesServ _classTimeSchedulesServ;
        private readonly IClassTimeScheduleActionDatesServ _classTimeScheduleActionDatesServ;
        private readonly IOE_InstitutionsServ _oeInstitutionServ;

        public DateTime Datetime { get; private set; }
        #endregion "Variables"

        #region "Constructor"
        public ClassTimeSchedulesController(
            IClassTimeSchedulesServ classTimeSchedulesServ,
            IClassTimeScheduleActionDatesServ classTimeScheduleActionDatesServ,
            IOE_InstitutionsServ oeInstitutionServ,
            ICommonServ commonServ
            )
        {
            _classTimeSchedulesServ = classTimeSchedulesServ;
            _classTimeScheduleActionDatesServ = classTimeScheduleActionDatesServ;
            _commonServ = commonServ;
            _oeInstitutionServ = oeInstitutionServ;
        }
        #endregion "Constructor"

        #region "Get_Methods"

        public IActionResult ClassTimeScheduleActionDates()
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString
                ("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var classTimeScheduleActionDates = _classTimeScheduleActionDatesServ.GetClassTimeScheduleActionDates(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        var list = new List<ClassTimeScheduleActionDatesListVM>();
                        foreach (var item in classTimeScheduleActionDates._C_ClassTimeScheduleActionDates)
                        {
                            var temp = new ClassTimeScheduleActionDatesListVM()
                            {
                                Id = item.Id,
                                Sorting = item.Sorting,
                                EffectiveStartDate = item.EffectiveStartDate,
                                EffectiveEndDate = item.EffectiveEndDate,
                                IsActive = item.IsActive
                            };
                            list.Add(temp);
                        }

                        var model = new IndexClassTimeScheduleActionDatesVM()
                        {
                            _ClassTimeScheduleActionDates = list,
                            InstitutionName = classTimeScheduleActionDates.InstitutionName
                        };
                        return View("ClassTimeScheduleActionDates", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:ClassTimeSchedules/ClassTimeScheduleActionDates -unauthorized access is not permitted";
                return View("ClassTimeScheduleActionDates");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:ClassTimeSchedules/ClassTimeScheduleActionDates - " + ex.Message;
                return View("ClassTimeScheduleActionDates");
            }

        }
        public IActionResult ClassTimeScheduleList(long classTimeScheduleActionDateId)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString
                ("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        var classTimeSchedules = _classTimeSchedulesServ.GetClassTimeScheduleList(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), classTimeScheduleActionDateId);

                        var list = new List<ClassTimeSchedulesListVM>();
                        foreach (var item in classTimeSchedules._C_ClassTimeSchedule)
                        {
                            var temp = new ClassTimeSchedulesListVM()
                            {
                                Id = item.Id,                                
                                Sorting = item.Sorting,                               
                                ClassTimeScheduleActionDateId = item.ClassTimeScheduleActionDateId,                               
                                ClassStartTimeSlot = item.ClassStartTimeSlot,
                                ClassEndTimeSlot = item.ClassEndTimeSlot                                
                            };
                            list.Add(temp);
                        }

                        var model = new IndexClassTimeSchedulesVM()
                        {
                            _ClassTimeSchedules = list,
                            InstitutionName = classTimeSchedules.InstitutionName,
                            ClassTimeScheduleActionDateId = classTimeScheduleActionDateId
                        };
                        return View("ClassTimeScheduleList", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:ClassTimeSchedules/ClassTimeScheduleList -unauthorized access is not permitted";
                return View("ClassTimeScheduleList");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:ClassTimeSchedules/ClassTimeScheduleList - " + ex.Message;
                return View("ClassTimeScheduleList");
            }
        }
        #endregion "Get_Methods"

        #region "Post_Methods"

        [HttpPost]
        public JsonResult InsertClassTimeScheduleActionDate(IndexClassTimeScheduleActionDatesVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;

            try
            {
                if (obj.ClassTimeScheduleActionDate != null)
                {
                    var classTimeScheduleActionDate = new ClassTimeScheduleActionDates()
                    {
                        EffectiveStartDate = obj.ClassTimeScheduleActionDate.EffectiveStartDate,
                        Sorting = obj.ClassTimeScheduleActionDate.Sorting,
                        IsActive = true,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = DateTime.Now
                    };

                    var model = new InsertClassTimeScheduleActionDates()
                    {
                        ClassTimeScheduleActionDates = classTimeScheduleActionDate
                    };
                    message = _classTimeScheduleActionDatesServ.InsertClassTimeScheduleActionDates(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:ClassTimeSchedules/InsertClassTimeScheduleActionDate - " + ex.Message });

            }
            return result;
        }
        [HttpPost]
        public JsonResult InsertClassTimeSchedule(IndexClassTimeSchedulesVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;

            try
            {
                if (obj.ClassTimeSchedules != null)
                {
                    var classTimeScheduleList = new ClassTimeSchedules()
                    {
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        ClassTimeScheduleActionDateId = obj.ClassTimeSchedules.ClassTimeScheduleActionDateId,
                        Sorting = obj.ClassTimeSchedules.Sorting,
                        ClassStartTime = obj.ClassTimeSchedules.ClassStartTime,
                        ClassEndTime = obj.ClassTimeSchedules.ClassEndTime,
                        IsActive = true,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = DateTime.Now
                    };

                    var model = new InsertClassTimeSchedules()
                    {
                        ClassTimeSchedules = classTimeScheduleList
                    };
                    message = _classTimeSchedulesServ.InsertClassTimeSchedules(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:ClassTimeSchedules/InsertClassTimeSchedules - " + ex.Message });

            }
            return result;
        }

        [HttpPost]
        public JsonResult EditClassTimeScheduleActionDate(IndexClassTimeScheduleActionDatesVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            try
            {
                if (obj.ClassTimeScheduleActionDate != null)
                {
                    var ctsAD = new ClassTimeScheduleActionDates()
                    {
                        Id = obj.ClassTimeScheduleActionDate.Id,
                        Sorting = obj.ClassTimeScheduleActionDate.Sorting,
                        EffectiveStartDate = obj.ClassTimeScheduleActionDate.EffectiveStartDate,
                        EffectiveEndDate = obj.ClassTimeScheduleActionDate.EffectiveEndDate,
                        ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        ModifiedDate = DateTime.Now
                    };
                    var model = new UpdateClassTimeScheduleActionDates()
                    {
                        ClassTimeScheduleActionDates = ctsAD
                    };
                    message = _classTimeScheduleActionDatesServ.UpdateClassTimeScheduleActionDates(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:ClassTimeSchedules/UpdateClassTimeScheduleActionDate - " + ex.Message });

            }
            return result;
        }

        [HttpPost]
        public JsonResult EditClassTimeSchedule(IndexClassTimeSchedulesVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;

            try
            {
                if (obj.ClassTimeSchedules != null)
                {
                    var cts = new ClassTimeSchedules()
                    {
                        Id = obj.ClassTimeSchedules.Id,
                        Sorting = obj.ClassTimeSchedules.Sorting,
                        ClassStartTime = obj.ClassTimeSchedules.ClassStartTime,
                        ClassEndTime = obj.ClassTimeSchedules.ClassEndTime,
                        ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        ModifiedDate = DateTime.Now
                    };
                    var model = new UpdateClassTimeSchedules()
                    {
                        ClassTimeSchedules = cts
                    };
                    message = _classTimeSchedulesServ.UpdateClassTimeSchedules(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:ClassTimeSchedules/UpdateClassTimeSchedules - " + ex.Message });

            }
            return result;
        }

        [HttpPost]
        public JsonResult DeleteClassTimeScheduleActionDate(long deleteId)
        {
            var result = (dynamic)null;           
            try
            {
                if (deleteId > 0)
                {                    
                    var model = new DeleteClassTimeScheduleActionDate()
                    {
                        Id = deleteId
                    };
                    var returnResult = _classTimeScheduleActionDatesServ.DeleteClassTimeScheduleActionDates(model);
                    result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                    return result;                    
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    ViewBag.msg = "Class Schedule Time Action Date is not possible to delete because it is used another place";
                }
                result = Json(new { success = false, Message = "ERROR101:ClassTimeSchedules/DeleteClassTimeScheduleActionDates - " + ex.Message });

            }
            return result;
        }

        [HttpPost]
        public JsonResult DeleteClassTimeSchedule(long deleteId)
        {
            var result = (dynamic)null;            
            try
            {
                if (deleteId > 0)
                {                    
                    var model = new DeleteClassTimeSchedule()
                    {
                        Id = deleteId
                    };
                    var returnResult = _classTimeSchedulesServ.DeleteClassTimeSchedules(model);
                    result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                    return result;                   
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    ViewBag.msg = "Class Schedule Time is not possible to delete because it is used another place";
                }
                result = Json(new { success = false, Message = "ERROR101:ClassTimeSchedules/DeleteClassTimeSchedules - " + ex.Message });

            }
            return result;
        }
        #endregion "Post_Methods"

    }
}
