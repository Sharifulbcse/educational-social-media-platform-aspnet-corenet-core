using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using OE.Data;
using OE.Repo;
using OE.Service;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class OE_UsersServ : CommonServ, IOE_UsersServ
    {
        #region "Variables"   
        private readonly ICOM_GendersRepo<COM_Genders> _comGendersRepo;

        private readonly IEmployeesRepo<Employees> _employeesRepo;

        private readonly IOE_UsersRepo<OE_Users> _oeUsersRepo;
        private readonly IOE_ActorsRepo<OE_Actors> _oeActorsRepo;
        private readonly IOE_UserAuthenticationsRepo<OE_UserAuthentications> _oeUserAuthenticationsRepo;                    
              
        private readonly ICommonFunctionsServ _commonFunctionsServ;
        #endregion "Variables"

        #region "Constructor"

        public OE_UsersServ(
            ICommonFunctionsServ commonFunctionsServ,
            IOE_ActorsRepo<OE_Actors> oeActorsRepo,
            IOE_UserAuthenticationsRepo<OE_UserAuthentications> oeUserAuthenticationsRepo,
            IOE_UsersRepo<OE_Users> oeUsersRepo,
            IEmployeesRepo<Employees> employeesRepo,
            ICOM_GendersRepo<COM_Genders> comGendersRepo
            )
        {
            _commonFunctionsServ = commonFunctionsServ;
            _oeUserAuthenticationsRepo = oeUserAuthenticationsRepo;
            _oeUsersRepo = oeUsersRepo;
            _oeActorsRepo = oeActorsRepo;
            _employeesRepo = employeesRepo;
            _comGendersRepo = comGendersRepo;
        }

        #endregion "Constructor"

        #region "GET Methods"     
        public string GenerateRandomValue(int length)
        {
            return Comm_GetUniqueKey(length);
        }
        public string CheckEmailForForgetPassword(string userLoginId, string randomValue)
        {
            string returnResult = (dynamic)null;
            try
            {
                var userIsValid = _oeUsersRepo.GetAll().Where(u => u.UserLoginId == userLoginId).SingleOrDefault();
                if (userIsValid != null)
                {
                    //Company mail info defining.
                    string senderName = "OurEdu";
                    string senderMail = "taher279279279@gmail.com";
                    string senderPassword = "O181905021o";

                    //Mail server defining.
                    string smtpServer = "smtp.gmail.com";
                    int smtpPort = 587;
                    bool ssl = false;

                    //Sending mail defining.
                    string receiverName = "Account Recovery";
                    string subject = "Account Recovery Message";
                    string msgBody = "Please use this Security code to recover your password. Code is: " + randomValue;

                    if (Comm_SendMail(senderName, senderMail, senderPassword, receiverName, userLoginId, subject, msgBody, smtpServer, smtpPort, ssl).Equals(true))
                    {
                        returnResult = null;
                    }
                    else
                        returnResult = "User Login Id is invalid";
                }
                else
                    returnResult = "User Login Id is invalid";
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:ForgetPassword/ForgetPassword - " + ex.Message;
            }

            return returnResult;
        }
        public string ResetPassword(string userLoginId, string password)
        {
            string returnResult = (dynamic)null;
            try
            {
                var fetchRecord = _oeUsersRepo.GetAll().Where(u => u.UserLoginId == userLoginId).SingleOrDefault();
                if (fetchRecord != null)
                {
                    fetchRecord.Password = password;
                    _oeUsersRepo.Update(fetchRecord);
                }
                else
                {
                    returnResult = "User Login Id is invalid";
                }

            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:ForgetPassword/ResetPassword - " + ex.Message;
            }

            return returnResult;
        }
        public bool GeneratingMailVerification(string receiverMail, string randomValue)
        {
            //Company mail info defining.
            string senderName = "OurEdu";
            string senderMail = "taher279279279@gmail.com";
            string senderPassword = "O181905021o";

            //Mail server defining.
            string smtpServer = "smtp.gmail.com";
            int smtpPort = 587;
            bool ssl = false;

            //Sending mail defining.
            string receiverName = "Confirmation Message";
            string subject = "Security Varification Message";
            string msgBody = "Please use this Security code to varify your mail account. Code is: " + randomValue;

            if (Comm_SendMail(senderName, senderMail, senderPassword, receiverName, receiverMail, subject, msgBody, smtpServer, smtpPort, ssl).Equals(true))
            {
                return true;
            }
            else return false;
        }
               
        public OE_Users GetUserLogin(string userId, string password)
        {
            var queryAll = _oeUsersRepo.GetAll();

            var query = (from u in queryAll
                         where u.UserLoginId == userId
                         && u.Password == password
                         && u.IsActive == true
                         select u).SingleOrDefault();
            return query;
        }
        public OE_Users GetUserByEmail(string email)
        {
            var queryAll = _oeUsersRepo.GetAll();
            var query = from e in queryAll
                        where e.EmailAddress == email
                        select e;
            return query.FirstOrDefault();
        }        
        
       
        public bool IsSameActorInPerson(long instituteId, long userId, long actorId)
        {
            bool flagReturn = false;
            var userAuthtic = _oeUserAuthenticationsRepo.GetAll();
            var checkIsActorIsSame = (dynamic)null;
            if (instituteId != 0 && userId != 0 && actorId != 0)
            {
                checkIsActorIsSame = (from ua in userAuthtic
                                      where ua.InstitutionId == instituteId && ua.UserId == userId && ua.ActorId == actorId
                                      select ua).SingleOrDefault();
            }

            if (checkIsActorIsSame != null)
            {
                flagReturn = true;
            }
            return flagReturn;
        }

        public UsersWithGenders GetUserByID(Int64 id)
        {
            var queryAll = _oeUsersRepo.GetAll();
            var queryGender = _comGendersRepo.GetAll();

            var returnQuery = (dynamic)null;

            var jointQuery = from e in queryAll
                             where e.Id == id
                             join g in queryGender on e.GenderId equals g.Id
                             select new { e, g };

            foreach (var item in jointQuery)
            {
                returnQuery = new UsersWithGenders()
                {
                    Genders = item.g,
                    Users = item.e
                };
            }
           
            return returnQuery;
          
        }

        public OE_UserAuthentications GetOE_UserAuthenticationsById(long Id)
        {
            var queryAll = _oeUserAuthenticationsRepo.GetAll();

            var query = (from u in queryAll
                         where u.Id == Id
                         select u).FirstOrDefault();
            return query;
        }

        public IEnumerable<OE_Users> GetUsersBySpecifiedValue(int value)
        {
            var queryAll = _oeUsersRepo.GetAll();
            if (value == 1)
            {
                var query = from e in queryAll
                            where e.IsActive == true
                            select e;
                return query;
            }
            if (value == 2)
            {
                var query = from e in queryAll
                            where e.IsActive == false
                            select e;
                return query;
            }
            if (value == 0)
            {
                var queryUsers = from e in queryAll
                                 select e;
                return queryUsers;
            }
            else return queryAll;
        }
        
        public IEnumerable<GetOELicenses> GetOELicenses(Int64 actorId, Int64 institutionId)
         {

            var getUser = _oeUsersRepo.GetAll().ToList();
            var getUserAuth = _oeUserAuthenticationsRepo.GetAll().ToList();
            var getActors = _oeActorsRepo.GetAll().ToList();
            if (actorId == 2)
            {
                var jointquery = from ua in getUserAuth
                                 where ua.ActorId == 3 && ua.InstitutionId == institutionId //[NOTE: list for OE_Manager access(3=OE_Writer)]
                                 join u in getUser
                                 on ua.UserId equals u.Id
                                 join a in getActors
                                 on ua.ActorId equals a.Id
                                 select new { ua, u, a };


                var queryUserAuth = new List<GetOELicenses>();
                foreach (var item in jointquery)
                {
                    var obj = new GetOELicenses()
                    {
                        OEUserAuthentications = item.ua,
                        OEUsers = item.u,
                        OEActors = item.a
                    };
                    queryUserAuth.Add(obj);
                }
                return queryUserAuth;
            }

            else if (actorId == 11)
            {
                var jointquery = from ua in getUserAuth
                                 where (ua.InstitutionId == institutionId) && (ua.ActorId == 12 || ua.ActorId == 13) //[NOTE: list for Admin]
                                 join u in getUser
                                 on ua.UserId equals u.Id
                                 join a in getActors
                                 on ua.ActorId equals a.Id
                                 select new { ua, u, a };


                var queryUserAuth = new List<GetOELicenses>();
                foreach (var item in jointquery)
                {
                    var obj = new GetOELicenses()
                    {
                        OEUserAuthentications = item.ua,
                        OEUsers = item.u,
                        OEActors = item.a
                    };
                    queryUserAuth.Add(obj);
                }
                return queryUserAuth;
            }

            else if (actorId == 1)
            {
                var jointquery = from ua in getUserAuth
                                 where (ua.InstitutionId == institutionId) && (ua.ActorId == 2 || ua.ActorId == 3) //List access for OE_Admin
                                 join u in getUser
                                 on ua.UserId equals u.Id
                                 join a in getActors
                                 on ua.ActorId equals a.Id
                                 select new { ua, u, a };


                var queryUserAuth = new List<GetOELicenses>();
                foreach (var item in jointquery)
                {
                    var obj = new GetOELicenses()
                    {
                        OEUserAuthentications = item.ua,
                        OEUsers = item.u,
                        OEActors = item.a
                    };
                    queryUserAuth.Add(obj);
                }
                return queryUserAuth;
            }

            else
            {
                var jointquery = from ua in getUserAuth
                                 where ua.InstitutionId == institutionId && ua.ActorId == 3 //List access for OE_Admin
                                 join u in getUser
                                 on ua.UserId equals u.Id
                                 join a in getActors
                                 on ua.ActorId equals a.Id
                                 select new { ua, u, a };


                var queryUserAuth = new List<GetOELicenses>();
                foreach (var item in jointquery)
                {
                    var obj = new GetOELicenses()
                    {
                        OEUserAuthentications = item.ua,
                        OEUsers = item.u,
                        OEActors = item.a
                    };
                    queryUserAuth.Add(obj);
                }
                return queryUserAuth;
            }

        }

        public IEnumerable<OE_Users> GetTeachersByInstitute(long instituteId)
        {
            var usr = _oeUsersRepo.GetAll();
            var usrAthn = _oeUserAuthenticationsRepo.GetAll();

            //[Note: Fetch data base on teacher and institute Id]
            var returnQry = from u in usr
                            join ua in usrAthn
                            on u.Id equals ua.UserId
                            where ua.InstitutionId == instituteId && ua.ActorId == 14 // [14 = Teacher]
                            select u;
            return returnQry;
        }

        #endregion "GET Methods"

        #region "Insert, Delete and Update Methods"

        public string InsertStaffLicense(InsertStaffLicenses staffLicenses)
        {
            var returnResult = (dynamic)null;
            //[NOTE: Checking UserLoginId is available or not]
            Boolean IsExistAsEmployee = _commonFunctionsServ.Function_OEUsers_IsUserAsEmployee(staffLicenses.UserLoginId);

            if (IsExistAsEmployee == false)
            {
                staffLicenses.Message = "OurEdu Id is invalid.";
                return returnResult = staffLicenses.Message;
            }
            if (IsExistAsEmployee == true)
            {
                var oeU = _oeUsersRepo.GetAll();
                var oeUser = (from u in oeU
                              where u.UserLoginId == staffLicenses.UserLoginId
                              select u).FirstOrDefault();
                //[NOTE: Checking User Authentication is already available or not based on UserId, ActorId and InstitutionId]
                if (!IsSameActorInPerson(staffLicenses.OE_UserAuthentications.InstitutionId, oeUser.Id, staffLicenses.OE_UserAuthentications.ActorId))
                {
                    staffLicenses.OE_UserAuthentications.UserId = oeUser.Id;
                    _oeUserAuthenticationsRepo.Insert(staffLicenses.OE_UserAuthentications);
                    staffLicenses.Message = (dynamic)null;
                    return returnResult = staffLicenses.Message;
                }
                else
                {
                    staffLicenses.Message = "This record is already exist.";
                    returnResult = staffLicenses.Message;
                }

            }
            return returnResult;
        }

        public void InsertUser(OE_Users users)
        {
            _oeUsersRepo.Insert(users);

        }        
        public void InsertUserAuthentication(OE_UserAuthentications userAuthentications)
        {
            _oeUserAuthenticationsRepo.Insert(userAuthentications);
        }

        public string UpdateStaffLicense(UpdateStaffLicenses staffLicenses)
        {
            var returnResult = (dynamic)null;
            //[NOTE: Checking UserLoginId is available or not]  
            Boolean IsExistAsEmployee = _commonFunctionsServ.Function_OEUsers_IsUserAsEmployee(staffLicenses.SelectedUserLoginId);
            if (IsExistAsEmployee == false)            
            {
                staffLicenses.Message = "OurEdu Id is invalid.";
                return returnResult = staffLicenses.Message;
            }            
            if (IsExistAsEmployee == true)
            {
                var oeU = _oeUsersRepo.GetAll();
                var oeUser = (from u in oeU
                              where u.UserLoginId == staffLicenses.SelectedUserLoginId
                              select u).FirstOrDefault();
                
                //[NOTE: Checking selected UserLoginId and selected Actor Id are same, if so than, only update without userId and actorId]
                if (staffLicenses.OE_UserAuthentications.ActorId == staffLicenses.SelectedActorId && staffLicenses.OE_UserAuthentications.UserId == oeUser.Id)
                {
                    _oeUserAuthenticationsRepo.Update(staffLicenses.OE_UserAuthentications);
                    return returnResult = null;
                }
                //[NOTE: if selected UserLoginId and selected Actor Id are different, again check user authentication is already available or not based on UserId, ActorId and InstitutionId]
                else if (!IsSameActorInPerson(staffLicenses.OE_UserAuthentications.InstitutionId, oeUser.Id, staffLicenses.SelectedActorId))
                {
                    staffLicenses.OE_UserAuthentications.UserId = oeUser.Id;
                    staffLicenses.OE_UserAuthentications.ActorId = staffLicenses.SelectedActorId;
                    _oeUserAuthenticationsRepo.Update(staffLicenses.OE_UserAuthentications);
                   return returnResult = null;
                }
                else
                {
                    staffLicenses.Message = "This record is already exist.";
                    returnResult = staffLicenses.Message;
                }

            }            
            return returnResult;
        }
        public void UpdateUserAuthentication(OE_UserAuthentications userAuthentications)
        {
            _oeUserAuthenticationsRepo.Update(userAuthentications);
        }
        public void UpdateUser(OE_Users users, IFormFile fle, string webRootPath)
        {
            if (users.Password == null)
            {
                var queryAll = _oeUsersRepo.GetAll().ToList();
                var query = (from u in queryAll
                             where u.Id == users.Id
                             select u).SingleOrDefault();

                if (users.FirstName != null && users.LastName != null)
                {
                    query.FirstName = users.FirstName;
                    query.LastName = users.LastName;

                    _oeUsersRepo.Update(query);
                }
                else if (users.DateOfBirth != null)
                {
                    query.DateOfBirth = users.DateOfBirth;
                    _oeUsersRepo.Update(query);
                }
               
                else if (users.GenderId != 0)
                {
                    query.GenderId = users.GenderId;
                    _oeUsersRepo.Update(query);
                }
              

                else if (users.EmailAddress != null)
                {
                    query.EmailAddress = users.EmailAddress;
                    _oeUsersRepo.Update(query);
                }
                else if (users.ContactNo != null)
                {
                    query.ContactNo = users.ContactNo;
                    _oeUsersRepo.Update(query);
                }


                else if (fle != null)
                {
                    string imagePath600X400 = "ClientDictionary/Users/IP600X400/";
                    string imagePath300X200 = "ClientDictionary/Users/IP300X200/";
                    if (Comm_ImageFormat(users.Id.ToString(), fle, webRootPath, imagePath600X400, 600, 400, ".jpg").Equals(true))
                    {
                        //[NOTE:Update image file]
                        query.IP600X400 = imagePath600X400 + users.Id + ".jpg";
                        _oeUsersRepo.Update(query);
                    }

                    if (Comm_ImageFormat(users.Id.ToString(), fle, webRootPath, imagePath300X200, 300, 200, ".jpg").Equals(true))
                    {
                        //[NOTE:Update image file]
                        query.IP300X200 = imagePath300X200 + users.Id + ".jpg";
                        _oeUsersRepo.Update(query);
                    }

                }

                else
                {
                    _oeUsersRepo.Update(users);
                }
            }

            else
            {
                _oeUsersRepo.Update(users);
            }

        }

        public DeleteOELicenses DelateUserAuthentication(DeleteOELicenses obj)
        
        {
            var returnModel = new DeleteOELicenses();
            
            try
            {
                if (obj.Id > 0)                
                {
                   
                    var userLicense = _oeUserAuthenticationsRepo.Get(obj.Id);
                
                    if (userLicense != null)
                    {
                        _oeUserAuthenticationsRepo.Delete(userLicense);
                    
                        returnModel.Message = "Delete Successful.";
                        returnModel.SuccessIndicator = true;                    
                    }
                }
            }
            catch (Exception ex)
            {
               
                if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    returnModel.Message = "Record is not possible to delete, because it used in other places.";
                    returnModel.SuccessIndicator = false;
                }
                else
                {
                    returnModel.Message = "ERROR102:OE_UsersServ/DelateUserAuthentication - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }
                
            }
           
            return returnModel;
           


        }
        public void DeleteProfleImg(OE_Users users, string webRootPath)
        {

            DelFileFromLocation(Path.Combine(webRootPath, users.IP300X200));
            DelFileFromLocation(Path.Combine(webRootPath, users.IP600X400));

            users.IP300X200 = null;
            users.IP600X400 = null;

            _oeUsersRepo.Update(users);
        }
        
        #endregion "Insert, Delete and Update Methods"        

        #region "Dropdown Methods"
        public IEnumerable<dropdown_Users> Dropdown_Users()
        {

            var getUser = _oeUsersRepo.GetAll().ToList();
            var queryResult = new List<dropdown_Users>();

            foreach (var item in getUser)
            {
                var u = new dropdown_Users()
                {
                    Id = item.Id,
                    Name = item.UserLoginId
                };
                queryResult.Add(u);
            }

            return queryResult;

        }
        #endregion "Dropdown Methods"
        
        
    }
}
