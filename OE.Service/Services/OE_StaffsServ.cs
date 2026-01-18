using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

using Microsoft.AspNetCore.Http;

namespace OE.Service
{
    public class OE_StaffsServ :CommonServ, IOE_StaffsServ
    {
        #region "Variables"
        private IOE_StaffsRepo<OE_Staffs> _oeStaffsRepo;        
        private IOE_StaffTypesRepo<OE_StaffTypes> _oeStaffTypesRepo ;
        #endregion "Variables"

        #region "Constructor"
        public OE_StaffsServ(
            IOE_StaffsRepo<OE_Staffs> oeStaffsRepo, 
            IOE_StaffTypesRepo<OE_StaffTypes> oeStaffTypesRepo
            )
        {
            _oeStaffsRepo = oeStaffsRepo;
            _oeStaffTypesRepo = oeStaffTypesRepo;
            
        }
        #endregion "Constructor"

        #region "Implementation of GetMethods"
        public IEnumerable<OE_Staffs> GetOE_Staff()
        {
            //[NOTE: get all departments]
            var queryAll = _oeStaffsRepo.GetAll();
            var queryOE_Staff = from e in queryAll
                                   select e;
            return queryOE_Staff;
        }

        public OE_Staffs GetOE_StaffById(Int64 id)
        {
            //[NOTE: get all departments]
            var queryAll = _oeStaffsRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }

        public IEnumerable<GetOE_StaffAndOE_StaffType> GetOE_StaffAndOE_StaffType()
        {
           
            var staffType = _oeStaffTypesRepo.GetAll().ToList();
            var oe_staff = _oeStaffsRepo.GetAll().ToList();

            var jointQuery = from st in staffType
                             join s in oe_staff
                             on st.Id equals s.StaffTypeId
                             
                             select new {st,s };

            var queryResult = new List<GetOE_StaffAndOE_StaffType>();
            foreach (var item in jointQuery)
            {
                var obj = new GetOE_StaffAndOE_StaffType()
                {
                   
                    OE_Staff = item.s,
                    OE_StaffType = item.st
                };
                queryResult.Add(obj);
            }


            return queryResult;
        }
        #endregion "Implementation of GetMethods"

        #region "Insert, Update and Delete functions"
      
        public void InsertStaff(OE_Staffs oe_staff, IFormFile fle, string webRootPath)
        { //[NOTE: Insert Staff data for 1st time]
            _oeStaffsRepo.Insert(oe_staff);
            //[NOTE: Reading last record of Staff]
            var lastAddingRecord = _oeStaffsRepo.GetAll().Last();
            string imagePath600X400 = "ClientDictionary/Staffs/IP600X400/";
            string imagePath300X200 = "ClientDictionary/Staffs/IP300X200/";

            if (Comm_ImageFormat(lastAddingRecord.Id.ToString(), fle, webRootPath, imagePath600X400, 600, 400, ".jpg").Equals(true))
            {
                //[NOTE:Update image file]
                lastAddingRecord.IP600X400 = imagePath600X400 + lastAddingRecord.Id + ".jpg";
                _oeStaffsRepo.Update(lastAddingRecord);
            }
            else { }
            if (Comm_ImageFormat(lastAddingRecord.Id.ToString(), fle, webRootPath, imagePath300X200, 300, 200, ".jpg").Equals(true))
            {
                //[NOTE:Update image file]
                lastAddingRecord.IP300X200 = imagePath300X200 + lastAddingRecord.Id + ".jpg";
                _oeStaffsRepo.Update(lastAddingRecord);
            }
            else { }

        }
        public void UpdateStaff(OE_Staffs oe_staff, IFormFile fle, string webRootPath)
        {
            if (fle != null)
            {
                string imagePath600X400 = "ClientDictionary/Staffs/IP600X400/";
                string imagePath300X200 = "ClientDictionary/Staffs/IP300X200/";
                if (Comm_ImageFormat(oe_staff.Id.ToString(), fle, webRootPath, imagePath600X400, 600, 400, ".jpg").Equals(true))
                {
                    //[NOTE:Update image file]
                    oe_staff.IP600X400 = imagePath600X400 + oe_staff.Id + ".jpg";
                    _oeStaffsRepo.Update(oe_staff);
                }
                else { }
                if (Comm_ImageFormat(oe_staff.Id.ToString(), fle, webRootPath, imagePath300X200, 300, 200, ".jpg").Equals(true))
                {
                    //[NOTE:Update image file]
                    oe_staff.IP300X200 = imagePath300X200 + oe_staff.Id + ".jpg";
                    _oeStaffsRepo.Update(oe_staff);
                }
                else { }
            }
            else
            {
                _oeStaffsRepo.Update(oe_staff);
            }

        }
        public void DeleteStaff(OE_Staffs oe_staff, string webRootPath)
        {
            _oeStaffsRepo.Delete(oe_staff);
            DelFileFromLocation(Path.Combine(webRootPath, oe_staff.IP300X200));
            DelFileFromLocation(Path.Combine(webRootPath, oe_staff.IP600X400));
        }
        
        #endregion "Insert, Update and Delete functions"
    }
}
