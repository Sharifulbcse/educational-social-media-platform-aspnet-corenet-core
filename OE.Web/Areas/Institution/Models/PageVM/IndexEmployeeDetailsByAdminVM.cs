using Microsoft.AspNetCore.Http;
namespace OE.Web.Areas.Institution.Models
{
    public class IndexEmployeeDetailsByAdminVM
    {
        public string InstitutionName { get; set; }
        public EmployeeListVM employees { get; set; }
        public EmployeeDesignationsListVM DesignationsListVM { get; set; }

        //[NOTE: Extra Fields]
        public IFormFile ProfileImage { get; set; }
        public int authorizeStatus { get; set; }
    }
}
