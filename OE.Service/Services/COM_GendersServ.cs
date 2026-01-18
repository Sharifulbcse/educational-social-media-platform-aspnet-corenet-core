using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class COM_GendersServ : ICOM_GendersServ
    {
        #region "Variables"
        private readonly ICOM_GendersRepo<COM_Genders> _cOM_GendersRepo;
        #endregion "Variables"

        #region "Constructor"
        public COM_GendersServ(
            ICOM_GendersRepo<COM_Genders> cOM_GendersRepo
            
            )
        {
            _cOM_GendersRepo = cOM_GendersRepo;
        }
        #endregion "Constructor"


        #region "Dropdown Methods"        
        public IEnumerable<dropdown_COM_Genders> Dropdown_COM_Genders()
        {
            var genders = _cOM_GendersRepo.GetAll().ToList();           
            var query = from g in genders
                        orderby g.Name
                        select g;

            //[NOTE: add new record]
            var queryResult = new List<dropdown_COM_Genders>() {
                new dropdown_COM_Genders(){ Id=0, Name="Select Gender"}
            };

            foreach (var item in query)
            {
                var d = new dropdown_COM_Genders()
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
