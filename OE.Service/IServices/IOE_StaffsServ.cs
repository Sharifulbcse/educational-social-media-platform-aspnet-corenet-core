
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http; //[NOTE: for IFormFile]

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IOE_StaffsServ
    {
        #region "get Function Definitions"
        IEnumerable<OE_Staffs> GetOE_Staff();

        OE_Staffs GetOE_StaffById(Int64 id);
        
        IEnumerable<GetOE_StaffAndOE_StaffType> GetOE_StaffAndOE_StaffType();

        #endregion "get Function Definitions"

        #region "Insert, Delete and Update Function Definitions"

        void InsertStaff(OE_Staffs oe_staff, IFormFile fle, string webRootPath);
        void UpdateStaff(OE_Staffs oe_staff, IFormFile fle, string webRootPath);
        void DeleteStaff(OE_Staffs oe_staff, string webRootPath);

        #endregion "Insert, Delete and Update Function Definitions"  

    }
}
