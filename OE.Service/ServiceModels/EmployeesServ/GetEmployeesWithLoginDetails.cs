
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetEmployeesWithLoginDetails
    {
        public Employees Employees { get; set; }
        public EmployeeTypes EmployeeTypes { get; set; }
        public Departments Departments { get; set; }
        
    }
}
