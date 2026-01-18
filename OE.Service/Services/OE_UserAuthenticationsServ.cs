using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class OE_UserAuthenticationsServ : IOE_UserAuthenticationsServ
    {
        #region "Variables"
        private IOE_UserAuthenticationsRepo<OE_UserAuthentications> _oeUserAuthenticationsRepo;
        private IOE_ActorsRepo<OE_Actors> _oeActorsRepo;
        #endregion "Variables"

        #region "Constructor"
        public OE_UserAuthenticationsServ
            (
            IOE_UserAuthenticationsRepo<OE_UserAuthentications> oeUserAuthenticationsRepo,
            IOE_ActorsRepo<OE_Actors> oeActorsRepo
            )
        {
            _oeUserAuthenticationsRepo = oeUserAuthenticationsRepo;
            _oeActorsRepo = oeActorsRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
       
        public bool IsAuthorized(long instituteId, long userId, long actorId)
        {            
            var flagReturn = false;
            try
            {
                var userAuthtic = _oeUserAuthenticationsRepo.GetAll();
                var checkIsAuthorized = (dynamic)null;
                checkIsAuthorized = (from ua in userAuthtic
                                     where ua.InstitutionId == instituteId && ua.UserId == userId && ua.ActorId == actorId
                                     select ua.IsActive).SingleOrDefault();
                flagReturn = checkIsAuthorized;

            }
            catch (Exception)
            {
                flagReturn = false;
            }
            return flagReturn;
            
        }
        public OE_UserAuthentications Get_UserAuthenticationsByUserActorInstituteId(long userId, long actorId, long instituteId)
        {
            var list = _oeUserAuthenticationsRepo.GetAll();
            var returnQry = (from au in list
                             where au.UserId == userId && au.ActorId == actorId && au.InstitutionId == instituteId
                             select au).FirstOrDefault();
            return returnQry;
        }

        public IEnumerable<OE_UserAuthentications> GetUserAuthenticationsByUserId(Int64 userId)
        {
            var userAuthenticationQuery = _oeUserAuthenticationsRepo.GetAll().ToList();
            var ActorsQuery = _oeActorsRepo.GetAll().ToList();
            var query = (from ua in userAuthenticationQuery
                         join a in ActorsQuery on ua.ActorId equals a.Id
                         where ua.UserId == userId
                         && ua.IsActive == true
                         orderby a.OrderNo ascending                        
                         select new OE_UserAuthentications { ActorId = ua.ActorId, InstitutionId = ua.InstitutionId });
           return query;
        }
        public IEnumerable<OE_UserAuthentications> GetAuthenticUserByInstitute(long instituteId)
        {
            var list = _oeUserAuthenticationsRepo.GetAll();
            var returnQry = from au in list
                            where au.InstitutionId == instituteId
                            select au;
            return returnQry;
        }

         #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public void InsertUserAuthentication(OE_UserAuthentications userAuthentications)
        {
            _oeUserAuthenticationsRepo.Insert(userAuthentications);
        }
        public void DeleteUserAuthentication(OE_UserAuthentications userAuthentications)
        {
            _oeUserAuthenticationsRepo.Delete(userAuthentications);
        }
        #endregion "Insert Update Delete Methods"
        


    }
}
