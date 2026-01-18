using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class AssignedSectionsController : Controller
    {
        #region "Variables"
        private readonly IAssignedSectionsServ _assignedSectionsServ;
        private readonly IClassesServ _classesServ;
        private readonly IOE_InstitutionsServ _institutionServ;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public AssignedSectionsController(
            IAssignedSectionsServ assignedSectionsServ, 
            IClassesServ classesServ,
            IOE_InstitutionsServ institutionServ,
             ICommonServ commonServ
            )
        {
            _assignedSectionsServ = assignedSectionsServ;
            _classesServ = classesServ;
            _institutionServ = institutionServ;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get_Methods"
        [HttpGet]
        public IActionResult AssignedSections(int year, long ddlClass)
        {
            ViewBag.ddlClass = _classesServ.Dropdown_Classes(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));
            var fetchSection = _assignedSectionsServ.GetSections(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")), ddlClass, year);
           
            var currentInstitutionDetails = _institutionServ.GetOE_InstitutionsById(Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")));

            var sections = new List<AssignedSectionListVM>();
            foreach (var item in fetchSection)
            {
                var e = new AssignedSectionListVM()
                {
                    Id = item.assignedSections.Id,
                    Name = item.assignedSections.Name,
                    Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToLocalDate(item.assignedSections.Year)),

                    ClassId = item.assignedSections.ClassId,
                    ClassName = item.classes.Name,
                    InstitutionId = item.assignedSections.InstitutionId,

                    
                };
                sections.Add(e);
            }

            //[NOTE: find search year]                   
            int currentSearchYear = year != 0 ? year : _commonServ.CommDate_CurrentYear();
           
            var model = new IndexAssignedSectionVM()
            {
                AssignedSectionList = sections,
                InstitutionName = currentInstitutionDetails != null ? currentInstitutionDetails.Name : "",
                CurrentSearchYear = currentSearchYear
                
            };
            return View("AssignedSections", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"

        [HttpPost]
        public IActionResult Add(int addYear, string addSectionName, long addClassId)
        {
            long lastId = (_assignedSectionsServ.GetAllAssignSections().Count() == 0) ? 1 : _assignedSectionsServ.GetAllAssignSections().Last().Id + 1;
            var temp = new AssignedSections()
            {
                Id = lastId,
                Name = addSectionName,
                Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(new DateTime(addYear, 1, 1))),

                ClassId = addClassId,
                InstitutionId = Convert.ToInt64(HttpContext.Session.GetString("session_currentUserInstitutionId")),
                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)

            };
            _assignedSectionsServ.InsertAssignSections(temp);
            return RedirectToAction("AssignedSections");
        }
       
        [HttpPost]
        public IActionResult Edit(long editId, int editYear, string editSectionName, long editClassId)
        {
            var sections = _assignedSectionsServ.GetAssignedSectionsById(editId);
            sections.Name = editSectionName;
            sections.Year = Convert.ToDateTime(_commonServ.CommDate_ConvertToUtcDate(new DateTime(editYear, 1, 1)));

            sections.ClassId = editClassId;
            sections.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            sections.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

            _assignedSectionsServ.UpdateAssignSections(sections);

            return RedirectToAction("AssignedSections");
        }
       
        [HttpPost]
        public IActionResult Delete(long delId)
        {
            var sections = _assignedSectionsServ.GetAssignedSectionsById(delId);
            _assignedSectionsServ.DeleteAssignSections(sections);

            return RedirectToAction("AssignedSections");
        }
        #endregion "Post_Methods"
    }
}
