using OE.Data;
using OE.Repo;
using OE.Service.CustomEntitiesServ;
using OE.Service.ServiceModels;

using System;
using System.Collections.Generic;
using System.Linq;
namespace OE.Service
{
    public class AttendancesServ : CommonServ, IAttendancesServ
    {

        #region "Variables"
        private readonly IAttendancesRepo<Attendances> _attendancesRepo;
        private readonly IEmployeesRepo<Employees> _employeesRepo;
        private readonly IAttendanceCalculationsRepo<AttendanceCalculations> _attendanceCalculationsRepo;
        private readonly IStudentsRepo<Students> _studentsRepo;
        private readonly IAssignedStudentsRepo<AssignedStudents> _assignedStudents;
        private readonly IClassTimeSchedulesRepo<ClassTimeSchedules> _classTimeSchedulesRepo;
        private readonly IClassesRepo<Classes> _classesRepo;
        private readonly IAssignedSectionsRepo<AssignedSections> _assSectionsRepo;
        private readonly IAssignedCoursesRepo<AssignedCourses> _assignedCoursesRepo;
        private readonly ISubjectsRepo<Subjects> _subjectsRepo;
        private readonly IClassTimeScheduleActionDatesRepo<ClassTimeScheduleActionDates> _classTimeScheduleActionDatesRepo;
        #endregion "Variables"

        #region "Constructor"
        public AttendancesServ(
            IAttendancesRepo<Attendances> attendancesRepo,
            IAssignedStudentsRepo<AssignedStudents> assignedStudents,
            IAttendanceCalculationsRepo<AttendanceCalculations> attendanceCalculationsRepo,
            IAssignedCoursesRepo<AssignedCourses> assignedCoursesRepo,
            IAssignedSectionsRepo<AssignedSections> assSectionsRepo,

            IClassesRepo<Classes> classesRepo,
            IClassTimeSchedulesRepo<ClassTimeSchedules> classTimeSchedulesRepo,
            IClassTimeScheduleActionDatesRepo<ClassTimeScheduleActionDates> classTimeScheduleActionDatesRepo,

            IEmployeesRepo<Employees> employeesRepo,
            
            IStudentsRepo<Students> studentsRepo, 
            ISubjectsRepo<Subjects> subjectsRepo
            )
        {
            _attendancesRepo = attendancesRepo;
            _assignedStudents = assignedStudents;
            _employeesRepo = employeesRepo;
            _attendanceCalculationsRepo = attendanceCalculationsRepo;
            _studentsRepo = studentsRepo;
            _classTimeSchedulesRepo = classTimeSchedulesRepo;
            _classTimeScheduleActionDatesRepo = classTimeScheduleActionDatesRepo;
            _assignedCoursesRepo = assignedCoursesRepo;
            _classesRepo = classesRepo;
            _assSectionsRepo = assSectionsRepo;
            _subjectsRepo = subjectsRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"        
        public IEnumerable<Attendances> GetAttendances(long institutionId)
        {
            //[NOTE: get all attendances]
            var getAll = _attendancesRepo.GetAll();
            var returnQry = from e in getAll
                            where e.InstitutionId == institutionId
                            select e;
            return returnQry;
        }
        public Attendances GetAttendanceById(long id)
        {
            var getAll = _attendancesRepo.GetAll();
            var returnQry = (from e in getAll
                             where e.Id == id
                             select e).SingleOrDefault();
            return returnQry;
        }
        public ViewAttendances ViewAttendances(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId, long employeeId)
        {
            var attendanceCalculations = _attendanceCalculationsRepo.GetAll();
            var students = _studentsRepo.GetAll();
            var attendances = _attendancesRepo.GetAll();
            var assStudents = _assignedStudents.GetAll();
            var queryAttendance = attendances.Where(a => a.InstitutionId == institutionId && a.AttendanceDate.Year == year && a.ClassId == classId && a.AssignedCourseId == assignedCourseId && a.AssignedSectionId == assignedSectionId && a.EmployeeId == employeeId).GroupBy(a => a.StudentId).Select(a => a.First());
            var queryAttendanceCalculation = attendanceCalculations.Where(a => a.InstitutionId == institutionId && a.Year.Year == year && a.ClassId == classId && a.AssignedCourseId == assignedCourseId && a.AssignedSectionId == assignedSectionId).Select(a => a);
            var jointQry = from aStu in assStudents
                           join stu in students on aStu.StudentId equals stu.Id
                           where aStu.InstitutionId == institutionId
                           && aStu.Year.Year == year
                           && aStu.ClassId == classId
                           && aStu.AssignedCourseId == assignedCourseId
                           && aStu.AssignedSectionId == assignedSectionId
                           && aStu.IsActive == true
                           join a in queryAttendance on aStu.StudentId equals a.StudentId
                           join ac in queryAttendanceCalculation on a.StudentId equals ac.StudentId
                           select new { a, stu, ac, aStu };

            var list = new List<C_Attendances>();
            foreach (var item in jointQry)
            {
                var temp = new C_Attendances()
                {
                    StudentId = item.a.StudentId,
                    EmployeeId = item.a.EmployeeId,
                    StudentName = item.stu.Name,
                    IP300X200 = item.stu.IP300X200,
                    ClassId = item.a.ClassId,
                    AssignedCourseId = item.a.AssignedCourseId,
                    AssignedSectionId = item.a.AssignedSectionId,
                    TotalClasses = item.ac.TotalClass,
                    TotalPresents = item.ac.TotalAttendance,
                    TotalAbsents = item.ac.TotalClass - item.ac.TotalAttendance
                };
                list.Add(temp);
            }

            var model = new ViewAttendances()
            {
                _Attendances = list
            };
            return model;
        }

        public AttendanceListForDelete AttendanceListForDelete(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId, long employeeId)
        {
            var model = new AttendanceListForDelete();
            try
            {
                var attendances = _attendancesRepo.GetAll();
                var clssTimeSlots = _classTimeSchedulesRepo.GetAll();
                var filterClssTimeSlots = from e in clssTimeSlots
                                          where e.InstitutionId == institutionId
                                         && e.IsActive == true
                                          select e;
                var queryAttendance = attendances.Where(a => a.InstitutionId == institutionId && a.AttendanceDate.Year == year && a.ClassId == classId && a.AssignedCourseId == assignedCourseId && a.AssignedSectionId == assignedSectionId && a.EmployeeId == employeeId).GroupBy(a => a.AttendanceDate).Select(a => a.First());

                var jointQry = from a in queryAttendance
                               join slot in filterClssTimeSlots on a.ClassTimeScheduleId equals slot.Id
                               select new { a, slot };

                var list = new List<C_Attendances>();
                foreach (var item in jointQry)
                {
                    var temp = new C_Attendances()
                    {
                        Id = item.a.Id,
                        AttendanceDate = item.a.AttendanceDate,
                        TimeSlot = DateTime.Today.Add(item.slot.ClassStartTime).ToString("hh:mm:ss tt") + " - " + DateTime.Today.Add(item.slot.ClassEndTime).ToString("hh:mm:ss tt"),
                        ClassTimeScheduleId = item.a.ClassTimeScheduleId,
                        AssignedCourseId = item.a.AssignedCourseId,
                        ClassId = item.a.ClassId,
                        AssignedSectionId = item.a.AssignedSectionId,
                        EmployeeId = item.a.EmployeeId
                    };
                    list.Add(temp);
                }

                model._Attendances = list;
            }
            catch (Exception ex)
            {
                model.Message = "ERROR102:AttendancesServ/AttendanceListForDelete - " + ex.Message;
            }
            return model;
        }
        public AttendanceDetails AttendanceDetails(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId, long studentId)
        {

            var clssTimeSlots = _classTimeSchedulesRepo.GetAll();
            var filterClssTimeSlots = from e in clssTimeSlots
                                      where e.InstitutionId == institutionId
                                     && e.IsActive == true
                                      select e;
            var attendanceCalculation = _attendanceCalculationsRepo.GetAll().Where(ac => ac.Year.Year == year && ac.AssignedCourseId == assignedCourseId && ac.AssignedSectionId == assignedSectionId && ac.ClassId == classId && ac.StudentId == studentId).First();
            var attendances = _attendancesRepo.GetAll();
            var students = _studentsRepo.GetAll();
            var jointQry = from a in attendances
                           join slot in filterClssTimeSlots on a.ClassTimeScheduleId equals slot.Id
                           where a.InstitutionId == institutionId                           
                           && a.AttendanceDate.Year == year                          
                           && a.ClassId == classId
                           && a.AssignedCourseId == assignedCourseId
                           && a.AssignedSectionId == assignedSectionId
                           && a.StudentId == studentId                           
                           orderby a.AttendanceDate ascending                          
                           select new { a, slot };

            //[NOTE: Single Records] 
            var list = new List<C_Attendances>();
            foreach (var item in jointQry)
            {
                var temp = new C_Attendances()
                {

                    Id = item.a.Id,
                    StudentId = item.a.StudentId,
                    AttendanceDate = item.a.AttendanceDate,
                    TimeSlot = DateTime.Today.Add(item.slot.ClassStartTime).ToString("hh:mm:ss tt") + " - " + DateTime.Today.Add(item.slot.ClassEndTime).ToString("hh:mm:ss tt"),
                    ClassTimeScheduleId = item.a.ClassTimeScheduleId,
                    IsPresent = item.a.IsPresent
                };
                list.Add(temp);
            }

            var model = new AttendanceDetails()
            {
                TotalClasses = attendanceCalculation.TotalClass,
                TotalPresents = attendanceCalculation.TotalAttendance,
                TotalAbsents = attendanceCalculation.TotalClass - attendanceCalculation.TotalAttendance,
                _Attendances = list,
                StudentName = _studentsRepo.Get(studentId).Name,
                IP300X200 = _studentsRepo.Get(studentId).IP300X200,
                Class = _classesRepo.Get(classId).Name,
                AssignedCourse = _subjectsRepo.Get(_assignedCoursesRepo.Get(assignedCourseId).SubjectId).Name,
                AssignedSection = _assSectionsRepo.Get(assignedSectionId).Name
            };
            return model;
        }

        public AttendanceDetailsByStudent AttendanceDetailsByStudent(long institutionId, long year, long classId, long assignedCourseId, long assignedSectionId, long studentId)
        {
            var model = new AttendanceDetailsByStudent();
            try
            {
                var clssTimeSlots = _classTimeSchedulesRepo.GetAll();
                var filterClssTimeSlots = from e in clssTimeSlots
                                          where e.InstitutionId == institutionId
                                         && e.IsActive == true
                                          select e;
                var attendanceCalAll = _attendanceCalculationsRepo.GetAll();
                var attendanceCalculation = attendanceCalAll.Where(ac => ac.Year.Year == year && ac.AssignedCourseId == assignedCourseId && ac.AssignedSectionId == assignedSectionId && ac.ClassId == classId && ac.StudentId == studentId).Select(a => a).SingleOrDefault();
                
                var attendances = _attendancesRepo.GetAll();
                var students = _studentsRepo.GetAll();
                var jointQry = from a in attendances
                               join slot in filterClssTimeSlots on a.ClassTimeScheduleId equals slot.Id
                               where a.InstitutionId == institutionId
                               && a.AttendanceDate.Year == year
                               && a.ClassId == classId
                               && a.AssignedCourseId == assignedCourseId
                               && a.AssignedSectionId == assignedSectionId
                               && a.StudentId == studentId
                               orderby a.AttendanceDate ascending
                               select new { a, slot };

                //[NOTE: Single Records] 
                var list = new List<C_Attendances>();
                foreach (var item in jointQry)
                {
                    var temp = new C_Attendances()
                    {

                        Id = item.a.Id,
                        StudentId = item.a.StudentId,
                        AttendanceDate = item.a.AttendanceDate,
                        TimeSlot = DateTime.Today.Add(item.slot.ClassStartTime).ToString("hh:mm:ss tt") + " - " + DateTime.Today.Add(item.slot.ClassEndTime).ToString("hh:mm:ss tt"),
                        ClassTimeScheduleId = item.a.ClassTimeScheduleId,
                        IsPresent = item.a.IsPresent
                    };
                    list.Add(temp);
                }
                                
                if (attendanceCalculation != null)
                {
                    model.TotalClasses = attendanceCalculation.TotalClass;
                    model.TotalPresents = attendanceCalculation.TotalAttendance;
                    model.TotalAbsents = attendanceCalculation.TotalClass - attendanceCalculation.TotalAttendance;
                }
                
                model._Attendances = list;
                model.StudentName = _studentsRepo.Get(studentId).Name;
                model.IP300X200 = _studentsRepo.Get(studentId).IP300X200;
                model.ClassName = _classesRepo.Get(classId).Name;
                model.SubjectName = _subjectsRepo.Get(_assignedCoursesRepo.Get(assignedCourseId).SubjectId).Name;
                model.AssignedSectionName = _assSectionsRepo.Get(assignedSectionId).Name;
            }
            catch (Exception ex)
            {
                model.Message = "ERROR102:AttendancesServ/ViewAttendanceByStudent - " + ex.Message;
            }
            return model;
        }

        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertAttendance(InsertAttendances obj)
        {
            string message = "";
            var IsAttendanceExist = _attendancesRepo.GetAll().Where(a => a.InstitutionId == obj.InstitutionId && a.ClassTimeScheduleId == obj.ClassTimeScheduleId && a.AssignedCourseId == obj.AssignedCourseId && a.ClassId == obj.ClassId && a.AssignedSectionId == obj.AssignedSectionId && a.AttendanceDate.Date == obj.SelectedDate.Date);
            if (IsAttendanceExist.Count() != 0)
            {
                message = "Attendance has already taken.";
            }
            else
            {
                if (obj._Attendances != null)
                {
                    foreach (var item in obj._Attendances)
                    {
                        //[Create new attendance for individual Student]
                        var temp = new Attendances()
                        {
                            InstitutionId = item.InstitutionId,
                            StudentId = item.StudentId,
                            ClassTimeScheduleId = item.ClassTimeScheduleId,
                            ClassId = item.ClassId,
                            EmployeeId = item.EmployeeId,
                            AssignedCourseId = item.AssignedCourseId,
                            AssignedSectionId = item.AssignedSectionId,
                            AttendanceDate = item.AttendanceDate.Date,
                            IsPresent = item.IsPresent,
                            AddedBy = item.AddedBy,
                            AddedDate = item.AddedDate,
                            InsId = item.InsId
                        };
                        _attendancesRepo.Insert(temp);

                        
                        //[Attendance Calculation]
                        //[If, Attendance Calculation is exist, update]
                        var IsAttendanceCalculationExist = _attendanceCalculationsRepo.GetAll().Where(a => a.StudentId == item.StudentId && a.ClassId == item.ClassId && a.AssignedCourseId == item.AssignedCourseId && a.AssignedSectionId == item.AssignedSectionId && a.Year.Year == obj.SelectedDate.Year);
                        if (IsAttendanceCalculationExist.Count() != 0)
                        {
                            var updateCalculation = _attendanceCalculationsRepo.Get(IsAttendanceCalculationExist.First().Id);
                            if (item.IsPresent == true)
                            {
                                updateCalculation.TotalAttendance += 1;
                            }
                            updateCalculation.TotalClass += 1;
                            updateCalculation.ModifiedBy = item.AddedBy;
                            updateCalculation.ModifiedDate = item.AddedDate;
                            _attendanceCalculationsRepo.Update(updateCalculation);
                        }
                        else
                        {
                            //[NOTE: Extra Variable for storing value TotalAttendance field]
                            int totalAttendanceValue = 0;
                            if (item.IsPresent == true)
                            {
                                totalAttendanceValue = 1;
                            }
                            //[Else, Attendance Calculation is not exist, than create new]
                            var temp2 = new AttendanceCalculations()
                            {
                                StudentId = item.StudentId,
                                ClassId = item.ClassId,
                                AssignedCourseId = item.AssignedCourseId,
                                AssignedSectionId = item.AssignedSectionId,
                                Year = DateTime.Now.Date,
                                TotalAttendance = totalAttendanceValue,
                                TotalClass = 1,
                                AddedBy = item.AddedBy,
                                AddedDate = item.AddedDate,
                                InsId = item.InstitutionId,
                                InstitutionId = item.InstitutionId
                            };
                            _attendanceCalculationsRepo.Insert(temp2);
                        }
                        
                    }
                    message = "Attendance Saved.";
                }
                else message = "No new attendance created.";
            }
            return message;
        }

        public void InsertAttendance(Attendances attendance)
        {
            _attendancesRepo.Insert(attendance);
        }
        public string UpdateAttendance(UpdateAttendance obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj.Attendances != null)
                {
                    //[NOTE:Extra Variable for update Attendance calculation]
                    bool IsUpdateAttendanceCalculation = false;

                    var attendance = _attendancesRepo.Get(obj.Attendances.Id);
                    if (attendance.IsPresent != obj.Attendances.IsPresent)
                    {
                        IsUpdateAttendanceCalculation = true;
                    }
                    attendance.IsPresent = obj.Attendances.IsPresent;
                    attendance.ModifiedBy = obj.Attendances.ModifiedBy;
                    attendance.ModifiedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.Attendances.ModifiedDate));
                    _attendancesRepo.Update(attendance);

                    //[NOTE: Update in Attendance calculation table]
                    if (IsUpdateAttendanceCalculation == true)
                    {
                        var attendanceCal = _attendanceCalculationsRepo.GetAll().Where(at => at.StudentId == attendance.StudentId && at.ClassId == attendance.ClassId && at.AssignedCourseId == attendance.AssignedCourseId && at.AssignedSectionId == attendance.AssignedSectionId && at.Year.Year == attendance.AttendanceDate.Year).First();
                        if (obj.Attendances.IsPresent == true)
                        {
                            attendanceCal.TotalAttendance += 1;
                        }
                        else
                        {
                            attendanceCal.TotalAttendance -= 1;
                        }
                        attendanceCal.ModifiedBy = obj.Attendances.ModifiedBy;
                        attendanceCal.ModifiedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.Attendances.ModifiedDate));
                        _attendanceCalculationsRepo.Update(attendanceCal);
                    }
                    returnResult = "Updated";
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:AttendancesServ/UpdateAttendance - " + ex.Message;
            }
            return returnResult;
        }
        public DeleteAttendance DeleteAttendance(DeleteAttendance obj)
        {
            var returnModel = new DeleteAttendance();
            try
            {
                if (obj.EmployeeId > 0)
                {
                    var attendances = _attendancesRepo.GetAll().Where(a => a.ClassId == obj.ClassId && a.AssignedCourseId == obj.AssignedCourseId && a.AssignedSectionId == obj.AssignedSectionId && a.EmployeeId == obj.EmployeeId && a.AttendanceDate.Date == obj.AttendanceDate.Date);
                    if (attendances != null)
                    {
                        foreach (var item in attendances.ToList())
                        {                            
                            _attendancesRepo.Delete(item);

                            //[NOTE:Update Attendance calculation table for delete attendance]
                            var attendanceCal = _attendanceCalculationsRepo.GetAll().Where(at => at.StudentId == item.StudentId && at.ClassId == item.ClassId && at.AssignedCourseId == item.AssignedCourseId && at.AssignedSectionId == item.AssignedSectionId && at.Year.Year == item.AttendanceDate.Year).First();

                            attendanceCal.TotalClass -= 1;
                            if (item.IsPresent == true)
                            {
                                attendanceCal.TotalAttendance -= 1;
                            }
                            attendanceCal.ModifiedBy = obj.ModifiedBy;
                            attendanceCal.ModifiedDate = Convert.ToDateTime(CommDate_ConvertToUtcDate((DateTime)obj.ModifiedDate));
                            _attendanceCalculationsRepo.Update(attendanceCal);                           
                        }
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
                    returnModel.Message = "ERROR102:AttendancesServ/DeleteAttendance - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }
            }
            return returnModel;
        }
        #endregion "Insert Update Delete Methods"


    }
}

