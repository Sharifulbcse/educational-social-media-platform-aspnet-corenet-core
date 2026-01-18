
using OE.Data;

using Microsoft.EntityFrameworkCore;

namespace OE.Repo
{
    public class OurEduMediaContext : DbContext
    {
        public OurEduMediaContext(DbContextOptions<OurEduMediaContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //[NOTE: A series]
            new AssignedSectionsMap(modelBuilder.Entity<AssignedSections>());           
            new AssignedCoursesMap(modelBuilder.Entity<AssignedCourses>());
            new AssignedStudentsMap(modelBuilder.Entity<AssignedStudents>());
            new AssignedTeachersMap(modelBuilder.Entity<AssignedTeachers>());
            new AttendancesMap(modelBuilder.Entity<Attendances>());
            new AttendanceCalculationsMap(modelBuilder.Entity<AttendanceCalculations>());

            //[NOTE: C series]
            new COM_GendersMap(modelBuilder.Entity<COM_Genders>());
            new COM_RegistrationUserTypesMap(modelBuilder.Entity<COM_RegistrationUserTypes>());
            new COM_RegistrationItemTypesMap(modelBuilder.Entity<COM_RegistrationItemTypes>());

            new ClassesMap(modelBuilder.Entity<Classes>());
            new ClassTimeSchedulesMap(modelBuilder.Entity<ClassTimeSchedules>());
            new ClassTimeScheduleActionDatesMap(modelBuilder.Entity<ClassTimeScheduleActionDates>());
            new CountriesMap(modelBuilder.Entity<Countries>());

            //[NOTE: D series]
            new DepartmentsMap(modelBuilder.Entity<Departments>());
            new DistributionMarksMap(modelBuilder.Entity<DistributionMarks>());
            new DistributionMarkActionDatesMap(modelBuilder.Entity<DistributionMarkActionDates>());

            //[NOTE: E series]
            new EmployeesMap(modelBuilder.Entity<Employees>());            
            new EmployeeDetailsMap(modelBuilder.Entity<EmployeeDetails>());
            new EmployeeDesignationsMap(modelBuilder.Entity<EmployeeDesignations>());
            new EmployeeTypesMap(modelBuilder.Entity<EmployeeTypes>());            
            new EmployeeTypeCategoriesMap(modelBuilder.Entity<EmployeeTypeCategories>());
            new ExamTypesMap(modelBuilder.Entity<ExamTypes>());


            //[NOTE: G series]
            new GradeTypesMap(modelBuilder.Entity<GradeTypes>());

            //[NOTE: I series]
            new InsCategoriesMap(modelBuilder.Entity<InsCategories>());
            new InsPagesMap(modelBuilder.Entity<InsPages>());
            new InsPageDetailsMap(modelBuilder.Entity<InsPageDetails>());
            new InstitutionLinksMap(modelBuilder.Entity<InstitutionLinks>());

            //[NOTE: M series]
            new MarkTypesMap(modelBuilder.Entity<MarkTypes>());

            //[NOTE: O series]
            new OE_ActorsMap(modelBuilder.Entity<OE_Actors>());
            new OE_UsersMap(modelBuilder.Entity<OE_Users>());
            new OE_UserAuthenticationsMap(modelBuilder.Entity<OE_UserAuthentications>());
            new OE_InstitutionsMap(modelBuilder.Entity<OE_Institutions>());
            new OE_LicensesMap(modelBuilder.Entity<OE_Licenses>());
            new OE_StaffTypesMap(modelBuilder.Entity<OE_StaffTypes>());
            new OE_StaffsMap(modelBuilder.Entity<OE_Staffs>());
            new OE_InstitutionsMap(modelBuilder.Entity<OE_Institutions>());

            //[NOTE: R series]
            new RegistrationGroupsMap(modelBuilder.Entity<RegistrationGroups>());
            new RegistrationItemsMap(modelBuilder.Entity<RegistrationItems>());
            new ResultsMap(modelBuilder.Entity<Results>());

            //[NOTE: S series]           
            new StudentsMap(modelBuilder.Entity<Students>());
            new StudentDetailsMap(modelBuilder.Entity<StudentDetails>());           
            new StudentPromotionsMap(modelBuilder.Entity<StudentPromotions>());            
            new SubjectsMap(modelBuilder.Entity<Subjects>());
            new SubjectTypesMap(modelBuilder.Entity<SubjectTypes>());
                      
        }
    }
}
