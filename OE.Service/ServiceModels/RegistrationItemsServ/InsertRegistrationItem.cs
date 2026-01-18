using OE.Data;

namespace OE.Service.ServiceModels
{
    public class InsertRegistrationItem
    {
        public long InstitutionId { get; set; }

        public long RegistrationItemId { get; set; }
        public string RegistrationItemName { get; set; }

        public long RegistrationGroupId { get; set; }
        public string RegistrationGroupsName { get; set; }

        public long RegistrationItemTypeId { get; set; }
        public string RegistrationItemTypeName { get; set; }
        public long RegistrationUserTypeId { get; set; }

        public RegistrationItems RegistrationItems { get; set; }
        public StudentDetails studentDetails { get; set; }
        public EmployeeDetails employeeDetails { get; set; }
    }
}
