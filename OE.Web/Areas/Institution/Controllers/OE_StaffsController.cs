
using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;

using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class OE_StaffsController : Controller
    {
        #region "Variables"
        private readonly IOE_StaffsServ _oeStaffsServ;
        private readonly IOE_StaffTypesServ _oeStaffTypeServ;
        private readonly IHostingEnvironment he;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public OE_StaffsController(
            IHostingEnvironment he, 
            IOE_StaffsServ oeStaffsServ, 
            IOE_StaffTypesServ oeStaffTypeServ,
            ICommonServ commonServ
            )
        {
            _oeStaffsServ = oeStaffsServ;
            _oeStaffTypeServ = oeStaffTypeServ;
            this.he = he;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

      
        #region "Get_Methods"
        [HttpGet]
        public IActionResult OE_Staffs()
        {           
            var ddlOE_StffType = _oeStaffTypeServ.Dropdown_OE_StaffType().OrderBy(s => s.Value).Select(x => new { Id = x.Id, Value = x.Value });

            var listOE_Staff = _oeStaffsServ.GetOE_StaffAndOE_StaffType().ToList();
            var staffList = new List<OE_StaffsListVM>();
            foreach (var item in listOE_Staff)
            {
                var e = new OE_StaffsListVM();              
                e.Id = item.OE_Staff.Id;
                e.FirstName = item.OE_Staff.FirstName;
                e.LastName = item.OE_Staff.LastName;
                e.StaffTypeId = item.OE_Staff.StaffTypeId;
                e.StaffTypeName = item.OE_StaffType.Name;
                e.Email = item.OE_Staff.Email;
                e.IP300X200 = item.OE_Staff.IP300X200;
                e.IP600X400 = item.OE_Staff.IP600X400;
                e.Designation = item.OE_Staff.Designation;
                e.PresentAddress = item.OE_Staff.PresentAddress;
                e.PermanentAddress = item.OE_Staff.PermanentAddress;
                e.Contact = item.OE_Staff.Contact;
                staffList.Add(e);
            }
            //[NOTE: integrate all records for the specific page. ]
            var model = new IndexOE_StaffsVM()
            {
                _OE_StaffVM = staffList,
                _OE_StaffTypeList = new SelectList(ddlOE_StffType, "Id", "Value")
            };
            return View("OE_Staffs", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        
        [HttpPost]
        public IActionResult Add(Int64 ddlAddOE_StaffId, string txtAddFirstName, string txtAddLastName, string Email, IFormFile fleAddIP600X400, string Designation, string PresentAddress, string PermanentAddress, string Contact)
        {
            try
            {
                string webRootPath = he.WebRootPath;
                var temp = new OE_Staffs()
                {
                    FirstName = txtAddFirstName,
                    LastName = txtAddLastName,
                    StaffTypeId = ddlAddOE_StaffId,
                    Email = Email,
                    Designation = Designation,
                    PresentAddress = PresentAddress,
                    PermanentAddress = PermanentAddress,
                    Contact = Contact,
                    AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                    AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
                };
                _oeStaffsServ.InsertStaff(temp, fleAddIP600X400, webRootPath);
            }
            catch (Exception ex)
            {
                string s = ex.ToString();
                ViewBag.err = "Error occured while saving data.";
            }

            return RedirectToAction("OE_Staffs");
        }

        [HttpPost]
        public IActionResult Edit(Int64 editOE_StaffId, string txtEditFirstName, string txtEditLastName, IFormFile fleEditIP600X400, Int64 ddlEditOE_StaffTypeId, string editEmail, string editDesignation, string editPresentAddress, string editPermanentAddress, string editContact)
        {
            try
            {
                string webRootPath = he.WebRootPath;
                var oe_staff = _oeStaffsServ.GetOE_StaffById(editOE_StaffId);

                oe_staff.FirstName = txtEditFirstName;
                oe_staff.LastName = txtEditLastName;
                oe_staff.StaffTypeId = ddlEditOE_StaffTypeId;
                oe_staff.Id = editOE_StaffId;
                oe_staff.Email = editEmail;
                oe_staff.Designation = editDesignation;
                oe_staff.PresentAddress = editPresentAddress;
                oe_staff.PermanentAddress = editPermanentAddress;
                oe_staff.Contact = editContact;
                oe_staff.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
                oe_staff.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

                _oeStaffsServ.UpdateStaff(oe_staff, fleEditIP600X400, webRootPath);
            }
            catch (Exception)
            {
                ViewBag.err = "Error occured while saving data.";
            }
            return RedirectToAction("OE_Staffs");
        }
        [HttpPost]
        public IActionResult Delete(Int64 deleteOE_StaffID)
        {
            string webRootPath = he.WebRootPath;
            var oe_staff = _oeStaffsServ.GetOE_StaffById(deleteOE_StaffID);
            _oeStaffsServ.DeleteStaff(oe_staff, webRootPath);

            return RedirectToAction("OE_Staffs");
        }
        
        #endregion "Post_Methods"
    }
}
