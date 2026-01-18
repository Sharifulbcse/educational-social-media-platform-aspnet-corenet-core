using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

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
    public class OE_UsersController : Controller, IActionFilter
    {
        private readonly IOE_UsersServ _oeUsersServ;      
        private readonly IOE_ActorsServ _oeActorsServ;
        private readonly ICOM_GendersServ _comGendersServ;
        private readonly ICommonServ _commonServ;
        private readonly IHostingEnvironment he;
        public OE_UsersController(
            IOE_UsersServ oeUsersServ,
            IOE_ActorsServ oeActorsServ,
            ICOM_GendersServ comGendersServ,
            ICommonServ commonServ,
            IHostingEnvironment he
            )
        {
            _oeUsersServ = oeUsersServ;
            _comGendersServ = comGendersServ;
            _oeActorsServ = oeActorsServ;
            _commonServ = commonServ;
            this.he = he;
        }

        #region "GET Functions"
        public IActionResult Index()
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {                   

                    var p = _oeUsersServ.GetUserByID(Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")));
                    var temp = new OE_UsersListVM()
                    {

                        GenderId = p.Users.GenderId,
                        IP300X200 = p.Users.IP300X200


                    };
                    var model = new IndexOE_UsersVM()
                    {
                        users = temp
                    };

                    return View("Index/Index", model);
                }
                else
                {
                    return RedirectToAction("Login", "Home", new { area = "" });
                }

            }
            catch (Exception) { return RedirectToAction("Login", "Home", new { area = "" }); }
        }
        public ActionResult LogOut()
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    HttpContext.Session.Clear();
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
        public IActionResult ChangePassword()
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    var currentUser = _oeUsersServ.GetUserByID(Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")));

                    var userList = new List<OE_UsersListVM>();
                    var u = new OE_UsersListVM();                    
                    u.Id = currentUser.Users.Id;
                    u.GenderId = currentUser.Users.GenderId;
                    u.FirstName = currentUser?.Users?.FirstName;
                    u.LastName = currentUser?.Users?.LastName;
                    u.IP300X200 = currentUser?.Users?.IP300X200;
                    u.IP600X400 = currentUser?.Users?.IP600X400;
                    u.UserLoginId = currentUser?.Users?.UserLoginId;
                    u.Password = currentUser?.Users?.Password;
                    
                    userList.Add(u);

                    var model = new IndexOE_UsersVM()
                    {
                        OEUsersListVMs = userList
                    };
                    return View("ChangePassword/ChangePassword", model);

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
        public IActionResult OELicenses()
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                    {
                        if (cActorId.Substring(0, cActorId.IndexOf("_")) == "1" || cActorId.Substring(0, cActorId.IndexOf("_")) == "2" || cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                        {                            
                            var listOEManagers = _oeUsersServ.GetOELicenses(Convert.ToInt64(HttpContext.Session.GetString("session_currentActiveActorTab")), Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).ToList(); //[Access wise list]
                            ViewBag.ddlActors = _oeActorsServ.Dropdown_Actors(Convert.ToInt64(HttpContext.Session.GetString("session_currentActiveActorTab"))); 
                           
                            var managersList = new List<OE_UserAuthenticationsListVM>();
                            foreach (var item in listOEManagers)
                            {
                                var ua = new OE_UserAuthenticationsListVM();
                                ua.Id = item.OEUserAuthentications.Id;
                                ua.ActorId = item.OEUserAuthentications.ActorId;
                                ua.UserId = item.OEUserAuthentications.UserId;
                                ua.IsActive = item.OEUserAuthentications?.IsActive;

                                ua.UserLoginId = item.OEUsers?.UserLoginId;
                                ua.UserIP300X200 = item.OEUsers?.IP300X200;
                                ua.UserName = item.OEUsers?.FirstName + " " + item.OEUsers.LastName;
                                ua.GenderId = item.OEUsers.GenderId;
                                ua.ActorName = item.OEActors?.Name;
                                managersList.Add(ua);
                            }
                            var model = new IndexOEUserAuthenticationsVM()
                            {
                                OEUserAuthenticationsListVM = managersList                                
                            };

                            return View("OELicenses/OELicenses", model);
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
        public IActionResult Profile()
        {            
            var p = _oeUsersServ.GetUserByID(Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")));
            ViewBag.ddlGender = _comGendersServ.Dropdown_COM_Genders();
            var temp = new OE_UsersListVM()
            {
                Id = p.Users.Id,
                FirstName = p.Users.FirstName,
                LastName = p.Users.LastName,
                EmailAddress = p.Users.EmailAddress,
                DateOfBirth = _commonServ.CommDate_ConvertToLocalDate(Convert.ToDateTime(p.Users.DateOfBirth)),
                GenderId = p.Users.GenderId,
                GenderName = p.Genders.Name,
                ContactNo = p.Users.ContactNo,
                IP600X400 = p.Users.IP600X400,
                IP300X200 = p.Users.IP300X200
               

            };
            var model = new IndexOE_UsersVM()
            {
                users = temp
            };

            return View("Profile/Profile", model);
        }
        #endregion "GET Functions"

        #region "Post function"
        [HttpPost]
        public IActionResult AddImage(IFormFile fleAddIP600X400)
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    string webRootPath = he.WebRootPath;
                    var temp = new OE_Users()
                    {
                        Id = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"))

                    };
                    _oeUsersServ.UpdateUser(temp, fleAddIP600X400, webRootPath);
                    return RedirectToAction("profile");
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
        public JsonResult AddOELicense(string userEmail, Int64 ddlAddActorId, bool addIsActive)
        {            
            var result = (dynamic)null;
            var CreateAuthentication = new OE_UserAuthentications()
            {
                ActorId = ddlAddActorId,
                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                IsActive = addIsActive,
                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                AddedDate = DateTime.Now
            };
            var model = new InsertStaffLicenses()
            {
                OE_UserAuthentications = CreateAuthentication,
                UserLoginId = userEmail
            };
            var msg = _oeUsersServ.InsertStaffLicense(model);
            if (msg == null)
            {
                result = Json(new { success = true, successMessage = "" });
            }
            else
            {
                result = Json(new { success = true, errorMessage = msg });
            }
            
            return result;
        }
        [HttpPost]
        public IActionResult EditProfile(string txtEditFirstName, string txtEditLastName, string editEmailAddress, IFormFile fleEditIP600X400, long editGenderId, string editContact, DateTime? editDateofBirth)
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    var users = new OE_Users()
                    {
                        Id = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),

                        FirstName = txtEditFirstName,
                        LastName = txtEditLastName,
                        DateOfBirth = (editDateofBirth == null) ? editDateofBirth : _commonServ.CommDate_ConvertToUtcDate(Convert.ToDateTime(editDateofBirth)),
                        GenderId = editGenderId,
                     
                        EmailAddress = editEmailAddress,
                        ContactNo = editContact
                    };

                    _oeUsersServ.UpdateUser(users, null, null);

                    return RedirectToAction("profile");
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
        public JsonResult EditOELicense(long authenticId, string editUserEmail, Int64 ddlEditActorId, bool editIsActive)
        {
            
            var result = (dynamic)null;
            var UpdateAuthentication = _oeUsersServ.GetOE_UserAuthenticationsById(authenticId);
            UpdateAuthentication.IsActive = editIsActive;
            UpdateAuthentication.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            UpdateAuthentication.ModifiedDate = DateTime.Now;

            var model = new UpdateStaffLicenses()
            {
                OE_UserAuthentications = UpdateAuthentication,
                SelectedUserLoginId = editUserEmail,
                SelectedActorId = ddlEditActorId
            };
            var msg = _oeUsersServ.UpdateStaffLicense(model);
            if (msg == null)
            {
                result = Json(new { success = true, successMessage = "" });
            }
            else
            {
                result = Json(new { success = true, errorMessage = msg });
            }
            
            return result;
        }
        [HttpPost]
        public JsonResult DeleteOELicense(int delAuthenticId)
        {
            var result = (dynamic)null;            
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                    {
                        if (cActorId.Substring(0, cActorId.IndexOf("_")) == "1" || cActorId.Substring(0, cActorId.IndexOf("_")) == "2" || cActorId.Substring(0, cActorId.IndexOf("_")) == "11")
                        {
                            if (delAuthenticId > 0)
                            {
                                var model = new DeleteOELicenses()
                                {
                                    Id = delAuthenticId
                                };
                                var returnResult = _oeUsersServ.DelateUserAuthentication(model);
                                result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                                return result;                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:OE_Users/DeleteOELicense - " + ex.Message });
            }
            return result;
        }
        [HttpPost]
        public IActionResult DeleteImg(Int64 deleteUserId)
        {
            string webRootPath = he.WebRootPath;
            var oe_user = _oeUsersServ.GetUserByID(deleteUserId);
            _oeUsersServ.DeleteProfleImg(oe_user.Users, webRootPath);
           return RedirectToAction("Profile");
        }

        [HttpPost]
        public IActionResult ChangePassword(long Id, string Password)
        {
            try
            {
                if (HttpContext.Session.GetString("session_CurrentActiveUserId") != null)
                {
                    var fetchRecord = _oeUsersServ.GetUserByID(Id);                   
                    fetchRecord.Users.Password = Password;
                    _oeUsersServ.UpdateUser(fetchRecord.Users, null, null);                    
                    return RedirectToAction("ChangePassword", "OE_Users", new { area = "Institution" });
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

        #endregion "Post function"

        #region "Security methods"
        [HttpPost]
        public IActionResult SetCurrentActorTab(string CurrentActorTabId)
        {
            HttpContext.Session.SetString("session_currentActiveActorTab", CurrentActorTabId.Substring(0, CurrentActorTabId.IndexOf("_")));
            HttpContext.Session.SetString("session_currentUserInstitutionId", CurrentActorTabId.Substring(CurrentActorTabId.IndexOf("_") + 1, CurrentActorTabId.Length - (CurrentActorTabId.IndexOf("_") + 1)));

            return RedirectToAction("Index", "OE_Users", new { area = "Institution" });
            
        }
        #endregion "Security methods"
        
    }
}
