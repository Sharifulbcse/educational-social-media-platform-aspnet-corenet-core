using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http; //[NOTE: for file]

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class EmployeeDetailsServ : CommonServ, IEmployeeDetailsServ
    {
        #region "Variables"
        private readonly IEmployeeDetailsRepo<EmployeeDetails> _employeeDetailsRepo;
        private readonly IEmployeeDesignationsRepo<EmployeeDesignations> _employeeDesignationsRepo;
        private readonly IEmployeesRepo<Employees> _employeesRepo;
        private readonly ICOM_GendersRepo<COM_Genders> _genderRepo;
        private readonly IRegistrationItemsRepo<RegistrationItems> _registrationItemsRepo;
        private readonly IOE_UsersRepo<OE_Users> _oeUsersRepo;
        private readonly IOE_UserAuthenticationsRepo<OE_UserAuthentications> _oE_UserAuthenticationsRepo;
        private readonly ICommonFunctionsServ _commonFunctionsServ;


        #endregion "Variables"

        #region "Constructor"
        public EmployeeDetailsServ(
            ICommonFunctionsServ commonFunctionsServ,
            IEmployeeDetailsRepo<EmployeeDetails> employeeDetailsRepo,
             IEmployeeDesignationsRepo<EmployeeDesignations> employeeDesignationsRepo,
            IEmployeesRepo<Employees> employeesRepo,
            ICOM_GendersRepo<COM_Genders> genderRepo,
            IRegistrationItemsRepo<RegistrationItems> registrationItemsRepo,
            IOE_UsersRepo<OE_Users> oeUsersRepo,
            IOE_UserAuthenticationsRepo<OE_UserAuthentications> oE_UserAuthenticationsRepo
        )
        {
            _commonFunctionsServ = commonFunctionsServ;
            _employeeDetailsRepo = employeeDetailsRepo;
            _employeeDesignationsRepo = employeeDesignationsRepo;
            _employeeDetailsRepo = employeeDetailsRepo;
            _employeesRepo = employeesRepo;
            _genderRepo = genderRepo;
            _registrationItemsRepo = registrationItemsRepo;
            _oeUsersRepo = oeUsersRepo;
            _oE_UserAuthenticationsRepo = oE_UserAuthenticationsRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public IEnumerable<EmployeeDetails> GetEmployeeDetails()
        {
            var queryAll = _employeeDetailsRepo.GetAll();
            var query = from e in queryAll
                        select e;
            return query;
        }
        public EmployeeDetails GetEmployeeDetailsById(long id)
        {
            var queryAll = _employeeDetailsRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public void InsertEmployeeDetails(EmployeeDetails employeeDetails, IFormFile file, string webRoot, long? regIemTypeId)
        {
            _employeeDetailsRepo.Insert(employeeDetails);
            var fetchLastRecord = _employeeDetailsRepo.GetAll().Last();
            var msg = (dynamic)null; 
            if (file != null && regIemTypeId == 1)
            {
                string dbPath = "ClientDictionary/EmployeeDetails/FilePathValue/";
                string ext = Path.GetExtension(file.FileName);
                ext = ext.Remove(0, 1);
                if (Comm_FileSave(fetchLastRecord.Id, file, webRoot, dbPath, ext))
                {
                    msg = "File successfully saved.";
                    fetchLastRecord.FilePathValue = dbPath + fetchLastRecord.Id + "." + ext;
                }
                else { msg = "Error occured while saving file."; }
            }
            if (file != null && regIemTypeId == 2)
            {
                string dbPath = "ClientDictionary/EmployeeDetails/ImagePathValue/";
                string ext = ".png";
                if (Comm_ImageFormat(employeeDetails.Id.ToString(), file, webRoot, dbPath, 300, 200, ext))
                {
                    msg = "File successfully saved.";
                }
                else { msg = "Error occured while saving file."; }
                fetchLastRecord.ImagePathValue = dbPath + fetchLastRecord.Id + ext;
            }
            _employeeDetailsRepo.Update(fetchLastRecord);
        }

        public void UpdateEmployeeDetails(EmployeeDetails employeeDetails, IFormFile file, string webRoot, long? regIemTypeId)
        {
            var msg = (dynamic)null;
            if (file != null && regIemTypeId == 1)
            {
                string dbPath = "ClientDictionary/EmployeeDetails/FilePathValue/";
                string ext = Path.GetExtension(file.FileName);
                ext = ext.Remove(0, 1);
                var previousFile = Path.Combine(webRoot, employeeDetails.FilePathValue == null ? "" : employeeDetails.FilePathValue);
                if (File.Exists(previousFile))
                {
                    File.Delete(previousFile);
                }
                if (Comm_FileSave(employeeDetails.Id, file, webRoot, dbPath, ext))
                {
                    msg = "File successfully saved.";
                    employeeDetails.FilePathValue = dbPath + employeeDetails.Id + "." + ext;
                }
                else { msg = "Error occured while saving file."; }
            }
            if (file != null && regIemTypeId == 2)
            {
                string dbPath = "ClientDictionary/EmployeeDetails/ImagePathValue/";
                string ext = ".png";

                if (Comm_ImageFormat(employeeDetails.Id.ToString(), file, webRoot, dbPath, 600, 400, ext))
                {
                    msg = "File successfully saved.";
                }
                else { msg = "Error occured while saving file."; }
                employeeDetails.ImagePathValue = dbPath + employeeDetails.Id + ext;
            }
            _employeeDetailsRepo.Update(employeeDetails);
        }
        public string UpdateEmployeeDetailsByAdmin(UpdateEmployeeDetailsByAdmin emp)
        {
            var returnResult = (dynamic)null;
            if (emp != null)
            {
                var getAuthentications = _oE_UserAuthenticationsRepo.GetAll();
                if (emp.Employees != null)
                {
                    var PrimaryKey = emp.Employees.Id;
                    //[Shariful-26-6-19]
                    //var ImagePath = "ClientDictionary/Employees/IP300X200/";
                    //var currentIP300X200 = ImagePath + PrimaryKey + ".jpg";
                    //[~Shariful-26-6-19]


                    var employee = _employeesRepo.Get(PrimaryKey);
                    if (emp.OeUsers.UserLoginId != null)
                    {
                        if (_commonFunctionsServ.Function_OeUserHasUserLoginId(emp.OeUsers.UserLoginId) == true)
                        {
                            var getUserId = _oeUsersRepo.GetAll().Where(u => u.UserLoginId == emp.OeUsers.UserLoginId).SingleOrDefault();
                            if (employee.UserId != getUserId.Id)
                            {

                                if (_commonFunctionsServ.Function_IsEmployeeHasOeId(getUserId.Id, emp.CurrentInstitutionId) == true)
                                {
                                    emp.Message = "OurEdu Id is being used by another employee.";
                                    return returnResult = emp.Message;
                                }
                                else
                                {
                                    //[NOTE: Remove previous userId's authentications while userLoginId is update]
                                    var filteredAuthentications = (from au in getAuthentications
                                                                   where au.UserId == employee.UserId && au.InstitutionId == emp.CurrentInstitutionId
                                                                   select au).ToList();
                                    if (filteredAuthentications != null)
                                    {
                                        foreach (var item in filteredAuthentications)
                                        {
                                            _oE_UserAuthenticationsRepo.Delete(item);
                                        }
                                    }
                                    employee.UserId = getUserId.Id;
                                }
                            }
                        }
                        else
                        {
                            emp.Message = "OurEdu Id is invalid.";
                            return returnResult = emp.Message;
                        }
                    }
                    else
                    {
                        if (employee.UserId != null)
                        {
                            //[NOTE: Remove userId's authentications while userLoginId is null]
                            var filteredAuthentications = (from au in getAuthentications
                                                           where au.UserId == employee.UserId && au.InstitutionId == emp.CurrentInstitutionId
                                                           select au).ToList();
                            if (filteredAuthentications != null)
                            {
                                foreach (var item in filteredAuthentications)
                                {
                                    _oE_UserAuthenticationsRepo.Delete(item);
                                }
                                employee.UserId = (dynamic)null;
                            }
                        }

                    }
                    employee.FirstName = emp.Employees.FirstName;
                    employee.LastName = emp.Employees.LastName;
                    employee.GenderId = emp.Employees.GenderId;
                    employee.DOB = emp.Employees.DOB;
                    employee.EmailAddress = emp.Employees.EmailAddress;
                    employee.ContactNo = emp.Employees.ContactNo;
                    //[Shariful-26-6-19]
                    //employee.IP300X200 = currentIP300X200;
                    //[~Shariful-26-6-19]

                    employee.PresentAddress = emp.Employees.PresentAddress;
                    employee.PermanentAddress = emp.Employees.PermanentAddress;
                    employee.JoiningDate = emp.Employees.JoiningDate;
                    employee.IsActive = emp.Employees.IsActive;
                    //[Shariful-26-6-19]
                    //_employeesRepo.Update(employee);
                    //[~Shariful-26-6-19]

                    //[Note: Update static Image code is missing.]  
                    if (emp.ProfileImage != null)
                    {
                        //[Shariful3-26-6-19]
                        var ImagePath = "ClientDictionary/Employees/IP300X200/";
                        var currentIP300X200 = ImagePath + PrimaryKey + ".jpg";
                        employee.IP300X200 = currentIP300X200;
                        //[~Shariful3-26-6-19]
                        string msg = (dynamic)null;
                        if (Comm_ImageFormat(employee.Id.ToString(), emp.ProfileImage, emp.WebRootPath, ImagePath, 300, 200, ".jpg"))
                            msg = "Image Saved.";
                        else
                            msg = "Image is not saved.";
                    }
                    //[Shariful2-26-6-19]
                    _employeesRepo.Update(employee);
                    //[~Shariful2-26-6-19]
                }


                if (emp.Designations != null)
                {
                    var designation = _employeeDesignationsRepo.GetAll().Where(d => d.EmployeeId == emp.Employees.Id).SingleOrDefault();
                    //[Note: if already designation exist. -> Update ]
                    if (designation != null)
                    {
                        designation.EmployeeTypeId = emp.Designations.EmployeeTypeId;
                        designation.EmployeeTypeCategoryId = emp.Designations.EmployeeTypeCategoryId != null || emp.Designations.EmployeeTypeCategoryId != 0 ? emp.Designations.EmployeeTypeCategoryId : (dynamic)null;
                        designation.ModifiedBy = emp.Designations.ModifiedBy;
                        designation.ModifiedDate = CommDate_ConvertToUtcDate(DateTime.Now);
                        _employeeDesignationsRepo.Update(designation);
                    }
                    //[Note: if designation is not exist. -> create one ]
                    else
                    {
                        var newDesignation = new EmployeeDesignations()
                        {
                            InstitutionId = emp.Designations.InstitutionId,
                            EmployeeId = emp.Employees.Id,
                            EmployeeTypeId = emp.Designations.EmployeeTypeId,
                            EmployeeTypeCategoryId = emp.Designations.EmployeeTypeCategoryId,
                            StartDate = CommDate_ConvertToUtcDate(DateTime.Now),
                            IsActive = true,
                            AddedBy = emp.Designations.AddedBy,
                            AddedDate = CommDate_ConvertToUtcDate(DateTime.Now),
                            InsId = emp.Designations.InstitutionId
                        };
                        _employeeDesignationsRepo.Insert(newDesignation);
                    }
                }
                
            }
            return returnResult;
        }


        public void DeleteEmployeeDetails(EmployeeDetails employeeDetails)
        {
            _employeeDetailsRepo.Delete(employeeDetails);
        }
        public void DeleteStaticFile(EmployeeDetails emp, string rootPath, long? regItemTypeId)
        {
            var msg = (dynamic)null;
            if (regItemTypeId == 1)
            {
                if (DelFileFromLocation(Path.Combine(rootPath, emp.FilePathValue)))
                {
                    msg = "File deleted.";
                    emp.FilePathValue = (dynamic)null;
                    _employeeDetailsRepo.Update(emp);
                }
                else
                    msg = "Error Occured.";
            }
            if (regItemTypeId == 2)
            {
                if (DelFileFromLocation(Path.Combine(rootPath, emp.ImagePathValue)))
                {
                    msg = "File deleted.";
                    emp.ImagePathValue = (dynamic)null;
                    _employeeDetailsRepo.Update(emp);
                }
                else
                    msg = "Error Occured.";
            }
        }

        #endregion "Insert Update Delete Methods"        
    }
}


