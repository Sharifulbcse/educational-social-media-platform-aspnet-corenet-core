using Microsoft.AspNetCore.Http; //[NOTE: for session]
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using OE.Data;
using OE.Service;
using OE.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;


namespace OE.Web.Controllers
{
    public class HomeController : Controller, IActionFilter
    {
        #region "Variables"
        private IndexLayoutVM layout;

        private readonly IOE_UsersServ _usersService;
        private readonly IOE_UserAuthenticationsServ _userAuthenticationsService;
        private readonly IOE_StaffTypesServ oE_StaffTypesServ;
        private readonly IOE_StaffsServ oE_StaffsServ;
        private readonly IOE_InstitutionsServ _institutionServ;
        private readonly IInstitutionLinksServ _insLinksServ;
        private readonly IInsPagesServ _insPagesServ;
        private readonly IInsPageDetailsServ _insPageDetailsServ;

        #endregion "Variables"

        #region "Constructor"
        public HomeController(
            IOE_UsersServ userServ,
            IOE_UserAuthenticationsServ userAuthenticationServ,
            IOE_StaffTypesServ oE_StaffTypesServ, 
            IOE_StaffsServ oE_StaffsServ,
            IOE_InstitutionsServ institutionServ,
            IInstitutionLinksServ insLinksServ,
            IInsPagesServ insPagesServ,
            IInsPageDetailsServ insPageDetailsServ
            )
        {
            _usersService = userServ;
            _userAuthenticationsService = userAuthenticationServ;
            this.oE_StaffsServ = oE_StaffsServ;
            this.oE_StaffTypesServ = oE_StaffTypesServ;
            _institutionServ = institutionServ;
            _insLinksServ = insLinksServ;
            _insPagesServ = insPagesServ;
            _insPageDetailsServ = insPageDetailsServ;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {

            var headerFooterDetials = _institutionServ.GetLicensesDetails();

            var layout = new List<IndexLayoutVM>();
            var myController = context.Controller as HomeController;

            var inslink = (dynamic)null;
            var insLinksVM = new List<InstitutionLinksListVM>();
            if (headerFooterDetials != null)
            {
                inslink = _insLinksServ.GetInstitutionLinks(headerFooterDetials.Id).ToList();

                foreach (var item in inslink)
                {
                    var e = new InstitutionLinksListVM();
                    e.IP24X24 = item.IP24X24;
                    e.Url = item.Url;

                    insLinksVM.Add(e);
                }
            }


            myController.layout = new IndexLayoutVM
            {
                InstitutionName = headerFooterDetials != null ? headerFooterDetials.Name : "OurEdu",
                Logo = headerFooterDetials?.LogoPath,
                Favicon = headerFooterDetials?.FaviconPath,
                Email = headerFooterDetials?.Email,
                Contact = headerFooterDetials?.ContactNo,

                _insLinksVM = insLinksVM


            };

            myController.ViewBag.IndexLayoutVM = myController.layout;


        }
        #endregion "Constructor"

        #region "Get Functions"
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {

            var OeStaffType = oE_StaffTypesServ.GetAllOE_StaffType().ToList();
            var staffType = new List<OE_StaffTypesListVM>();

            foreach (var item in OeStaffType)
            {
                var temp = new OE_StaffTypesListVM()
                {                    
                    Id = item.Id,
                    Name = item.Name,
                };
                staffType.Add(temp);
            }

            var OeStaff = oE_StaffsServ.GetOE_Staff().ToList();
            var staff = new List<OE_StaffsListVM>();

            foreach (var item in OeStaff)
            {
                var temp = new OE_StaffsListVM()
                {
                    Id = item.Id,
                    FirstName = item.FirstName,
                    LastName = item.LastName,
                    StaffTypeId = item.StaffTypeId,
                    Email = item.Email,
                    IP300X200 = item.IP300X200,
                    Designation = item.Designation,
                    PresentAddress = item.PresentAddress,
                    PermanentAddress = item.PermanentAddress,
                    Contact = item.Contact
                };
                staff.Add(temp);
            }

            var model = new IndexAboutVM()
            {
                StaffList = staff,
                StaffTypeList = staffType
            };

            return View("About", model);

        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult InsPageDetails(long insPageId)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                var insPageDetails = _insPageDetailsServ.GetInsPageDetailsByInsPageId(insPageId);
                var ipDetailsList = new List<InsPageDetailsListVM>();

                foreach (var item in insPageDetails._InsPageDatailsList)
                {
                    var temp = new InsPageDetailsListVM()
                    {
                        Id = item.Id,
                        InsPageId = item.InsPageId,
                        Description = item.Description,
                        Title = item.Title,
                        Sorting = item.Sorting
                    };
                    ipDetailsList.Add(temp);
                }

                var model = new IndexInsPageDetailsVM()
                {
                    InsPageDetailsList = ipDetailsList,
                    InsPageId = insPageDetails.InsPageId,
                    InsPageTitle = insPageDetails.InsPageTitle,
                    IP300X200 = insPageDetails.IP300X200,
                    IP600X400 = insPageDetails.IP600X400
                };

                return View("InsPageDetails/InsPageDetails", model);
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:InsPages/InsPageDetails - " + ex.Message;
                return View("InsPageDetails/InsPageDetails");
            }
        }

        public IActionResult RedirectMessage()
        {
            ViewBag.Message = "You have no permission for this page. Please Login First";
            return View("AllMessages/RedirectMessage");
        }

        #endregion "Get Functions"    

        #region "Get Functions- Login"    
        public IActionResult CreateLoginAccount()
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                var reSult = TempData["result"];
                if (reSult != null)
                {
                    ViewData["result"] = reSult;
                }

                var section1 = TempData["section1"];
                var section2 = TempData["section2"];
                var section3 = TempData["section3"];
                var section4 = TempData["section4"];

                if (section2 != null)
                {
                    ViewBag.section1 = $"style = display:none";
                    ViewBag.section2 = section2;
                    ViewBag.active2 = $"style = background-color:forestgreen";
                    ViewBag.section3 = $"style = display:none";
                    ViewBag.section4 = $"style = display:none";
                }
                if (section3 != null)
                {
                    ViewBag.section1 = $"style = display:none";
                    ViewBag.section2 = $"style = display:none";
                    ViewBag.section3 = section3;
                    ViewBag.mail = TempData["mail"];
                    ViewBag.active3 = $"style = background-color:forestgreen";
                    ViewBag.section4 = $"style = display:none";
                }
                if (section4 != null)
                {
                    ViewBag.section1 = $"style = display:none";
                    ViewBag.section2 = $"style = display:none";
                    ViewBag.section3 = $"style = display:none";
                    ViewBag.section4 = section4;
                    ViewBag.steps = $"style = display:none";
                }
                if (section2 == null && section3 == null && section4 == null)
                {
                    ViewBag.section1 = $"style = display:normal";
                    ViewBag.active1 = $"style = background-color:forestgreen";
                    ViewBag.section2 = $"style = display:none";
                    ViewBag.section3 = $"style = display:none";
                    ViewBag.section4 = $"style = display:none";
                }

                var insInformation = _institutionServ.GetLicensesDetails();
                var model = new IndexLoginVM()
                {
                    InstitutionName = insInformation.Name,
                    Logo = insInformation.LogoPath
                };
                return View("login/Registration", model);
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Registration - " + ex.Message;
                return View("login/Registration");
            }

        }
        public IActionResult Login(string msg = "")
        {           
            ViewBag.OeErrorMessage = null;
            try
            {
                ViewBag.msg = msg;
                var insInformation = _institutionServ.GetLicensesDetails();
                var model = new IndexLoginVM()
                {
                    InstitutionName = insInformation.Name,
                    Logo = insInformation.LogoPath
                };
                return View("Login/Login", model);
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Login - " + ex.Message;
                return View("Login/Login");
            }            
        }

        public IActionResult ForgetPassword(string msg = "")
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                ViewBag.msg = msg;

                return View("ForgetPassword/ForgetPassword");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:ForgetPassword/ForgetPassword - " + ex.Message;
                return View("ForgetPassword/ForgetPassword");
            }
        }

        public IActionResult VerifyCodeForForgetPassword(string msg = "")
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                ViewBag.msg = msg;

                return View("ForgetPassword/VerifyCodeForForgetPassword");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:ForgetPassword/VerifyCodeForForgetPassword - " + ex.Message;
                return View("ForgetPassword/VerifyCodeForForgetPassword");
            }
        }

        public IActionResult ResetPassword(string msg = "")
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                ViewBag.msg = msg;
                return View("ForgetPassword/ResetPassword");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:ForgetPassword/ResetPassword - " + ex.Message;
                return View("ForgetPassword/ResetPassword");
            }
        }
        #endregion "Get Functions- Login"               

        #region "POST Functions" 

        [HttpPost]
        public IActionResult EmailVerification(string email)
        {
            try
            {
                if (_usersService.GetUserByEmail(email) == null)
                {
                    HttpContext.Session.SetString("session_CurrentEmail", email);
                    var randomValue = _usersService.GenerateRandomValue(20);
                    HttpContext.Session.SetString("session_CurrentGeneratedValue", randomValue);

                    if (_usersService.GeneratingMailVerification(email, randomValue).Equals(true))
                    {
                        TempData["section2"] = $"style = display:normal";
                        return RedirectToAction("CreateLoginAccount");
                    }
                    else
                    {
                        TempData["result"] = $"<label id=\"result\" class=text-danger>{"Sorry, generating email verification code is failed."}</label>";
                    }
                }
                else
                {
                    TempData["result"] = $"<label id=\"result\" class=text-danger>{"Email is already being used."}</label>";
                }
            }
            catch (Exception)
            {
                TempData["result"] = $"<label id=\"result\" class=text-danger>{"Sorry, something went wrong."}</label>";
            }

            return RedirectToAction("CreateLoginAccount");
        }
        [HttpPost]
        public IActionResult CheckingVerification(string code)
        {
            try
            {
                var generatedCode = HttpContext.Session.GetString("session_CurrentGeneratedValue");
                var email = HttpContext.Session.GetString("session_CurrentEmail");
                if (generatedCode == code)
                {
                    TempData["section3"] = $"style = display:normal";
                    TempData["mail"] = email;
                }
                else
                {
                    TempData["result"] = $"<label id=\"result\" class=text-danger>{"Verification code invalid."}</label>";
                    TempData["section2"] = $"style = display:normal";
                }
            }
            catch (Exception)
            {
                TempData["result"] = $"<label id=\"result\" class=text-danger>{"Sorry, something went wrong."}</label>";
            }
            return RedirectToAction("CreateLoginAccount");
        }

        [HttpPost]
        public IActionResult Registration(string vmail, string fname, string lname, string pswrd1, string pswrd2, DateTime DOB, string phone, string radioResult)
        {
            var verifiedEmail = HttpContext.Session.GetString("session_CurrentEmail");
            if (pswrd1 != pswrd2)
            {
                TempData["result"] = $"<label id=\"result\" class=text-danger>{"Confirm Password is not matched."}</label>";
                TempData["section3"] = $"style = display:normal";
            }
            else
            {
                try
                {
                    var temp = new OE_Users()
                    {
                        EmailAddress = vmail,
                        UserLoginId = vmail,
                        FirstName = fname,
                        LastName = lname,
                        Password = pswrd2,
                        ContactNo = phone,
                        DateOfBirth = DOB,
                        GenderId = 1,
                        IsActive = true
                    };
                    _usersService.InsertUser(temp);
                    HttpContext.Session.Clear();
                    TempData["result"] = $"<label id=\"result\" class=text-success>{"Registration Completed."}</label>";
                    TempData["section4"] = $"style = display:normal";
                }
                catch (Exception)
                {
                    TempData["result"] = $"<label id=\"result\" class=text-danger>{"Registration Failed."}</label>";
                    TempData["section3"] = $"style = display:normal";
                    TempData["mail"] = verifiedEmail;
                }
            }
            return RedirectToAction("CreateLoginAccount");
        }
                
        [HttpPost]        
        public IActionResult Login(IndexLoginVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {                
                var currentLoginDetails = _usersService.GetUserLogin(obj.UserLoginId, obj.Password);
                
                //[NOTE:if login is not match]
                if (currentLoginDetails == null)
                {
                    var insInformation = _institutionServ.GetLicensesDetails();
                    var model = new IndexLoginVM()
                    {
                        InstitutionName = insInformation.Name,
                        Logo = insInformation.LogoPath
                    };

                    ViewBag.msg = "Invalid User Information";
                    return View("Login/Login", model);
                }

                //[NOTE:Creating session and access the system actor wise.]
                else
                {
                    var userAuthenticationList = _userAuthenticationsService.GetUserAuthenticationsByUserId(currentLoginDetails.Id).ToList();
                    string result = "-1_-1;";
                    if (userAuthenticationList != null)
                    {
                        foreach (var item in userAuthenticationList)
                        {
                            result = result + item.ActorId.ToString() + "_" + item.InstitutionId.ToString() + ";";
                        }
                    }

                    string activeUser = (currentLoginDetails.Id).ToString();
                    HttpContext.Session.SetString("session_CurrentActiveUserId", activeUser);
                    HttpContext.Session.SetString("session_UserName", currentLoginDetails.FirstName);
                    HttpContext.Session.SetString("session_currentUserAuthentications", result);
                    HttpContext.Session.SetString("session_currentActiveActorTab", "-1");
                    HttpContext.Session.SetString("session_currentUserInstitutionId", "-1");
                    HttpContext.Session.SetString("session_loginInstitutionName", layout.InstitutionName);
                    HttpContext.Session.SetString("session_loginInstitutionFavicon", layout.Favicon);
                    return RedirectToAction("Index", "OE_Users", new { area = "Institution" });
                }
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:Login - " + ex.Message;
                return View("Login/Login");

            }
        }
       
        [HttpPost]
        public IActionResult ForgetPassword(IndexLoginVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                string result = (dynamic)null;
                var randomValue = _usersService.GenerateRandomValue(6);
                result = _usersService.CheckEmailForForgetPassword(obj.UserLoginId, randomValue);

                if (result == null)
                {
                    HttpContext.Session.SetString("session_CurrentEmail", obj.UserLoginId);
                    HttpContext.Session.SetString("session_CurrentGeneratedValue", randomValue);
                    return RedirectToAction("VerifyCodeForForgetPassword", "Home");
                }
                else
                {
                    ViewBag.msg = result;
                }

                return View("ForgetPassword/ForgetPassword");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:ForgetPassword/ForgetPassword - " + ex.Message;
                return View("ForgetPassword/ForgetPassword");

            }
        }

        [HttpPost]
        public IActionResult VerifyCode(string verifyCode)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                if (verifyCode.ToString() == HttpContext.Session.GetString("session_CurrentGeneratedValue").ToString())
                {
                    return RedirectToAction("ResetPassword", "Home");
                }
                else
                {
                    ViewBag.msg = "Verify code is not correct";
                }

                return View("ForgetPassword/VerifyCodeForForgetPassword");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:ForgetPassword/VerifyCodeForForgetPassword - " + ex.Message;
                return View("ForgetPassword/VerifyCodeForForgetPassword");

            }
        }

        [HttpPost]
        public IActionResult ResetPassword(IndexLoginVM obj)
        {
            ViewBag.OeErrorMessage = null;
            try
            {
                string result = (dynamic)null;
                result = _usersService.ResetPassword(obj.UserLoginId, obj.Password);
                if (result == null)
                {
                    return RedirectToAction("Login", "Home");
                }
                else
                {
                    ViewBag.msg = result;
                }
                return View("ForgetPassword/ResetPassword");
            }
            catch (Exception ex)
            {
                ViewBag.OeErrorMessage = "ERROR101:ForgetPassword/ResetPassword - " + ex.Message;
                return View("ForgetPassword/ResetPassword");
            }
        }
      
        #endregion "POST Fuctions"
    }
}
