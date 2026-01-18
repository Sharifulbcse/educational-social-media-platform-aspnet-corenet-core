using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Http;

namespace OE.Web.Areas.Institution.Models
{
    public class EmployeeListVM
    {
        public Int64 Id { get; set; }
        public long GenderId { get; set; }
        public long? UserId { get; set; }
        public long InstitutionId { get; set; }
        public long InsId { get; set; }

        public Int64 DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public long? EmployeeTypeId { get; set; }
        public string EmployeeTypeName { get; set; }       
        public string Designation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IP300X200 { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string ContactNo { get; set; }
        public string EmailAddress { get; set; }
        public Int64 AddedBy { get; set; }
        public DateTime? AddedDate { get; set; }
        public Int64 ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string DataType { get; set; }        
        public DateTime DOB { get; set; }        
        public DateTime JoiningDate { get; set; }        
        public bool? IsActive { get; set; }

        //[NOTE:Extra fields from 'OE_Users' entity]
        public string UsersIP300X200 { get; set; }
        public string UserLoginId { get; set; }

        //[NOTE:Extra fields from 'Genders' entity]
        public string GenderName { get; set; }

        //[NOTE: Extra fileds]        
        public Boolean? IsLoginAccount { get; set; }
        public IFormFile EmployeeImage { get; set; }
        //[NOTE: "ActualFile" property must be removed and replace "EmployeeImage" property] 
        public IFormFile ActualFile { get; set; }

        //[NOTE:Extra fields from 'EmployeeDesignations' entity]
        public long? EmployeeCategoryTypeId { get; set; }
        public string EmployeeCategoryTypeName { get; set; }


    }
}
