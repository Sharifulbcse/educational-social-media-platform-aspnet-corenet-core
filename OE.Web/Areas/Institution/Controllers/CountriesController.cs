
using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using OE.Data;
using OE.Service;
using OE.Web.Areas.Institution.Models;

namespace OE.Web.Areas.Institution.Controllers
{
    [Area("Institution")]
    public class CountriesController : Controller
    {
        #region "Variables"
        private readonly ICountriesServ _countriesServ;
        
        #endregion "Variables"

        #region "Constructor"
        public CountriesController(
            ICountriesServ countriesServ 
            
            )
        {
            _countriesServ = countriesServ;
            
        }
        #endregion "Constructor"
        
        #region "Get_Methods"
        [HttpGet]
        public IActionResult Countries()
        {           
            var listcountries = _countriesServ.GetCountries().ToList();
            var countriesv = new List<CountriesListVM>();
            foreach (var item in listcountries)
            {
                var e = new CountriesListVM()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                countriesv.Add(e);
            }
            var model = new IndexCountriesVM()
            {
                _Countries = countriesv
            };
            return View("Countries", model);
        }
        #endregion "Get_Methods"

        #region "Post_Methods"
        [HttpPost]
        public IActionResult AddCountry(string txtAddCountryName)
        {
            var temp = new Countries()
            {
                Name = txtAddCountryName
            };
            _countriesServ.InsertCountries(temp);
           
            return RedirectToAction("Countries");
        }

        [HttpPost]
        public IActionResult Edit(long editCountryId, string txtEditCountryName)
        {
            var countries = _countriesServ.GetCountriesById(editCountryId);
            countries.Name = txtEditCountryName;

            _countriesServ.UpdateCountries(countries);

            return RedirectToAction("Countries");
        }

        [HttpPost]
        public IActionResult Delete(long deleteCountryId)
        {
            var countries = _countriesServ.GetCountriesById(deleteCountryId);
            _countriesServ.DeleteCountries(countries);

            return RedirectToAction("Countries");
        }
        #endregion "Post_Methods"       
    }
}
