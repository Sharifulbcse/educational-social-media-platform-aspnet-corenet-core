
using OE.Data;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetEmployeeDetails
    {
        public long InstitutionId { get; set; }      
        public long? EmployeeUserId { get; set; }
        public long RegistrationItemId { get; set; }
        public string RegistrationItemName { get; set; }
        public long RegistrationGroupId { get; set; }
        public string RegistrationGroupsName { get; set; }        
        public long RegistrationItemTypeId { get; set; }
        public string RegistrationItemTypeName { get; set; }
        public long RegistrationUserTypeId { get; set; }

        public StudentDetails studentDetails { get; set; }
        public COM_RegistrationUserTypes COM_RegistrationUserTypes { get; set; }
        public RegistrationGroups RegistrationGroups { get; set; }       
        public GetEmployeeDetails EmployeeDetails { get; set; }
        public Employees employees { get; set; }
        public COM_Genders genders { get; set; }
        public EmployeeDetails employeeDetails { get; set; }
        public RegistrationItems registrationItems { get; set; }
        public OE_Users Users { get; set; }
        public EmployeeTypes EmployeeTypes { get; set; }
        public EmployeeTypeCategories EmployeeTypeCategories { get; set; }

        public List<GetEmployeeDetails> RegGroup { get; set; }
        public List<GetEmployeeDetails> RegItemWithDetails { get; set; }
        public List<GetEmployeeDetails> RegItems { get; set; }
              
    }
}
