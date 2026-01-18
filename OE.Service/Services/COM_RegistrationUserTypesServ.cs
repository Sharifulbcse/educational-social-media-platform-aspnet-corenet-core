
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;

namespace OE.Service
{
    public class COM_RegistrationUserTypesServ : ICOM_RegistrationUserTypesServ
    {
        #region "Variables"
        private readonly ICOM_RegistrationUserTypesRepo<COM_RegistrationUserTypes> _cOM_RegistrationUserTypesRepo;

        #endregion "Variables"

        #region "Constructor"
        public COM_RegistrationUserTypesServ(
            ICOM_RegistrationUserTypesRepo<COM_RegistrationUserTypes> cOM_RegistrationUserTypesRepo

            )
        {
            _cOM_RegistrationUserTypesRepo = cOM_RegistrationUserTypesRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"

        public COM_RegistrationUserTypes GetRegistrationUserTypeById(long id)
        {
            //[NOTE: get all departments]
            var queryAll = _cOM_RegistrationUserTypesRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        #endregion "Get Methods"

        #region "Dropdown Methods"
        public IEnumerable<Dropdown_COM_RegistrationUserTypes> Dropdown_COM_RegistrationUserTypes()
        {
            var getRegUsrTyp = _cOM_RegistrationUserTypesRepo.GetAll().ToList();
            var queryResult = new List<Dropdown_COM_RegistrationUserTypes>();
            foreach (var item in getRegUsrTyp)
            {
                var g = new Dropdown_COM_RegistrationUserTypes()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                queryResult.Add(g);
            }
            return queryResult;
        }
        #endregion "Dropdown Methods"

    }
}
