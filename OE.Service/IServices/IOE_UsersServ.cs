
using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IOE_UsersServ
    {
        #region "Get Function Definitions"
        string GenerateRandomValue(int length);
        string CheckEmailForForgetPassword(string userLoginId, string randomValue);
        string ResetPassword(string userLoginId, string password);

        bool GeneratingMailVerification(string receiverMail, string randomValue);
        
        OE_Users GetUserLogin(string UserId, string Password);
        OE_Users GetUserByEmail(string email);
      
        UsersWithGenders GetUserByID(Int64 id);
        OE_UserAuthentications GetOE_UserAuthenticationsById(long Id);

        IEnumerable<OE_Users> GetUsersBySpecifiedValue(int value);        
        IEnumerable<GetOELicenses> GetOELicenses(Int64 actorId, Int64 institutionId);       
        IEnumerable<OE_Users> GetTeachersByInstitute(long instituteId);
        #endregion "Get Function Definitions"

        #region "Insert, delete and update Function Definitions"
        string InsertStaffLicense(InsertStaffLicenses insertStaffLicenses);       
        void InsertUser(OE_Users users);
        void InsertUserAuthentication(OE_UserAuthentications userAuthentications);

        string UpdateStaffLicense(UpdateStaffLicenses staffLicenses);
        void UpdateUser(OE_Users users, IFormFile fle, string webRootPath);        
        void UpdateUserAuthentication(OE_UserAuthentications userAuthentications);

        DeleteOELicenses DelateUserAuthentication(DeleteOELicenses obj);
        void DeleteProfleImg(OE_Users users, string webRootPath);

        #endregion"Insert, delete and update Function Definitions"

        #region "DropDown Function Definitions"
        IEnumerable<dropdown_Users> Dropdown_Users();
        #endregion "DropDown Function Definitions"        

    }
}
