
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class InsCategoriesServ : IInsCategoriesServ
    {
        #region "Variables"
        private IInsCategoriesRepo<InsCategories> _insCategoriesRepo;
        private ICountriesRepo<Countries> _countriesRepo;
        #endregion "Variables"

        #region "Constructor"
        public InsCategoriesServ(
            IInsCategoriesRepo<InsCategories> insCategoriesRepo, 
            ICountriesRepo<Countries> countriesRepo
            )
        {
            _insCategoriesRepo = insCategoriesRepo;
            _countriesRepo = countriesRepo;
        }
        #endregion "Constructor"

        #region "Implementation of GetMethods"
        public IEnumerable<InsCategories> GetInsCategory()
        {
            //[NOTE: get all departments]
            var queryAll = _insCategoriesRepo.GetAll();
            var getResult = from e in queryAll
                                   select e;
            return getResult;
        }

        public InsCategories GetInsCategoryById(Int64 id)
        {
            //[NOTE: get all departments]
            var queryAll = _insCategoriesRepo.GetAll();
            var getResult = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return getResult;
        }

        public IEnumerable<GetInsCategoryAndCountries> GetInsCategoryAndCountries()
        {
            var getInsCategory = _insCategoriesRepo.GetAll().ToList();
            var getCountries = _countriesRepo.GetAll().ToList();

            var joinQuery = from i in getInsCategory
                            join c in getCountries
                            on i.CountryId equals c.Id
                            select new { i, c };
            var returnQuery = new List<GetInsCategoryAndCountries>();

            foreach (var item in joinQuery)
            {
                var temp = new GetInsCategoryAndCountries()
                {
                    Countries = item.c,
                    InsCategory = item.i
                };
                returnQuery.Add(temp);
            }

            return returnQuery;
        }

        #endregion "Implementation of GetMethods"

        #region "Implementation of Insert_Update_Delete Methods"
        public void InsertInsCategory(InsCategories insCategory)
        {
            _insCategoriesRepo.Insert(insCategory);
        }        
        public void UpdateInsCategory(InsCategories insCategory)
        {
            _insCategoriesRepo.Update(insCategory);
        }        
        public void DeleteInsCategory(InsCategories insCategory)
        {
            _insCategoriesRepo.Delete(insCategory);
        }
        #endregion "Implementation of Insert_Update_Delete Methods"

        #region "Implementation of Dropdown Methods"
        public IEnumerable<dropdown_InsCategories> Dropdown_InsCategory()
        {

            var getDept = _insCategoriesRepo.GetAll().ToList();

            var queryResult = new List<dropdown_InsCategories>();

            foreach (var item in getDept)
            {
                var d = new dropdown_InsCategories()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                queryResult.Add(d);
            }

            return queryResult;

        }
        #endregion "Implementation of Dropdown Methods"

    }
}
