using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class RegistrationItemsServ : IRegistrationItemsServ
    {

        #region "Variables"
        private IRegistrationItemTypesRepo<COM_RegistrationItemTypes> _comRegistrationItemTypesRepo;

        private IRegistrationItemsRepo<RegistrationItems> _registrationItemsRepo;
        private IRegistrationGroupsRepo<RegistrationGroups> _registrationGroupsRepo;
        
        private IStudentDetailsRepo<StudentDetails> _studentDetailsRepo;
        private IEmployeeDetailsRepo<EmployeeDetails> _employeeDetailsRepo;
        #endregion "Variables"

        #region "Constructor"
        public RegistrationItemsServ(
            IRegistrationItemsRepo<RegistrationItems> registrationItemsRepo, 

            IRegistrationItemTypesRepo<COM_RegistrationItemTypes> comRegistrationItemTypesRepo, 
            IRegistrationGroupsRepo<RegistrationGroups> registrationGroupsRepo,
            IStudentDetailsRepo<StudentDetails> studentDetailsRepo,
            IEmployeeDetailsRepo<EmployeeDetails> employeeDetailsRepo

            )
        {
            _registrationItemsRepo = registrationItemsRepo;
            _comRegistrationItemTypesRepo = comRegistrationItemTypesRepo;
            _registrationGroupsRepo = registrationGroupsRepo;           
            _studentDetailsRepo = studentDetailsRepo;
            _employeeDetailsRepo = employeeDetailsRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public RegistrationItems GetRegistrationItemById(long Id)
        {
            var queryAll = _registrationItemsRepo.GetAll();

            var query = (from ri in queryAll
                         where ri.Id == Id
                         select ri).FirstOrDefault();
            return query;
        }
        public IEnumerable<GetRegItems> GetRegItems(long institutionId, long regUserTypeId = 0)
        {
            var regItms = _registrationItemsRepo.GetAll().ToList();
            var regITypes = _comRegistrationItemTypesRepo.GetAll().ToList();
            var regGroups = _registrationGroupsRepo.GetAll().ToList();
            var jointQuery = (dynamic)null;
            if (regUserTypeId == 0)
            {
                jointQuery = from ri in regItms
                             where ri.InstitutionId == institutionId
                             join rit in regITypes
                             on ri.RegistrationItemTypeId equals rit.Id
                             join rg in regGroups
                             on ri.RegistrationGroupId equals rg.Id
                             select new { ri, rit, rg };
            }
            else
            {
                jointQuery = from ri in regItms
                             where ri.InstitutionId == institutionId && ri.RegistrationUserTypeId == regUserTypeId
                             join rit in regITypes
                             on ri.RegistrationItemTypeId equals rit.Id
                             join rg in regGroups
                             on ri.RegistrationGroupId equals rg.Id
                             select new { ri, rit, rg };
            }
            var queryResult = new List<GetRegItems>();
            foreach (var item in jointQuery)
            {
                var obj = new GetRegItems()
                {
                    RegistrationItemId = item.ri.Id,
                    RegistrationItemName = item.ri.Name,
                    RegistrationGroupId = item.rg.Id,
                    RegistrationGroupsName = item.rg.Name,
                    RegistrationItemTypeId = item.rit.Id,
                    RegistrationItemTypeName = item.rit.Name,
                    RegistrationUserTypeId = item.ri.RegistrationUserTypeId
                };
                queryResult.Add(obj);
            }
            return queryResult;
        }
        public IEnumerable<GetRegItems> GetRegItemsWithDetails(long instituteId, long studentOrEmployeeId, long regUerType)
        {           
            var regItem = _registrationItemsRepo.GetAll();
            var jointQry = (dynamic)null;
            var returnQry = new List<GetRegItems>();
            if (regUerType == 1)//[NOTE:For Student Type]
            {
                var studentDetails = _studentDetailsRepo.GetAll();
                jointQry = from r in regItem
                           join sd in studentDetails on r.Id equals sd.RegistrationItemId
                           where sd.StudentId == studentOrEmployeeId && sd.InstitutionId == instituteId && r.RegistrationUserTypeId == regUerType
                           select new { r, sd };
                foreach (var item in jointQry)
                {
                    var temp = new GetRegItems()
                    {
                        RegistrationItemId = item.r.Id,
                        RegistrationItemName = item.r.Name,
                        studentDetails = item.sd,
                        RegistrationItems = item.r
                    };
                    returnQry.Add(temp);
                }
            }
            if (regUerType == 2)//[NOTE:For Employee Type]
            {
                var empDetails = _employeeDetailsRepo.GetAll();
                jointQry = from r in regItem
                           join sd in empDetails on r.Id equals sd.RegistrationItemId
                           where sd.EmployeeId == studentOrEmployeeId && sd.InstitutionId == instituteId && r.RegistrationUserTypeId == regUerType
                           select new { r, sd };
                foreach (var item in jointQry)
                {
                    var temp = new GetRegItems()
                    {
                        RegistrationItemId = item.r.Id,
                        RegistrationItemName = item.r.Name,
                        employeeDetails = item.sd,
                        RegistrationItems = item.r
                    };
                    returnQry.Add(temp);
                }
            }

            return returnQry;
           

        }

        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertRegistrationItem(InsertRegistrationItem obj)
        {

            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'RegistrationGroup' table]
                    if (obj.RegistrationItems != null)
                    {
                        var regItem = new RegistrationItems()
                        {
                            Name = obj.RegistrationItems.Name,
                            RegistrationGroupId = obj.RegistrationItems.RegistrationGroupId,
                            RegistrationUserTypeId = obj.RegistrationItems.RegistrationUserTypeId,
                            RegistrationItemTypeId = obj.RegistrationItems.RegistrationItemTypeId,
                            InstitutionId = obj.RegistrationItems.InstitutionId,
                            IsActive = true,
                            AddedBy = obj.RegistrationItems.AddedBy,
                            AddedDate = DateTime.Now

                        };

                        _registrationItemsRepo.Insert(regItem);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:RegistrationItemsServ/InsertRegistrationItems - " + ex.Message;
            }

            return returnResult;

        }
        public void UpdateRegistrationItem(RegistrationItems registrationItem)
        {
            _registrationItemsRepo.Update(registrationItem);
        }
        public void DeleteRegistrationItem(RegistrationItems registrationItem)
        {
            _registrationItemsRepo.Delete(registrationItem);
        }
        #endregion "Insert Update Delete Methods"        
    }
}
