
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc.Rendering;

namespace OE.Web.Areas.Institution.Models
{
    public class IndexResultsVM
    {
        public string InstitutionName { get; set; }
        public long EmployeeId { get; set; }
        public long StudentId { get; set; }
        public long changeYear { get; set; }
        public long InstitutionId { get; set; }
        public long MarkTypeId { get; set; }
        public long Mark { get; set; }

        public List<ResultsListVM> resultsListVM { get; set; }

        public SelectList _students{ get; set; }
        public SelectList _classes{ get; set; }
        public SelectList _xmTyp { get; set; }
        public SelectList _subject { get; set; }
        public SelectList _mrkTyp { get; set; }
        public SelectList _employee { get; set; }
       
        public List<ExamTypeListVM> _ExamTypes { get; set; }
        public List<MarkTypesListVM> _MarkTypes { get; set; }
        public List<DistributionMarksListVM> _DistributionMarks { get; set; }
        public List<GradeTypesListVM> _GradeTypes { get; set; }
        public List<AssignedStudentsListVM> _AssignedStudents { get; set; }
        public List<ResultsListVM> _Results { get; set; }

        public long SectionId { get; set; }
        public long SubjectId { get; set; }
        public long SelectedYear { get; set; }

        //[NOTE: Extra field from Employee entity]
        public string EmployeeName { get; set; }
        public long CurrentYear { get; set; }
        public long ExamTypeId { get; set; }

        //[NOTE: Extra Fields from AssignedTeachers]
        public long ClassId { get; set; }        
        public string SubjectName { get; set; }

        //[NOTE: Extra field from student entity]
        public string StudentName { get; set; }

        //[NOTE:Extra field from Class entity]
        public string ClassName { get; set; }

        //[NOTE:extra field from Examtype entity]
        public string ExamTypeName { get; set; }

        //[NOTE:extra field from AssignedCourses entity]
        public long AssignedCourseId { get; set; }

    }
}


