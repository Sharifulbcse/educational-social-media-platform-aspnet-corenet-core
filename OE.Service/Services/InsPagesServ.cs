using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;
using Microsoft.AspNetCore.Http;

namespace OE.Service
{
    public class InsPagesServ : CommonServ, IInsPagesServ
    {
        #region "Variables"
        private IInsPagesRepo<InsPages> _insPagesRepo;

        #endregion "Variables"

        #region "Constructor"
        public InsPagesServ(IInsPagesRepo<InsPages> insPagesRepo)
        {
            _insPagesRepo = insPagesRepo;

        }
        #endregion "Constructor"

        #region "Get Methods" 
        public IEnumerable<InsPages> GetInsPages()
        {

            var queryAll = _insPagesRepo.GetAll();
            var queryinsPages = from e in queryAll
                                select e;
            return queryinsPages;
        }
        public InsPages GetInsPagesById(long Id)
        {

            var queryAll = _insPagesRepo.GetAll();
            var queryinsPages = (from e in queryAll
                                 where e.Id == Id
                                 select e).SingleOrDefault();
            return queryinsPages;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public void InsertInsPages(InsertInsPages obj, string WebRoot)
        {

            string imagePathIP300X200 = "ClientDictionary/InsPages/IP300X200/";
            string imagePathIP600X400 = "ClientDictionary/InsPages/IP600X400/";
            string extension = ".png";

            if (obj.image != null)
            {
                var result1 = Comm_ImageFormat(obj.insPages.Id.ToString(), obj.image, WebRoot, imagePathIP300X200, 300, 200, extension);
                var result2 = Comm_ImageFormat(obj.insPages.Id.ToString(), obj.image, WebRoot, imagePathIP600X400, 600, 400, extension);
                obj.insPages.IP300X200 = imagePathIP300X200 + obj.insPages.Id + extension;
                obj.insPages.IP600X400 = imagePathIP600X400 + obj.insPages.Id + extension;
                _insPagesRepo.Update(obj.insPages);
            }
            _insPagesRepo.Insert(obj.insPages);

        }

        public void UpdateInsPages(InsPages insPages, IFormFile fle, string webRootPath)
        {
            if (fle != null)
            {
                string imagePathIP300X200 = "ClientDictionary/InsPages/IP300X200/";
                string imagePathIP600X400 = "ClientDictionary/InsPages/IP600X400/";
                string extension = ".png";

                if (Comm_ImageFormat(insPages.Id.ToString(), fle, webRootPath, imagePathIP300X200, 300, 200, extension).Equals(true) && Comm_ImageFormat(insPages.Id.ToString(), fle, webRootPath, imagePathIP600X400, 600, 400, extension).Equals(true))
                {
                    //[NOTE:Update image file]
                    insPages.IP300X200 = imagePathIP300X200 + insPages.Id + extension;
                    insPages.IP600X400 = imagePathIP600X400 + insPages.Id + extension;
                    _insPagesRepo.Update(insPages);
                }
                else { }
            }
            else
            {
                _insPagesRepo.Update(insPages);
            }

        }

        public void DeleteInsPages(InsPages insPages, string webRootPath1, string webRootPath2)
        {
            _insPagesRepo.Delete(insPages);
            DelFileFromLocation(webRootPath1);
            DelFileFromLocation(webRootPath2);
        }
        #endregion "Insert Update Delete Methods"
    }
}



