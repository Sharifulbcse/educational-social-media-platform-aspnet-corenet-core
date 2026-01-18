using System;
using OE.Data;
using System.Collections.Generic;

namespace OE.Service.CustomEntitiesServ
{
    public class C_AssignTeachers
    {
        public long Id { get; set; }
        public long ClassId { get; set; }
        public long AssignCourseId { get; set; }
        public long AssignSectionId { get; set; }
        public long InstitutionId { get; set; }
        public DateTime Year { get; set; }

        //[NOTE: Extra fiends from 'Subjects'Entity]
        public long CourseId { get; set; }
        public string CourseName { get; set; }

        //[NOTE: Extra fiends from 'AssignSections'Entity]
        public string AssignSectionName { get; set; }

        //[NOTE: Extra fiends from 'Clases'Entity]
        public string ClassName { get; set; }
    }
}
