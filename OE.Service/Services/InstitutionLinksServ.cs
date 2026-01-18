using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;
using Microsoft.AspNetCore.Http;

namespace OE.Service
{
    public class InstitutionLinksServ : CommonServ, IInstitutionLinksServ
    {
        #region "Variables"
        private IInstitutionLinksRepo<InstitutionLinks> _institutionLinksRepo;

        #endregion "Variables"

        #region "Constructor"
        public InstitutionLinksServ(IInstitutionLinksRepo<InstitutionLinks> institutionLinksRepo)
        {
            _institutionLinksRepo = institutionLinksRepo;

        }
        #endregion "Constructor"

        #region "Get Methods" 
        public IEnumerable<InstitutionLinks> GetInstitutionLinks(long institutionId)
        {

            var queryAll = _institutionLinksRepo.GetAll();
            var queryinsLinks = from e in queryAll
                                where e.InstitutionId == institutionId
                                select e;
            return queryinsLinks;
        }

        public InstitutionLinks GetInstitutionLinksById(long Id)
        {

            var queryAll = _institutionLinksRepo.GetAll();
            var queryinsLinks = (from e in queryAll
                                 where e.Id == Id
                                 select e).SingleOrDefault();
            return queryinsLinks;
        }

        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertInstitutionLinks(InsertInstitutionLinks obj, string WebRoot)
        {

            string imagePathIP24X24 = "ClientDictionary/InstitutionLinks/IP24X24/";
            string extension = ".ico";            
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {

                    if (obj.socialimage != null)
                    {

                        _institutionLinksRepo.Insert(obj.institutionLinks);
                        var lastId = _institutionLinksRepo.GetAll().Last();
                        var result = Comm_ImageFormat(lastId.Id.ToString(), obj.socialimage, WebRoot, imagePathIP24X24, 24, 24, extension);
                        obj.institutionLinks.IP24X24 = imagePathIP24X24 + lastId.Id + extension;
                        _institutionLinksRepo.Update(obj.institutionLinks);

                        returnResult = "Saved";

                    }
                    else
                    {
                        _institutionLinksRepo.Insert(obj.institutionLinks);
                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:InstitutionLinksServ/InsertInstitutionLinks - " + ex.Message;
            }

            return returnResult;            

        }
        public void UpdateInstitutionLinks(InstitutionLinks institutionLinks, IFormFile fle, string webRootPath)
        {
            if (fle != null)
            {
                string imagePathIP24X24 = "ClientDictionary/InstitutionLinks/IP24X24/";
                string extension = ".ico";

                if (Comm_ImageFormat(institutionLinks.Id.ToString(), fle, webRootPath, imagePathIP24X24, 24, 24, extension).Equals(true))
                {
                    //[NOTE:Update image file]
                    institutionLinks.IP24X24 = imagePathIP24X24 + institutionLinks.Id + extension;
                    _institutionLinksRepo.Update(institutionLinks);
                }
                else { }
            }
            else
            {
                _institutionLinksRepo.Update(institutionLinks);
            }

        }
        public DeleteInstitutionLinks DeleteInstitutionLinks(DeleteInstitutionLinks obj, string webRootPath)
        {
            var returnModel = new DeleteInstitutionLinks();
            try
            {
                if (obj.Id > 0)
                {
                    var insLinks = _institutionLinksRepo.Get(obj.Id);
                    if (insLinks != null)
                    {
                        _institutionLinksRepo.Delete(insLinks);
                        DelFileFromLocation(webRootPath);
                        returnModel.Message = "Delete Successful.";
                        returnModel.SuccessIndicator = true;
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    returnModel.Message = "Record is not possible to delete, because it used in other places.";
                    returnModel.SuccessIndicator = false;
                }
                else
                {
                    returnModel.Message = "ERROR102:InstitutionLinksServ/DeleteInstitutionLinks - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }                
            }
            
            return returnModel;            
        }
    }
}

#endregion "Insert Update Delete Methods"

