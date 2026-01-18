
using Microsoft.AspNetCore.Http;
using OE.Data;
using OE.Repo;
using OE.Service.CustomEntitiesServ;
using OE.Service.ServiceModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OE.Service
{
    public class EmployeesServ : IEmployeesServ
    {

        #region "Variables"
        private readonly IAssignedTeachersRepo<AssignedTeachers> _assignedTeachersRepo;
        private readonly IAssignedCoursesRepo<AssignedCourses> _assignedCoursesRepo;

        private readonly ICOM_GendersRepo<COM_Genders> _comGendersRepo;
        private readonly IRegistrationItemTypesRepo<COM_RegistrationItemTypes> _comRegistrationItemTypesRepo;
        private readonly ICOM_RegistrationUserTypesRepo<COM_RegistrationUserTypes> _comRegistrationUserTypesRepo;

        private readonly IEmployeesRepo<Employees> _employeesRepo;
        private readonly IEmployeeDetailsRepo<EmployeeDetails> _employeeDetailsRepo;
        private readonly IEmployeeDesignationsRepo<EmployeeDesignations> _employeeDesignationsRepo;
        private readonly IEmployeeTypesRepo<EmployeeTypes> _employeeTypesRepo;
        private readonly IEmployeeTypeCategoriesRepo<EmployeeTypeCategories> _employeeTypeCategoriesRepo;

        private readonly IOE_ActorsRepo<OE_Actors> _oeActorsRepo;
        private readonly IOE_UserAuthenticationsRepo<OE_UserAuthentications> _oeUserAuthenticationsRepo;
        private readonly IOE_UsersRepo<OE_Users> _oeUsersRepo;

        private readonly IRegistrationItemsRepo<RegistrationItems> _registrationItemsRepo;        
        private readonly IRegistrationGroupsRepo<RegistrationGroups> _registrationGroupsRepo;
                                     
        private readonly ICommonServ _commonServ;
        private readonly ICommonFunctionsServ _commonFunctionsServ;
        #endregion "Variables"

        #region "Constructor"       
        public EmployeesServ(
        IAssignedTeachersRepo<AssignedTeachers> assignedTeachersRepo,
        IAssignedCoursesRepo<AssignedCourses> assignedCoursesRepo,

        ICOM_GendersRepo<COM_Genders> comGendersRepo,
        IRegistrationItemTypesRepo<COM_RegistrationItemTypes> comRegistrationItemTypesRepo,
        ICOM_RegistrationUserTypesRepo<COM_RegistrationUserTypes> comRegistrationUserTypesRepo,

        IEmployeeDetailsRepo<EmployeeDetails> employeeDetailsRepo,
        IEmployeesRepo<Employees> employeesRepo,
        IEmployeeDesignationsRepo<EmployeeDesignations> employeeDesignationsRepo,
        IEmployeeTypesRepo<EmployeeTypes> employeeTypesRepo,
        IEmployeeTypeCategoriesRepo<EmployeeTypeCategories> employeeTypeCategoriesRepo,

        IOE_UsersRepo<OE_Users> oeUsersRepo,
        IOE_UserAuthenticationsRepo<OE_UserAuthentications> oeUserAuthenticationsRepo,
        IOE_ActorsRepo<OE_Actors> oeActorsRepo,

        IRegistrationItemsRepo<RegistrationItems> registrationItemsRepo,
        IRegistrationGroupsRepo<RegistrationGroups> registrationGroupsRepo,        
        
        ICommonServ commonServ,
        ICommonFunctionsServ commonFunctionsServ

        )
        {
            _commonServ = commonServ;
            _commonFunctionsServ = commonFunctionsServ;
            _comGendersRepo = comGendersRepo;

            _employeesRepo = employeesRepo;
            _employeeDetailsRepo = employeeDetailsRepo;
            _employeeDesignationsRepo = employeeDesignationsRepo;
            _employeeTypesRepo = employeeTypesRepo;
            _employeeTypeCategoriesRepo = employeeTypeCategoriesRepo;

            _oeUserAuthenticationsRepo = oeUserAuthenticationsRepo;
            _registrationItemsRepo = registrationItemsRepo;
            _oeUsersRepo = oeUsersRepo;
            _assignedTeachersRepo = assignedTeachersRepo;
            _assignedCoursesRepo = assignedCoursesRepo;
            _registrationGroupsRepo = registrationGroupsRepo;
            _comRegistrationUserTypesRepo = comRegistrationUserTypesRepo;
            _comRegistrationItemTypesRepo = comRegistrationItemTypesRepo;
            _oeActorsRepo = oeActorsRepo;

        }

        #endregion "Constructor"

        #region Get Methods"
        public Employees GetEmployeeByTeacher(long institutionId, long userId)
        {
            var queryAll = _employeesRepo.GetAll();

            var returnQuery = (from e in queryAll
                               where e.InstitutionId == institutionId && e.UserId == userId
                               select e).SingleOrDefault();            
            return returnQuery;
        }
        public Employees EmployeeById(long id, long instituteId, long userId)
        {
            var emp = _employeesRepo.GetAll();

            var returnQry = (dynamic)null;
            if (userId == 0)
            {
                returnQry = (from e in emp
                             where e.Id == id && e.InstitutionId == instituteId
                             select e).SingleOrDefault();
            }
            else if (userId != 0)
            {
                returnQry = (from e in emp
                             where e.InstitutionId == instituteId && e.UserId == userId
                             select e).SingleOrDefault();

            }
            return returnQry;
        }
        public GetEmployeeDetails GetEmployeeDetails(long institutionId, long empId)
        {
            var gender = _comGendersRepo.GetAll();
            var empDetails = _employeeDetailsRepo.GetAll();
            var emp = _employeesRepo.GetAll();
            var users = _oeUsersRepo.GetAll();
            var regItem = _registrationItemsRepo.GetAll();
            var regUserType = _comRegistrationUserTypesRepo.GetAll();
            var regGroups = _registrationGroupsRepo.GetAll();
            var regITypes = _comRegistrationItemTypesRepo.GetAll().ToList();
            
            var designation = _employeeDesignationsRepo.GetAll().Where(d => d.EmployeeId == empId).SingleOrDefault();
            var empType = _employeeTypesRepo.GetAll().Where(et => et.Id == designation?.EmployeeTypeId).SingleOrDefault();
            var empCategoryType = _employeeTypeCategoriesRepo.GetAll().Where(ect => ect.Id == designation?.EmployeeTypeCategoryId).SingleOrDefault();
           
            //[NOTE:Get Employee details]
            var checkEmployeeIsRegister = from e in emp
                                          join u in users on e.UserId equals u.Id
                                          where e.Id == empId
                                          select e;
            var employeeDetails = (dynamic)null;
            long employeeUserId = 0;

            if (checkEmployeeIsRegister.Count() != 0)
            {
                var jointQry = from e in emp
                               join g in gender on e.GenderId equals g.Id
                               join u in users on e.UserId equals u.Id
                               join ed in empDetails on e.Id equals ed.EmployeeId
                               into stu
                               from all in stu.DefaultIfEmpty()
                               where e.InstitutionId == institutionId && e.Id == empId
                               select new { e, all, g, u };

                foreach (var item in jointQry)
                {
                    employeeDetails = new GetEmployeeDetails()
                    {
                        employees = item.e,
                        employeeDetails = item.all,
                        genders = item.g,
                        Users = item.u
                    };
                }
                employeeUserId = Convert.ToInt64(jointQry.Single().e.UserId);
            }
            else
            {
                var jointQry = from e in emp
                               join g in gender on e.GenderId equals g.Id
                               join ed in empDetails on e.Id equals ed.EmployeeId
                               into stu
                               from all in stu.DefaultIfEmpty()
                               where e.InstitutionId == institutionId && e.Id == empId
                               select new { e, all, g };

                foreach (var item in jointQry)
                {
                    employeeDetails = new GetEmployeeDetails()
                    {
                        employees = item.e,
                        employeeDetails = item.all,
                        genders = item.g
                    };
                }
            }
            //[NOTE:Get registration group]
            var queryRegistrationGroups = from r in regGroups
                                          join ru in regUserType on r.RegistrationUserTypeId equals ru.Id
                                          where r.InstitutionId == institutionId && r.RegistrationUserTypeId == 2
                                          select new { r, ru };

            var regGroup = new List<GetEmployeeDetails>();
            foreach (var item in queryRegistrationGroups)
            {
                var temp = new GetEmployeeDetails()
                {
                    RegistrationGroups = item.r,
                    COM_RegistrationUserTypes = item.ru
                }; regGroup.Add(temp);
            }

            //[NOTE:Get regItemWithDetails]
            var jointQryRegItemWithDetails = from r in regItem
                                             join sd in empDetails on r.Id equals sd.RegistrationItemId
                                             where sd.EmployeeId == empId && sd.InstitutionId == institutionId && r.RegistrationUserTypeId == 2
                                             select new { r, sd };

            var regItemWithDetails = new List<GetEmployeeDetails>();
            foreach (var item in jointQryRegItemWithDetails)
            {
                var temp = new GetEmployeeDetails()
                {
                    RegistrationItemId = item.r.Id,
                    RegistrationItemName = item.r.Name,
                    employeeDetails = item.sd,
                    registrationItems = item.r
                };
                regItemWithDetails.Add(temp);
            }

            //[NOTE:Get RegItem]          
            var jointQueryRegItems = from ri in regItem
                                     where ri.InstitutionId == institutionId && ri.RegistrationUserTypeId == 2
                                     join rit in regITypes
                                     on ri.RegistrationItemTypeId equals rit.Id
                                     join rg in regGroups
                                     on ri.RegistrationGroupId equals rg.Id
                                     select new { ri, rit, rg };
            var regItems = new List<GetEmployeeDetails>();
            foreach (var item in jointQueryRegItems)
            {
                var obj = new GetEmployeeDetails()
                {
                    RegistrationItemId = item.ri.Id,
                    RegistrationItemName = item.ri.Name,
                    RegistrationGroupId = item.rg.Id,
                    RegistrationGroupsName = item.rg.Name,
                    RegistrationItemTypeId = item.rit.Id,
                    RegistrationItemTypeName = item.rit.Name,
                    RegistrationUserTypeId = item.ri.RegistrationUserTypeId
                };
                regItems.Add(obj);
            }

            var returnQry = new GetEmployeeDetails()
            {
               
                EmployeeTypes = empType,
                EmployeeTypeCategories = empCategoryType,                
                EmployeeDetails = employeeDetails,
                RegGroup = regGroup,
                RegItemWithDetails = regItemWithDetails,
                RegItems = regItems
            };

            return returnQry;
        }
        //[NOTE: ULR-Employees/EmployeeListByRegister]
        public GetEmployeeListByRegister GetEmployeeListByRegister(long instituteId)
        {
            var employees = _employeesRepo.GetAll();
            var genders = _comGendersRepo.GetAll();
            var users = _oeUsersRepo.GetAll();
            var designations = _employeeDesignationsRepo.GetAll();
            var employeeTypes = _employeeTypesRepo.GetAll();
            var employeeCategoryTypes = _employeeTypeCategoriesRepo.GetAll();

            var queryEmployees = (from e in employees
                                  where e.InstitutionId == instituteId
                                  && e.IsActive == true
                                  orderby e.FirstName

                                  join g in genders on e.GenderId equals g.Id
                                  join d in designations
                                       on e?.Id equals d?.EmployeeId into ALLDesignations
                                  from designation in ALLDesignations.DefaultIfEmpty()
                                  join EmpType in employeeTypes
                                        on designation?.EmployeeTypeId equals EmpType?.Id into ALLEmpType
                                  from empType in ALLEmpType.DefaultIfEmpty()
                                  join EmpCategoryType in employeeCategoryTypes
                                        on designation?.EmployeeTypeCategoryId equals EmpCategoryType?.Id into ALLEmpCategoryType
                                  from empCategoryType in ALLEmpCategoryType.DefaultIfEmpty()
                                  join u in users
                                       on e.UserId equals u.Id into ALLCOLUMNS
                                  from user in ALLCOLUMNS.DefaultIfEmpty()
                                  select new { e, g, user, designation, empType, empCategoryType });



            var listEmployees = new List<C_Employees>();
            foreach (var item in queryEmployees)
            {
                var temp = new C_Employees()
                {
                    Id = item.e.Id,
                    InstitutionId = item.e.InstitutionId,
                    UserId = item.e.UserId,
                    GenderId = item.e.GenderId,
                    FirstName = item.e.FirstName,
                    LastName = item.e.LastName,
                    IP300X200 = item.e.IP300X200,
                    PresentAddress = item.e.PresentAddress,
                    PermanentAddress = item.e.PermanentAddress,
                    ContactNo = item.e.ContactNo,
                    EmailAddress = item.e.EmailAddress,
                    DOB = (dynamic)_commonServ.CommDate_ConvertToLocalDate(item.e.DOB),
                    JoiningDate = (dynamic)_commonServ.CommDate_ConvertToLocalDate(item.e.JoiningDate),
                    GenderName = item.g?.Name,
                    UsersIP300X200 = item.user?.IP300X200,
                    UserLoginId = item.user?.UserLoginId,
                    EmployeeTypeId = item.designation?.EmployeeTypeId,
                    EmployeeTypeName = item.empType != null ? item.empType.Name : "",
                    EmployeeCategoryTypeId = item.designation?.EmployeeTypeCategoryId,
                    EmployeeCategoryTypeName = item.empCategoryType != null ? item.empCategoryType.Name : ""
                };
                listEmployees.Add(temp);
            }
            var returnQry = new GetEmployeeListByRegister()
            {
                _Employees = listEmployees

            };

            return returnQry;
        }
        //[NOTE: ULR-Employees/EmployeeListByAdmin]
        public GetEmployeeListByAdmin GetEmployeeListByAdmin(long instituteId, bool employeeStatus)
        {
            var employees = _employeesRepo.GetAll();
            var genders = _comGendersRepo.GetAll();
            var users = _oeUsersRepo.GetAll();
            var designations = _employeeDesignationsRepo.GetAll();
            var employeeTypes = _employeeTypesRepo.GetAll();
            var employeeCategoryTypes = _employeeTypeCategoriesRepo.GetAll();

            var queryEmployees = (from e in employees
                                  where e.InstitutionId == instituteId
                                  && e.IsActive == employeeStatus
                                  orderby e.FirstName

                                  join g in genders on e.GenderId equals g.Id                                  
                                  join d in designations
                                       on e?.Id equals d?.EmployeeId into ALLDesignations
                                  from designation in ALLDesignations.DefaultIfEmpty()
                                  join EmpType in employeeTypes
                                        on designation?.EmployeeTypeId equals EmpType?.Id into ALLEmpType
                                  from empType in ALLEmpType.DefaultIfEmpty()
                                  join EmpCategoryType in employeeCategoryTypes
                                        on designation?.EmployeeTypeCategoryId equals EmpCategoryType?.Id into ALLEmpCategoryType
                                  from empCategoryType in ALLEmpCategoryType.DefaultIfEmpty()
                                  join u in users
                                       on e.UserId equals u.Id into ALLCOLUMNS
                                  from user in ALLCOLUMNS.DefaultIfEmpty()
                                  select new { e, g, user, designation, empType, empCategoryType });



            var listEmployees = new List<C_Employees>();
            foreach (var item in queryEmployees)
            {
                var temp = new C_Employees()
                {
                    Id = item.e.Id,
                    InstitutionId = item.e.InstitutionId,
                    UserId = item.e.UserId,
                    GenderId = item.e.GenderId,
                    FirstName = item.e.FirstName,
                    LastName = item.e.LastName,
                    IP300X200 = item.e.IP300X200,
                    PresentAddress = item.e.PresentAddress,
                    PermanentAddress = item.e.PermanentAddress,
                    ContactNo = item.e.ContactNo,
                    EmailAddress = item.e.EmailAddress,
                    DOB = (dynamic)_commonServ.CommDate_ConvertToLocalDate(item.e.DOB),
                    JoiningDate = (dynamic)_commonServ.CommDate_ConvertToLocalDate(item.e.JoiningDate),
                    GenderName = item.g?.Name,
                    UsersIP300X200 = item.user?.IP300X200,
                    UserLoginId = item.user?.UserLoginId,
                    EmployeeTypeId = item.designation?.EmployeeTypeId,
                    EmployeeTypeName = item.empType != null ? item.empType.Name : "",
                    EmployeeCategoryTypeId = item.designation?.EmployeeTypeCategoryId,
                    EmployeeCategoryTypeName = item.empCategoryType != null ? item.empCategoryType.Name : ""
                };
                listEmployees.Add(temp);
            }
            var returnQry = new GetEmployeeListByAdmin()
            {
                _Employees = listEmployees

            };

            return returnQry;
        }
        public GetEmployeeDetailsByAdmin GetEmployeeDetailsByAdmin(long institutionId, long empId)
        {
            var employees = _employeesRepo.GetAll();
            
            var queryEmployee = _employeesRepo.GetAll().Where(e => e.Id == empId && e.InstitutionId == institutionId).SingleOrDefault();
            var oeUser = _oeUsersRepo.GetAll().Where(u => u.Id == queryEmployee.UserId).SingleOrDefault();
            var gender = _comGendersRepo.GetAll().Where(g => g.Id == queryEmployee.GenderId).SingleOrDefault();
            var designation = _employeeDesignationsRepo.GetAll().Where(d => d.EmployeeId == queryEmployee.Id).SingleOrDefault();
            var empType = _employeeTypesRepo.GetAll().Where(et => et.Id == designation.EmployeeTypeId).SingleOrDefault();
            var empCategoryType = _employeeTypeCategoriesRepo.GetAll().Where(ect => ect.Id == designation.EmployeeTypeCategoryId).SingleOrDefault();

            var returnQry = new GetEmployeeDetailsByAdmin()
            {
                Employees = queryEmployee,
                OEUsers = oeUser,
                ComGenders = gender,
                EmployeeDesignations = designation,
                EmployeeTypes = empType,
                EmployeeTypeCategories = empCategoryType,
            };
            
            return returnQry;
        }
        public IEnumerable<GetTeacherLicenses> GetTeacherLicenses(long institutionId)
        {
            var getEmployees = _employeesRepo.GetAll();
            var getUsers = _oeUsersRepo.GetAll();
            var getAuthentications = _oeUserAuthenticationsRepo.GetAll();
            var getActors = _oeActorsRepo.GetAll();

            var jointQry = from au in getAuthentications
                           join u in getUsers on au.UserId equals u.Id
                           join ac in getActors on au.ActorId equals ac.Id
                           join e in getEmployees on au.UserId equals e.UserId
                           where au.ActorId == 14 && au.InstitutionId == institutionId
                           select new { au, u, ac, e };

            var returnQry = new List<GetTeacherLicenses>();
            foreach (var item in jointQry)
            {
                var temp = new GetTeacherLicenses()
                {
                    OE_Actors = item.ac,
                    OE_UserAuthentications = item.au,
                    OE_Users = item.u,
                    Employees = item.e
                }; returnQry.Add(temp);
            }
            return returnQry;
        }
        public IEnumerable<Employees> GetEmployees(long instituteId)
        {
            var queryAll = _employeesRepo.GetAll();
            var returnQry = from e in queryAll
                            where e.InstitutionId == instituteId
                            select e;
            return returnQry;
        }

        #endregion "Get Methods"

        #region "Insert Update Delete Methods"        
        public string InsertEmployee(InsertEmployee emp, string webRootPath)
        {
            var returnResult = (dynamic)null;            
            var lastId = _employeesRepo.GetLastId() + 1;
           
            if (emp != null)
            {
                //[Note: insert 'Employees' table]
                if (emp.Employees != null)
                {
                    var addedPrimaryKey = (dynamic)null;
                    var addedImagePath = (dynamic)null;
                    var currentIP300X200 = (dynamic)null;

                    if (emp.EmployeeImagefile != null)
                    {
                        addedPrimaryKey = (_employeesRepo.GetLastId() + 1).ToString();
                        addedImagePath = "ClientDictionary/Employees/IP300X200/";
                        string ext = Path.GetExtension(emp.EmployeeImagefile.FileName);
                        var result = (dynamic)_commonServ.Comm_ImageFormat(addedPrimaryKey, emp.EmployeeImagefile, webRootPath, addedImagePath, 300, 200, ext);
                        if (result == false)
                        {
                            return returnResult = "Image is not saved";
                        }
                        currentIP300X200 = addedImagePath + addedPrimaryKey + ext;
                    }

                    var e = new Employees()
                    {
                        InstitutionId = emp.Employees.InstitutionId,
                        UserId = emp.Employees.UserId,
                        GenderId = emp.Employees.GenderId,
                        FirstName = emp.Employees.FirstName,
                        LastName = emp.Employees.LastName,
                        EmailAddress = emp.Employees.EmailAddress,
                        ContactNo = emp.Employees.ContactNo,
                        JoiningDate = emp.Employees.JoiningDate,
                        PresentAddress = emp.Employees.PresentAddress,
                        PermanentAddress = emp.Employees.PermanentAddress,
                        DOB = emp.Employees.DOB,
                        IP300X200 = currentIP300X200 == null ? "" : currentIP300X200,
                        IsActive = emp.Employees.IsActive,
                        AddedBy = emp.Employees.AddedBy,
                        AddedDate = emp.Employees.AddedDate,
                        InsId = emp.Employees.InstitutionId
                    };

                    if (emp.OeUsers.UserLoginId != null)
                    {
                        if (_commonFunctionsServ.Function_OeUserHasUserLoginId(emp.OeUsers.UserLoginId) == true)
                        {
                            var getUserId = _oeUsersRepo.GetAll().Where(u => u.UserLoginId == emp.OeUsers.UserLoginId).SingleOrDefault();

                            if (_commonFunctionsServ.Function_IsEmployeeHasOeId(getUserId.Id, emp.CurrentInstitutionId) == true)
                            {
                                emp.Message = "OurEdu Id is being used by another employee.";
                                return returnResult = emp.Message;
                            }
                            else
                            {
                                e.UserId = getUserId.Id;
                            }
                        }
                        else
                        {
                            emp.Message = "OurEdu Id is invalid.";
                            return returnResult = emp.Message;
                        }
                    }
                    _employeesRepo.Insert(e);

                    if (emp.EmployeeTypeId != 0)
                    {

                        var ed = new EmployeeDesignations()
                        {
                            InstitutionId = emp.Employees.InstitutionId,
                            EmployeeId = _employeesRepo.GetAll().LastOrDefault().Id,
                            EmployeeTypeId = emp.EmployeeTypeId,
                            EmployeeTypeCategoryId = emp.EmployeeCategoryTypeId == 0 ? (dynamic)null : emp.EmployeeCategoryTypeId,
                            InsId = emp.Employees.InstitutionId,
                            IsActive = true,
                            AddedBy = emp.Employees.AddedBy,
                            AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date)
                        };
                        _employeeDesignationsRepo.Insert(ed);
                    }
                    if (emp.EmployeeDetails != null)
                    {
                        
                        foreach (var item in emp.EmployeeDetails)
                        {
                            var newEmp = new EmployeeDetails();
                            newEmp.InstitutionId = item.InstitutionId;
                            newEmp.EmployeeId = lastId;
                            newEmp.RegistrationItemId = item.RegistrationItemId;
                            newEmp.InsId = item.InsId;
                            newEmp.IsActive = true;
                            newEmp.AddedBy = item.AddedBy;
                            newEmp.AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);
                            newEmp.IsActive = true;

                            switch (item.RegistrationItemTypeId)
                            {
                                case 1:
                                    if (item.ActualFile != null)
                                    {
                                        var lastEmpDetailsId = _employeeDetailsRepo.GetAll().Count() == 0 ? 1 : _employeeDetailsRepo.GetAll().Last().Id + 1;
                                        string dbPath = "ClientDictionary/EmployeeDetails/FilePathValue/";
                                        string ext = Path.GetExtension(item.ActualFile.FileName);
                                        ext = ext.Remove(0, 1);
                                        var result = (dynamic)_commonServ.Comm_FileSave(lastEmpDetailsId, item.ActualFile, webRootPath, dbPath, ext);
                                        if (result == false)
                                        {
                                            return returnResult = "File is not saved.";
                                        }
                                        newEmp.FilePathValue = dbPath + lastEmpDetailsId.ToString() + "." + ext;
                                    }
                                    break;
                                case 2:
                                    if (item.ActualImage != null)
                                    {
                                        var lastEmpDetailsId = _employeeDetailsRepo.GetAll().Count() == 0 ? 1 : _employeeDetailsRepo.GetAll().Last().Id + 1;
                                        string dbPath = "ClientDictionary/EmployeeDetails/ImagePathValue/";
                                        string ext = Path.GetExtension(item.ActualImage.FileName);
                                        var result = (dynamic)_commonServ.Comm_ImageFormat(lastEmpDetailsId.ToString(), item.ActualImage, webRootPath, dbPath, 300, 200, ext);
                                        if (result == false)
                                        {
                                            return returnResult = "Image is not saved";
                                        }
                                        newEmp.ImagePathValue = dbPath + lastEmpDetailsId + ext;
                                    }
                                    break;
                                case 3:
                                    {
                                        newEmp.BitValue = item.BitValue;
                                    };
                                    break;
                                case 4:

                                    {
                                        newEmp.StringValue = item.StringValue;
                                    };
                                    break;
                                case 5:
                                    {

                                        newEmp.WholeValue = item.WholeValue;
                                    };
                                    break;
                                case 6:
                                    {
                                        newEmp.FloatValue = item.FloatValue;
                                    };
                                    break;
                                case 7:
                                    {
                                        newEmp.TextAreaValue = item.TextAreaValue;
                                    };
                                    break;
                                case 8:
                                    {
                                        newEmp.DateValue = item.DateValue;
                                    };
                                    break;
                                default:
                                    break;
                            }

                            _employeeDetailsRepo.Insert(newEmp);
                        }
                    }
                }
            }
            return returnResult;
        }

        public string InsertEmployeeByAdmin(InsertEmployeeByAdmin emp, string webRootPath)
        {
            var msg = (dynamic)null;
            var returnResult = (dynamic)null;
            if (emp != null)
            {
                //[Note: insert 'Employees' table]
                if (emp.Employees != null)
                {
                    var addedPrimaryKey = (dynamic)null;
                    var addedImagePath = (dynamic)null;
                    var currentIP300X200 = (dynamic)null;

                    if (emp.EmployeeImagefile != null)
                    {
                        addedPrimaryKey = (_employeesRepo.GetLastId() + 1).ToString();
                        addedImagePath = "ClientDictionary/Employees/IP300X200/";
                        currentIP300X200 = addedImagePath + addedPrimaryKey + ".jpg";
                    }
                    
                    var e = new Employees()
                    {
                        InstitutionId = emp.Employees.InstitutionId,
                        UserId = emp.Employees.UserId,
                        GenderId = emp.Employees.GenderId,
                        FirstName = emp.Employees.FirstName,
                        LastName = emp.Employees.LastName,
                        EmailAddress = emp.Employees.EmailAddress,
                        ContactNo = emp.Employees.ContactNo,
                        JoiningDate = emp.Employees.JoiningDate,
                        PresentAddress = emp.Employees.PresentAddress,
                        PermanentAddress = emp.Employees.PermanentAddress,
                        DOB = emp.Employees.DOB,
                        IP300X200 = currentIP300X200 == null ? "" : currentIP300X200,
                        IsActive = emp.Employees.IsActive,
                        AddedBy = emp.Employees.AddedBy,
                        AddedDate = emp.Employees.AddedDate,
                        InsId = emp.Employees.InstitutionId
                    };

                    if (emp.OeUsers.UserLoginId != null)
                    {
                        if (_commonFunctionsServ.Function_OeUserHasUserLoginId(emp.OeUsers.UserLoginId) == true)
                        {
                            var getUserId = _oeUsersRepo.GetAll().Where(u => u.UserLoginId == emp.OeUsers.UserLoginId).SingleOrDefault();

                            if (_commonFunctionsServ.Function_IsEmployeeHasOeId(getUserId.Id, emp.CurrentInstitutionId) == true)
                            {
                                emp.Message = "OurEdu Id is being used by another employee.";
                                return returnResult = emp.Message;
                            }
                            else
                            {
                                e.UserId = getUserId.Id;
                            }
                        }
                        else
                        {
                            emp.Message = "OurEdu Id is invalid.";
                            return returnResult = emp.Message;
                        }
                    }
                    _employeesRepo.Insert(e);
                    
                    if (emp.EmployeeTypeId != 0)
                    {
                        
                        var ed = new EmployeeDesignations()
                        {
                            InstitutionId = emp.Employees.InstitutionId,
                            EmployeeId = _employeesRepo.GetAll().LastOrDefault().Id,
                            EmployeeTypeId = emp.EmployeeTypeId,
                            EmployeeTypeCategoryId = emp.EmployeeCategoryTypeId == 0 ? (dynamic)null : emp.EmployeeCategoryTypeId,
                            InsId = emp.Employees.InstitutionId,
                            IsActive = true,
                            AddedBy = emp.Employees.AddedBy,
                            AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date)
                        };
                        _employeeDesignationsRepo.Insert(ed);
                      
                    }
                  
                    //[NOTE:add image file]
                    if (emp.EmployeeImagefile != null)
                    {
                        var result = (dynamic)_commonServ.Comm_ImageFormat(addedPrimaryKey, emp.EmployeeImagefile, webRootPath, addedImagePath, 300, 200, ".jpg");
                        if (result == true)
                            msg = "Image Saved";
                        else
                            msg = "Error occured.";
                    }
                }
            }
            return returnResult;
        }
        public string InsertTeacherLicense(InsertTeacherLicenses teacherLicenses)
        {
            var returnResult = (dynamic)null;
            //[NOTE: Checking UserLoginId is available or not]
            bool IsExistAsEmployee = _commonFunctionsServ.Function_OEUsers_IsUserAsEmployee(teacherLicenses.UserLoginId);

            if (IsExistAsEmployee == false)
            {
                teacherLicenses.Message = "OurEdu Id is invalid.";
                return returnResult = teacherLicenses.Message;
            }
            if (IsExistAsEmployee == true)
            {
                var oeU = _oeUsersRepo.GetAll();
                var oeUser = (from u in oeU
                              where u.UserLoginId == teacherLicenses.UserLoginId
                              select u).FirstOrDefault();
                //[NOTE: Checking User Authentication is already available or not based on UserId, ActorId and InstitutionId]
                var getAuthenticaitons = _oeUserAuthenticationsRepo.GetAll();
                var IsAuthenticationExist = from au in getAuthenticaitons
                                            where au.UserId == oeUser.Id && au.ActorId == 14 && au.InstitutionId == teacherLicenses.InstitutionId
                                            select au;

                if (IsAuthenticationExist.Count() == 0)
                {
                    var createNewAuthenticaion = new OE_UserAuthentications()
                    {
                        UserId = oeUser.Id,
                        ActorId = teacherLicenses.ActorId,
                        IsActive = teacherLicenses.IsActive,
                        InstitutionId = teacherLicenses.InstitutionId,
                        AddedBy = teacherLicenses.AddedBy,
                        AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date)
                    };
                    _oeUserAuthenticationsRepo.Insert(createNewAuthenticaion);
                    teacherLicenses.Message = (dynamic)null;
                    return returnResult = teacherLicenses.Message;
                }
                else
                {
                    teacherLicenses.Message = "This User has already authentication.";
                    returnResult = teacherLicenses.Message;
                }

            }
            return returnResult;
        }

        public string UpdateTeacherLicense(UpdateTeacherLicenses teacherLicenses)
        {
            var returnResult = (dynamic)null;
            //[NOTE: Checking UserLoginId is available or not]  
            bool IsExistAsEmployee = _commonFunctionsServ.Function_OEUsers_IsUserAsEmployee(teacherLicenses.UserLoginId);
            if (IsExistAsEmployee == false)
            {
                teacherLicenses.Message = "OurEdu Id is invalid.";
                return returnResult = teacherLicenses.Message;
            }
            if (IsExistAsEmployee == true)
            {
                var oeU = _oeUsersRepo.GetAll();
                var oeUser = (from u in oeU
                              where u.UserLoginId == teacherLicenses.UserLoginId
                              select u).FirstOrDefault();

                var getExistingAuthentication = _oeUserAuthenticationsRepo.Get(teacherLicenses.Id);

                //[NOTE: Checking selected UserLoginId and selected Actor Id are same, if so than, only update without userId and actorId]
                if (getExistingAuthentication.UserId == oeUser.Id && getExistingAuthentication.ActorId == teacherLicenses.ActorId)
                {
                    getExistingAuthentication.IsActive = teacherLicenses.IsActive;
                    getExistingAuthentication.ModifiedBy = teacherLicenses.ModifiedBy;
                    getExistingAuthentication.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date);
                    _oeUserAuthenticationsRepo.Update(getExistingAuthentication);
                    return returnResult = null;
                }
                //[NOTE: if selected UserLoginId is different, again check user authentication is already available or not based on UserId, ActorId and InstitutionId]
                if (getExistingAuthentication.UserId != oeUser.Id)
                {
                    var getAuthentications = _oeUserAuthenticationsRepo.GetAll();
                    var IsAuthenticationExist = from au in getAuthentications
                                                where au.UserId == oeUser.Id && au.ActorId == teacherLicenses.ActorId && au.InstitutionId == teacherLicenses.InstitutionId
                                                select au;
                    if (IsAuthenticationExist.Count() == 0)
                    {
                        getExistingAuthentication.UserId = oeUser.Id;
                        getExistingAuthentication.ActorId = teacherLicenses.ActorId;
                        getExistingAuthentication.IsActive = teacherLicenses.IsActive;
                        getExistingAuthentication.ModifiedBy = teacherLicenses.ModifiedBy;
                        getExistingAuthentication.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now.Date);
                        _oeUserAuthenticationsRepo.Update(getExistingAuthentication);
                        return returnResult = null;
                    }
                    else
                    {
                        teacherLicenses.Message = "This User has already authentication.";
                        returnResult = teacherLicenses.Message;
                    }
                }
            }
            return returnResult;
        }
        public string UpdateEmployees(UpdateEmployees obj, string webRootPath)
        {
            var returnResult = (dynamic)null;
            var msg = (dynamic)null;
            if (obj != null)
            {
                if (obj.Employees != null)
                {
                    var user = _oeUsersRepo.GetAll();
                    var emp = _employeesRepo.GetAll();
                    var employee = (from e in emp where e.Id == obj.Employees.Id select e).SingleOrDefault();
                    var authentications = _oeUserAuthenticationsRepo.GetAll();
                    if (obj.SelectedUserLoginId != null)
                    {
                        if (_commonFunctionsServ.Function_OeUserHasUserLoginId(obj.SelectedUserLoginId) == true)
                        {
                            var getUserId = _oeUsersRepo.GetAll().Where(u => u.UserLoginId == obj.SelectedUserLoginId).SingleOrDefault();
                            if (employee.UserId != getUserId.Id)
                            {

                                if (_commonFunctionsServ.Function_IsEmployeeHasOeId(getUserId.Id, obj.CurrentInstituteId) == true)
                                {
                                    obj.Message = "OurEdu Id is being used by another employee.";
                                    return returnResult = obj.Message;
                                }
                                else
                                {
                                    //[NOTE: Remove previous userId's authentications while userLoginId is update]
                                    var filteredAuthentications = (from au in authentications
                                                                   where au.UserId == employee.UserId && au.InstitutionId == obj.CurrentInstituteId
                                                                   select au).ToList();
                                    if (filteredAuthentications != null)
                                    {
                                        foreach (var item in filteredAuthentications)
                                        {
                                            _oeUserAuthenticationsRepo.Delete(item);
                                        }
                                    }
                                    employee.UserId = getUserId.Id;
                                }
                            }
                        }
                        else
                        {
                            obj.Message = "OurEdu Id is invalid.";
                            return returnResult = obj.Message;
                        }
                    }
                    else
                    {
                        if (employee.UserId != null)
                        {
                            //[NOTE: Remove userId's authentications while userLoginId is null]
                            var filteredAuthentications = (from au in authentications
                                                           where au.UserId == employee.UserId && au.InstitutionId == obj.CurrentInstituteId
                                                           select au).ToList();
                            if (filteredAuthentications != null)
                            {
                                foreach (var item in filteredAuthentications)
                                {
                                    _oeUserAuthenticationsRepo.Delete(item);
                                }
                                employee.UserId = (dynamic)null;
                            }
                        }
                    }

                    employee.FirstName = obj.Employees.FirstName;
                    employee.LastName = obj.Employees.LastName;
                    employee.GenderId = obj.Employees.GenderId;
                    employee.DOB = obj.Employees.DOB;
                    employee.EmailAddress = obj.Employees.EmailAddress;
                    employee.ContactNo = obj.Employees.ContactNo;
                    employee.PresentAddress = obj.Employees.PresentAddress;
                    employee.PermanentAddress = obj.Employees.PermanentAddress;
                    employee.JoiningDate = obj.Employees.JoiningDate;
                    employee.IsActive = obj.Employees.IsActive;

                    if (obj.EmployeesStaticFile != null)
                    {
                        string ip300X200 = "ClientDictionary/Employees/IP300X200/";
                        if ((dynamic)_commonServ.Comm_ImageFormat(obj.Employees.Id.ToString(), obj.EmployeesStaticFile, webRootPath, ip300X200, 300, 200, ".jpg").Equals(true))
                        {
                            employee.IP300X200 = ip300X200 + obj.Employees.Id + ".jpg";
                        }
                    }
                    _employeesRepo.Update(employee);

                }

                if (obj.EmployeeDetails != null)
                {
                    if (obj.RegistrationItemTypeId == 1 && obj.EmployeeDetailsStaticFile != null)
                    {
                        string dbPath = "ClientDictionary/EmployeeDetails/FilePathValue/";
                        string ext = Path.GetExtension(obj.EmployeeDetailsStaticFile.FileName);
                        ext = ext.Remove(0, 1);
                        var previousFile = Path.Combine(webRootPath, obj.EmployeeDetails.FilePathValue == null ? "" : obj.EmployeeDetails.FilePathValue);
                        if (File.Exists(previousFile))
                        {
                            File.Delete(previousFile);
                        }
                        if ((dynamic)_commonServ.Comm_FileSave(obj.EmployeeDetails.Id, obj.EmployeeDetailsStaticFile, webRootPath, dbPath, ext))
                        {
                            msg = "File successfully saved.";
                            obj.EmployeeDetails.FilePathValue = dbPath + obj.EmployeeDetails.Id + "." + ext;
                        }
                        else { msg = "Error occured while saving file."; }
                    }
                    if (obj.RegistrationItemTypeId == 2 && obj.EmployeeDetailsStaticFile != null)
                    {
                        string dbPath = "ClientDictionary/EmployeeDetails/ImagePathValue/";
                        string ext = ".png";

                        if ((dynamic)_commonServ.Comm_ImageFormat(obj.EmployeeDetails.Id.ToString(), obj.EmployeeDetailsStaticFile, webRootPath, dbPath, 600, 400, ext))
                        {
                            msg = "File successfully saved.";
                        }
                        else { msg = "Error occured while saving file."; }
                        obj.EmployeeDetails.ImagePathValue = dbPath + obj.EmployeeDetails.Id + ext;
                    }
                    _employeeDetailsRepo.Update(obj.EmployeeDetails);
                }
               
                if (obj.Designations != null)
                {
                    var designation = _employeeDesignationsRepo.GetAll().Where(d => d.EmployeeId == obj.Employees.Id).SingleOrDefault();
                    //[Note: if already designation exist. -> Update ]
                    if (designation != null)
                    {
                        designation.EmployeeTypeId = obj.Designations.EmployeeTypeId;
                        designation.EmployeeTypeCategoryId = obj.Designations.EmployeeTypeCategoryId != null || obj.Designations.EmployeeTypeCategoryId != 0 ? obj.Designations.EmployeeTypeCategoryId : (dynamic)null;
                        designation.ModifiedBy = obj.Designations.ModifiedBy;
                        designation.ModifiedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now);
                        _employeeDesignationsRepo.Update(designation);
                    }
                    //[Note: if designation is not exist. -> create one ]
                    else
                    {
                        var newDesignation = new EmployeeDesignations()
                        {
                            InstitutionId = obj.Designations.InstitutionId,
                            EmployeeId = obj.Employees.Id,
                            EmployeeTypeId = obj.Designations.EmployeeTypeId,
                            EmployeeTypeCategoryId = obj.Designations.EmployeeTypeCategoryId,
                            StartDate =_commonServ.CommDate_ConvertToUtcDate(DateTime.Now),
                            IsActive = true,
                            AddedBy = obj.Designations.AddedBy,
                            AddedDate = _commonServ.CommDate_ConvertToUtcDate(DateTime.Now),
                            InsId = obj.Designations.InstitutionId
                        };
                        _employeeDesignationsRepo.Insert(newDesignation);
                    }
                }
                
            }
            return returnResult;
        }

        public DeleteEmployees DeleteEmployees(DeleteEmployees obj)
        {
            var returnModel = new DeleteEmployees();
            var employee = new Employees();
            try
            {
                if (obj.Id > 0)
                {
                    employee = _employeesRepo.Get(obj.Id);
                    if (employee != null)
                    {
                        _employeesRepo.Delete(employee);                        
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
                    returnModel.Message = "ERROR102:EmployeesServ/DeleteEmployees - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }
            }
            return returnModel;
        }
        public void DeleteStaticFile(Employees emp, string rootPath)
        {
            var msg = (dynamic)null;
            if ((dynamic)_commonServ.DelFileFromLocation(Path.Combine(rootPath, emp.IP300X200)))
            {
                msg = "File deleted.";
                emp.IP300X200 = (dynamic)null;
                _employeesRepo.Update(emp);


            }
            else
                msg = "Error Occured.";
        }
       
        public DeleteTeacherLicenses DeleteTeacherLicense(DeleteTeacherLicenses obj)
        {
            var returnModel = new DeleteTeacherLicenses();

            try
            {
                if (obj.Id > 0)
                {

                    var techerLicense = _oeUserAuthenticationsRepo.Get(obj.Id);

                    if (techerLicense != null)
                    {

                        _oeUserAuthenticationsRepo.Delete(techerLicense);


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
                    returnModel.Message = "ERROR102:EmployeesServ/DeleteTeacherLicense - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }
            }
            return returnModel;
        }
        

        public void MakeArchive(MakeArchiveEmployees obj)
        {
            if (obj.Employees != null)
            {
                var emp = _employeesRepo.Get(obj.Employees.Id);
                emp.IsActive = obj.Employees.IsActive;

                _employeesRepo.Update(emp);
            }
        }
        #endregion "Insert Update Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_Employees> Dropdown_Employees(long institutionId)
        {
            var getAll = _employeesRepo.GetAll().ToList();

            var getEmp = from e in getAll
                         where e.InstitutionId == institutionId
                         select e;
            var queryResult = new List<dropdown_Employees>();

            foreach (var item in getEmp)
            {
                var d = new dropdown_Employees()
                {
                    Id = item.Id,
                    Name = item.FirstName + " " + item.LastName
                };
                queryResult.Add(d);
            }

            return queryResult;

        }
        public IEnumerable<dropdown_EmployeeWithAssignTeacher> dropdown_EmployeeWithAssignTeacher(long institutionId, long year, long classId, long subjectId)
        {
            var asgnThrQuery = _assignedTeachersRepo.GetAll().ToList();
            var empQuery = _employeesRepo.GetAll().ToList();

            var asgnCQuery = _assignedCoursesRepo.GetAll().ToList();
            var query = from at in asgnThrQuery
                        where at.InstitutionId == institutionId && at.Year.Year == year && at.ClassId == classId
                        join ac in asgnCQuery on at.AssignedCourseId equals ac.Id
                        where ac.SubjectId == subjectId
                        join e in empQuery on at.EmployeeId equals e.Id
                        select new { at, e, ac };


            var queryResult = new List<dropdown_EmployeeWithAssignTeacher>();

            foreach (var item in query.ToList())
            {
                var temp = new dropdown_EmployeeWithAssignTeacher()
                {
                    Id = item.e.Id,
                    Name = item.e.FirstName + " " + item.e.LastName

                };
                queryResult.Add(temp);
            }
            return queryResult;
        }
        #endregion "Dropdown Methods"
    }
}

