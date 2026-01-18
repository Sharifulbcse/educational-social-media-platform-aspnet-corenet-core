
using System;
using System.Collections.Generic;

using OE.Data;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public interface IOE_UserAuthenticationsServ
    {
        #region "Get Function Definitions"
        bool IsAuthorized(long instituteId, long userId, long actorId);
        OE_UserAuthentications Get_UserAuthenticationsByUserActorInstituteId(long userId, long actorId, long instituteId);
        IEnumerable<OE_UserAuthentications> GetUserAuthenticationsByUserId(Int64 UserId);
        IEnumerable<OE_UserAuthentications> GetAuthenticUserByInstitute(long instituteId);
        #endregion "Get Function Definitions"

        #region "Insert Update Delete Function Definitions"
        void InsertUserAuthentication(OE_UserAuthentications userAuthentications);
        void DeleteUserAuthentication(OE_UserAuthentications userAuthentications);
        #endregion "Insert Update Delete Function Definitions"


    }
}