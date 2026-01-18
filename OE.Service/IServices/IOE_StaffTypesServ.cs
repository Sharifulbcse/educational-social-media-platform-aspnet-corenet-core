
using System;
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IOE_StaffTypesServ
    {
        #region "get Function Definitions"

        IEnumerable<OE_StaffTypes> GetAllOE_StaffType();
        OE_StaffTypes GetOE_StaffTypeById(Int64 id);
        #endregion "get Function Definitions"

        #region "Insert, Delete and Update Function Definitions"
        void InsertOE_StaffType(InsertStaffTypes obj);
        void UpdateOE_StaffType(UpdateStaffTypes obj);
        void DeleteOE_StaffType(DeleteStaffTypes obj);
        #endregion "Insert, Delete and Update Function Definitions" 

        #region "dropdown Function Definitions"
        IEnumerable<Dropdown_OE_StaffTypes> Dropdown_OE_StaffType();
        #endregion "dropdown Function Definitions"

    }
}
