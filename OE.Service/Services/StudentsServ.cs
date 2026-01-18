using System;
using System.Linq;
using System.IO; //[NOTE: for file path]
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;
using OE.Service.CustomEntitiesServ;
namespace OE.Service
{
    public class StudentsServ :CommonServ, IStudentsServ,ICommonServ
    {
        #region "Variables"
        private readonly IClassesRepo<Classes> _classesRepo;
        private readonly ICOM_GendersRepo<COM_Genders> _comGendersRepo;

        private readonly IOE_UsersRepo<OE_Users> _oeUsersRepo;
        private readonly IOE_UserAuthenticationsRepo<OE_UserAuthentications> _oeUserAuthenticationsRepo;
        private readonly IOE_InstitutionsRepo<OE_Institutions> _oeInstitutionsRepo;

        private readonly IRegistrationItemsRepo<RegistrationItems> _registrationItemsRepo;
        private readonly IRegistrationGroupsRepo<RegistrationGroups> _registrationGroupsRepo;

        private readonly IStudentsRepo<Students> _studentsRepo;
        private readonly IStudentDetailsRepo<StudentDetails> _studentDetailsRepo;
        private readonly IStudentPromotionsRepo<StudentPromotions> _studentPromotionsRepo;

        private readonly ICommonFunctionsServ _commonFunctionsServ;

        #endregion "Variables"

        #region "Constructor"

        public StudentsServ(
            IOE_UsersRepo<OE_Users> oeUsersRepo,
            IStudentsRepo<Students> studentsRepo,
            IClassesRepo<Classes> classesRepo,
            ICOM_GendersRepo<COM_Genders> comGendersRepo,
            IStudentDetailsRepo<StudentDetails> studentDetailsRepo, 
            IRegistrationItemsRepo<RegistrationItems> registrationItemsRepo,
            IRegistrationGroupsRepo<RegistrationGroups> registrationGroupsRepo,
            IStudentPromotionsRepo<StudentPromotions> studentPromotionsRepo,
            IOE_UserAuthenticationsRepo<OE_UserAuthentications> oeUserAuthenticationsRepo,
            IOE_InstitutionsRepo<OE_Institutions> oeInstitutionsRepo,
             ICommonFunctionsServ commonFunctionsServ
            )
        {
            _studentsRepo = studentsRepo;
            _classesRepo = classesRepo;
            _comGendersRepo = comGendersRepo;
            _studentDetailsRepo = studentDetailsRepo;
            _registrationItemsRepo = registrationItemsRepo;
            _registrationGroupsRepo = registrationGroupsRepo;
            _studentPromotionsRepo = studentPromotionsRepo;
            _oeUsersRepo = oeUsersRepo;
            _oeUserAuthenticationsRepo = oeUserAuthenticationsRepo;
            _oeInstitutionsRepo = oeInstitutionsRepo;
            _commonFunctionsServ = commonFunctionsServ;
        }
        #endregion "Constructor"

        #region Get Methods"
        
        public string CheckValidUserLoginIdAndRoll(string userLoginId, long institutionId, string oldUserLoginId, long admittedYear, long admittedPYear, long admittedClassId, long ddlClassId, long rollNo, long oldRollNo)
        {
            string returnMsg = (dynamic)null;
            var user = _oeUsersRepo.GetAll();

            //[NOTE: Checking Roll No is existing or not]
            if (admittedPYear == admittedYear && admittedClassId == ddlClassId && rollNo == oldRollNo)
            {
                returnMsg = null;
            }
            else
            {
                if (_commonFunctionsServ.Function_IsStudentHasExistingRollNo(ddlClassId, admittedYear, rollNo) == true)
                {
                    return returnMsg = "Roll No is already used for this class.....";
                }
                else
                    returnMsg = null;
            }

            //[NOTE: Checking UserId is available for OE_Users table]           
            if (userLoginId == null || userLoginId == oldUserLoginId)
            {
                return returnMsg;
            }

            var CheckUserIsAvaliable = from u in user
                                       where u.UserLoginId == userLoginId
                                       select u;
            if (CheckUserIsAvaliable.Count() != 0)
            {
                if (_commonFunctionsServ.Function_IsStudentHasOeId(CheckUserIsAvaliable.SingleOrDefault().Id, institutionId) == true)
                {
                    return returnMsg = "OurEdu Id is being used by another student.";
                }
                else

                    return returnMsg;
            }
            else return returnMsg = "UserId is invalid.";
        }

        public IEnumerable<Students> GetStudents()
        {
            //[NOTE: get all departments]
            var queryAll = _studentsRepo.GetAll();
            var queryStudents = from e in queryAll
                                select e;
            return queryStudents;
        }
       
        public IEnumerable<StudentLicensesValidation> StudentLicensesValidation(List<StudentLicensesValidation> obj)
        {
            string msg = null;
            if (obj != null)
            {
                foreach (var item in obj)
                {
                    var getUsers = _oeUsersRepo.GetAll();
                    //[NOTE: Check UserLoginId available or not]
                    var userHasloginId = (from u in getUsers
                                          where u.UserLoginId == item.SelectedUserLoginId
                                          select u).SingleOrDefault();
                    if (userHasloginId != null)
                    {
                        var getStudents = _studentsRepo.GetAll();
                        var userIdExist = (from s in getStudents
                                           where s.UserId == userHasloginId.Id && s.InstitutionId == item.InstitutionId
                                           select s).SingleOrDefault();
                        if (userIdExist != null)
                        {
                            msg = "UserLoginId already used.";
                        }
                        else
                        {
                            msg = "UserLoginId can be use.";
                        }
                    }
                    else
                    {
                        msg = "UserLoginId is invalid.";
                    }
                    item.Message = msg;
                }
            }
            return obj;
        }
        
        public Students GetStudentsById(long id, long instituteId, long userId)
        {
            var student = _studentsRepo.GetAll();
            var returnQry = (dynamic)null;
            if (userId == 0)
            {
                returnQry = (from s in student
                             where s.Id == id && s.InstitutionId == instituteId
                             select s).SingleOrDefault();
            }
            else if (userId != 0)
            {
                returnQry = (from s in student
                             where s.InstitutionId == instituteId && s.UserId == userId
                             select s).SingleOrDefault();
            }
            return returnQry;
        }
        public GetStudentLicenses GetStudentLicenses(int year, long institutionId, int ddlLicenseId, long classId)
        {
            var institution = _oeInstitutionsRepo.Get(institutionId);
            var students = _studentsRepo.GetAll();
            var classes = _classesRepo.GetAll();
            var licenses = _oeUserAuthenticationsRepo.GetAll().Where(u => u.ActorId == 10);
            var users = _oeUsersRepo.GetAll();
            var stuPromotions = _studentPromotionsRepo.GetAll();
            
            var searchRecords = from stuPro in stuPromotions
                                join stu in students on stuPro.StudentId equals stu.Id
                                where stuPro.Year.Year == year && stuPro.InstitutionId == institutionId
                                join cls in classes on stuPro.ClassId equals cls.Id
                                join license in licenses on stu.UserId equals license.UserId
                                into AllCOLUMNS
                                from license in AllCOLUMNS.DefaultIfEmpty()
                                select new { stuPro, stu, cls, license };
            //[Work will be start from here]
            if (classId != 0)
            {
                searchRecords = from student in searchRecords
                                where student.stuPro.ClassId == classId
                                select new { student.stuPro, student.stu, student.cls, student.license };
            }
            if (ddlLicenseId == 1)
            {
                searchRecords = from student in searchRecords
                                where student.license == null
                                select new { student.stuPro, student.stu, student.cls, student.license };
            }
            if (ddlLicenseId == 2)
            {
                searchRecords = from student in searchRecords
                                where student.license != null
                                select new { student.stuPro, student.stu, student.cls, student.license };
            }
            var lstofStudent = new List<C_Students>();

            foreach (var item in searchRecords)
            {
                var sp = new C_Students()
                {
                    Id = item.stu.Id,

                    Name = item.stu.Name,
                    UserId = item.stu.UserId,
                    UserLoginId = item.stu.UserId != null ? _oeUsersRepo.Get(Convert.ToInt64(item.stu.UserId)).UserLoginId : "",
                    StudentPromotionClassId = item.stuPro.ClassId,
                    ClassName = item.cls.Name,
                    RollNo = item.stuPro.RollNo,
                    IsActive = item.license == null ? false : item.license.IsActive,
                }; lstofStudent.Add(sp);
            }
            var returnQry = new GetStudentLicenses()
            {
                InstitutionName = institution.Name,
                Students = lstofStudent
            };
            return returnQry;
        }


        public GetStudentDetails GetStudentDetails(long institutionId, long studentId)
        {
            var returnModel = new GetStudentDetails();
            try
            {
                if (studentId > 0)
                {
                    var students = _studentsRepo.GetAll().Where(s => s.Id == studentId && s.InstitutionId == institutionId).SingleOrDefault();
                    var stdPromotion = _studentPromotionsRepo.GetAll().Where(p => p.StudentId == studentId && p.InstitutionId == institutionId && p.Year.Year == CommDate_CurrentYear()).SingleOrDefault();
                    var gender = _comGendersRepo.GetAll().Where(g => g.Id == students.GenderId).SingleOrDefault();
                    var classs = _classesRepo.GetAll().Where(c => c.Id == stdPromotion?.ClassId).SingleOrDefault();
                    var users = _oeUsersRepo.GetAll().Where(u => u.Id == students?.UserId).SingleOrDefault();

                    var regGroupItems = _registrationGroupsRepo.GetAll().Where(rg => rg.InstitutionId == institutionId && rg.RegistrationUserTypeId == 1 && rg.IsActive == true).ToList();
                    var regItems = _registrationItemsRepo.GetAll().Where(r => r.InstitutionId == institutionId && r.RegistrationUserTypeId == 1 && r.IsActive == true).ToList();
                    var studentDetails = _studentDetailsRepo.GetAll().Where(sd => sd.StudentId == studentId && sd.InstitutionId == institutionId).ToList();
                    var regItemWithStudentDetails = from sd in studentDetails
                                                    join ri in regItems on sd.RegistrationItemId equals ri.Id
                                                    where ri.RegistrationUserTypeId == 1 && ri.IsActive == true
                                                    select new { sd, ri };

                    var lstRegItemWithSD = new List<C_StudentDetails>();
                    foreach (var item in regItemWithStudentDetails)
                    {
                        var sd = new C_StudentDetails()
                        {
                            Id = item.sd.Id,
                            StudentId = item.sd.StudentId,
                            RegistrationItemId = item.sd.RegistrationItemId,
                            StringValue = item.sd.StringValue,
                            WholeValue = item.sd.WholeValue,
                            FloatValue = item.sd.FloatValue,
                            DateValue = item.sd.DateValue,
                            FilePathValue = item.sd.FilePathValue,
                            ImagePathValue = item.sd.ImagePathValue,
                            BitValue = item.sd.BitValue,
                            TextAreaValue = item.sd.TextAreaValue,
                            IsActive = item.sd.IsActive,

                            RegistrationItemName = item.ri.Name,
                            RegistrationGroupId = item.ri.RegistrationGroupId,
                            RegistrationItemTypeId = item.ri.RegistrationItemTypeId,
                        };
                        lstRegItemWithSD.Add(sd);
                    }

                    returnModel.Students = students;
                    returnModel.StudentPromotions = stdPromotion;
                    returnModel.Genders = gender;
                    returnModel.Classes = classs;
                    returnModel.Users = users;
                    returnModel._RegistrationGroups = regGroupItems;
                    returnModel._RegistrationItems = regItems;
                    returnModel._StudentDetails = lstRegItemWithSD;
                    returnModel.Message = "";
                }
            }
            catch (Exception ex)
            {
                returnModel.Message = "ERROR102:StudentServ/GetStudentDetails - " + ex.Message;
                return returnModel;
            }
            return returnModel;            
        }
        public StudentPromotions GetStudentPromotionsById(long Id)
        {
            var queryAll = _studentPromotionsRepo.GetAll();

            var query = (from sdcw in queryAll
                         where sdcw.Id == Id
                         select sdcw).FirstOrDefault();
            return query;
        }
        public GetStudentList GetStudentList(long institutionId, int year, long classId)
        {
            var students = _studentsRepo.GetAll();
            var stdPromotion = _studentPromotionsRepo.GetAll();
            var gender = _comGendersRepo.GetAll();
            var users = _oeUsersRepo.GetAll();
            var classs = _classesRepo.GetAll();
            var jointQry = (dynamic)null;
            if (classId == 0)
            {
                jointQry = from s in students
                           join g in gender on s.GenderId equals g.Id
                           join c in classs on s.ClassId equals c.Id
                           where s.InstitutionId == institutionId && s.AdmittedYear.Year == year
                           join sp in stdPromotion on s.Id equals sp.StudentId
                           where sp.Year.Year == year
                           join u in users on s.UserId equals u.Id into ALLCOLUMNS
                           from user in ALLCOLUMNS.DefaultIfEmpty()                           
                           select new { s, g, c, sp, user };
                

            }
            else
            {
                jointQry = from s in students
                           join g in gender on s.GenderId equals g.Id
                           join c in classs on s.ClassId equals c.Id
                           where s.InstitutionId == institutionId && s.ClassId == classId && s.AdmittedYear.Year == year
                           join sp in stdPromotion on s.Id equals sp.StudentId
                           where sp.ClassId == classId && sp.Year.Year == year
                           join u in users on s.UserId equals u.Id into ALLCOLUMNS
                           from user in ALLCOLUMNS.DefaultIfEmpty()                               
                           select new { s, g, c, sp, user };
                

            }

            var listStudent = new List<C_Students>();
            foreach (var item in jointQry)
            {
                var temp = new C_Students()
                {
                    Id = item.s.Id,
                    InstitutionId = item.s.InstitutionId,
                    ClassId = item.s.ClassId,
                    GenderId = item.s.GenderId,
                    Name = item.s.Name,
                    IP300X200 = item.s.IP300X200,
                    PresentAddress = item.s.PresentAddress,
                    PermanentAddress = item.s.PermanentAddress,
                    UserId = item.s.UserId,
                    DOB = (dynamic)CommDate_ConvertToLocalDate(item.s.DOB),
                    AdmittedYear = (dynamic)CommDate_ConvertToLocalDate(item.s.AdmittedYear),
                    ClassName = item.c?.Name,
                    RollNo = item.sp?.RollNo,
                    UserLoginId = item.user?.UserLoginId,
                    UsersIP300X200 = item.user?.IP300X200,
                    GenderName = item.g?.Name
                };
                listStudent.Add(temp);
            }
            var returnQry = new GetStudentList()
            {
                _Students = listStudent
            };
            return returnQry;
        }
        public IEnumerable<GetStudentPromotions> GetStudentPromotions(long institutionId, long fromYear, long classId)
        {
            var studentPromotions = _studentPromotionsRepo.GetAll().ToList();
            var students = _studentsRepo.GetAll().ToList();
            var classes = _classesRepo.GetAll().ToList();

            var jointQuery = from sdcw in studentPromotions
                             where sdcw.InstitutionId == institutionId && sdcw.Year.Year == fromYear && sdcw.ClassId == classId
                             join s in students
                             on sdcw.StudentId equals s.Id
                             join c in classes
                             on sdcw.ClassId equals c.Id
                             select new { sdcw, s, c };

            var queryResult = new List<GetStudentPromotions>();
            foreach (var item in jointQuery)
            {
                var obj = new GetStudentPromotions()
                {
                    StudentPromotions = item.sdcw,
                    Students = item.s,
                    Classes = item.c
                };
                queryResult.Add(obj);
            }

            return queryResult;
        }
        
        public IEnumerable<GetStudentPromotions> PromotedStudents(long institutionId, int year)
        {
            var studentPromotions = _studentPromotionsRepo.GetAll().ToList();
            var students = _studentsRepo.GetAll().ToList();
            var classes = _classesRepo.GetAll().ToList();
            var jointQuery = (dynamic)null;

            if (year == 0)
            {
                jointQuery = from sdcw in studentPromotions
                             where sdcw.InstitutionId == institutionId
                             orderby sdcw.Year descending
                             orderby sdcw.ClassId ascending
                             join s in students
                             on sdcw.StudentId equals s.Id
                             join c in classes
                             on sdcw.ClassId equals c.Id
                             select new { sdcw, s, c };
            }
            else
            {
                jointQuery = from sdcw in studentPromotions
                             where sdcw.InstitutionId == institutionId && sdcw.Year.Year == year
                             orderby sdcw.Year descending
                             orderby sdcw.ClassId ascending
                             join s in students
                             on sdcw.StudentId equals s.Id
                             join c in classes
                             on sdcw.ClassId equals c.Id
                             select new { sdcw, s, c };
            }
            var queryResult = new List<GetStudentPromotions>();
            foreach (var item in jointQuery)
            {
                var obj = new GetStudentPromotions()
                {
                    StudentPromotions = item.sdcw,
                    Students = item.s,
                    Classes = item.c
                };
                queryResult.Add(obj);
            }

            return queryResult;
        }

        #endregion "Get Methods"

        #region "Insert Update Delete Methods"        
        public string InsertStudents(InsertStudents obj, IFormFile file, string webRootPath)
        {
            var returnResult = (dynamic)null;
            var currentIP300X200 = (dynamic)null;
            long newStudentId = _studentsRepo.GetLastId() + 1;
            
            if (obj.Students != null)
            {
                //[NOTE:add image file]                   
                if (file != null)
                {
                    var addedImagePath = "ClientDictionary/Students/IP300X200/";

                    string ext = Path.GetExtension(file.FileName);
                    ext = "." + ext.Remove(0, 1);

                    currentIP300X200 = addedImagePath + newStudentId + ext;
                    var result = Comm_ImageFormat(newStudentId.ToString(), file, webRootPath, addedImagePath, 300, 200, ext);
                   
                    if (result == false)
                    {
                        return returnResult = "Image is not saved";
                    }
                }

                var stu = new Students()
                {
                    InstitutionId = obj.Students.InstitutionId,
                    UserId = (obj.Students.UserId != null) ? obj.Students.UserId : (dynamic)null,
                    ClassId = obj.Students.ClassId,
                    GenderId = obj.Students.GenderId,
                    Name = obj.Students.Name,
                    IP300X200 = currentIP300X200,
                    AdmittedYear = DateTime.Now,
                    PresentAddress = obj.Students.PresentAddress,
                    PermanentAddress = obj.Students.PermanentAddress,
                    DOB = obj.Students.DOB,
                    IsActive = true,
                    AddedBy = obj.Students.AddedBy,
                    AddedDate = CommDate_ConvertToUtcDate(DateTime.Now.Date),
                    InsId = obj.Students.InstitutionId
                };
                if (obj.OeUsers.UserLoginId != null)
                {
                    if (_commonFunctionsServ.Function_OeUserHasUserLoginId(obj.OeUsers.UserLoginId) == true)
                    {
                        var getUserId = _oeUsersRepo.GetAll().Where(u => u.UserLoginId == obj.OeUsers.UserLoginId).SingleOrDefault();

                        if (_commonFunctionsServ.Function_IsStudentHasOeId(getUserId.Id, obj.CurrentInstitutionId) == true)
                        {
                            obj.Message = "OurEdu Id is being used by another student.";
                            return returnResult = obj.Message;
                        }
                        else
                        {
                            stu.UserId = getUserId.Id;
                        }
                    }
                    else
                    {
                        obj.Message = "OurEdu Id is invalid.";
                        return returnResult = obj.Message;
                    }
                }
                _studentsRepo.Insert(stu);

                if (obj.StudentDetails != null)
                {
                    foreach (var item in obj.StudentDetails)
                    {
                        var studentDetails = new StudentDetails();
                        studentDetails.InstitutionId = item.InstitutionId;
                        studentDetails.StudentId = newStudentId;
                        
                        studentDetails.RegistrationItemId = item.RegistrationItemId;
                        studentDetails.IsActive = true;
                        studentDetails.AddedBy = item.AddedBy;
                        studentDetails.AddedDate = CommDate_ConvertToUtcDate(Convert.ToDateTime(DateTime.Now));


                        switch (item.RegistrationItemTypeId)
                        {
                            case 1:
                                {                                    
                                    var lastStudentDetailsId = _studentDetailsRepo.GetAll().Count() == 0 ? 1 : _studentDetailsRepo.GetAll().Last().Id + 1;
                                    string pathString = "ClientDictionary/StudentDetails/FilePathValue/";
                                    string fileExtension = Path.GetExtension(item.ActualFile.FileName);
                                    fileExtension = fileExtension.Remove(0, 1);
                                    Comm_FileSave(lastStudentDetailsId, item.ActualFile, webRootPath, pathString, fileExtension);
                                    studentDetails.FilePathValue = pathString + lastStudentDetailsId + "." + fileExtension;
                                    break;
                                }
                            case 2:
                                {                                    
                                    var lastStudentDetailsId = _studentDetailsRepo.GetAll().Count() == 0 ? 1 : _studentDetailsRepo.GetAll().Last().Id + 1;
                                    string pathString = "ClientDictionary/StudentDetails/ImagePathValue/";
                                    string imageExtension = Path.GetExtension(item.ActualImage.FileName);
                                    imageExtension = imageExtension.Remove(0, 1);
                                    Comm_FileSave(lastStudentDetailsId, item.ActualImage, webRootPath, pathString, imageExtension);
                                    studentDetails.ImagePathValue = pathString + lastStudentDetailsId + "." + imageExtension;
                                    break;
                                }
                            case 3:
                                {
                                    studentDetails.BitValue = item.BitValue;
                                    break;
                                }
                            case 4:
                                {
                                    studentDetails.StringValue = item.StringValue;
                                    break;
                                }
                            case 5:
                                {
                                    studentDetails.WholeValue = item.WholeValue;
                                    break;
                                }
                            case 6:
                                {
                                    studentDetails.FloatValue = item.FloatValue;
                                    break;
                                }
                            case 7:
                                {
                                    studentDetails.TextAreaValue = item.TextAreaValue;
                                    break;
                                }
                            case 8:
                                {
                                    studentDetails.DateValue = CommDate_ConvertToUtcDate(Convert.ToDateTime(item.DateValue));
                                    break;
                                }
                            default:
                                {
                                    break;
                                }
                        }
                        _studentDetailsRepo.Insert(studentDetails);
                    }
                }

                //[Note: insert 'StudentPromotions' table]
                if (_commonFunctionsServ.Function_IsStudentHasExistingRollNo(obj.Students.ClassId, obj.Students.AdmittedYear.Year, obj.RollNo) == true)
                {
                    obj.Message = "Roll No is already used for this class.....";
                    return returnResult = obj.Message;
                }
                else
                {
                    var stuPromotions = new StudentPromotions()
                    {
                        InstitutionId = obj.Students.InstitutionId,
                        StudentId = (long)(_studentsRepo.GetLastId()),
                        ClassId = obj.Students.ClassId,
                        RollNo = obj.RollNo,
                        IsActive = true,
                        Year = obj.Students.AdmittedYear,
                        AddedBy = obj.Students.AddedBy,
                        AddedDate = CommDate_ConvertToUtcDate(DateTime.Now.Date),
                    };
                    _studentPromotionsRepo.Insert(stuPromotions);
                }

            }
            return returnResult;
        }
        public void InsertStudentPromotions(InsertStudentPromotions obj)
       {
            if (obj.StudentPromotions != null)
            {
                foreach (var item in obj.StudentPromotions)
                {
                    //[NOTE: Check Student already promoted or not?]
                    var isPromoted = _studentPromotionsRepo.GetAll().Where(p => p.StudentId == item.StudentId && p.InstitutionId == item.InstitutionId && p.ClassId == item.ClassId && p.Year.Year == item.Year.Year).SingleOrDefault();
                    if (isPromoted == null)
                    {
                        var promotion = new StudentPromotions()
                        {
                            InstitutionId = item.InstitutionId,
                            StudentId = item.StudentId,
                            ClassId = item.ClassId,
                            RollNo = item.RollNo,
                            Year = item.Year,
                            IsActive = item.IsActive,
                            AddedBy = item.AddedBy,
                            AddedDate = item.AddedDate,
                            InsId = item.InstitutionId
                        };
                        _studentPromotionsRepo.Insert(promotion);
                    }
                    else
                    {
                        var updatePromotedStudent = _studentPromotionsRepo.Get(isPromoted.Id);
                        updatePromotedStudent.RollNo = item.RollNo;
                        updatePromotedStudent.IsActive = item.IsActive;
                        updatePromotedStudent.ModifiedBy = item.ModifiedBy;
                        updatePromotedStudent.ModifiedDate = item.ModifiedDate;

                        _studentPromotionsRepo.Update(updatePromotedStudent);
                    }
                }

            }
        }

        public InsertUpdateStudentLicenses InsertUpdateStudentLicenses(InsertUpdateStudentLicenses obj)
        {
            var model = new InsertUpdateStudentLicenses();

            //[NOTE:messageList for license]
            var studentLicense = new List<C_StudentLicenses>();
            try
            {
                if (obj._StudentLicenses != null)
                {
                    foreach (var item in obj._StudentLicenses)
                    {
                        //[NOTE:need to add single message]
                        var temp = new C_StudentLicenses();

                        var student = _studentsRepo.Get(item.Id);
                        var userAvailable = _oeUsersRepo.GetAll().Where(u => u.UserLoginId == item.UserLoginId).SingleOrDefault();
                        var IsOldLicense = _oeUserAuthenticationsRepo.GetAll().Where(u => u.UserId == item.UserId && u.ActorId == 10).SingleOrDefault();

                        if (item.UserLoginId == null || item.UserLoginId == item.oldUserLoginId)
                        {
                            if (IsOldLicense == null && item.IsActive == true && item.UserLoginId != null)
                            {
                                var createAuthentication = new OE_UserAuthentications()
                                {
                                    UserId = userAvailable.Id,
                                    ActorId = 10,
                                    InstitutionId = item.InstitutionId,
                                    AddedBy = item.AddedBy,
                                    AddedDate = CommDate_ConvertToUtcDate(DateTime.Now)
                                };
                                _oeUserAuthenticationsRepo.Insert(createAuthentication);
                                temp.Id = item.Id;
                                temp.Message = "Successful!";
                            }
                            else if (IsOldLicense != null && item.IsActive != item.oldIsActive)
                            {
                                var fetchAuthentication = _oeUserAuthenticationsRepo.Get(IsOldLicense.Id);
                                fetchAuthentication.IsActive = item.IsActive;
                                fetchAuthentication.ModifiedBy = item.ModifiedBy;
                                fetchAuthentication.ModifiedDate = CommDate_ConvertToUtcDate(DateTime.Now);
                                _oeUserAuthenticationsRepo.Update(fetchAuthentication);
                                temp.Id = item.Id;
                                temp.Message = "Successful!";
                            }
                            else if (item.UserLoginId == null && item.oldUserLoginId != null)
                            {
                                var fetchStudent = _studentsRepo.Get(item.Id);
                                fetchStudent.UserId = null;
                                fetchStudent.ModifiedBy = item.ModifiedBy;
                                fetchStudent.ModifiedDate = CommDate_ConvertToUtcDate(DateTime.Now);
                                _studentsRepo.Update(fetchStudent);
                                if (IsOldLicense != null)
                                {
                                    _oeUserAuthenticationsRepo.Delete(IsOldLicense);
                                }
                                temp.Id = item.Id;
                                temp.Message = "Successful!";
                            }
                            else
                            {
                                temp.Id = item.Id;
                                temp.Message = "Unchanged!";
                            }
                        }
                        else
                        {
                            if (userAvailable != null)
                            {
                                if (_commonFunctionsServ.Function_IsStudentHasOeId(userAvailable.Id, item.InstitutionId) == true)
                                {
                                    temp.Id = item.Id;
                                    temp.Message = "OurEdu Id is being used by another student!";
                                }
                                else
                                {
                                    var createAuthentication = new OE_UserAuthentications()
                                    {
                                        UserId = userAvailable.Id,
                                        ActorId = 10,
                                        InstitutionId = item.InstitutionId,
                                        AddedBy = item.AddedBy,
                                        AddedDate = CommDate_ConvertToUtcDate(DateTime.Now)
                                    };
                                    _oeUserAuthenticationsRepo.Insert(createAuthentication);

                                    var fetchStudent = _studentsRepo.Get(item.Id);
                                    fetchStudent.UserId = userAvailable.Id;
                                    fetchStudent.ModifiedBy = item.ModifiedBy;
                                    fetchStudent.ModifiedDate = CommDate_ConvertToUtcDate(DateTime.Now);
                                    _studentsRepo.Update(fetchStudent);

                                    temp.Id = item.Id;
                                    temp.Message = "Successful!";
                                }
                            }
                            else
                            {
                                temp.Id = item.Id;
                                temp.Message = "UserId is invalid!";
                            }
                        }
                        //[NOTE: adding message list to the licenseList]
                        studentLicense.Add(temp);
                    }
                    //[NOTE:model for LicenList message]
                    model._StudentLicenses = studentLicense;
                }
            }
            catch (Exception ex)
            {
                //[NOTE:eror message add to model]
                model.errorMessage = "ERROR102:StudentsServ/InsertUpdateStudentLicenses - " + ex.Message;
            }
            return model;
        }

        public void UpdateStudents(UpdateStudents students, IFormFile file, string webRootPath)
        {
            var student = _studentsRepo.Get(students._Student.Id);
            student.Name = students._Student.Name;
            student.ClassId = students._Student.ClassId;
            student.GenderId = students._Student.GenderId;
            student.DOB = students._Student.DOB;
            student.PermanentAddress = students._Student.PermanentAddress;
            student.PresentAddress = students._Student.PresentAddress;
            student.AdmittedYear = students._Student.AdmittedYear;
            student.IsActive = students._Student.IsActive;

            if (file != null)
            {
                string imagePath300X200 = "ClientDictionary/Students/IP300X200/";
                string ext = Path.GetExtension(file.FileName);
                ext = "." + ext.Remove(0, 1);

                if (Comm_ImageFormat(student.Id.ToString(), file, webRootPath, imagePath300X200, 300, 200, ext).Equals(true))
                {
                    //[NOTE:Update image file]
                    student.IP300X200 = imagePath300X200 + student.Id + ext;                    
                }
            }
            
            if (students._Student.UserLoginId != students._Student.OldUserLoginId)
            {
                var getUserId = (dynamic)null;
                if (students._Student.UserLoginId != null)
                {
                    getUserId = _oeUsersRepo.GetAll().Where(u => u.UserLoginId == students._Student.UserLoginId).SingleOrDefault().Id;
                }

                if (students._Student.OldUserLoginId == null)
                {
                    student.UserId = getUserId;
                }
                else
                {
                    var getAuthentic = _oeUserAuthenticationsRepo.GetAll().Where(u => u.UserId == student.UserId && u.ActorId == 10).SingleOrDefault();
                    if (getAuthentic != null)
                    {
                        _oeUserAuthenticationsRepo.Delete(getAuthentic);
                    }
                    student.UserId = getUserId;
                }
            }           
            _studentsRepo.Update(student);

            var stdPromotion = _studentPromotionsRepo.GetAll().Where(s => s.StudentId == students._Student.Id && s.ClassId == students._Student.AdmittedClassId && s.Year.Year == students._Student.AdmittedPYear).SingleOrDefault();
            stdPromotion.RollNo = students._Student.RollNo;
            stdPromotion.Year = students._Student.AdmittedYear;
            stdPromotion.ClassId = students._Student.ClassId;
            _studentPromotionsRepo.Update(stdPromotion);

        }
        public void UpdateStudentPromotions(UpdateStudentPromotions obj)
       {
            if (obj.StudentPromotions != null)
            {
                var uptStudentPromotion = _studentPromotionsRepo.Get(obj.StudentPromotions.Id);
                uptStudentPromotion.ClassId = obj.StudentPromotions.ClassId;
                uptStudentPromotion.Year = obj.StudentPromotions.Year;
                uptStudentPromotion.RollNo = obj.StudentPromotions.RollNo;
                _studentPromotionsRepo.Update(uptStudentPromotion);
            }
        }        
       
       public void DeleteStudents(Students students)
        {
            _studentsRepo.Delete(students);
        }
       public void DeleteStaticFile(Students students, string rootPath)
        {
            var msg = (dynamic)null;
            if (DelFileFromLocation(Path.Combine(rootPath, students.IP300X200)))
            {
                msg = "File deleted.";
                students.IP300X200 = (dynamic)null;
                _studentsRepo.Update(students);
            }
            else
                msg = "Error Occured.";

        }
       public void DeleteStudentPromotions(DeleteStudentPromotions obj)
       {
            if (obj.StudentPromotions != null)
            {
                var dltStudentPromotion = _studentPromotionsRepo.Get(obj.StudentPromotions.Id);
                _studentPromotionsRepo.Delete(dltStudentPromotion);

            }
        }
        
        #endregion "Insert Update Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_Students> Dropdown_Students(long institutionId)
        {

            var getAll = _studentsRepo.GetAll().ToList();
            var getStudent = from s in getAll
                             where s.InstitutionId == institutionId
                             select s;
            var queryResult = new List<dropdown_Students>();

            foreach (var item in getStudent)
            {
                var s = new dropdown_Students()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                queryResult.Add(s);
            }

            return queryResult;

        }
        #endregion "Dropdown Methods"        

    }
}
