
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class RegistrationGroupsServ : IRegistrationGroupsServ
    {

        #region "Variables"
        private IRegistrationGroupsRepo<RegistrationGroups> _registrationGroupsRepo;
        private ICOM_RegistrationUserTypesRepo<COM_RegistrationUserTypes> _comRegistrationUserTypesRepo;

        #endregion "Variables"

        #region "Constructor"
        public RegistrationGroupsServ(
            IRegistrationGroupsRepo<RegistrationGroups> registrationGroupsRepo,
            ICOM_RegistrationUserTypesRepo<COM_RegistrationUserTypes> comRegistrationUserTypesRepo
            )
        {
            _registrationGroupsRepo = registrationGroupsRepo;
            _comRegistrationUserTypesRepo = comRegistrationUserTypesRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"    
        public RegistrationGroups GetRegistrationGroupById(long Id)
        {
            var queryAll = _registrationGroupsRepo.GetAll();

            var query = (from rg in queryAll
                         where rg.Id == Id
                         select rg).FirstOrDefault();
            return query;
        }
        public IEnumerable<GetRegistrationGroups> GetRegistrationGroups(long institutionId, long regUserTypeId = 0)
        {
            var regUserType = _comRegistrationUserTypesRepo.GetAll();
            var regGroup = _registrationGroupsRepo.GetAll();
            var queryRegistrationGroups = (dynamic)null;
            if (regUserTypeId == 0)
            {
                queryRegistrationGroups = from r in regGroup
                                          join ru in regUserType on r.RegistrationUserTypeId equals ru.Id
                                          where r.InstitutionId == institutionId
                                          select new { r, ru };
            }
            else
            {
                queryRegistrationGroups = from r in regGroup
                                          join ru in regUserType on r.RegistrationUserTypeId equals ru.Id
                                          where r.InstitutionId == institutionId && r.RegistrationUserTypeId == regUserTypeId
                                          select new { r, ru };
            }
            var returnQry = new List<GetRegistrationGroups>();
            foreach (var item in queryRegistrationGroups)
            {
                var temp = new GetRegistrationGroups()
                {
                    RegistrationGroups = item.r,
                    COM_RegistrationUserTypes = item.ru
                }; returnQry.Add(temp);
            }
            return returnQry;
        }

        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertRegistrationGroup(InsertRegistrationGroup obj)
        {

            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'RegistrationGroup' table]
                    if (obj.RegistrationGroups != null)
                    {
                        var regGroup = new RegistrationGroups()
                        {
                            Name = obj.RegistrationGroups.Name,
                            InstitutionId = obj.RegistrationGroups.InstitutionId,

                            RegistrationUserTypeId = obj.RegistrationGroups.RegistrationUserTypeId,
                            AddedDate = obj.RegistrationGroups.AddedDate,

                            IsActive = true,
                            AddedBy = obj.RegistrationGroups.AddedBy,
                            ModifiedBy = 0

                        };

                        _registrationGroupsRepo.Insert(regGroup);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:RegistrationGroupsv/InsertRegistrationGroups - " + ex.Message;
            }

            return returnResult;

        }

        public void UpdateRegistrationGroup(RegistrationGroups registrationGroup)
        {
            _registrationGroupsRepo.Update(registrationGroup);
        }

        public void DeleteRegistrationGroup(RegistrationGroups registrationGroup)
        {
            _registrationGroupsRepo.Delete(registrationGroup);
        }
        #endregion "Insert Update Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_RegistrationGroups> Dropdown_RegistrationGroups(long institutionId, long userRegistrationTypeId = 0)
         {
            var getRG = _registrationGroupsRepo.GetAll();
            var returnQry = (dynamic)null;
            if (institutionId != 0 && userRegistrationTypeId != 0)
            {
                returnQry = from rg in getRG
                            where rg.InstitutionId == institutionId && rg.RegistrationUserTypeId == userRegistrationTypeId
                            orderby rg.Name
                            select rg;
            }
            else
            {
                returnQry = from rg in getRG
                            orderby rg.Name
                            select rg;
            }
            

            var queryResult = new List<dropdown_RegistrationGroups>();
            
            foreach (var item in returnQry)
            
            {
                var rg = new dropdown_RegistrationGroups()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                queryResult.Add(rg);
            }

            return queryResult;

        }
        #endregion "Dropdown Methods"

    }
}
