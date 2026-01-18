using System;
using System.Linq;
using OE.Data;
using OE.Repo;

namespace OE.Service
{
    public class CommonFunctionsServ : ICommonFunctionsServ
    {
        #region "Variables"
        private readonly IOE_UsersRepo<OE_Users> _oeUsersRepo;
        private readonly IEmployeesRepo<Employees> _employeesRepo;
        private readonly IStudentsRepo<Students> _studentsRepo;
        private readonly IStudentPromotionsRepo<StudentPromotions> _studentPromotionsRepo;
        #endregion "Variables"

        #region "Constructor"
        public CommonFunctionsServ(
            IOE_UsersRepo<OE_Users> oeUsersRepo,
            IEmployeesRepo<Employees> employeesRepo,
            IStudentsRepo<Students> studentsRepo,
            IStudentPromotionsRepo<StudentPromotions> studentPromotionsRepo
         )
        {
            _studentsRepo = studentsRepo;
            _oeUsersRepo = oeUsersRepo;
            _employeesRepo = employeesRepo;
            _studentPromotionsRepo = studentPromotionsRepo;
        }
        #endregion "Constructor"


        #region "OE_UsersServ"     
        public Boolean Function_OEUsers_IsUserAsEmployee(string userLoginId)
        {
            Boolean retrunResult = false;
            var oeUsers = _oeUsersRepo.GetAll();
            var emp = _employeesRepo.GetAll();
            var oeUser = (dynamic)null;
            var employee = (dynamic)null;

            //[NOTE: check user is available or not from 'OE_Users'entity"]
            if (!string.IsNullOrEmpty(userLoginId))
            {
                oeUser = (from u in oeUsers
                          where u.UserLoginId == userLoginId
                          select u).FirstOrDefault();
            }

            //[NOTE: check employee is available or not from 'Employee'entity"]
            if (oeUser != null)
            {
                if (oeUser.Id != 0)
                {
                    employee = (from e in emp
                                where e.UserId == oeUser.Id
                                select e).FirstOrDefault();
                }
            }

            if (employee != null)
                retrunResult = true;
            else
                retrunResult = false;


            return retrunResult;
        }
       
        public bool Function_OeUserHasUserLoginId(string userLoginId)
        {
            bool retrunResult = false;
            var oeUsers = _oeUsersRepo.GetAll();

            //[NOTE: check user is available or not from 'OE_Users'entity"]
            if (!string.IsNullOrEmpty(userLoginId))
            {
                var oeUser = (from u in oeUsers
                              where u.UserLoginId == userLoginId
                              select u).SingleOrDefault();
                if (oeUser != null)
                {
                    return retrunResult = true;
                }
            }
            return retrunResult;
        }
        
        #endregion "OE_UsersServ"

        #region "Employees"
        public bool Function_IsEmployeeHasOeId(long oEId, long instituteId)
        {
            bool retrunResult = false;
            var emp = _employeesRepo.GetAll();

            //[NOTE: check user is available or not from 'OE_Users'entity"]
            var empHasUserId = (from e in emp
                                where e.UserId == oEId && e.InstitutionId == instituteId
                                select e).SingleOrDefault();
            if (empHasUserId != null)
            {
                retrunResult = true;
            }
            return retrunResult;
        }
        #endregion "Employees"

        #region "Students"
        public bool Function_IsStudentHasOeId(long oEId, long instituteId)
        {
            bool retrunResult = false;
            var stu = _studentsRepo.GetAll();

            //[NOTE: check user is available or not from 'OE_Users'entity"]
            var stuHasUserId = (from e in stu
                                where e.UserId == oEId && e.InstitutionId == instituteId
                                select e).SingleOrDefault();
            if (stuHasUserId != null)
            {
                retrunResult = true;
            }
            return retrunResult;
        }
        //[NOTE: common function for checking student roll no. is existing or not]
        public bool Function_IsStudentHasExistingRollNo(long classId, long year, long rollNo)
        {
            bool returnResult = false;
            var stdPromotions = _studentPromotionsRepo.GetAll();

            var result = (from s in stdPromotions
                          where s.ClassId == classId && s.Year.Year == year && s.RollNo == rollNo
                          select s).SingleOrDefault();
            if (result != null)
            {
                returnResult = true;
            }
            return returnResult;
        }
        #endregion "Students"

    }
}
