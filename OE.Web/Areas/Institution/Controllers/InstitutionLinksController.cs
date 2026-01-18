using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using OE.Data;
using OE.Service;
using OE.Service.ServiceModels;
using OE.Web.Areas.Institution.Models;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class InstitutionLinksController : Controller
    {

        #region "Variables"
        private readonly IInstitutionLinksServ _institutionLinksServ;
        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        private readonly IHostingEnvironment he;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public InstitutionLinksController(
            IInstitutionLinksServ institutionLinksServ,
            IOE_InstitutionsServ oeInstitutionsServ, 
            IHostingEnvironment he,
            ICommonServ commonServ
            )
        {
            _institutionLinksServ = institutionLinksServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            this.he = he;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get_Methods"
        [HttpGet]
        public IActionResult InstitutionLinks()
        {            
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        var getInsLinks = _institutionLinksServ.GetInstitutionLinks(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).ToList();
                        var currentInstitutionDetails = _oeInstitutionsServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

                        var insLinks = new List<InstitutionLinksListVM>();
                        foreach (var item in getInsLinks)
                        {
                            var e = new InstitutionLinksListVM()
                            {
                                Id = item.Id,
                                Name = item.Name,
                                IP24X24 = item.IP24X24,
                                Url = item.Url,
                                InstitutionId = item.InstitutionId
                            };
                            insLinks.Add(e);
                        }
                        //[NOTE: integrate all records for the specific page. ]
                        var model = new IndexInstitutionLinksVM()
                        {
                            InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                            _InstitutionLinksList = insLinks
                        };

                        return View("InstitutionLinks", model);
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:InstitutionLinks -unauthorized access is not permitted";
                return View("InstitutionLinks");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:InstitutionLinks - " + ex.Message;
                return View("InstitutionLinks");
            }            
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public JsonResult InsertInstitutionLinks(IndexInstitutionLinksVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            ViewBag.OeErrorMessage = null;

            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        if (obj.InstitutionLinks != null)
                        {
                            var insLinks = new InstitutionLinks()
                            {
                                Name = obj.InstitutionLinks.Name,
                                Url = obj.InstitutionLinks.Url,
                                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                                IP24X24 = obj.InstitutionLinks.IP24X24,

                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                ModifiedBy = 0,
                                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                            };

                            var model = new InsertInstitutionLinks()
                            {
                                institutionLinks = insLinks,
                                socialimage = obj.socialimage
                            };
                           
                            message = _institutionLinksServ.InsertInstitutionLinks(model, he.WebRootPath);
                            result = Json(new { success = true, Message = message });
                            
                        }
                    }
                }   
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:InstitutionLinks/InsertInstitutionLinks - " + ex.Message });

            }
            return result;
            
        }
        [HttpPost]        
        public IActionResult EditInstitutionLinks(IndexInstitutionLinksVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        string webRootPath = he.WebRootPath;
                        var insLinks = _institutionLinksServ.GetInstitutionLinksById(obj.InstitutionLinks.Id);
                        insLinks.Id = obj.InstitutionLinks.Id;
                        insLinks.Name = obj.InstitutionLinks.Name;
                        insLinks.Url = obj.InstitutionLinks.Url;
                        insLinks.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                        insLinks.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                        _institutionLinksServ.UpdateInstitutionLinks(insLinks, obj.socialimage, webRootPath);
                        return RedirectToAction("InstitutionLinks");
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:InstitutionLinks-unauthorized access is not permitted";
                return View("InstitutionLinks");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:InstitutionLinks - " + ex.Message;
                return View("InstitutionLinks");
            }
        }       
        [HttpPost]
        public JsonResult DeleteInstitutionLinks(int insLinksId)
        {           
            var result = (dynamic)null;           
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        if (insLinksId > 0)
                        {
                            var insLinks = _institutionLinksServ.GetInstitutionLinksById(insLinksId);
                            string webRootPath = Path.Combine(he.WebRootPath, insLinks.IP24X24);                           
                            var model = new DeleteInstitutionLinks()
                            {
                                Id = insLinksId
                            };
                            var returnResult = _institutionLinksServ.DeleteInstitutionLinks(model, webRootPath);
                            result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                            return result;                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:InstitutionLinks/DeleteInstitutionLinks - " + ex.Message });
            }
            return result;
        }
        #endregion "Post_SocialMedia"
    }
}
