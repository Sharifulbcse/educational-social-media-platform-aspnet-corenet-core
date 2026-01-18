using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class RegistrationItemTypesServ : IRegistrationItemTypesServ
    {
        #region "Variables"
        private readonly IRegistrationItemTypesRepo<COM_RegistrationItemTypes> _registrationItemTypesRepo;
        #endregion "Variables"

        #region "Constructor"
        public RegistrationItemTypesServ(
            IRegistrationItemTypesRepo<COM_RegistrationItemTypes> registrationItemTypesRepo
        )
        {
            _registrationItemTypesRepo = registrationItemTypesRepo;

        }
        #endregion "Constructor"      

        #region "Dropdown Methods"
        public IEnumerable<dropdown_RegistrationItemTypes> Dropdown_RegistrationItemTypes()
        {           
            var getRIT = _registrationItemTypesRepo.GetAll();
            var ascendingOrder = from rit in getRIT
                                 orderby rit.Name
                                 select rit;

           

            var queryResult = new List<dropdown_RegistrationItemTypes>();
            
            foreach (var item in ascendingOrder)
            
            {
                var rit = new dropdown_RegistrationItemTypes()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                queryResult.Add(rit);
            }
            return queryResult;
        }

        #endregion "Dropdown Methods"

    }
}
