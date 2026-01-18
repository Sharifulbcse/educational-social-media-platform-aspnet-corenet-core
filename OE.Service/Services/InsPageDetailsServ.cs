using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;
using Microsoft.AspNetCore.Http;

namespace OE.Service
{
    public class InsPageDetailsServ : CommonServ, IInsPageDetailsServ
    {
        #region "Variables"
        private IInsPagesRepo<InsPages> _insPagesRepository;
        private IInsPageDetailsRepo<InsPageDetails> _insPageDetailsRepository;

        #endregion "Variables"

        #region "Constructor"
        public InsPageDetailsServ(IInsPageDetailsRepo<InsPageDetails> insPageDetailsRepository,
            IInsPagesRepo<InsPages> insPagesRepository)
        {
            _insPageDetailsRepository = insPageDetailsRepository;
            _insPagesRepository = insPagesRepository;
        }
        #endregion "Constructor"

        #region "Get Methods" 
        public IEnumerable<InsPageDetails> GetInsPageDetails()
        {

            var queryAll = _insPageDetailsRepository.GetAll();
            var queryinsPageDetails = from e in queryAll
                                      select e;
            return queryinsPageDetails;
        }
        public GetInsPageDetailsByInsPageId GetInsPageDetailsByInsPageId(long insPageId)
        {
            var insPage = _insPagesRepository.GetAll().Where(i => i.Id == insPageId).SingleOrDefault();
            var insPageDetails = _insPageDetailsRepository.GetAll();
            var queryResult = (from ipd in insPageDetails
                               where ipd.InsPageId == insPageId
                               orderby ipd.Sorting
                               select ipd).ToList();
            queryResult.Select(m => { m.Description = Comm_ParagraphFormatForView(m.Description); return m; }).ToList();

            var returnResult = new GetInsPageDetailsByInsPageId()
            {
                InsPageId = insPage.Id,
                InsPageTitle = insPage.Title,
                IP300X200 = insPage.IP300X200,
                IP600X400 = insPage.IP600X400,
                _InsPageDatailsList = queryResult
            };

            return returnResult;
        }
        public InsPageDetails GetInsPageDetailsById(long id)
        {
            var insPageDetails = _insPageDetailsRepository.GetAll();
            var queryResult = (from ipd in insPageDetails
                               where ipd.Id == id
                               select ipd).SingleOrDefault();
            return queryResult;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertInsPageDetails(InsertInsPageDetails obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'Departments' table]
                    if (obj.insPageDetails != null)
                    {
                        var insPageDetails = new InsPageDetails()
                        {
                            Id = obj.insPageDetails.Id,
                            Title = obj.insPageDetails.Title,
                            Description = obj.insPageDetails.Description,
                            InsPageId = obj.insPageDetails.InsPageId,
                            Sorting = obj.insPageDetails.Sorting,
                            IsActive = obj.insPageDetails.IsActive,

                            AddedBy = obj.insPageDetails.AddedBy,
                            AddedDate = obj.insPageDetails.AddedDate
                        };

                        _insPageDetailsRepository.Insert(insPageDetails);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:InsPageDetailsServ/InsertInsPageDetails - " + ex.Message;
            }

            return returnResult;

        }

        public void UpdateInsPageDetails(InsPageDetails insPageDetails)
        {
            _insPageDetailsRepository.Update(insPageDetails);
        }

       public DeleteInsPages DeleteInsPageDetails(DeleteInsPages obj)
        {
            var returnModel = new DeleteInsPages();
             try
            {
                if (obj.Id > 0)
                
                {
                    var insPageDetails = _insPageDetailsRepository.Get(obj.Id);
                    if (insPageDetails != null)
                    {
                        _insPageDetailsRepository.Delete(insPageDetails);

                        
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
                    returnModel.Message = "ERROR102:DepartmentsServ/DeleteDepartments - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }
               
            }
          ;
            return returnModel;
        }

        #endregion "Insert Update Delete Methods"
    }
}



