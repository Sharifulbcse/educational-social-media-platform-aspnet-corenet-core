using System;
using OE.Data;
using System.Collections.Generic;

namespace OE.Service.CustomEntitiesServ
{
   public class C_Employees
    {
        public long Id { get; set; }
        public long GenderId { get; set; }
        public long? UserId { get; set; }
        public long InstitutionId { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IP300X200 { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string ContactNo { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DOB { get; set; }
        public DateTime JoiningDate { get; set; }

        public long InsId { get; set; }

        //[NOTE: Extra fiends from 'Genders'Entity]
        public string GenderName { get; set; }

        //[NOTE: Extra fiends from 'OE_Users'Entity]
        public string UsersIP300X200 { get; set; }
        public string UserLoginId { get; set; }

        //[NOTE: Extra fiends from 'EmployeeDesignations'Entity]
        public long? EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; }
        public long? EmployeeCategoryTypeId { get; set; }
        public string EmployeeCategoryTypeName { get; set; }
    }
}
