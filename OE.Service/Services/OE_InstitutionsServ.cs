
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class OE_InstitutionsServ : CommonServ, IOE_InstitutionsServ 
    {
        #region "Variables"
        private IOE_InstitutionsRepo<OE_Institutions> _oeInstitutionsRepo;
        private IOE_LicensesRepo<OE_Licenses> _oeLicensesRepo;
        private ICountriesRepo<Countries> _countriesRepo;
        #endregion "Variables"

        #region "Constructor"
        public OE_InstitutionsServ(
             IOE_InstitutionsRepo<OE_Institutions> oeInstitutionsRepo, 
             IOE_LicensesRepo<OE_Licenses> oeLicensesRepo, 
             ICountriesRepo<Countries> countriesRepo
             )
         {
            _oeInstitutionsRepo = oeInstitutionsRepo;
            _oeLicensesRepo = oeLicensesRepo;
            _countriesRepo = countriesRepo;
        }
        #endregion "Constructor"

        #region "GET Methods"
        public MyInstitutionDetails MyInstitutionDetails(Int64 insId)
        {
            var institutions = _oeInstitutionsRepo.GetAll();
            var license = _oeLicensesRepo.GetAll().Where(l => l.InstitutionId == insId).SingleOrDefault();
            var queryInstitution = (from ins in institutions
                                    where ins.Id == insId
                                    select new { ins }).SingleOrDefault();            
            var returnQry = (dynamic)null;
            if (queryInstitution != null)
            {
                returnQry = new MyInstitutionDetails()
                {
                   OELicenses = license,
                   OE_Institutions = queryInstitution.ins
                };
            }
            return returnQry;
        }

        public OE_Institutions GetOE_InstitutionsById(Int64 id)
        {
            
            var queryAll = _oeInstitutionsRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        public OE_Institutions GetLicensesDetails()
        {

            var queryAll = _oeInstitutionsRepo.GetAll();
            var query = (from i in queryAll
                         where i.DataType != "FIXED" && i.IsActive == true
                         select i).SingleOrDefault();

            return query;

        }

        public IEnumerable<OE_Institutions> GetOE_Institutions()
        {
            //[NOTE: get all departments]
            var queryAll = _oeInstitutionsRepo.GetAll();
            var queryOE_Institutions = from e in queryAll
                                   select e;
            return queryOE_Institutions;
        }
        public IEnumerable<GetInstitutionAndCountries> GetInstitutionAndCountries()
        {
            var getInsitution = _oeInstitutionsRepo.GetAll().ToList();
            var getCountries = _countriesRepo.GetAll().ToList();

            var joinQuery = from i in getInsitution
                            join c in getCountries
                            on i.CountryId equals c.Id
                            select new { i, c };
            var returnQuery = new List<GetInstitutionAndCountries>();

            foreach (var item in joinQuery)
            {
                var temp = new GetInstitutionAndCountries()
                {
                    Countries = item.c,
                    OE_Institutions = item.i
                };
                returnQuery.Add(temp);
            }

            return returnQuery;
        }
        public IEnumerable<Int64> GetLicenses()
        {
            var getInstitution = _oeInstitutionsRepo.GetAll();
            var getLicense = _oeLicensesRepo.GetAll();
            var query = from i in getInstitution
                        join l in getLicense
                        on i.Id equals l.InstitutionId
                        select i.Id;

            return query;
        }
        public IEnumerable<OE_Institutions> GetOE_InstitutionsByKeyWord(string keyword)
        {
            var fetchAll = _oeInstitutionsRepo.GetAll();
            var QueryResults = from u in fetchAll where EF.Functions.Like(u.Name, "%" + keyword + "%") select u;
            return QueryResults;
        }
        #endregion "GET Methods"

        #region "Insert Delete and Edit Methods"
        
        public void InsertOE_Institutions(InsertOE_Institution obj, string WebRoot)
        {

            var msg = (dynamic)null;
            var logo = "ClientDictionary/InstitutionLogo/Logo/";
            var favicon = "ClientDictionary/InstitutionLogo/Favicon/";
            var extension = ".png";
            var extensionFav = ".ico";

            var lastAddingRecord = _oeInstitutionsRepo.GetAll().Last();
            
            if (obj.logo != null)
            {
                //[NOTE:Insert image file]
                var result = Comm_ImageFormat(lastAddingRecord.Id.ToString(), obj.logo, WebRoot, logo, 100, 100, extension);
                obj.oe_Institution.LogoPath = logo + lastAddingRecord.Id + extension;
                _oeInstitutionsRepo.Update(lastAddingRecord);

                if (result == true)
                    msg = "Image Saved";
                else
                    msg = "Error occured.";
            }

            if (obj.favicon != null)
            {
                //[NOTE:Insert image file]
                var result = Comm_ImageFormat(lastAddingRecord.Id.ToString(), obj.favicon, WebRoot, favicon, 20, 20, extensionFav);
                obj.oe_Institution.FaviconPath = favicon + lastAddingRecord.Id + extensionFav;
                _oeInstitutionsRepo.Update(lastAddingRecord);

                if (result == true)
                    msg = "Image Saved";
                else
                    msg = "Error occured.";
            }
            else { }

            _oeInstitutionsRepo.Insert(obj.oe_Institution);

        }

        public void UpdateOE_Institutions(OE_Institutions oE_Institutions, IFormFile logo, IFormFile favicon, string WebRoot )
        {
            
            if (logo != null)
            {
                string logoPath = "ClientDictionary/InstitutionLogo/Logo/";
                string extension = ".png";
                if (Comm_ImageFormat(oE_Institutions.Id.ToString(), logo, WebRoot, logoPath, 100, 100, extension).Equals(true))
                {
                    //[NOTE:Insert image file]
                    oE_Institutions.LogoPath = logoPath + oE_Institutions.Id + extension;
                    _oeInstitutionsRepo.Update(oE_Institutions);
                }

            }

            if (favicon != null)
            {
                string faviconPath = "ClientDictionary/InstitutionLogo/Favicon/";
                string extensionFav = ".ico";
                if (Comm_ImageFormat(oE_Institutions.Id.ToString(), favicon, WebRoot, faviconPath, 20, 20, extensionFav).Equals(true))
                {
                    //[NOTE:Insert image file]
                    oE_Institutions.FaviconPath = faviconPath + oE_Institutions.Id + extensionFav;
                    _oeInstitutionsRepo.Update(oE_Institutions);
                }

               
            }

            _oeInstitutionsRepo.Update(oE_Institutions);
        }
        public void UpdateOE_InstitutionsByAdmin(OE_Institutions oE_Institutions)
        {
            var institutions = _oeInstitutionsRepo.GetAll();
            var queryInstitution = (from ins in institutions
                                    where ins.Id == oE_Institutions.Id
                                    select ins).SingleOrDefault();

            queryInstitution.Id = oE_Institutions.Id;
            queryInstitution.Email = oE_Institutions.Email;
            queryInstitution.ContactNo = oE_Institutions.ContactNo;

            _oeInstitutionsRepo.Update(queryInstitution);

        }
       
        public void DeleteOE_Institutions(OE_Institutions oE_Institutions,string webRootPath)
        {
            _oeInstitutionsRepo.Delete(oE_Institutions);

            DelFileFromLocation(Path.Combine(webRootPath, oE_Institutions.LogoPath));
            DelFileFromLocation(Path.Combine(webRootPath, oE_Institutions.FaviconPath));

        }
        
        #endregion "Insert Delete and Edit Methods"

        #region "Dropdown Methods"       
        public IEnumerable<dropdown_Institutions> Dropdown_Institutions()
        {            
            var institutionQuery = _oeInstitutionsRepo.GetAll().ToList();
            var licenseQuery = _oeLicensesRepo.GetAll().ToList();
            var getInstitution = from i in institutionQuery
                                 select new { i } into q1
                                 where !licenseQuery.Any(l => l.InstitutionId == q1.i.Id)
                                 select q1;

            var queryResult = new List<dropdown_Institutions>();
            foreach (var item in getInstitution)
            {
                var i = new dropdown_Institutions()
                {
                    Id = item.i.Id,
                    Name = item.i.Name
                };
                queryResult.Add(i);
            }
            return queryResult; 
        }
        #endregion "Dropdown Methods"
    }
}
