using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

using OE.Data;
using OE.Service;
using OE.Service.ServiceModels;
using OE.Web.Areas.Institution.Models;


namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class OELicensesController : Controller
    {
        #region "Variables"
        private readonly IOE_LicensesServ _oeLicensesServ;
        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        private readonly IOE_UsersServ _oeUsersServ;
        private readonly ICommonServ _commonServ;
        private readonly IHostingEnvironment he;

        #endregion "Variables"

        #region "Constructor"
        public OELicensesController(
            IOE_LicensesServ oeLicensesServ, 
            IOE_InstitutionsServ oeInstitutionsServ, 
            IOE_UsersServ oeUsersServ,            
             ICommonServ commonServ,
            IHostingEnvironment e
            )
        {
            _oeLicensesServ = oeLicensesServ;
            _oeInstitutionsServ = oeInstitutionsServ;
            _oeUsersServ = oeUsersServ;            
            _commonServ = commonServ;
            he = e;
        }
        #endregion "Constructor"

        #region "Get Methods"
        [HttpGet]
        public IActionResult Licenses()
        {
            var ddlInstitutions = _oeInstitutionsServ.Dropdown_Institutions().OrderBy(c => c.Name).Select(x => new { Id = x.Id, Value = x.Name });
            
            var ddlUsers = _oeUsersServ.Dropdown_Users().OrderBy(c => c.Name).Select(x => new { Id = x.Id, Value = x.Name });
            var listlicenses = _oeLicensesServ.GetLicensesList().ToList();
            var licensesv = new List<OELicensesListVM>();
            foreach (var item in listlicenses)
            {
                var l = new OELicensesListVM()
                {
                    Id = item.Licenses.Id,
                    InstitutionId = item.Licenses.InstitutionId,
                    InstitutionName = item.OEInstitutions.Name,
                    LicenseNumber = item.Licenses.LicenseNumber,
                    DP = item.Licenses.DP,
                    IsActive = item.Licenses.IsActive,
                    StartDate = _commonServ.CommDate_ConvertToLocalDate(Convert.ToDateTime(item.Licenses.StartDate)),
                    EndDate = _commonServ.CommDate_ConvertToLocalDate(Convert.ToDateTime(item.Licenses.EndDate))

                };
                licensesv.Add(l);
            }
            var model = new IndexOELicensesVM()
            {
                _Licenses = licensesv,
                _Institutions = new SelectList(ddlInstitutions, "Id", "Value"),
                _Users = new SelectList(ddlUsers, "Id", "Value")

            };
            return View("Licenses", model);
        }
                
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public IActionResult AddLicense(Int64 ddlAddInstitutionId, DateTime? addEndDate, DateTime? addStartDate, bool addIsActive, Int64 ddlAddUserId, IFormFile fleAddDP)
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                    {
                        if (cActorId.Substring(0, cActorId.IndexOf("_")) == "1" || cActorId.Substring(0, cActorId.IndexOf("_")) == "2")
                        
                        {
                            string webRootPath = he.WebRootPath;

                            var tempLicense = new OE_Licenses()
                            {
                                InstitutionId = ddlAddInstitutionId,
                                StartDate = _commonServ.CommDate_ConvertToUtcDate(Convert.ToDateTime(addStartDate)),
                                EndDate = _commonServ.CommDate_ConvertToUtcDate(Convert.ToDateTime(addEndDate)),
                                IsActive = addIsActive,
                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                            };
                            
                             var model = new InsertLicenses()
                            {
                                Licenses = tempLicense,
                                EnableAuthentic = addIsActive,
                                UserId= ddlAddUserId
                            };
                            _oeLicensesServ.InsertLicenses(model, fleAddDP, webRootPath);
                            return RedirectToAction("Licenses", "OELicenses", new { area = "Institution" });
                        }
                    }
                    return RedirectToAction("Login", "Home", new { area = "" });
                }
                else
                {
                    return RedirectToAction("Login", "Home", new { area = "" });
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Login", "Home", new { area = "" });
            }
        }

        
        [HttpPost]
        public IActionResult EditLicense(IndexOELicensesVM obj)
        {
            try
            {
                var EditLicense = new OE_Licenses()
                {
                    Id = obj.Licenses.Id,

                    StartDate = obj.Licenses.StartDate,
                    EndDate = obj.Licenses.EndDate,
                    IsActive = obj.Licenses.IsActive,

                    ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                    ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                };


                _oeLicensesServ.UpdateLicenses(EditLicense);

            }
            catch (Exception)
            {
                ViewBag.err = "Error occured while saving data.";
            }
            return RedirectToAction("Licenses");
        }
        #endregion "Post_Methods"     

    }
}
