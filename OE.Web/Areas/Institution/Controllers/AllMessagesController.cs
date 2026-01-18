
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
    public class AllMessagesController : Controller
    {

        #region "Get_Methods"
        public IActionResult RedirectMessage()
        {
                ViewBag.Message = "You have no permission for this page. Please contact with Institute Authority";
                return View("RedirectMessage");
        }
        #endregion "Get_Methods"

    }
}