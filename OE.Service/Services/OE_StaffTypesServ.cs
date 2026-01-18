
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class OE_StaffTypesServ : IOE_StaffTypesServ
    {
        #region "Variables"
        private readonly IOE_StaffTypesRepo<OE_StaffTypes> _oeStaffTypesRepo;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public OE_StaffTypesServ(
            IOE_StaffTypesRepo<OE_StaffTypes> oeStaffTypesRepo,
            ICommonServ commonServ
        )
        {
            _oeStaffTypesRepo = oeStaffTypesRepo;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Implementation of GetMethods"
        public IEnumerable<OE_StaffTypes> GetAllOE_StaffType()
        {
            //[NOTE: get all events]
            var queryAll = _oeStaffTypesRepo.GetAll();
            var queryOE_StaffType = from e in queryAll
                                     select e;
            return queryOE_StaffType;
        }

        public OE_StaffTypes GetOE_StaffTypeById(Int64 id)
        {
            //[NOTE: get all EmployeeType]
            var queryAll = _oeStaffTypesRepo.GetAll();
            var query = (from e in queryAll                            
                         where e.Id == id                         
                         select e).SingleOrDefault();
            return query;
        }
        #endregion "Implementation of GetMethods"

        #region "Implementation of Insert_Update_Delete Methods"
        public void InsertOE_StaffType(InsertStaffTypes obj)
        {
            if (obj.StaffTypes != null)
            {
                var staffType = new OE_StaffTypes()
                {
                    Name = obj.StaffTypes.Name,
                    IsActive = obj.StaffTypes.IsActive,
                    AddedBy = obj.StaffTypes.AddedBy,
                    AddedDate = obj.StaffTypes.AddedDate
                };
                _oeStaffTypesRepo.Insert(staffType);
            }
        }

        public void UpdateOE_StaffType(UpdateStaffTypes obj)
        {
            if (obj.StaffTypes != null)
            {
                var staffType = _oeStaffTypesRepo.Get(obj.StaffTypes.Id);
                staffType.Name = obj.StaffTypes.Name;
                staffType.ModifiedBy = obj.StaffTypes.ModifiedBy;
                staffType.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);
                _oeStaffTypesRepo.Update(staffType);
            }
        }

        public void DeleteOE_StaffType(DeleteStaffTypes obj)
        {
            if (obj.StaffTypes != null)
            {
                var staffType = _oeStaffTypesRepo.Get(obj.StaffTypes.Id);
                _oeStaffTypesRepo.Delete(staffType);
            }
        }
        #endregion "Implementation of Insert_Update_Delete Methods"

        #region "Implementation of Dropdown Methods"
        public IEnumerable<Dropdown_OE_StaffTypes> Dropdown_OE_StaffType() {
            var staffTypeQuery = _oeStaffTypesRepo.GetAll().ToList();                      
            var query = from e in staffTypeQuery                        
                        select new { e };
            var queryResult = new List<Dropdown_OE_StaffTypes>();
            foreach (var item in query.ToList())
            {
                Dropdown_OE_StaffTypes e = new Dropdown_OE_StaffTypes
                {
                    Id = item.e.Id,
                    Value = item.e.Name
                };
                queryResult.Add(e);
            };
            return queryResult;
        }
        #endregion "Implementation of Dropdown Methods"
    }
}
