using OE.Data;
using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetAssignedOrUnassignedStudents : MessageModel
    {
        public Classes Classes { get; set; }
        public Subjects Subjects { get; set; }
        public C_AssignedCourses AssignedCourses { get; set; }
        public AssignedSections AssignedSections { get; set; }
        public List<C_StudentPromotions> _StudentPromotions { get; set; }
        
        //[NOTE: Extra Fields]
        public long Year { get; set; }
    }

}

