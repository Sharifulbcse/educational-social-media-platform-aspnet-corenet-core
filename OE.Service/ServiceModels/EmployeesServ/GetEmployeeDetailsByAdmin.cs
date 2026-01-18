using OE.Data;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetEmployeeDetailsByAdmin
    {
         public Employees Employees { get; set; }
         public OE_Users OEUsers { get; set; }
         public COM_Genders ComGenders { get; set; }
        public EmployeeDesignations EmployeeDesignations { get; set; }
        public EmployeeTypes EmployeeTypes { get; set; }
        public EmployeeTypeCategories EmployeeTypeCategories { get; set; }
    }
}
