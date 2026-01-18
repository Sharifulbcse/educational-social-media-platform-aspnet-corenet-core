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
    public class OE_InstitutionsController : Controller
    {
        #region "Variables"
        private readonly ICountriesServ _countriesServ;
        private readonly ICommonServ _commonServ;

        private readonly IHostingEnvironment he;

        private readonly IOE_InstitutionsServ _oeInstitutionsServ;
        private readonly IOE_LicensesServ _oeLicenseServ;
        
        #endregion "Variables"

        #region "Constructor"
        public OE_InstitutionsController(
            
            IOE_LicensesServ oeLicenseServ,
            IOE_InstitutionsServ oeInstitutionsServ, 
            ICountriesServ countriesServ,
             ICommonServ commonServ,
            IHostingEnvironment e
            )
        {
            _oeInstitutionsServ = oeInstitutionsServ;

            _oeLicenseServ = oeLicenseServ;
            _countriesServ = countriesServ;
            _commonServ = commonServ;
            he = e;
        }
        #endregion "Constructor"       

        #region "Get_Methods"      
        public IActionResult OE_Institutions(Int64 ddlLicensesId)
        {
            ViewBag.ddlCountries = _countriesServ.Dropdown_Countries();
            var listOE_Institutions = _oeLicenseServ.GetLicenses(ddlLicensesId).ToList();
            var licenses = _oeInstitutionsServ.GetLicenses().ToList();
            var oE_Institutionsv = new List<OE_InstitutionsListVM>();

            var list = _oeInstitutionsServ.GetOE_InstitutionsById(ddlLicensesId);

            foreach (var item in listOE_Institutions)
            {

                var e = new OE_InstitutionsListVM();
                e.Id = item.OE_Institutions.Id;
                e.Name = item.OE_Institutions.Name;
                e.CountryId = item.OE_Institutions.CountryId;
                e.CountryName = item.Countries.Name;
                
                e.LogoPath = item.OE_Institutions.LogoPath;
                e.FaviconPath = item.OE_Institutions.FaviconPath;
                e.Email = item.OE_Institutions.Email;
                e.ContactNo = item.OE_Institutions.ContactNo;
                e.Address = item.OE_Institutions.Address;
                e.IsActive = item.OE_Institutions.IsActive;
                e.DataType = item.OE_Institutions.DataType;
                oE_Institutionsv.Add(e);
            }

            //[NOTE: integrate all records for the specific page. ]
            var model = new IndexOE_InstitutionsVM()
            {
                _OE_InstitutionsList = oE_Institutionsv,
                licenses = licenses
            };
            return View("OE_Institutions", model);
        }
        public IActionResult MyInstitutionDetails()
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                    {
                        var institutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"));
                        var currentInstitutionDetails = (dynamic)null;
                        if (institutionId != 0)
                        {
                            currentInstitutionDetails = _oeInstitutionsServ.MyInstitutionDetails(institutionId);
                        };

                        var ins = (dynamic)null;
                        if (currentInstitutionDetails != null)
                        {
                            ins = new OE_InstitutionsListVM
                            {
                                Id = currentInstitutionDetails.OE_Institutions.Id,
                                Name = currentInstitutionDetails.OE_Institutions.Name,
                                LogoPath = currentInstitutionDetails.OE_Institutions.LogoPath,
                                FaviconPath = currentInstitutionDetails.OE_Institutions.FaviconPath,
                                Email = currentInstitutionDetails.OE_Institutions.Email,
                                ContactNo = currentInstitutionDetails.OE_Institutions.ContactNo,
                                Address = currentInstitutionDetails.OE_Institutions.Address,
                                IsActive = currentInstitutionDetails.OE_Institutions.IsActive
                            };
                        }
                        
                        var licenseInformation = (dynamic)null;
                        if (currentInstitutionDetails.OELicenses != null)
                        {
                            licenseInformation = new OELicensesListVM
                            {
                                Id = currentInstitutionDetails.OELicenses.Id,
                                InstitutionId = currentInstitutionDetails.OELicenses.InstitutionId,
                                StartDate = _commonServ.CommDate_ConvertToLocalDate(currentInstitutionDetails.OELicenses.StartDate),
                                EndDate = _commonServ.CommDate_ConvertToLocalDate(currentInstitutionDetails.OELicenses.EndDate),
                                IsActive = currentInstitutionDetails.OELicenses.IsActive
                            };
                        }
                        var model = new IndexOE_InstitutionsByAdminVM()
                        {
                            OE_Institution = ins,                            
                            licenses = licenseInformation                           

                        };
                        return View("OE_InstitutionsByAdmin", model);
                        
                    }
                }
                ViewBag.OeErrorMessage = "ERROR101:OE_InstitutionsByAdmin-unauthorized access is not permitted";
                return View("OE_InstitutionsByAdmin");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:OE_InstitutionsByAdmin - " + ex.Message;
                return View("OE_InstitutionsByAdmin");
            }            
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]   
        public IActionResult AddInstitution(IndexOE_InstitutionsVM obj)
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                    {
                        if (cActorId.Substring(0, cActorId.IndexOf("_")) == "1" || cActorId.Substring(0, cActorId.IndexOf("_")) == "2" || cActorId.Substring(0, cActorId.IndexOf("_")) == "3")
                        {

                            var ins = new OE_Institutions()
                            {
                                Name = obj.OE_Institution.Name,
                                CountryId = obj.OE_Institution.CountryId,
                                Email = obj.OE_Institution.Email,
                                ContactNo = obj.OE_Institution.ContactNo,
                                Address = obj.OE_Institution.Address,
                                LogoPath = obj.OE_Institution.LogoPath,
                                FaviconPath = obj.OE_Institution.FaviconPath,
                                IsActive = obj.OE_Institution.IsActive,
                                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

                            };

                            var model = new InsertOE_Institution()
                            {
                                oe_Institution = ins,
                                logo = obj.logo,
                                favicon = obj.favicon
                            };
                            _oeInstitutionsServ.InsertOE_Institutions(model, he.WebRootPath);

                            return RedirectToAction("OE_Institutions");
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
        public IActionResult EditInstitution(IndexOE_InstitutionsVM obj)
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                    {
                        if (cActorId.Substring(0, cActorId.IndexOf("_")) == "1" || cActorId.Substring(0, cActorId.IndexOf("_")) == "2" || cActorId.Substring(0, cActorId.IndexOf("_")) == "3")
                        {                            
                            var oE_Institutions = _oeInstitutionsServ.GetOE_InstitutionsById(obj.OE_Institution.Id);
                            oE_Institutions.Name = obj.OE_Institution.Name;
                            oE_Institutions.CountryId = obj.OE_Institution.CountryId;
                            oE_Institutions.Email = obj.OE_Institution.Email;
                            oE_Institutions.ContactNo = obj.OE_Institution.ContactNo;
                            oE_Institutions.LogoPath = obj.OE_Institution.LogoPath;
                            oE_Institutions.FaviconPath = obj.OE_Institution.FaviconPath;
                            oE_Institutions.IsActive = obj.OE_Institution.IsActive;
                            oE_Institutions.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                            oE_Institutions.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);


                            _oeInstitutionsServ.UpdateOE_Institutions(oE_Institutions, obj.logo, obj.favicon, he.WebRootPath);
                           
                            return RedirectToAction("OE_Institutions");
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
        public IActionResult EditInstitutionByAdmin(IndexOE_InstitutionsByAdminVM obj)
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                    {
                        if (cActorId.Substring(0, cActorId.IndexOf("_")) == "11")

                        {
                            var oE_Institutions = _oeInstitutionsServ.GetOE_InstitutionsById(obj.OE_Institution.Id);

                            oE_Institutions.Email = obj.OE_Institution.Email;
                            oE_Institutions.ContactNo = obj.OE_Institution.ContactNo;

                            _oeInstitutionsServ.UpdateOE_InstitutionsByAdmin(oE_Institutions);

                            return RedirectToAction("MyInstitutionDetails");
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
        public IActionResult Delete(IndexOE_InstitutionsVM obj)
        {           
            string webRootPath = he.WebRootPath;
            var oE_Institutions = _oeInstitutionsServ.GetOE_InstitutionsById(obj.OE_Institution.Id);
            _oeInstitutionsServ.DeleteOE_Institutions(oE_Institutions, webRootPath);
            return RedirectToAction("OE_Institutions");
        }
        #endregion "Post_Methods"
    }
}
