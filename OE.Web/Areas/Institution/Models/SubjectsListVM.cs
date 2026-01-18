using System;


namespace OE.Web.Areas.Institution.Models
{
    public class SubjectsListVM
    {
        public Int64 Id { get; set; }
        public string Name { get; set; }
        public Int64 ClassId { get; set; }
        public string ClassName { get; set; }
       
        public Int64 SubjectTypeId { get; set; }
        public string SubjectTypeName { get; set; }

        
        
    }
}
