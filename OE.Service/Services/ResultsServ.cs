
using OE.Data;
using OE.Repo;
using OE.Service.CustomEntitiesServ;
using OE.Service.ServiceModels;


using System.Collections.Generic;
using System.Linq;

namespace OE.Service
{
    public class ResultsServ : IResultsServ
    {
        #region "Variables"
        private readonly IAssignedCoursesRepo<AssignedCourses> _assignedCoursesRepo;
        private readonly IAssignedStudentsRepo<AssignedStudents> _assignedStudentsRepo;
        private readonly IAssignedTeachersRepo<AssignedTeachers> _assignedTeachersRepo;

        private readonly IClassesRepo<Classes> _classesRepo;

        private readonly IDistributionMarksRepo<DistributionMarks> _distributionMarksRepo;

        private readonly IGradeTypesRepo<GradeTypes> _gradeTypesRepo;

        private readonly IEmployeesRepo<Employees> _employeesRepo;
        private readonly IExamTypesRepo<ExamTypes> _examTypesRepo;

        private readonly IMarkTypesRepo<MarkTypes> _markTypesRepo;

        private readonly IResultsRepo<Results> _resultsRepo;

        private readonly IStudentsRepo<Students> _studentsRepo;
        private readonly ISubjectsRepo<Subjects> _subjectsRepo;  
        private readonly ISubjectTypesRepo<SubjectTypes> _subjectTypesRepo;
    
        #endregion "Variables"

        #region "Constructor"
        public ResultsServ(
            IStudentsRepo<Students> studentsRepo, 
            IEmployeesRepo<Employees> employeesRepo, 
            IClassesRepo<Classes> classesRepo, 

            IExamTypesRepo<ExamTypes> examTypesRepo, 
            ISubjectsRepo<Subjects> subjectsRepo, 
            IMarkTypesRepo<MarkTypes> markTypesRepo, 
            IResultsRepo<Results> resultsRepo,
            IAssignedCoursesRepo<AssignedCourses> assignedCoursesRepo,
            IAssignedStudentsRepo<AssignedStudents> assignedStudentsRepo,
            IAssignedTeachersRepo<AssignedTeachers> assignedTeachersRepo,
            IGradeTypesRepo<GradeTypes> gradeTypesRepo,
             ISubjectTypesRepo<SubjectTypes> subjectTypesRepo,
            IDistributionMarksRepo<DistributionMarks> distributionMarksRepo
            )
        {
            _resultsRepo = resultsRepo;
            _studentsRepo = studentsRepo;
            _employeesRepo = employeesRepo;
            _classesRepo = classesRepo;

            _examTypesRepo = examTypesRepo;
            _subjectsRepo = subjectsRepo;
            _markTypesRepo = markTypesRepo;
            _assignedCoursesRepo = assignedCoursesRepo;
            _assignedStudentsRepo = assignedStudentsRepo;
            _assignedTeachersRepo = assignedTeachersRepo;
            _gradeTypesRepo = gradeTypesRepo;
            _subjectTypesRepo = subjectTypesRepo;
            _distributionMarksRepo = distributionMarksRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public IEnumerable<Results> GetResults()
        {
            var queryAll = _resultsRepo.GetAll();
            var returnQuery = from e in queryAll
                                   select e;
            return returnQuery;
        }

        public Results GetResultById(long Id)
        {
            var queryAll = _resultsRepo.GetAll();
            var returnQuery = (from e in queryAll
                         where e.Id == Id
                         select e).SingleOrDefault();
            return returnQuery;
        }

        public Gradings Gradings(long year, long institutionId, long employeeId, long classId, long courseId, long examTypeId)
        {
            //[NOTE: Getting OverView information]
            var emp = _employeesRepo.Get(employeeId);
            var cls = _classesRepo.Get(classId);
            var assCourse = _assignedCoursesRepo.Get(courseId);            
            var sub = _subjectsRepo.Get(assCourse.SubjectId);            
            var xmtyp = _examTypesRepo.Get(examTypeId);
            var markDistributions = _distributionMarksRepo.GetAll();

            //[NOTE: Getting List ExamTypes]
            var lstXm = _examTypesRepo.GetAll().Where(x => x.InstitutionId == institutionId && x.ClassId == classId && x.IsActive == true).ToList();

            //[NOTE: Getting List MarkTypes]
            var lstMrk = _markTypesRepo.GetAll().Where(x => x.InstitutionId == institutionId && x.IsActive == true).ToList();
            var markWithDistributionMark = from m in lstMrk
                                           join dm in markDistributions on m.Id equals dm.MarkTypeId
                                           where dm.InstitutionId == institutionId && dm.SubjectId == sub.Id && dm.ClassId == classId
                                           select new { m, dm };

            //[NOTE: Getting List GradeTypes]
            var lstGrd = _gradeTypesRepo.GetAll().Where(x => x.InstitutionId == institutionId && x.ClassId == classId && x.IsActive == true).ToList();
            //[NOTE: Getting List Results]
            var lstRslt = _resultsRepo.GetAll().Where(x => x.InstitutionId == institutionId && x.EmployeeId == employeeId && x.ClassId == classId && x.SubjectId == sub.Id && x.IsActive == true).ToList();
            var distributionDetails = from md in markDistributions
                                      join m in lstMrk on md.MarkTypeId equals m.Id
                                      where md.ClassId == classId && md.SubjectId == assCourse.SubjectId
                                      select new { md, m };
            var lstMrkDis = new List<C_DistributionMarks>();
            foreach (var item in distributionDetails)
            {
                var temp = new C_DistributionMarks()
                {
                    Id = item.md.Id,
                    SubjectId = item.md.SubjectId,
                    MarkTypeId = item.md.MarkTypeId,

                    BreakDownInP = item.md.BreakDownInP,
                    //EffectiveStartDate = item.md.EffectiveStartDate,
                    //EffectiveEndDate = item.md.EffectiveEndDate,
                    InstitutionId = item.md.InstitutionId,
                    ClassId = item.md.ClassId,
                    MarkTypeName = item.m.Name

                };
                lstMrkDis.Add(temp);
            }

            //[NOTE: Getting List AssignedStudents with Students]
            var AssStu = _assignedStudentsRepo.GetAll();
            var students = _studentsRepo.GetAll();
            var AssStuDetails = from aS in AssStu
                                join s in students on aS.StudentId equals s.Id
                                where aS.InstitutionId == institutionId && aS.ClassId == classId && aS.AssignedCourseId == courseId && aS.Year.Year == year && aS.IsActive == true
                                select new { aS, s };

            var lstAsStu = new List<C_AssignedStudents>();
            foreach (var item in AssStuDetails)
            {
                var temp = new C_AssignedStudents()
                {
                    Id = item.aS.Id,
                    StudentId = item.aS.StudentId,
                    ClassId = item.aS.ClassId,
                    AssignedCourseId = item.aS.AssignedCourseId,
                    StudentName = item.s.Name
                };
                lstAsStu.Add(temp);
            }

            var markList = new List<C_MarkTypes>();
            foreach (var item in markWithDistributionMark)
            {
                var temp = new C_MarkTypes()
                {
                    Id = item.m.Id,
                    Name = item.m.Name,
                    BreakDownInP = item.dm.BreakDownInP
                };
                markList.Add(temp);
            }

            var result = new Gradings()
            {
                Employees = emp,
                Classes = cls,
                AssignedCourses = assCourse,
                Subjects = sub,
                ExamTypes = xmtyp,
                _ExamTypes = lstXm,
                _MarkTypes = markList,
                _DsitributionMarks = lstMrkDis,
                _GradeTypes = lstGrd,
                _Results = lstRslt,
                _AssignedStudents = lstAsStu
            };
            return result;
        }

        public GetStudentResultSheet GetStudentResultSheet(long institutionId, long year, long studentId, long examTypeId)
        {
            var results = _resultsRepo.GetAll();
            var subjects = _subjectsRepo.GetAll();
            var markTypes = _markTypesRepo.GetAll();

            var agnCourses = _assignedCoursesRepo.GetAll();
            var agnStudents = _assignedStudentsRepo.GetAll();

            //[NOTE:Need for single record]
            var students = _studentsRepo.GetAll();
            var examType = _examTypesRepo.GetAll();
            var clas = _classesRepo.GetAll();

            //[NOTE:need for getting grade]
            var gradeType = _gradeTypesRepo.GetAll();

            var queryResult = from r in results
                              where r.InstitutionId == institutionId && r.Year.Year == year && r.StudentId == studentId && r.ExamTypeId == examTypeId
                              select new { r };

            var assignedStudent = from s in agnStudents
                                  where institutionId == s.InstitutionId && studentId == s.StudentId && year == s.Year.Year
                                  join agnC in agnCourses on s.AssignedCourseId equals agnC.Id
                                  join sub in subjects on agnC.SubjectId equals sub.Id
                                  select new { s, agnC, sub };


            var queryStudentWithClass = (from s in students
                                         where s.InstitutionId == institutionId && s.Id == studentId
                                         join c in clas on s.ClassId equals c.Id
                                         select new { s, c }).SingleOrDefault();

            var queryExamType = (from et in examType
                                 where et.Id == examTypeId && et.InstitutionId == institutionId
                                 select et).SingleOrDefault();


            var resultList = new List<C_AssignedStudents>();
            foreach (var item in assignedStudent)
            {
                var result = new C_AssignedStudents();
                result.SubjectName = item.sub.Name;

                foreach (var mrk in markTypes)
                {
                    var mark = (from re in queryResult
                                where re.r.MarkTypeId == mrk.Id && re.r.SubjectId == item.agnC.SubjectId
                                select re).SingleOrDefault();
                    result.TotalMark += mark == null ? 0 : mark.r.Mark;
                }
                foreach (var grd in gradeType)
                {
                    if (result.TotalMark >= grd.StartMark && result.TotalMark <= grd.EndMark)
                    {
                        result.Grade = grd.Grade;
                    }
                }
                resultList.Add(result);
            }
            var returnQry = new GetStudentResultSheet()
            {
                _CourseResults = resultList,
                _StudentId = queryStudentWithClass.s.Id,
                _StudentName = queryStudentWithClass.s.Name,
                _ClassName = queryStudentWithClass.c.Name,
                _ExamTypeId = queryExamType.Id,
                _ExamTypeName = queryExamType.Name,
                _ResultSearchYear = year
            };

            return returnQry;
        }
        public IEnumerable<GetStuClsXmTypSubMrkTyp> GetStuClsXmTypSubMrkTyps(long institutionId)
        {
            var getResults = _resultsRepo.GetAll().ToList();
            var getStudents = _studentsRepo.GetAll().ToList();
            var getClasses = _classesRepo.GetAll().ToList();
            var getXmTyp = _examTypesRepo.GetAll().ToList();
            var getSubjects = _subjectsRepo.GetAll().ToList();
            var getMrkTyp = _markTypesRepo.GetAll().ToList();

            var jointQuery = from r in getResults
                             where r.InstitutionId == institutionId
                             join s in getStudents on r.StudentId equals s.Id
                             join c in getClasses on r.ClassId equals c.Id
                             join x in getXmTyp on r.ExamTypeId equals x.Id
                             join su in getSubjects on r.SubjectId equals su.Id
                             join m in getMrkTyp on r.MarkTypeId equals m.Id
                             select new { r, s, c, x, su, m };

            var queryResult = new List<GetStuClsXmTypSubMrkTyp>();
            foreach (var item in jointQuery)
            {
                var obj = new GetStuClsXmTypSubMrkTyp()
                {
                    Results = item.r,
                    Students = item.s,
                    Classes = item.c,
                    ExamTypes = item.x,
                    Subjects = item.su,
                    MarkTypes = item.m
                };
                queryResult.Add(obj);
            }


            return queryResult;
        }
        public IEnumerable<ResultSheet> ResultSheet(long institutionId, int year, long studentId, long classId, long examTypeId)
        {
            var results = _resultsRepo.GetAll().ToList();
            var students = _studentsRepo.GetAll().ToList();
            var employees = _employeesRepo.GetAll().ToList();
            var classes = _classesRepo.GetAll().ToList();
            var markTypes = _markTypesRepo.GetAll().ToList();
            var examTypes = _examTypesRepo.GetAll().ToList();
            var subjects = _subjectsRepo.GetAll().ToList();
            var jointQuery = (dynamic)null;

            if (year == 0 && studentId == 0 && classId == 0 && examTypeId == 0)
            {
                jointQuery = from r in results
                             where r.InstitutionId == institutionId
                             orderby r.Year descending
                             orderby r.ClassId ascending
                             join s in students
                             on r.StudentId equals s.Id
                             join e in employees
                             on r.EmployeeId equals e.Id
                             join c in classes
                             on r.ClassId equals c.Id
                             join sub in subjects
                             on r.SubjectId equals sub.Id
                             join mt in markTypes
                             on r.MarkTypeId equals mt.Id
                             join et in examTypes
                             on r.ExamTypeId equals et.Id
                             select new { r, s, e, c, sub, mt, et };
            }
            else if (year != 0 && studentId != 0 && classId != 0 && examTypeId != 0)
            {
                jointQuery = from r in results
                             where r.InstitutionId == institutionId && r.Year.Year == year && r.StudentId == studentId && r.ClassId == classId && r.ExamTypeId == examTypeId
                             orderby r.Year descending
                             orderby r.ClassId ascending
                             join s in students
                             on r.StudentId equals s.Id
                             join e in employees
                             on r.EmployeeId equals e.Id
                             join c in classes
                             on r.ClassId equals c.Id
                             join sub in subjects
                             on r.SubjectId equals sub.Id
                             join mt in markTypes
                             on r.MarkTypeId equals mt.Id
                             join et in examTypes
                             on r.ExamTypeId equals et.Id
                             select new { r, s, e, c, sub, mt, et };
            }
            else
            {
                jointQuery = from r in results
                             where r.InstitutionId == institutionId && r.Year.Year == year
                             orderby r.Year descending
                             orderby r.ClassId ascending
                             join s in students
                             on r.StudentId equals s.Id
                             join e in employees
                             on r.EmployeeId equals e.Id
                             join c in classes
                             on r.ClassId equals c.Id
                             join sub in subjects
                             on r.SubjectId equals sub.Id
                             join mt in markTypes
                             on r.MarkTypeId equals mt.Id
                             join et in examTypes
                             on r.ExamTypeId equals et.Id
                             select new { r, s, e, c, sub, mt, et };
            }
            var queryResult = new List<ResultSheet>();
            foreach (var item in jointQuery)
            {
                var obj = new ResultSheet()
                {
                    results = item.r,
                    students = item.s,
                    employees = item.e,
                    classes = item.c,
                    subjects = item.sub,
                    markTypes = item.mt,
                    examTypes = item.et
                };
                queryResult.Add(obj);
            }

            return queryResult;
        }
        public IEnumerable<ResultByTeacher> ResultByTeacher(long institutionId, long year, long employeeId, long classId, long sectionId, long subjectId, long markTypeId, long examTypeId)
        {
            var assStudents = _assignedStudentsRepo.GetAll();
            var assTeachers = _assignedTeachersRepo.GetAll();
            var assCourses = _assignedCoursesRepo.GetAll();
            var classes = _classesRepo.GetAll();
            var subjects = _subjectsRepo.GetAll();
            var examTypes = _examTypesRepo.GetAll();
            var markTypes = _markTypesRepo.GetAll();
            var employees = _employeesRepo.GetAll();
            var students = _studentsRepo.GetAll();
            var jointQuery = (dynamic)null;
            if (year != 0 && employeeId != 0 && classId == 0 && sectionId == 0 && subjectId == 0 && markTypeId == 0 && examTypeId == 0)
            {
                jointQuery = from agnSt in assStudents
                             where agnSt.Year.Year == year && agnSt.InstitutionId == institutionId
                             join st in students on agnSt.StudentId equals st.Id
                             join cls in classes on agnSt.ClassId equals cls.Id
                             join agnCr in assCourses on agnSt.AssignedCourseId equals agnCr.Id
                             where agnCr.Year.Year == year && agnCr.InstitutionId == institutionId
                             join sb in subjects on agnCr.SubjectId equals sb.Id
                             join mt in markTypes on agnSt.InstitutionId equals mt.InstitutionId
                             join ext in examTypes on agnSt.InstitutionId equals ext.InstitutionId
                             join agnT in assTeachers on agnCr.Id equals agnT.AssignedCourseId
                             where agnT.Year.Year == year && agnT.InstitutionId == institutionId && agnT.EmployeeId == employeeId
                             join em in employees on agnT.EmployeeId equals em.Id
                             select new { agnSt, st, cls, agnCr, sb, mt, ext, agnT, em };
            }
            if (year != 0 && employeeId != 0 && classId != 0 && sectionId == 0 && subjectId == 0 && markTypeId == 0 && examTypeId == 0)
            {
                jointQuery = from agnSt in assStudents
                             where agnSt.Year.Year == year && agnSt.InstitutionId == institutionId
                             join st in students on agnSt.StudentId equals st.Id
                             join cls in classes on agnSt.ClassId equals cls.Id
                             where cls.Id == classId
                             join agnCr in assCourses on agnSt.AssignedCourseId equals agnCr.Id
                             where agnCr.Year.Year == year && agnCr.InstitutionId == institutionId
                             join sb in subjects on agnCr.SubjectId equals sb.Id
                             join mt in markTypes on agnSt.InstitutionId equals mt.InstitutionId
                             join ext in examTypes on agnSt.InstitutionId equals ext.InstitutionId
                             join agnT in assTeachers on agnCr.Id equals agnT.AssignedCourseId
                             where agnT.Year.Year == year && agnT.InstitutionId == institutionId && agnT.EmployeeId == employeeId
                             join em in employees on agnT.EmployeeId equals em.Id
                             select new { agnSt, st, cls, agnCr, sb, mt, ext, agnT, em };
            }
            if (year != 0 && employeeId != 0 && classId != 0 && sectionId != 0 && subjectId == 0 && markTypeId == 0 && examTypeId == 0)
            {
                jointQuery = from agnSt in assStudents
                             where agnSt.Year.Year == year && agnSt.InstitutionId == institutionId && agnSt.AssignedSectionId == sectionId
                             join st in students on agnSt.StudentId equals st.Id
                             join cls in classes on agnSt.ClassId equals cls.Id
                             where cls.Id == classId
                             join agnCr in assCourses on agnSt.AssignedCourseId equals agnCr.Id
                             where agnCr.Year.Year == year && agnCr.InstitutionId == institutionId
                             join sb in subjects on agnCr.SubjectId equals sb.Id
                             join mt in markTypes on agnSt.InstitutionId equals mt.InstitutionId
                             join ext in examTypes on agnSt.InstitutionId equals ext.InstitutionId
                             join agnT in assTeachers on agnCr.Id equals agnT.AssignedCourseId
                             where agnT.Year.Year == year && agnT.InstitutionId == institutionId && agnT.EmployeeId == employeeId
                             join em in employees on agnT.EmployeeId equals em.Id
                             select new { agnSt, st, cls, agnCr, sb, mt, ext, agnT, em };
            }

            if (year != 0 && employeeId != 0 && classId != 0 && sectionId != 0 && subjectId != 0 && markTypeId == 0 && examTypeId == 0)
            {
                jointQuery = from agnSt in assStudents
                             where agnSt.Year.Year == year && agnSt.InstitutionId == institutionId && agnSt.AssignedSectionId == sectionId
                             join st in students on agnSt.StudentId equals st.Id
                             join cls in classes on agnSt.ClassId equals cls.Id
                             where cls.Id == classId
                             join agnCr in assCourses on agnSt.AssignedCourseId equals agnCr.Id
                             where agnCr.Year.Year == year && agnCr.InstitutionId == institutionId
                             join sb in subjects on agnCr.SubjectId equals sb.Id
                             where sb.Id == subjectId
                             join mt in markTypes on agnSt.InstitutionId equals mt.InstitutionId
                             join ext in examTypes on agnSt.InstitutionId equals ext.InstitutionId
                             join agnT in assTeachers on agnCr.Id equals agnT.AssignedCourseId
                             where agnT.Year.Year == year && agnT.InstitutionId == institutionId && agnT.EmployeeId == employeeId
                             join em in employees on agnT.EmployeeId equals em.Id
                             select new { agnSt, st, cls, agnCr, sb, mt, ext, agnT, em };
            }
            if (year != 0 && employeeId != 0 && classId != 0 && sectionId != 0 && subjectId != 0 && markTypeId != 0 && examTypeId == 0)
            {
                jointQuery = from agnSt in assStudents
                             where agnSt.Year.Year == year && agnSt.InstitutionId == institutionId && agnSt.AssignedSectionId == sectionId
                             join st in students on agnSt.StudentId equals st.Id
                             join cls in classes on agnSt.ClassId equals cls.Id
                             where cls.Id == classId
                             join agnCr in assCourses on agnSt.AssignedCourseId equals agnCr.Id
                             where agnCr.Year.Year == year && agnCr.InstitutionId == institutionId
                             join sb in subjects on agnCr.SubjectId equals sb.Id
                             where sb.Id == subjectId
                             join mt in markTypes on agnSt.InstitutionId equals mt.InstitutionId
                             where mt.Id == markTypeId
                             join ext in examTypes on agnSt.InstitutionId equals ext.InstitutionId
                             join agnT in assTeachers on agnCr.Id equals agnT.AssignedCourseId
                             where agnT.Year.Year == year && agnT.InstitutionId == institutionId && agnT.EmployeeId == employeeId
                             join em in employees on agnT.EmployeeId equals em.Id
                             select new { agnSt, st, cls, agnCr, sb, mt, ext, agnT, em };
            }
            if (year != 0 && employeeId != 0 && classId != 0 && sectionId != 0 && subjectId != 0 && markTypeId != 0 && examTypeId != 0)
            {
                jointQuery = from agnSt in assStudents
                             where agnSt.Year.Year == year && agnSt.InstitutionId == institutionId && agnSt.AssignedSectionId == sectionId
                             join st in students on agnSt.StudentId equals st.Id
                             join cls in classes on agnSt.ClassId equals cls.Id
                             where cls.Id == classId
                             join agnCr in assCourses on agnSt.AssignedCourseId equals agnCr.Id
                             where agnCr.Year.Year == year && agnCr.InstitutionId == institutionId
                             join sb in subjects on agnCr.SubjectId equals sb.Id
                             where sb.Id == subjectId
                             join mt in markTypes on agnSt.InstitutionId equals mt.InstitutionId
                             where mt.Id == markTypeId
                             join ext in examTypes on agnSt.InstitutionId equals ext.InstitutionId
                             where ext.Id == examTypeId
                             join agnT in assTeachers on agnCr.Id equals agnT.AssignedCourseId
                             where agnT.Year.Year == year && agnT.InstitutionId == institutionId && agnT.EmployeeId == employeeId
                             join em in employees on agnT.EmployeeId equals em.Id
                             select new { agnSt, st, cls, agnCr, sb, mt, ext, agnT, em };
            }
            var returnQuery = new List<ResultByTeacher>();
            foreach (var item in jointQuery)
            {
                var temp = new ResultByTeacher()
                {
                    AssignedStudents = item.agnSt,
                    Students = item.st,
                    Employees = item.em,
                    Subjects = item.sb,
                    Classes = item.cls,
                    ExamTypes = item.ext,
                    MarkTypes = item.mt,
                };
                returnQuery.Add(temp);
            }
            return returnQuery;
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public void InsertResults(Results results)
        {
            _resultsRepo.Insert(results);
        }
       
        public void InsertGradings(InsertGradings insertGradings)
        {
            if (insertGradings.ResultList != null)
            {
                var filter = from f in insertGradings.ResultList
                             where f.ExamTypeId == insertGradings.ExampTypeId
                             select f;

                foreach (var item in filter)
                {
                    if (item.Id == 0)
                        _resultsRepo.Insert(item);
                    else
                    {
                        _resultsRepo.Update(item);
                    }
                }
            }
        }
        
        public void UpdateResults(Results results)
        {
            _resultsRepo.Update(results);
        }        
        public void DeleteResults(Results results)
        {
            _resultsRepo.Delete(results);
        }
        #endregion "Insert Update Delete Methods"    
    }
}
