
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
    public class OE_StaffTypesController : Controller
    {
        #region "Variables"
        private readonly IOE_StaffTypesServ _oeStaffTypesServ;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public OE_StaffTypesController(
            IOE_StaffTypesServ oeStaffTypesServ,
            ICommonServ commonServ
        )
        {
            _oeStaffTypesServ = oeStaffTypesServ;
            _commonServ = commonServ;
        }
        #endregion "Constructor"


        #region "Get_Methods"
        [HttpGet]
        public IActionResult OE_StaffTypes()
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "1")
                    {
                        var getOE_StaffType = _oeStaffTypesServ.GetAllOE_StaffType().ToList();
                        var staffv = new List<OE_StaffTypesListVM>();
                        foreach (var item in getOE_StaffType)
                        {
                            var e = new OE_StaffTypesListVM();
                            {
                                e.Id = item.Id;
                                e.Name = item.Name;
                            };
                            staffv.Add(e);
                        }
                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexOE_StaffTypesVM()
                        {
                            _OE_StaffTypeList = staffv
                        };
                        return View("OE_StaffTypes", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:OE_StaffTypes -unauthorized access is not permitted";
                return View("OE_StaffTypes");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:OE_StaffTypes - " + ex.Message;
                return View("OE_StaffTypes");
            }

        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public IActionResult Add(IndexOE_StaffTypesVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "1")
                    {
                        var temp = new OE_StaffTypes()
                        {
                            Name = obj.StaffType.Name,
                            IsActive = true,
                            AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                            AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                        };
                        var model = new InsertStaffTypes()
                        {
                            StaffTypes = temp
                        };
                        _oeStaffTypesServ.InsertOE_StaffType(model);

                        return RedirectToAction("OE_StaffTypes");
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:OE_StaffTypes -unauthorized access is not permitted";
                return RedirectToAction("OE_StaffTypes");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:OE_StaffTypes - " + ex.Message;
                return RedirectToAction("OE_StaffTypes");
            }
        }

        [HttpPost]
        public IActionResult Edit(IndexOE_StaffTypesVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "1")
                    {
                        var staffType = new OE_StaffTypes()
                        {
                            Id = obj.StaffType.Id,
                            Name = obj.StaffType.Name,
                            ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                            ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                        };
                        var model = new UpdateStaffTypes()
                        {
                            StaffTypes = staffType
                        };
                        staffType.Name = obj.StaffType.Name;
                        _oeStaffTypesServ.UpdateOE_StaffType(model);

                        return RedirectToAction("OE_StaffTypes");
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:OE_StaffTypes -unauthorized access is not permitted";
                return RedirectToAction("OE_StaffTypes");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:OE_StaffTypes - " + ex.Message;
                return RedirectToAction("OE_StaffTypes");
            }
        }

        [HttpPost]
        public IActionResult Delete(IndexOE_StaffTypesVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "1")
                    {
                        var staffType = new OE_StaffTypes()
                        {
                            Id = obj.StaffType.Id
                        };
                        var model = new DeleteStaffTypes()
                        {
                            StaffTypes = staffType
                        };
                        _oeStaffTypesServ.DeleteOE_StaffType(model);

                        return RedirectToAction("OE_StaffTypes");
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:OE_StaffTypes -unauthorized access is not permitted";
                return RedirectToAction("OE_StaffTypes");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:OE_StaffTypes - " + ex.Message;
                return RedirectToAction("OE_StaffTypes");
            }
        }
        #endregion "Post_Methods"
    }
}
