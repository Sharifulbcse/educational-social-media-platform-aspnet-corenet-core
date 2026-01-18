using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Filters;

using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;
using OE.Service.ServiceModels;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class MarkTypesController : Controller
    {
        
        #region "Variables"
       private readonly IMarkTypesServ _markTypesServ;
       private readonly IOE_InstitutionsServ _institutionServ;
       private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public MarkTypesController(            
            IMarkTypesServ markTypesServ,
            IOE_InstitutionsServ institutionServ,
             ICommonServ commonServ
            )
        {
            _markTypesServ = markTypesServ;
            _institutionServ = institutionServ;
            _commonServ = commonServ;
        }
        #endregion "Constructor"
               
        #region "Get_Methods"
        [HttpGet]
        public IActionResult MarkTypes()
        {
            var getMarkTypes = _markTypesServ.GetMarkTypes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId"))).ToList();
            var currentInstitutionDetails = _institutionServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

            var marktv = new List<MarkTypesListVM>();
            foreach (var item in getMarkTypes)
            {
                var e = new MarkTypesListVM()
                {
                    Id = item.Id,
                    Name = item.Name,
                    InstitutionId = item.InstitutionId
                };
                marktv.Add(e);
            }
            //[NOTE: integrate all records for the specific page. ]
            var model = new IndexMarkTypesVM()
            {
                InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                _MarkTypesList = marktv
            };

            return View("MarkTypes", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public JsonResult InsertMarkTypes(IndexMarkTypesVM obj)
        {
            var result = (dynamic)null;
            string message = (dynamic)null;
            try
            {
                if (obj.MarkTypesList != null)
                {
                    var markType = new MarkTypes()
                    {
                        Name = obj.MarkTypesList.Name,
                        InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                        IsActive = true,
                        AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                    };

                    var model = new InsertMarkTypes()
                    {
                        MarkTypes = markType
                    };
                    message = _markTypesServ.InsertMarkTypes(model);
                    result = Json(new { success = true, Message = message });
                }
            }
            catch (Exception ex)
            {
                result = Json(new { success = false, Message = "ERROR101:MarkTypes/InsertMarkTypes - " + ex.Message });

            }
            return result;            
        }
        [HttpPost]
        public IActionResult Edit(long editMarkTypesId, string txtEditMarkTypesName)
        {
            var fetchRecord = _markTypesServ.GetMarkTypesById(editMarkTypesId);
            fetchRecord.Name = txtEditMarkTypesName;
            fetchRecord.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            fetchRecord.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

            _markTypesServ.UpdateMarkTypes(fetchRecord);
            return RedirectToAction("MarkTypes");
        }
        [HttpPost]
        public JsonResult DeleteMarkTypes(int markTypesId)
        {            
            var result = (dynamic)null;            
            try
            {
                foreach (var cActorId in HttpContext.Session.GetString("session_currentUserAuthentications").Split(';'))
                {
                    if (cActorId.Substring(0, cActorId.IndexOf("_")) == "12")
                    {
                        if (markTypesId > 0)
                        {
                            var model = new DeleteMarkTypes()
                            {
                                Id = markTypesId
                            };
                            var returnResult = _markTypesServ.DeleteMarkTypes(model);
                            result = Json(new { success = returnResult.SuccessIndicator, message = returnResult.Message });
                            return result;                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    result = Json(new { success = false, Message = "Mark Type is not possible to delete because it is used another place" });
                }
                else
                {
                    result = Json(new { success = false, Message = "ERROR101:MarkTypes/DeleteMarkTypes - " + ex.Message });
                }
            }
            return result;
        }
        #endregion "Post_Methods"

    }
}
