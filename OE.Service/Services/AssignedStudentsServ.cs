using System;
using System.Linq;
using System.Collections.Generic;
using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;
using OE.Service.CustomEntitiesServ;
namespace OE.Service
{
    public class AssignedStudentsServ : CommonServ, IAssignedStudentsServ
    {
        #region "Variables"
        private readonly IAssignedStudentsRepo<AssignedStudents> _assignedStudentsRepo;
        private readonly IAssignedCoursesRepo<AssignedCourses> _assignedCoursesRepo;
        private readonly IAssignedSectionsRepo<AssignedSections> _assignedSectionsRepo;
        private readonly IAssignedTeachersRepo<AssignedTeachers> _assignedTeachersRepo;
        private readonly IStudentsRepo<Students> _studentsRepo;
        private readonly IExamTypesRepo<ExamTypes> _examTypesRepo;
        private readonly IMarkTypesRepo<MarkTypes> _markTypesRepo;
        private readonly IEmployeesRepo<Employees> _employeesRepo;
        private readonly IClassesRepo<Classes> _classesRepo;
        private readonly ISubjectsRepo<Subjects> _subjectsRepo;
        private readonly IStudentPromotionsRepo<StudentPromotions> _studentPromotions;
        #endregion "Variables"

        #region "Constructor"
        public AssignedStudentsServ(
            IAssignedStudentsRepo<AssignedStudents> assignedStudentsRepo,
            IAssignedCoursesRepo<AssignedCourses> assignedCoursesRepo,
            IAssignedSectionsRepo<AssignedSections> assignedSectionsRepo,
            IAssignedTeachersRepo<AssignedTeachers> assignedTeachersRepo,
            IStudentsRepo<Students> studentsRepo,
            IExamTypesRepo<ExamTypes> examTypesRepo,
            IMarkTypesRepo<MarkTypes> markTypesRepo,
            IEmployeesRepo<Employees> employeesRepo,
            IClassesRepo<Classes> classesRepo,
            ISubjectsRepo<Subjects> subjectsRepo,
            IStudentPromotionsRepo<StudentPromotions> studentPromotions

            )
        {
            _assignedStudentsRepo = assignedStudentsRepo;
            _assignedCoursesRepo = assignedCoursesRepo;
            _assignedSectionsRepo = assignedSectionsRepo;
            _assignedTeachersRepo = assignedTeachersRepo;
            _studentsRepo = studentsRepo;
            _examTypesRepo = examTypesRepo;
            _markTypesRepo = markTypesRepo;
            _employeesRepo = employeesRepo;
            _classesRepo = classesRepo;
            _subjectsRepo = subjectsRepo;
            _studentPromotions = studentPromotions;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public AssignedStudents GetAssignedStudentById(long id)
        {
            var queryAll = _assignedStudentsRepo.GetAll();
            var query = (from e in queryAll
                         where e.Id == id
                         select e).SingleOrDefault();
            return query;
        }

        public GetSubjectList GetSubjectList(int year, long userId, long institutionId)
        {
            var model = new GetSubjectList();
            try
            {
                var assgnStudent = _assignedStudentsRepo.GetAll();
                var assgnCourse = _assignedCoursesRepo.GetAll();
                var assgnSection = _assignedSectionsRepo.GetAll();
                var course = _subjectsRepo.GetAll();
                var classs = _classesRepo.GetAll();

                if (year == 0)
                    year = DateTime.Now.Year;

                var student = _studentsRepo.GetAll().Where(s => s.UserId == userId && s.InstitutionId == institutionId).SingleOrDefault();

                var queryJoint = from agnS in assgnStudent
                                 where agnS.Year.Year == year && agnS.StudentId == student.Id
                                 join agnC in assgnCourse on agnS.AssignedCourseId equals agnC.Id
                                 join agnSec in assgnSection on agnS.AssignedSectionId equals agnSec.Id
                                 join cls in classs on agnS.ClassId equals cls.Id
                                 join sub in course on agnC.SubjectId equals sub.Id
                                 select new { agnS, agnC, agnSec, cls, sub };

                var returnList = new List<C_AssignedStudents>();
                foreach (var item in queryJoint)
                {
                    var temp = new C_AssignedStudents
                    {
                        Id = item.agnS.Id,
                        AssignedCourseId = item.agnS.AssignedCourseId,
                        SubjectName = item.sub.Name,
                        ClassId = item.agnS.ClassId,
                        ClassName = item.cls.Name,
                        AssignedSectionId = item.agnS.AssignedSectionId,
                        SectionName = item.agnSec.Name
                    };
                    returnList.Add(temp);
                }

                model._CourseList = returnList;
                model.StudentId = student.Id;
                model.StudentName = student.Name;
            }
            catch (Exception ex)
            {
                model.Message = "ERROR102:AssignedStudentsServ/GetSubjectList - " + ex.Message;
            }
            return model;
        }

        public GetRespectedCoursesByStudent GetRespectedCoursesByStudent(int year, long userId, long institutionId)
        {
            var model = new GetRespectedCoursesByStudent();
            try
            {
                var assgnStudent = _assignedStudentsRepo.GetAll();
                var assgnCourse = _assignedCoursesRepo.GetAll();
                var assgnSection = _assignedSectionsRepo.GetAll();
                var course = _subjectsRepo.GetAll();
                var classs = _classesRepo.GetAll();

                if (year == 0)
                    year = DateTime.Now.Year;

                var student = _studentsRepo.GetAll().Where(s => s.UserId == userId && s.InstitutionId == institutionId).SingleOrDefault();

                var queryJoint = from agnS in assgnStudent
                                 where agnS.Year.Year == year && agnS.StudentId == student.Id
                                 join agnC in assgnCourse on agnS.AssignedCourseId equals agnC.Id
                                 join agnSec in assgnSection on agnS.AssignedSectionId equals agnSec.Id
                                 join cls in classs on agnS.ClassId equals cls.Id
                                 join sub in course on agnC.SubjectId equals sub.Id
                                 select new { agnS, agnC, agnSec, cls, sub };

                var returnList = new List<C_AssignedStudents>();
                foreach (var item in queryJoint)
                {
                    var temp = new C_AssignedStudents
                    {
                        Id = item.agnS.Id,
                        AssignedCourseId = item.agnS.AssignedCourseId,
                        SubjectName = item.sub.Name,
                        ClassId = item.agnS.ClassId,
                        ClassName = item.cls.Name,
                        AssignedSectionId = item.agnS.AssignedSectionId,
                        SectionName = item.agnSec.Name
                    };
                    returnList.Add(temp);
                }

                model._CourseList = returnList;
                model.StudentId = student.Id;
            }
            catch (Exception ex)
            {
                model.Message = "ERROR102:AssignedStudentsServ/GetRespectedCoursesByStudent - " + ex.Message;
            }
            return model;
        }
        public GetAssignedStudentsForAttendance GetAssignedStudentsForAttendance(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId)
        {
            var assStudents = _assignedStudentsRepo.GetAll();
            var students = _studentsRepo.GetAll();
            var jointQry = from aStu in assStudents
                           join stu in students on aStu.StudentId equals stu.Id
                           where aStu.InstitutionId == institutionId
                           && aStu.Year.Year == year
                           && aStu.ClassId == classId
                           && aStu.AssignedCourseId == assignedCourseId
                           && aStu.AssignedSectionId == assignedSectionId
                           && aStu.IsActive == true
                           select new { aStu, stu };

            var list = new List<C_AssignedStudents>();
            foreach (var item in jointQry)
            {
                var temp = new C_AssignedStudents()
                {
                    Id = item.aStu.Id,
                    StudentId = item.aStu.StudentId,
                    StudentName = item.stu.Name,
                    IP300X200 = item.stu.IP300X200,
                    ClassId = item.aStu.ClassId,
                    AssignedCourseId = item.aStu.AssignedCourseId,
                    AssignedSectionId = item.aStu.AssignedSectionId,
                    Year = item.aStu.Year
                };
                list.Add(temp);
            }
            var model = new GetAssignedStudentsForAttendance()
            {
                _AssignedStudents = list

            };
            return model;
        }
        public GetAssignedOrUnassignedStudents GetAssignedOrUnassignedStudents(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId)
        {
            var model = new GetAssignedOrUnassignedStudents();
            try
            {
                var promotedStudents = _studentPromotions.GetAll().Where(sp => sp.Year.Year == year && sp.ClassId == classId && sp.InstitutionId == institutionId).ToList();
                var assignedStudents = _assignedStudentsRepo.GetAll().Where(aS => aS.Year.Year == year && aS.ClassId == classId && aS.AssignedCourseId == assignedCourseId && aS.AssignedSectionId == assignedSectionId && aS.InstitutionId == institutionId).ToList();

                var students = _studentsRepo.GetAll();
                var joinQry = from ps in promotedStudents
                              join s in students on ps.StudentId equals s.Id
                              select new { ps, s };

                var classs = _classesRepo.Get(classId);
                var section = _assignedSectionsRepo.Get(assignedSectionId);
                var assignedCourse = _assignedCoursesRepo.Get(assignedCourseId);
                var subject = _subjectsRepo.Get(assignedCourse.SubjectId);

                //[NOTE: Creating new list of PromotedStudent with extra filed 'IsAssigned'.]
                var lstOfPromotedStudents = new List<C_StudentPromotions>();
                foreach (var item in joinQry)
                {
                    var sp = new C_StudentPromotions();
                    sp.Id = item.ps.Id;
                    sp.InstitutionId = item.ps.InstitutionId;
                    sp.StudentId = item.ps.StudentId;
                    sp.StudentName = item.s.Name;
                    sp.ClassId = item.ps.ClassId;
                    sp.RollNo = item.ps.RollNo;
                    sp.Year = item.ps.Year;
                    sp.IsActive = item.ps.IsActive;
                    sp.IsAssigned = false;

                    foreach (var inside in assignedStudents)
                    {
                        if (item.ps.StudentId == inside.StudentId)
                        {
                            sp.IsAssigned = true;
                        }
                    }
                    lstOfPromotedStudents.Add(sp);
                }

                var aCourse = new C_AssignedCourses()
                {
                    Id = assignedCourse.Id,
                    Year = assignedCourse.Year,
                    ClassId = assignedCourse.ClassId,
                    AssignedSectionId = assignedCourse.AssignedSectionId,
                    SubjectId = assignedCourse.SubjectId,
                    SubjectName = subject?.Name
                };
                model = new GetAssignedOrUnassignedStudents()
                {
                    Year = year,
                    Classes = classs,
                    AssignedCourses = aCourse,                  
                    Subjects = subject,                  

                    AssignedSections = section,
                    _StudentPromotions = lstOfPromotedStudents,
                    Message = "",
                    SuccessIndicator = true
                };

            }
            catch (Exception ex)
            {
                model.Message = "ERROR102:AssignedStudents/GetAssignedOrUnassignedStudents - " + ex.Message;
                model.SuccessIndicator = false;
                return model;
            }
            return model;
        }


        public IEnumerable<GetAssignedStudents> GetAssignedStudents(long institutionId, long year, long classId, long subjectId, long markTypeId, long examTypeId, long employeeId, long assignedCourseId = 0, long assignedSectionId = 0)
        {
            var assStudents = _assignedStudentsRepo.GetAll();
            var assTeachers = _assignedTeachersRepo.GetAll();
            var assCourses = _assignedCoursesRepo.GetAll();
            var assSections = _assignedSectionsRepo.GetAll();

            var classes = _classesRepo.GetAll();
            var subjects = _subjectsRepo.GetAll();
            var examTypes = _examTypesRepo.GetAll();
            var markTypes = _markTypesRepo.GetAll();
            var employees = _employeesRepo.GetAll();
            var students = _studentsRepo.GetAll();
            var JointQuery = (dynamic)null;
            if (year != 0 && classId == 0 && subjectId == 0 && markTypeId == 0 && examTypeId == 0 && employeeId == 0)
            {
                var querySubject = from agnc in assCourses
                                   where agnc.Year.Year == year && agnc.InstitutionId == institutionId
                                   join s in subjects on agnc.SubjectId equals s.Id
                                   select new { agnc, s };
                var queryEmployee = from agnt in assTeachers
                                    where agnt.Year.Year == year && agnt.InstitutionId == institutionId
                                    join e in employees on agnt.EmployeeId equals e.Id
                                    select new { agnt, e };
                var queryStudent = from agns in assStudents
                                   where agns.Year.Year == year && agns.InstitutionId == institutionId
                                   join s in students on agns.StudentId equals s.Id
                                   select new { agns, s };

                JointQuery = from st in queryStudent
                             join c in classes on st.agns.ClassId equals c.Id
                             join sb in querySubject on st.agns.AssignedCourseId equals sb.agnc.Id
                             join mt in markTypes on st.agns.InstitutionId equals mt.InstitutionId
                             join ext in examTypes on st.agns.InstitutionId equals ext.InstitutionId
                             join em in queryEmployee on c.Id equals em.agnt.ClassId
                             select new { st, c, sb, mt, ext, em };
            }
            if (year != 0 && classId != 0 && subjectId == 0 && markTypeId == 0 && examTypeId == 0 && employeeId == 0)
            {
                var querySubject = from agnc in assCourses
                                   where agnc.Year.Year == year && agnc.InstitutionId == institutionId
                                   join s in subjects on agnc.SubjectId equals s.Id
                                   select new { agnc, s };
                var queryEmployee = from agnt in assTeachers
                                    where agnt.Year.Year == year && agnt.InstitutionId == institutionId
                                    join e in employees on agnt.EmployeeId equals e.Id
                                    select new { agnt, e };
                var queryStudent = from agns in assStudents
                                   where agns.Year.Year == year && agns.InstitutionId == institutionId
                                   join s in students on agns.StudentId equals s.Id
                                   select new { agns, s };

                JointQuery = from st in queryStudent
                             join c in classes on st.agns.ClassId equals c.Id
                             where c.Id == classId
                             join sb in querySubject on st.agns.AssignedCourseId equals sb.agnc.Id
                             join mt in markTypes on st.agns.InstitutionId equals mt.InstitutionId
                             join ext in examTypes on st.agns.InstitutionId equals ext.InstitutionId
                             join em in queryEmployee on c.Id equals em.agnt.ClassId
                             select new { st, c, sb, mt, ext, em };

            }
            if (year != 0 && classId != 0 && subjectId != 0 && markTypeId == 0 && examTypeId == 0 && employeeId == 0)
            {
                var querySubject = from agnc in assCourses
                                   where agnc.Year.Year == year && agnc.InstitutionId == institutionId
                                   join s in subjects on agnc.SubjectId equals s.Id
                                   where s.Id == subjectId
                                   select new { agnc, s };
                var queryEmployee = from agnt in assTeachers
                                    where agnt.Year.Year == year && agnt.InstitutionId == institutionId
                                    join e in employees on agnt.EmployeeId equals e.Id
                                    select new { agnt, e };
                var queryStudent = from agns in assStudents
                                   where agns.Year.Year == year && agns.InstitutionId == institutionId
                                   join s in students on agns.StudentId equals s.Id
                                   select new { agns, s };

                JointQuery = from st in queryStudent
                             join c in classes on st.agns.ClassId equals c.Id
                             where c.Id == classId
                             join sb in querySubject on st.agns.AssignedCourseId equals sb.agnc.Id
                             join mt in markTypes on st.agns.InstitutionId equals mt.InstitutionId
                             join ext in examTypes on st.agns.InstitutionId equals ext.InstitutionId
                             join em in queryEmployee on c.Id equals em.agnt.ClassId
                             select new { st, c, sb, mt, ext, em };
            }
            if (year != 0 && classId != 0 && subjectId != 0 && markTypeId != 0 && examTypeId == 0 && employeeId == 0)
            {
                var querySubject = from agnc in assCourses
                                   where agnc.Year.Year == year && agnc.InstitutionId == institutionId
                                   join s in subjects on agnc.SubjectId equals s.Id
                                   where s.Id == subjectId
                                   select new { agnc, s };
                var queryEmployee = from agnt in assTeachers
                                    where agnt.Year.Year == year && agnt.InstitutionId == institutionId
                                    join e in employees on agnt.EmployeeId equals e.Id
                                    select new { agnt, e };
                var queryStudent = from agns in assStudents
                                   where agns.Year.Year == year && agns.InstitutionId == institutionId
                                   join s in students on agns.StudentId equals s.Id
                                   select new { agns, s };

                JointQuery = from st in queryStudent
                             join c in classes on st.agns.ClassId equals c.Id
                             where c.Id == classId
                             join sb in querySubject on st.agns.AssignedCourseId equals sb.agnc.Id
                             join mt in markTypes on st.agns.InstitutionId equals mt.InstitutionId
                             where mt.Id == markTypeId
                             join ext in examTypes on st.agns.InstitutionId equals ext.InstitutionId
                             join em in queryEmployee on c.Id equals em.agnt.ClassId
                             select new { st, c, sb, mt, ext, em };
            }
            if (year != 0 && classId != 0 && subjectId != 0 && markTypeId != 0 && examTypeId != 0 && employeeId == 0)
            {
                var querySubject = from agnc in assCourses
                                   where agnc.Year.Year == year && agnc.InstitutionId == institutionId
                                   join s in subjects on agnc.SubjectId equals s.Id
                                   where s.Id == subjectId
                                   select new { agnc, s };
                var queryEmployee = from agnt in assTeachers
                                    where agnt.Year.Year == year && agnt.InstitutionId == institutionId
                                    join e in employees on agnt.EmployeeId equals e.Id
                                    select new { agnt, e };
                var queryStudent = from agns in assStudents
                                   where agns.Year.Year == year && agns.InstitutionId == institutionId
                                   join s in students on agns.StudentId equals s.Id
                                   select new { agns, s };

                JointQuery = from st in queryStudent
                             join c in classes on st.agns.ClassId equals c.Id
                             where c.Id == classId
                             join sb in querySubject on st.agns.AssignedCourseId equals sb.agnc.Id
                             join mt in markTypes on st.agns.InstitutionId equals mt.InstitutionId
                             where mt.Id == markTypeId
                             join ext in examTypes on st.agns.InstitutionId equals ext.InstitutionId
                             where ext.Id == examTypeId
                             join em in queryEmployee on c.Id equals em.agnt.ClassId
                             select new { st, c, sb, mt, ext, em };
            }
            if (year != 0 && classId != 0 && subjectId != 0 && markTypeId != 0 && examTypeId != 0 && employeeId != 0)
            {
                var querySubject = from agnc in assCourses
                                   where agnc.Year.Year == year && agnc.InstitutionId == institutionId
                                   join s in subjects on agnc.SubjectId equals s.Id
                                   where s.Id == subjectId
                                   select new { agnc, s };
                var queryEmployee = from agnt in assTeachers
                                    where agnt.Year.Year == year && agnt.InstitutionId == institutionId
                                    join e in employees on agnt.EmployeeId equals e.Id
                                    where e.Id == employeeId
                                    select new { agnt, e };
                var queryStudent = from agns in assStudents
                                   where agns.Year.Year == year && agns.InstitutionId == institutionId
                                   join s in students on agns.StudentId equals s.Id
                                   select new { agns, s };

                JointQuery = from st in queryStudent
                             join c in classes on st.agns.ClassId equals c.Id
                             where c.Id == classId
                             join sb in querySubject on st.agns.AssignedCourseId equals sb.agnc.Id
                             join mt in markTypes on st.agns.InstitutionId equals mt.InstitutionId
                             where mt.Id == markTypeId
                             join ext in examTypes on st.agns.InstitutionId equals ext.InstitutionId
                             where ext.Id == examTypeId
                             join em in queryEmployee on c.Id equals em.agnt.ClassId
                             select new { st, c, sb, mt, ext, em };
            }
           
            if (year != 0 && classId != 0 && assignedCourseId != 0 && assignedSectionId != 0)
            {
                JointQuery = from aS in assStudents
                             join stu in students on aS.StudentId equals stu.Id
                             join cls in classes on aS.ClassId equals cls.Id
                             join aC in assCourses on aS.AssignedCourseId equals aC.Id
                             join sub in subjects on aC.SubjectId equals sub.Id
                             join aSec in assSections on aS.AssignedSectionId equals aSec.Id
                             where aS.InstitutionId == institutionId && aS.ClassId == classId && aS.Year.Year == year
                             select new { aS, stu, cls, aC, aSec, sub };
                var returnQry = new List<GetAssignedStudents>();
                foreach (var item in JointQuery)
                {
                    var temp = new GetAssignedStudents()
                    {
                        AssignedStudents = item.aS,
                        AssignedCourses = item.aC,
                        AssignedSections = item.aSec,
                        Students = item.stu,
                        Classes = item.cls,

                        Subjects = item.sub


                    };
                    returnQry.Add(temp);
                }
                return returnQry;
            }

            

            var returnQuery = new List<GetAssignedStudents>();
            foreach (var item in JointQuery)
            {
                var temp = new GetAssignedStudents()
                {
                    AssignedStudents = item.st.agns,
                    Students = item.st.s,
                    Employees = item.em.e,
                    Subjects = item.sb.s,
                    Classes = item.c,
                    ExamTypes = item.ext,
                    MarkTypes = item.mt,
                };
                returnQuery.Add(temp);
            }
            return returnQuery;

        }
        public IEnumerable<AssignedStudents> AllAssignedStudents()
        {
            var getAll = _assignedStudentsRepo.GetAll();
            return getAll;
        }

        #endregion "Get Methods"
               
        #region "Insert Update and Delete Methods"
        public void InsertAssignedStudents(AssignedStudents stu)
        {
            _assignedStudentsRepo.Insert(stu);
        }
        public void UpdateAssignedStudents(AssignedStudents stu)
        {
            _assignedStudentsRepo.Update(stu);
        }        
        public UpdateAssignedStudents UpdateAssignedStudents(UpdateAssignedStudents obj)
        {
            var model = new UpdateAssignedStudents();
            try
            {
                if (obj._AssignedStudents != null)
                {
                    foreach (var item in obj._AssignedStudents)
                    {
                        //[NOTE: Check AssignedStudent record is exist?]
                        var iSAssignedStdudent = _assignedStudentsRepo.GetAll().Where(aS => aS.InstitutionId == item.InstitutionId && aS.StudentId == item.StudentId && aS.ClassId == item.ClassId && aS.AssignedCourseId == item.AssignedCourseId && aS.AssignedSectionId == item.AssignedSectionId && aS.Year.Year == item.Year.Year).SingleOrDefault();
                        if (iSAssignedStdudent == null)
                        {
                            if (item.IsAssigned == true)
                            {
                                var newAS = new AssignedStudents()
                                {
                                    InstitutionId = item.InstitutionId,
                                    StudentId = item.StudentId,
                                    ClassId = item.ClassId,
                                    AssignedCourseId = item.AssignedCourseId,
                                    AssignedSectionId = item.AssignedSectionId,
                                    SubjectTypeId = item.SubjectTypeId,
                                    Year = item.Year,
                                    IsActive = true,
                                    AddedBy = item.AddedBy,
                                    AddedDate = CommDate_ConvertToUtcDate(DateTime.Now.Date),
                                    InsId = item.InstitutionId
                                };
                                _assignedStudentsRepo.Insert(newAS);
                            }
                        }
                        else if (iSAssignedStdudent != null)
                        {
                            if (item.IsAssigned == false)
                            {
                                _assignedStudentsRepo.Delete(iSAssignedStdudent);
                            }
                        }
                    }
                }
                model.Message = "";
                model.SuccessIndicator = true;
            }
            catch (Exception ex)
            {

                model.Message = "ERROR102:UpdateAssignedStudents/AssignedStudentsServ - " + ex.Message;
                model.SuccessIndicator = false;
                return model;
            }
            return model;
        }

        public UpdateAssignedStudent UpdateAssignedStudent(UpdateAssignedStudent obj)
        {
            var model = new UpdateAssignedStudent();
            try
            {
                //[NOTE: Check AssignedStudent record is exist?]
                var iSAssignedStdudent = _assignedStudentsRepo.GetAll().Where(aS => aS.InstitutionId == obj.AssignedStudent.InstitutionId && aS.StudentId == obj.AssignedStudent.StudentId && aS.ClassId == obj.AssignedStudent.ClassId && aS.AssignedCourseId == obj.AssignedStudent.AssignedCourseId && aS.AssignedSectionId == obj.AssignedStudent.AssignedSectionId && aS.Year.Year == obj.AssignedStudent.Year.Year).SingleOrDefault();
                if (iSAssignedStdudent == null)
                {
                    if (obj.AssignedStudent.IsAssigned == true)
                    {
                        var newAS = new AssignedStudents()
                        {
                            InstitutionId = obj.AssignedStudent.InstitutionId,
                            StudentId = obj.AssignedStudent.StudentId,
                            ClassId = obj.AssignedStudent.ClassId,
                            AssignedCourseId = obj.AssignedStudent.AssignedCourseId,
                            AssignedSectionId = obj.AssignedStudent.AssignedSectionId,
                            SubjectTypeId = obj.AssignedStudent.SubjectTypeId,
                            Year = obj.AssignedStudent.Year,
                            IsActive = true,
                            AddedBy = obj.AssignedStudent.AddedBy,
                            AddedDate = CommDate_ConvertToUtcDate(DateTime.Now.Date),
                            InsId = obj.AssignedStudent.InstitutionId
                        };
                        _assignedStudentsRepo.Insert(newAS);
                    }
                }
                else if (iSAssignedStdudent != null)
                {
                    if (obj.AssignedStudent.IsAssigned == false)
                    {
                        _assignedStudentsRepo.Delete(iSAssignedStdudent);
                    }
                }
                model.Message = "";
                model.SuccessIndicator = true;
            }
            catch (Exception ex)
            {

                model.Message = "ERROR102:UpdateAssignedStudent/AssignedStudentsServ - " + ex.Message;
                model.SuccessIndicator = false;
                return model;
            }
            return model;
        }
        public void DeleteAssignedStudents(AssignedStudents stu)
        {
            _assignedStudentsRepo.Delete(stu);
        }
        #endregion "Insert Update and Delete Methods"

        #region "Dropdown Methods"
        #endregion "Dropdown Methods"
    }
}

