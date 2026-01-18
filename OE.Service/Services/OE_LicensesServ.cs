
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class OE_LicensesServ : CommonServ, IOE_LicensesServ
    {
        #region "Variables"
        private readonly IOE_LicensesRepo<OE_Licenses> _oeLicensesRepo;
        private readonly IOE_InstitutionsRepo<OE_Institutions> _oeInstitutionsRepo;
        private readonly ICountriesRepo<Countries> _countriesRepo;
        private readonly IOE_UserAuthenticationsRepo<OE_UserAuthentications> _oeUserAuthenticationsRepo;

        #endregion "Variables"

        #region "Constructor"
        public OE_LicensesServ(
            IOE_LicensesRepo<OE_Licenses> oeLicensesRepo, 
            IOE_InstitutionsRepo<OE_Institutions> oeInstitutionsRepo,
            ICountriesRepo<Countries> countriesRepo,
            IOE_UserAuthenticationsRepo<OE_UserAuthentications> oeUserAuthenticationsRepo
            )
        {
            _oeLicensesRepo = oeLicensesRepo;
            _oeInstitutionsRepo = oeInstitutionsRepo;
            _countriesRepo = countriesRepo;
            _oeUserAuthenticationsRepo = oeUserAuthenticationsRepo;
        }
        #endregion "Constructor"

        #region "GET Methods"
        public IEnumerable<GetLicensesList> GetLicensesList()
        {
            //[NOTE: get all lecenses]
            var licenseQuery = _oeLicensesRepo.GetAll();
            var institutionQuery = _oeInstitutionsRepo.GetAll();
            var queryJoin = from l in licenseQuery
                            join i in institutionQuery
                            on l.InstitutionId equals i.Id
                            select new { l, i };
            var queryResult = new List<GetLicensesList>();
            foreach (var item in queryJoin)
            {
                var obj = new GetLicensesList()
                {
                    Licenses = item.l,
                    OEInstitutions = item.i
                };
                queryResult.Add(obj);
            }


            return queryResult;
        }

        
        public IEnumerable<GetLicenses> GetLicenses(Int64 institutionTypeId) {
            var institutionQuery = _oeInstitutionsRepo.GetAll().ToList();
            var countryQuery = _countriesRepo.GetAll().ToList();
            var licenseQuery = _oeLicensesRepo.GetAll().ToList();
            var query = (dynamic)null;
            if (institutionTypeId == 0)
            {
                query = from i in institutionQuery
                        join c in countryQuery
                        on i.CountryId equals c.Id
                        select new { i, c };
            }
            if (institutionTypeId == 1)
            {
                query = from i in institutionQuery
                        join l in licenseQuery
                        on i.Id equals l.InstitutionId
                        join c in countryQuery
                        on i.CountryId equals c.Id
                        select new { i, c };
            }
            if (institutionTypeId == 2)
            {
                var oE_Institutions = new List<OE_Institutions>();
                foreach (var item in institutionQuery)
                {
                    bool matchedDetected = false;
                    foreach (var inside in licenseQuery)
                    {
                        if (item.Id == inside.InstitutionId)
                            matchedDetected = true;
                    }
                    if (matchedDetected == false)
                    {
                        var oE = new OE_Institutions()
                        {
                            Id = item.Id,
                            Name = item.Name,
                            CountryId = item.CountryId,
                        };
                        oE_Institutions.Add(oE);
                    }
                }
                query = from i in oE_Institutions
                        join c in countryQuery
                        on i.CountryId equals c.Id
                        select new { i, c };
            }
            if (institutionTypeId == 3)
            {
                query = from i in institutionQuery
                        join c in countryQuery
                        on i.CountryId equals c.Id
                        orderby i.Name
                        select new { i, c };
            }
            if (institutionTypeId == 4)
            {
                query = from i in institutionQuery
                        join c in countryQuery
                        on i.CountryId equals c.Id
                        orderby i.Name descending
                        select new { i, c };
            }
            
            var queryResult = new List<GetLicenses>();
            foreach (var item in query)
            {
                var temp = new GetLicenses()
                {
                    Countries = item.c,
                    OE_Institutions = item.i
                };
                queryResult.Add(temp);
            };
            return queryResult;
        }

        #endregion "GET Methods"

        #region "Insert, Delete and update Methods"
        
        public void InsertLicenses(InsertLicenses obj, IFormFile fle, string webRootPath)
        {            
            if (obj.Licenses != null)
            {
                //[Note: insert 'OE_Licenses' table]
                if (obj.Licenses != null)
                {
                    Int64 InstituteId = obj.Licenses.InstitutionId;
                    var lic = new OE_Licenses()
                    {
                        InstitutionId = InstituteId,
                        LicenseNumber = function_LicenseNumberForInstitution(Convert.ToInt64( InstituteId)),
                        DP = obj.Licenses.DP,
                        IsActive = obj.Licenses.IsActive,
                        StartDate = CommDate_ConvertToUtcDate((DateTime)obj.Licenses.StartDate),
                        EndDate = CommDate_ConvertToUtcDate((DateTime)obj.Licenses.EndDate),
                        AddedBy = obj.Licenses.AddedBy,
                        AddedDate = CommDate_ConvertToUtcDate(DateTime.Now.Date)

                    };
                    _oeLicensesRepo.Insert(lic);
                    }


                //[Note: insert 'OE_UserAuthentications' table]
                //if (obj.EnableAuthentic == true && obj.Students.UserId != null)
                if (obj.Licenses != null)
                {
                    Int64 InstituteId = obj.Licenses.InstitutionId;
                    var authentic = new OE_UserAuthentications()
                    {
                        ActorId = 11,  //[NOTE: 11 means 'Admin']
                        InstitutionId = InstituteId,
                        UserId = obj.UserId,
                        IsActive = obj.EnableAuthentic,
                        AddedBy = obj.Licenses.AddedBy,
                        AddedDate = CommDate_ConvertToUtcDate(DateTime.Now.Date)
                    };
                    _oeUserAuthenticationsRepo.Insert(authentic);
                }
            }
        }
       
        public void UpdateLicenses(OE_Licenses licenses)
        {
            var licence = _oeLicensesRepo.GetAll();
            var query = (from l in licence
                        where l.Id == licenses.Id
                        select l).SingleOrDefault();

            //[NOTE: updated]
            query.Id = licenses.Id;
            query.StartDate = licenses.StartDate;
            query.EndDate = licenses.EndDate;
            query.IsActive = licenses.IsActive;
            query.ModifiedBy = licenses.ModifiedBy;

            _oeLicensesRepo.Update(query);
        }
        public void DeleteLicenses(OE_Licenses licenses)
        {
            _oeLicensesRepo.Delete(licenses);
        }
        #endregion "Insert, Delete and update Methods"

       
        public long function_LicenseNumberForInstitution(long InstitutionId)
        {
            var LicenseNumber = (dynamic)null;
            Int64 SL = 0;

            //[NOTE: Generate Current Year]
            Int64 cYear = DateTime.Now.Year;

            //[NOTE: Checking License year from last record]
            var recordNo = _oeLicensesRepo.GetAll().Count();

            if (recordNo < 1)
            {
                //[NOTE: Generate Serial number]
                SL = 1;
            }
            else 
            {
                var lastRecord = _oeLicensesRepo.GetAll().Last();
                var lastLY = lastRecord.LicenseNumber.ToString().Substring(0, 4);
                var lastSL = lastRecord.LicenseNumber.ToString().Substring(7, lastRecord.LicenseNumber.ToString().Length - 7);

                //[NOTE: Generate Serial number]                
                if (Convert.ToInt64(lastLY) == cYear)
                    SL = Convert.ToInt64(lastSL) + 1;
                else if (Convert.ToInt64(lastLY) < cYear)
                    SL = 1;

                
            }
            

            //[NOTE: Generate Country code]
            var institutions = _oeInstitutionsRepo.GetAll();
            var countryCode = (from i in institutions
                               where i.Id == InstitutionId
                               select i.CountryId).SingleOrDefault();
            string cCode = countryCode.ToString();
            if (cCode.Length == 1)
            {
                cCode = "00" + cCode;
            }
            else if (cCode.Length == 2)
            {
                cCode = "0" + cCode;
            }

            LicenseNumber = Convert.ToInt64(cYear.ToString() + cCode + SL.ToString());

                                 
            return LicenseNumber;
        }
        

    }
}
