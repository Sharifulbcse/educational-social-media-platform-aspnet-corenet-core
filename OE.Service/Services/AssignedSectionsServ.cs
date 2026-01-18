
using System.Linq;
using System.Collections.Generic;
using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;
using OE.Service;

namespace OE.Service
{
    public class AssignedSectionsServ : IAssignedSectionsServ
    {
        #region "Variables"       
        private readonly IAssignedSectionsRepo<AssignedSections> _assignedSectionsRepo;
        private readonly IAssignedTeachersRepo<AssignedTeachers> _assignedTeachersRepo;
        private readonly IClassesRepo<Classes> _classesRepo;
        private readonly ICommonServ _commonServ;
        #endregion "Variables"

        #region "Constructor"
        public AssignedSectionsServ(
            IAssignedSectionsRepo<AssignedSections> assignedSectionsRepo,
            IAssignedTeachersRepo<AssignedTeachers> assignedTeachersRepo,
            IClassesRepo<Classes> classesRepo,
            ICommonServ commonServ
            )
        {
            _assignedSectionsRepo = assignedSectionsRepo;
            _assignedTeachersRepo = assignedTeachersRepo;
            _classesRepo = classesRepo;
            _commonServ = commonServ;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public AssignedSections GetAssignedSectionsById(long id)
        {
            //[NOTE: get all AssingSection]
            var queryAll = _assignedSectionsRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }
        public IEnumerable<AssignedSections> GetAllAssignSections()
        {
            //[NOTE: get all AssingSection]
            var queryAll = _assignedSectionsRepo.GetAll();
            var querySections = from e in queryAll
                                select e;
            return querySections;
        }
        public IEnumerable<GetSections> GetSections(long instituteId, long ddlClass, int year)
        {
            var Sec = _assignedSectionsRepo.GetAll();
            var Cls = _classesRepo.GetAll();
            CommonServ com = new CommonServ();
            var joinQuery = (dynamic)null;

            var filterQry = Sec;

            if (year == 0 && ddlClass == 0)
            {
                joinQuery = from s in Sec
                            join c in Cls
                            on s.ClassId equals c.Id
                            where s.InstitutionId == instituteId && s.Year.Year == _commonServ.CommDate_CurrentYear()
                            select new { s, c };
            }
            else
            {
                 if (ddlClass != 0)
                {
                    filterQry = from f in filterQry
                                where f.ClassId == ddlClass
                                select f;
                }

                joinQuery = from s in filterQry
                            join c in Cls
                            on s.ClassId equals c.Id
                            where s.InstitutionId == instituteId && s.Year.Year == year
                            select new { s, c };
            }

            var listofClsSec = new List<GetSections>();
            foreach (var item in joinQuery)
            {
                var temp = new GetSections()
                {
                    assignedSections = item.s,
                    classes = item.c
                };
                listofClsSec.Add(temp);
            }
            return listofClsSec;
        }
        #endregion "Get Methods"

        #region "Insert Update and Delete Methods"
        public void InsertAssignSections(AssignedSections assSec)
        {
            _assignedSectionsRepo.Insert(assSec);
        }
        public void UpdateAssignSections(AssignedSections assSec)
        {
            _assignedSectionsRepo.Update(assSec);
        }
        public void DeleteAssignSections(AssignedSections assSec)
        {
            _assignedSectionsRepo.Delete(assSec);
        }
        #endregion "Insert Update and Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_AssignedSections> dropdown_AssignedSections(long institutionId, long year = 0, long classId = 0)
        {
            var asgnSections = _assignedSectionsRepo.GetAll();

            var query = (dynamic)null;
            if (institutionId != 0 && year == 0 && classId == 0)
            {
                query = from aS in asgnSections
                        where aS.InstitutionId == institutionId
                        select new { aS };
            }
            if (institutionId != 0 && year != 0 && classId != 0)
            {
                query = from aS in asgnSections
                        where aS.InstitutionId == institutionId && aS.Year.Year == year && aS.ClassId == classId
                        orderby aS.Name
                        select new { aS };

            }
            
            var queryResult = new List<dropdown_AssignedSections>();
            
            foreach (var item in query)
            {
                var temp = new dropdown_AssignedSections()
                {
                    Id = item.aS.Id,
                    Name = item.aS.Name
                };
                queryResult.Add(temp);
            }
            return queryResult;
        }
        public IEnumerable<dropdown_AssignedSections> dropdown_AssignedSections(long institutionId, long year, long employeeId, long classId)
        {
            var asgnSections = _assignedSectionsRepo.GetAll();
            var asgnTeachers = _assignedTeachersRepo.GetAll();
            var query = (dynamic)null;

            query = (from aS in asgnSections
                     where aS.InstitutionId == institutionId && aS.Year.Year == year && aS.ClassId == classId
                     join aT in asgnTeachers on aS.Id equals aT.AssignedSectionId
                     where aT.EmployeeId == employeeId
                     orderby aS.Name
                     select new { aS }).Distinct();

            //[NOTE:Section problem]

            var queryResult = new List<dropdown_AssignedSections>() {
                new dropdown_AssignedSections{ Id=0, Name="All"}
            };

            foreach (var item in query)
            {
                var temp = new dropdown_AssignedSections()
                {
                    Id = item.aS.Id,
                    Name = item.aS.Name
                };
                queryResult.Add(temp);
            }
            return queryResult;
        }

        #endregion "Dropdown Methods"
    }
}
