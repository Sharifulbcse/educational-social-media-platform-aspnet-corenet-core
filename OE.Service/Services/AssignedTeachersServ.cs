using OE.Data;
using OE.Repo;
using OE.Service.CustomEntitiesServ;
using OE.Service.ServiceModels;

using System;
using System.Collections.Generic;
using System.Linq;

namespace OE.Service
{
    public class AssignedTeachersServ : IAssignedTeachersServ
    {
        #region "Variables"       
        private IClassesRepo<Classes> _classesRepo;
        private ISubjectsRepo<Subjects> _subjectsRepo;       
        private IEmployeesRepo<Employees> _employees;
        private IExamTypesRepo<ExamTypes> _examTypesRepo;
        private IAssignedSectionsRepo<AssignedSections> _assSectionsRepo;
        private IAssignedTeachersRepo<AssignedTeachers> _assignTeacher;
        private readonly IAssignedCoursesRepo<AssignedCourses> _assignedCoursesRepo;
        private readonly ICommonServ _commonServ;
               
        #endregion "Variables"

        #region "Constructor"
        public AssignedTeachersServ(
        IAssignedCoursesRepo<AssignedCourses> assignedCoursesRepo,
        IAssignedTeachersRepo<AssignedTeachers> assignTeacher,
            IClassesRepo<Classes> classesRepo,
            ISubjectsRepo<Subjects> subjectsRepo,
            IAssignedSectionsRepo<AssignedSections> assSectionsRepo,
            IEmployeesRepo<Employees> employees,
            IExamTypesRepo<ExamTypes> examTypesRepo,
            ICommonServ commonServ
            )
        {
            _assignedCoursesRepo = assignedCoursesRepo;
             _assignTeacher = assignTeacher;
            _classesRepo = classesRepo;
            _subjectsRepo = subjectsRepo;
            _assSectionsRepo = assSectionsRepo;
            _employees = employees;
            _examTypesRepo = examTypesRepo;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get Methods"        
        public GetCourseListForTeacher GetCourseListForTeacher(long year, long employeeId, long examTypeId)
        {
            var agnTeacher = _assignTeacher.GetAll();
            var agnCourse = _assignedCoursesRepo.GetAll();
            var agnSection = _assSectionsRepo.GetAll();
            var course = _subjectsRepo.GetAll();
            var classs = _classesRepo.GetAll();

            //[NOTE:Need for single record]
            var employee = _employees.GetAll();
            var examType = _examTypesRepo.GetAll();

            var queryCourseList = from at in agnTeacher
                                  where at.Year.Year == year && at.EmployeeId == employeeId
                                  join acrs in agnCourse on at.AssignedCourseId equals acrs.Id
                                  join crs in course on acrs.SubjectId equals crs.Id
                                  join cls in classs on at.ClassId equals cls.Id
                                  join sec in agnSection on at.AssignedSectionId equals sec.Id
                                  select new { at, acrs, crs, cls, sec };

            var queryEmployee = (from e in employee
                                 where e.Id == employeeId
                                 select e).SingleOrDefault();

            var queryExamType = (from et in examType
                                 where et.Id == examTypeId
                                 select et).SingleOrDefault();
            var listCourses = new List<C_AssignTeachers>();
            foreach (var item in queryCourseList)
            {
                var temp = new C_AssignTeachers()
                {
                    Id = item.at.Id,
                    ClassId = item.at.ClassId,
                    AssignCourseId = item.at.AssignedCourseId,
                    AssignSectionId = item.at.AssignedSectionId,
                    InstitutionId = item.at.InstitutionId,
                    CourseId = item.acrs.SubjectId,
                    CourseName = item.crs.Name,
                    AssignSectionName = item.sec.Name,
                    ClassName = item.cls.Name
                };
                listCourses.Add(temp);
            }
            var returnQry = new GetCourseListForTeacher()
            {
                _CourseList = listCourses,
                _EmployeeId = queryEmployee.Id,
                _EmployeeName = queryEmployee.FirstName + " " + queryEmployee.LastName,
                _ExamTypeId = queryExamType.Id,
                _ExamTypeName = queryExamType.Name,
                _ResultSearchYear = year
            };
            return returnQry;
        }
        public GetAssignedCoursesForAttendance GetAssignedCoursesForAttendance(int year, long userId, long institutionId)
        {
            var agnTeacher = _assignTeacher.GetAll();
            var agnCourse = _assignedCoursesRepo.GetAll();
            var agnSection = _assSectionsRepo.GetAll();
            var course = _subjectsRepo.GetAll();
            var classs = _classesRepo.GetAll();
            
            if (year == 0)
                year = DateTime.Now.Year;
            
            var employee = _employees.GetAll().Where(e => e.UserId == userId && e.InstitutionId == institutionId).SingleOrDefault();

            var queryCourseList = from at in agnTeacher
                                  where at.Year.Year == year && at.EmployeeId == employee.Id
                                  join acrs in agnCourse on at.AssignedCourseId equals acrs.Id
                                  join crs in course on acrs.SubjectId equals crs.Id
                                  join cls in classs on at.ClassId equals cls.Id
                                  join sec in agnSection on at.AssignedSectionId equals sec.Id
                                  select new { at, acrs, crs, cls, sec };

            var returnlist = new List<C_AssignTeachers>();
            foreach (var item in queryCourseList)
            {
                var temp = new C_AssignTeachers
                {
                    Id = item.at.Id,
                    ClassId = item.at.ClassId,
                    ClassName = item.cls.Name,
                    AssignCourseId = item.at.AssignedCourseId,
                    CourseName = item.crs.Name,
                    AssignSectionId = item.at.AssignedSectionId,
                    AssignSectionName = item.sec.Name,
                };
                returnlist.Add(temp);
            }
            var model = new GetAssignedCoursesForAttendance()
            {
                _CourseList = returnlist,
                EmployeeId = employee.Id
            };
            return model;
        }

        public AssignedTeachers GetAssignedTeacherById(long id)
        {
            var queryAll = _assignTeacher.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        public IEnumerable<AssignedTeachers> AllAssignedTeachers()
        {
            var getAll = _assignTeacher.GetAll();
            return getAll;
        }
        public IEnumerable<GetAssignedTeachers> GetDetailsAssignedTeachers(long institutionId, long ddlClass, long ddlSubject, long ddlSection, long ddlTeacher, int year)
        {
            //[NOTE: get all AssingTeachers]
            var assTeachers = _assignTeacher.GetAll();
            var classes = _classesRepo.GetAll();
            var subjects = _subjectsRepo.GetAll();
            var assSections = _assSectionsRepo.GetAll();
            var employee = _employees.GetAll();
            var assCourses = _assignedCoursesRepo.GetAll();

            var filterQry = assTeachers;
            CommonServ com = new CommonServ();
            var JointQry = (dynamic)null;
            if (ddlClass == 0 && ddlSubject == 0 && ddlSection == 0 && ddlTeacher == 0 && year == 0)
            {
                JointQry = from aT in assTeachers
                           join c in classes on aT.ClassId equals c.Id
                           join aC in assCourses on aT.AssignedCourseId equals aC.Id
                           join s in subjects on aC.SubjectId equals s.Id
                           join aS in assSections on aT.AssignedSectionId equals aS.Id
                           join oe in employee on aT.EmployeeId equals oe.Id
                           where aT.InstitutionId == institutionId && aT.Year.Year == _commonServ.CommDate_CurrentYear()
                           select new { aT, c, s, aS, oe, aC };

            }
            else
            {
                if (ddlClass != 0)
                {
                    filterQry = from f in filterQry
                                where f.ClassId == ddlClass
                                select f;
                }
                if (ddlSubject != 0)
                {
                    filterQry = from f in filterQry
                                where f.AssignedCourseId == ddlSubject
                                select f;
                }
                if (ddlSection != 0)
                {
                    filterQry = from f in filterQry
                                where f.AssignedSectionId == ddlSection
                                select f;
                }
                if (ddlTeacher != 0)
                {
                    filterQry = from f in filterQry
                                where f.EmployeeId == ddlTeacher
                                select f;
                }
                JointQry = from aT in filterQry
                           join c in classes on aT.ClassId equals c.Id
                           join aC in assCourses on aT.AssignedCourseId equals aC.Id
                           join s in subjects on aC.SubjectId equals s.Id
                           join aS in assSections on aT.AssignedSectionId equals aS.Id
                           join oe in employee on aT.EmployeeId equals oe.Id
                           where aT.InstitutionId == institutionId && aT.Year.Year == year
                           select new { aT, c, s, aS, oe, aC };

            }
            var returnQry = new List<GetAssignedTeachers>();
            foreach (var item in JointQry)
            {
                var temp = new GetAssignedTeachers()
                {
                    AssignedCourses = item.aC,
                    assignedTeachers = item.aT,
                    classes = item.c,
                    subjects = item.s,
                    sections = item.aS,
                    employees = item.oe
                };
                returnQry.Add(temp);
            }
            return returnQry;
        }
        #endregion "Get Methods"

        #region "Insert Update and Delete Methods"
        public void InsertAssignedTeachers(AssignedTeachers teacher)
        {
            _assignTeacher.Insert(teacher);
        }
        public void UpdateAssignedTeachers(AssignedTeachers teacher)
        {
            _assignTeacher.Update(teacher);
        }
        public void DeleteAssignedTeachers(AssignedTeachers teacher)
        {
            _assignTeacher.Delete(teacher);
        }
        #endregion "Insert Update and Delete Methods"

        #region "Dropdown Methods"
        #endregion "Dropdown Methods"
    }
}
