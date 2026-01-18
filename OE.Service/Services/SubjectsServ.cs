
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class SubjectsServ : ISubjectsServ
    {
        #region "Variables"
        private IAssignedCoursesRepo<AssignedCourses> _assignedCoursesRepo;
        private IAssignedTeachersRepo<AssignedTeachers> _assignedTeachersRepo;

        private IClassesRepo<Classes> _classesRepo;

        private ISubjectsRepo<Subjects> _subjectsRepo;       
        private ISubjectTypesRepo<SubjectTypes> _subjectTypesRepo;
       
        #endregion "Variables"

        #region "Constructor"
        public SubjectsServ(
            ISubjectsRepo<Subjects> subjectsRepo,
             IClassesRepo<Classes> classesRepo,
            ISubjectTypesRepo<SubjectTypes> subjectTypesRepo, 
            IAssignedCoursesRepo<AssignedCourses> assignedCoursesRepo,
             IAssignedTeachersRepo<AssignedTeachers> assignedTeachersRepo
            )
        {
            _classesRepo = classesRepo;
            _subjectsRepo = subjectsRepo;
            _subjectTypesRepo = subjectTypesRepo;
            _assignedCoursesRepo = assignedCoursesRepo;
            _assignedTeachersRepo = assignedTeachersRepo;

        }
        #endregion "Constructor"

        #region "Get Methods"
        public IEnumerable<Subjects> GetSubjects()
        {
            var queryAll = _subjectsRepo.GetAll();
            var returnQuery = from e in queryAll
                                   select e;
            return returnQuery;
        }

        public Subjects GetSubjectsById(long id)
        {
            var queryAll = _subjectsRepo.GetAll();
            var returnQuery = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return returnQuery;
        }

        public IEnumerable<GetSubClassSubTypes> GetSubClassSubTypes()
        {
            var subjectTypes = _subjectTypesRepo.GetAll().ToList();
            var classes = _classesRepo.GetAll().ToList();
            var subjects = _subjectsRepo.GetAll().ToList();

            var jointQuery = from c in classes
                             join s in subjects
                             on c.Id equals s.ClassId
                             join st in subjectTypes
                             on s.SubjectTypeId equals st.Id
                             select new { c, s, st };

            var queryResult = new List<GetSubClassSubTypes>();
            foreach (var item in jointQuery)
            {
                var obj = new GetSubClassSubTypes()
                {
                    Subjects = item.s,
                    Classes = item.c,
                    SubjectTypes = item.st
                };
                queryResult.Add(obj);
            }


            return queryResult;
        }
        
        public IEnumerable<GetSubClassSubTypes> ClassWiseSubjectView(long classId, long institutionId)
        {
            var subjectTypes = _subjectTypesRepo.GetAll().ToList();
            var classes = _classesRepo.GetAll().ToList();
            var subjects = _subjectsRepo.GetAll().ToList();

            var jointQuery = (dynamic)null;

            if (classId == 0)
            {
                jointQuery = from c in classes
                             join s in subjects
                             on c.Id equals s.ClassId
                             join st in subjectTypes
                             on s.SubjectTypeId equals st.Id
                             where c.InstitutionId == institutionId && s.InstitutionId == institutionId
                             orderby s.Name ascending
                             select new { c, s, st };
            }
            else
            {
                jointQuery = from c in classes
                             join s in subjects
                             on c.Id equals s.ClassId
                             join st in subjectTypes
                             on s.SubjectTypeId equals st.Id
                             where s.ClassId == classId && c.InstitutionId == institutionId && s.InstitutionId == institutionId
                             orderby s.Name ascending
                             select new { c, s, st };
            }
            var queryResult = new List<GetSubClassSubTypes>();
            foreach (var item in jointQuery)
            {
                var obj = new GetSubClassSubTypes()
                {
                    Subjects = item.s,
                    Classes = item.c,
                    SubjectTypes = item.st
                };
                queryResult.Add(obj);
            }
            return queryResult;
        }

        #endregion "Get Methods"

        #region "Insert Update and Delete Methods"
        public string InsertSubjects(InsertSubjects obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'Subjects' table]
                    if (obj.Subjects != null)
                    {
                        var sub = new Subjects()
                        {
                            Name = obj.Subjects.Name,
                            ClassId = obj.Subjects.ClassId,
                            InstitutionId = obj.Subjects.InstitutionId,
                            IsActive = true,
                            AddedBy = obj.Subjects.AddedBy,
                            AddedDate = obj.Subjects.AddedDate,
                            SubjectTypeId = obj.Subjects.SubjectTypeId
                        };

                        _subjectsRepo.Insert(sub);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:SubjectsServ/InsertSubjects - " + ex.Message;
            }

            return returnResult;

        }
        public void UpdateSubjects(Subjects subjects)
        {
            _subjectsRepo.Update(subjects);
        }
        
        public DeleteSubjects DeleteSubjects(DeleteSubjects obj)        
        {
            var returnModel = new DeleteSubjects();            
            try
            {   
                if (obj.Id > 0)                
                {
                   var subject = _subjectsRepo.Get(obj.Id);
                    
                    if (subject != null)
                    {
                        _subjectsRepo.Delete(subject);                       
                        
                        returnModel.Message = "Delete Successful.";
                        returnModel.SuccessIndicator = true;
                        
                    }
                }
            }
            catch (Exception ex)
            {
               if (ex.Source == "Microsoft.EntityFrameworkCore.Relational")
                {
                    returnModel.Message = "Record is not possible to delete, because it used in other places.";
                    returnModel.SuccessIndicator = false;
                }
                else
                {
                    returnModel.Message = "ERROR102:SubjectsServ/DeleteSubjects - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }               
            }  
            return returnModel;
          
        }
        #endregion "Insert Update and Delete Methods"

        #region "Dropdown Methods"

        public IEnumerable<dropdown_Subjects> dropdown_Subjects(long institutionId, long year = 0, long classId = 0)
        {
            var subject = _subjectsRepo.GetAll().ToList();
            var asgnCrs = _assignedCoursesRepo.GetAll().ToList();

            var getSub = (dynamic)null;

            var queryResult = (dynamic)null;

            if (institutionId != 0 && year == 0 && classId == 0)
            {
                getSub = from s in subject
                         where s.InstitutionId == institutionId
                         select s;

                queryResult = new List<dropdown_Subjects>();
                foreach (var item in getSub)
                {
                    var temp = new dropdown_Subjects()
                    {
                        Id = item.Id,
                        Name = item.Name
                    };
                    queryResult.Add(temp);
                }

            }

            if (institutionId != 0 && year != 0 && classId != 0)
            {
                getSub = from ac in asgnCrs
                         where ac.InstitutionId == institutionId && ac.Year.Year == year && ac.ClassId == classId
                         join s in subject on ac.SubjectId equals s.Id
                         select new { ac, s };

                queryResult = new List<dropdown_Subjects>();
                foreach (var item in getSub)
                {
                    var temp = new dropdown_Subjects()
                    {
                        Id = item.s.Id,
                        Name = item.s.Name
                    };
                    queryResult.Add(temp);
                }
            }
            if (institutionId != 0 && year == 0 && classId != 0)
            {
                getSub = from s in subject
                         where s.InstitutionId == institutionId && s.ClassId == classId
                         select new { s };

                queryResult = new List<dropdown_Subjects>()
                {                    
                };
                foreach (var item in getSub)
                {
                    var temp = new dropdown_Subjects()
                    {
                        Id = item.s.Id,
                        Name = item.s.Name
                    };
                    queryResult.Add(temp);
                }

            }
            return queryResult;

        }
        public IEnumerable<dropdown_Subjects> dropdown_Subjects(long institutionId, long year, long classId, long empId, long sectionId)
        {
            var subject = _subjectsRepo.GetAll().ToList();
            var asgnCrs = _assignedCoursesRepo.GetAll().ToList();
            var asgnT = _assignedTeachersRepo.GetAll().ToList();

            var getSub = from ac in asgnCrs

                         where ac.InstitutionId == institutionId && ac.Year.Year == year && ac.ClassId == classId && ac.AssignedSectionId == sectionId

                         join at in asgnT on ac.Id equals at.AssignedCourseId
                         where at.EmployeeId == empId
                         join s in subject on ac.SubjectId equals s.Id
                         select new { s };


            var queryResult = new List<dropdown_Subjects>()
            { new dropdown_Subjects{ Id=0,Name="All"} };

            foreach (var item in getSub)
            {
                var temp = new dropdown_Subjects()
                {
                    Id = item.s.Id,
                    Name = item.s.Name
                };
                queryResult.Add(temp);
            }
            return queryResult;
        }

        #endregion "Dropdown Methods"

    }
}
