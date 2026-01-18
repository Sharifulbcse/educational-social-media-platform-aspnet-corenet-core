
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;

using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class InsCategoriesController : Controller
    {
        #region "Variables"
        private readonly ICountriesServ _countriesServ;
        private readonly ICommonServ _commonServ;

        private readonly IInsCategoriesServ _InsCategoriesServ;
        #endregion "Variables"

        #region "Constructor"
        public InsCategoriesController(
            IInsCategoriesServ InsCategoriesServ, 
            ICountriesServ countriesServ,
            ICommonServ commonServ
        )
        {
            _InsCategoriesServ = InsCategoriesServ;
            _countriesServ = countriesServ;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

      
        #region "Get_Methods"
        [HttpGet]
        public IActionResult InsCategory()
        {
            var ddlCountries = _countriesServ.Dropdown_Countries().OrderBy(c => c.Name).Select(x => new { Id = x.Id, Value = x.Name });
            var listcountries = _InsCategoriesServ.GetInsCategoryAndCountries().ToList();

            var returnModel = new List<InsCategoryListVM>();

            foreach (var item in listcountries)
            {
                var temp = new InsCategoryListVM()
                {
                    Id = item.InsCategory.Id,
                    Name = item.InsCategory.Name,
                    CountryId = item.InsCategory.CountryId,
                    CountryName = item.Countries.Name
                };

                returnModel.Add(temp);
            }

            var model = new IndexInsCategoryVM()
            {
                InsCategories = returnModel,
                Countries = new SelectList(ddlCountries, "Id", "Value")
            };
            return View("InsCategory", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public IActionResult AddInsCategory(string txtAddInsCategoryName, long ddlAddCountryId)
        {
            var temp = new InsCategories()
            {
                Name = txtAddInsCategoryName,
                CountryId = ddlAddCountryId,
                AddedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId")),
                AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now)
            };
            _InsCategoriesServ.InsertInsCategory(temp);
           
            return RedirectToAction("InsCategory");
        }

        [HttpPost]
        public IActionResult Edit(long editInsCategoryId, string txtEditInsCategoryName, long ddlEditCountryId)
        {
            var fetchRecord = _InsCategoriesServ.GetInsCategoryById(editInsCategoryId);
            fetchRecord.Name = txtEditInsCategoryName;
            fetchRecord.CountryId = ddlEditCountryId;
            fetchRecord.ModifiedBy = Convert.ToInt64(HttpContext.Session.GetString("session_CurrentActiveUserId"));
            fetchRecord.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);

            _InsCategoriesServ.UpdateInsCategory(fetchRecord);

            return RedirectToAction("InsCategory");
        }

        [HttpPost]
        public IActionResult Delete(long deleteInsCategoryId)
        {
            var fetchRecord = _InsCategoriesServ.GetInsCategoryById(deleteInsCategoryId);
            _InsCategoriesServ.DeleteInsCategory(fetchRecord);

            return RedirectToAction("InsCategory");
        }
        #endregion "Post_Methods"
    }
}
