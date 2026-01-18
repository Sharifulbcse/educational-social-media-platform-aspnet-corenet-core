
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class CountriesServ : ICountriesServ
    {
        #region "Variables"
        private ICountriesRepo<Countries> _countriesRepo;
        #endregion "Variables"

        #region "Constructor"
        public CountriesServ(
            ICountriesRepo<Countries> countriesRepo
        )
        {
            _countriesRepo = countriesRepo;

        }
        #endregion "Constructor"

        #region "Get Methods"
        public IEnumerable<Countries> GetCountries()
        {
            //[NOTE: get all departments]
            var queryAll = _countriesRepo.GetAll();
            var queryCountries = from e in queryAll
                                   select e;
            return queryCountries;
        }

        public Countries GetCountriesById(Int64 id)
        {
            //[NOTE: get all departments]
            var queryAll = _countriesRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        #endregion "Get Methods"
        
        #region "Insert Update Delete Methods"
        public void InsertCountries(Countries countries)
        {
            _countriesRepo.Insert(countries);
        }        
        public void UpdateCountries(Countries countries)
        {
            _countriesRepo.Update(countries);
        }        
        public void DeleteCountries(Countries countries)
        {
            _countriesRepo.Delete(countries);
        }
        #endregion "Insert Update Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_Countries> Dropdown_Countries()
        {            
            var getcountry = _countriesRepo.GetAll().ToList();
            var query = from c in getcountry
                        orderby c.Name
                        select c;

            //[NOTE: add new record]
            var queryResult = new List<dropdown_Countries>() {
                new dropdown_Countries(){ Id=0, Name="Select Country"}
            };

            foreach (var item in query)
            {
                var d = new dropdown_Countries()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                queryResult.Add(d);
            }            
            return queryResult;

        }
        #endregion "Dropdown Methods"
    }
}
