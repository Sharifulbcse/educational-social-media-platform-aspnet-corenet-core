using Microsoft.AspNetCore.Hosting;
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
    public class InsPagesController : Controller
    {
        private readonly ICommonServ _commonServ;
        private readonly IHostingEnvironment he;

        private readonly IInsPagesServ _insPagesServ;
        private readonly IInsPageDetailsServ _insPageDetailsServ;

        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        
       
        public InsPagesController(
            IInsPagesServ insPagesServ,
            IInsPageDetailsServ insPageDetailsServ,
            IOE_InstitutionsServ oeInstitutionsServ,
            IHostingEnvironment e,
            ICommonServ commonServ
            )
        {
            _insPagesServ = insPagesServ;
            _insPageDetailsServ = insPageDetailsServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            he = e;
            _commonServ = commonServ;
        }

        #region "Get Functions"
        public IActionResult InsPages()
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
                        var listInsPages = _insPagesServ.GetInsPages().ToList();

                        var insPageList = new List<InsPagesListVM>();

                        foreach (var item in listInsPages)
                        {
                            var ip = new InsPagesListVM();
                            ip.Id = item.Id;
                            ip.Title = item.Title;
                            ip.IP300X200 = item.IP300X200;
                            ip.IP600X400 = item.IP600X400;
                            insPageList.Add(ip);
                        }
                        var model = new IndexInsPagesVM()
                        {
                            InsPages = insPageList,
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : ""
                        };

                        return View("InsPages/InsPages", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:InsPages/InsPages -unauthorized access is not permitted";
                return View("InsPages/InsPages");
            }

            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:InsPages/InsPages - " + ex.Message;
                return View("InsPages/InsPages");
            }
        }
        public IActionResult GetInsPageDetails(long insPageId)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        var listInsPageDetails = _insPageDetailsServ.GetInsPageDetailsByInsPageId(insPageId);
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        var insPageDetailsList = new List<InsPageDetailsListVM>();

                        foreach (var item in listInsPageDetails._InsPageDatailsList)
                        {
                            var ipd = new InsPageDetailsListVM();

                            ipd.Id = item.Id;
                            ipd.Sorting = item.Sorting;
                            ipd.InsPageId = item.InsPageId;
                            ipd.Description = item.Description;
                            ipd.Title = item.Title;
                            insPageDetailsList.Add(ipd);
                        }

                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexInsPageDetailsVM()
                        {
                            InsPageDetailsList = insPageDetailsList,
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            InsPageId = insPageId

                        };

                        return View("GetInsPageDetails/GetInsPageDetails", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:GetInsPageDetails/GetInsPageDetails -unauthorized access is not permitted";
                return View("GetInsPageDetails/GetInsPageDetails");
            }

            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:GetInsPageDetails/GetInsPageDetails - " + ex.Message;
                return View("GetInsPageDetails/GetInsPageDetails");
            }
        }
        #endregion"Get Functions"

        #region "Post Functions"
        [HttpPost]
        public JsonResult InsertPageDetails(IndexInsPageDetailsVM obj)        
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        if (obj.InsPageDetails != null)
                        {
                            var insPages = new InsPageDetails()
                            {
                                InsPageId = obj.InsPageDetails.InsPageId,
                                Sorting = obj.InsPageDetails.Sorting,
                                Description = obj.InsPageDetails.Description,
                                Title = obj.InsPageDetails.Title,

                                IsActive = true,
                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                            };

                            var model = new InsertInsPageDetails()
                            {
                                insPageDetails = insPages
                            };
                            message = _insPageDetailsServ.InsertInsPageDetails(model);
                            result = Json(new { success = true, Message = message });
                        }                        
                    }                   
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:InsPageDetails/InsertInsPageDetails - " + ex.Message });
            }
            return result;
        }

        [HttpPost]
        public IActionResult EditInsPages(IndexInsPagesVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        string webRootPath = he.WebRootPath;
                        var insPage = _insPagesServ.GetInsPagesById(obj.InsPage.Id);

                        _insPagesServ.UpdateInsPages(insPage, obj.image, webRootPath);
                        return RedirectToAction("InsPages");
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:InsPages-unauthorized access is not permitted";
                return RedirectToAction("InsPages");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:InsPages - " + ex.Message;
                return RedirectToAction("InsPages");
            }
        }

        [HttpPost]
        public IActionResult EditPageDetails(Int64 txtEditId, Int64 txtEditInsPageId, string txtEditPageDetailsTitle, string txtEditPageDetailsDescription, Int64 txtEditPageDetailsSorting)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        var pageDetails = _insPageDetailsServ.GetInsPageDetailsById(txtEditId);
                        pageDetails.Title = txtEditPageDetailsTitle;
                        pageDetails.Description = txtEditPageDetailsDescription;
                        pageDetails.Sorting = txtEditPageDetailsSorting;
                        pageDetails.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        pageDetails.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);


                        _insPageDetailsServ.UpdateInsPageDetails(pageDetails);

                        return RedirectToAction("GetInsPageDetails", new { insPageId = txtEditInsPageId });
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:GetInsPageDetails/GetInsPageDetails -unauthorized access is not permitted";
                return View("GetInsPageDetails/GetInsPageDetails");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:GetInsPageDetails/GetInsPageDetails - " + ex.Message;
                return View("GetInsPageDetails/GetInsPageDetails");
            }
        }

        [HttpPost]
        public JsonResult DeletePageDetails(int insPageDetailId)
        {
            var result = (dynamic)null;
            try
            {
                if (insPageDetailId > 0)
                {
                    var model = new DeleteInsPages()
                    {
                        Id = insPageDetailId
                    };
                    var returnResult = _insPageDetailsServ.DeleteInsPageDetails(model);
                    result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                    return result;                   
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:InsPages/DeletePageDetails - " + ex.Message });

            }
            return result;

        }

        #endregion "Post Functions"
    }
}

