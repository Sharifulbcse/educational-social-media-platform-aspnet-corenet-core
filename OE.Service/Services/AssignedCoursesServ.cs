
using System.Linq;
using System.Collections.Generic;
using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class AssignedCoursesServ : IAssignedCoursesServ
    {
        #region "Variables"
        
        private readonly IAssignedCoursesRepo<AssignedCourses> _assignedCoursesRepo;
        private readonly IAssignedSectionsRepo<AssignedSections> _assignedSectionsRepo;
        
        private readonly IClassesRepo<Classes> _classesRepo;
        private readonly ISubjectsRepo<Subjects> _subjectsRepo;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public AssignedCoursesServ(
             IAssignedSectionsRepo<AssignedSections> assignedSectionRepo,
            IAssignedCoursesRepo<AssignedCourses> assignedCoursesRepo,           
            IClassesRepo<Classes> classesRepo,
            ISubjectsRepo<Subjects> subjectsRepo,
            ICommonServ commonServ
            )
        {
            _assignedCoursesRepo = assignedCoursesRepo;
            _assignedSectionsRepo = assignedSectionRepo;         
            _classesRepo = classesRepo;          
            _subjectsRepo = subjectsRepo;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public AssignedCourses GetAssignedCoursesById(long id)
        {
            var queryAll = _assignedCoursesRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        public GetAssignedCourses AssignedCoursesById(long id)
        {
            var assignCourse = _assignedCoursesRepo.GetAll();
            var subject = _subjectsRepo.GetAll();
            var query = from aC in assignCourse
                        join sub in subject on aC.SubjectId equals sub.Id
                        where aC.Id == id
                        select new { aC, sub };
            var returnQry = new GetAssignedCourses()
            {
                courses = query.Select(x => x.aC).Single(),
                subjects = query.Select(x => x.sub).Single()
            };

            return returnQry;
        }
        public IEnumerable<AssignedCourses> GetAllAssignCourses()
        {
            //[NOTE: get all AssingCourses]
            var queryAll = _assignedCoursesRepo.GetAll();
            var querySections = from e in queryAll
                                select e;
            return querySections;
        }
        public IEnumerable<GetAssignedCourses> GetAssignedCourses(long instituteId, long ddlClass, long ddlSection, int year)
        {
            var cours = _assignedCoursesRepo.GetAll();
            var sec = _assignedSectionsRepo.GetAll();
            var cls = _classesRepo.GetAll();
            var sub = _subjectsRepo.GetAll();
            var joinQuery = (dynamic)null;            
            var filterQry = cours;
           
            if (ddlClass == 0 && ddlSection == 0 && year == 0)
            {
                joinQuery = from crs in cours
                            join c in cls on crs.ClassId equals c.Id
                            join se in sec on crs.AssignedSectionId equals se.Id
                            join su in sub on crs.SubjectId equals su.Id
                            where crs.InstitutionId == instituteId && crs.Year.Year == _commonServ.CommDate_CurrentYear()
                            select new { crs, c, se, su };
            }
            else
            {                
                if (ddlClass != 0)
                {
                    filterQry = from f in filterQry
                                where f.ClassId == ddlClass
                                select f;
                }

                if (ddlSection != 0)
                {
                    filterQry = from f in filterQry
                                where f.AssignedSectionId == ddlSection
                                select f;
                }
                
                joinQuery = from crs in filterQry                               
                            join c in cls on crs.ClassId equals c.Id
                            join se in sec on crs.AssignedSectionId equals se.Id
                            join su in sub on crs.SubjectId equals su.Id
                            where crs.InstitutionId == instituteId && crs.Year.Year == year
                            select new { crs, c, se, su };
            }
            var list = new List<GetAssignedCourses>();
            foreach (var item in joinQuery)
            {
                var temp = new GetAssignedCourses()
                {
                    courses = item.crs,
                    classes = item.c,
                    subjects = item.su,
                    sections = item.se
                };
                list.Add(temp);
            }
            return list;
        }
        #endregion "Get Methods"

        #region "Insert Update and Delete Methods"
        public void InsertAssignedCourses(AssignedCourses courses)
        {
            _assignedCoursesRepo.Insert(courses);
        }
        public void UpdateAssignedCourses(AssignedCourses courses)
        {
            _assignedCoursesRepo.Update(courses);
        }
        public void DeleteAssignedCourses(AssignedCourses courses)
        {
            _assignedCoursesRepo.Delete(courses);
        }
        #endregion "Insert Update and Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_AssignCourse> dropdown_AssignedCourses(long institutionId, long year, long classId, long assignedSectionId = 0)

        {
            var asgnCrsQuery = _assignedCoursesRepo.GetAll().ToList();
            var subQuery = _subjectsRepo.GetAll().ToList();
            
            var query = from ac in asgnCrsQuery
                        where ac.InstitutionId == institutionId && ac.Year.Year == year && ac.ClassId == classId && ac.AssignedSectionId == assignedSectionId
                        join s in subQuery on ac.SubjectId equals s.Id
                        orderby s.Name
                        select new { ac, s };
            
            var queryResult = new List<dropdown_AssignCourse>(); 

            foreach (var item in query.ToList())
            {
                var temp = new dropdown_AssignCourse()
                {
                    Id = item.ac.Id,
                    Name = item.s.Name
                };
                queryResult.Add(temp);
            }
            return queryResult;
        }

        #endregion "Dropdown Methods"

    }
}
