
using System;

namespace OE.Web.Areas.Institution.Models
{
    public class ClassesListVM
    {        
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public Int64 Sorting { get; set; }
        public Int64 InsCategoryId { get; set; }
        public string InsCategoryName { get; set; }
        public Int64 DepartmentId { get; set; }
        public string DepartmentName { get; set; }
    }
}
