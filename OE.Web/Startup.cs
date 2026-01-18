using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OE.Repo;
using OE.Service;

using Rotativa.AspNetCore;

using System;

namespace OE.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
               .SetBasePath(env.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
               .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
               .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddDistributedMemoryCache();

            //[NOTE: set session time for 1 year]
            services.AddSession(options => {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = ".ASPNetCoreSession";
                options.IdleTimeout = TimeSpan.FromMinutes(525600);
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.Path = "/";

            });


            //[NOTE:Add database connection]
            services.AddDbContext<OurEduMediaContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            #region "A series"
            //[NOTE:Actors]
            services.AddScoped(typeof(IOE_ActorsRepo<>), typeof(OE_ActorsRepo<>));
            services.AddTransient<IOE_ActorsServ, OE_ActorsServ>();

            //AssignedSections]
            services.AddScoped(typeof(IAssignedSectionsRepo<>), typeof(AssignedSectionsRepo<>));
            services.AddTransient<IAssignedSectionsServ, AssignedSectionsServ>();

            //[NOTE:AssignedCourses ]        
            services.AddScoped(typeof(IAssignedCoursesRepo<>), typeof(AssignedCoursesRepo<>));
            services.AddTransient<IAssignedCoursesServ, AssignedCoursesServ>();

            //[NOTE:AssignedStudents]
            services.AddScoped(typeof(IAssignedStudentsRepo<>), typeof(AssignedStudentsRepo<>));
            services.AddTransient<IAssignedStudentsServ, AssignedStudentsServ>();

            //[NOTE:AssignedTeachers]
            services.AddScoped(typeof(IAssignedTeachersRepo<>), typeof(AssignedTeachersRepo<>));
            services.AddTransient<IAssignedTeachersServ, AssignedTeachersServ>();

            //[NOTE:Attendances]
            services.AddScoped(typeof(IAttendancesRepo<>), typeof(AttendancesRepo<>));
            services.AddTransient<IAttendancesServ, AttendancesServ>();

            //[NOTE:AttendanceCalculations]
            services.AddScoped(typeof(IAttendanceCalculationsRepo<>), typeof(AttendanceCalculationsRepo<>));
            services.AddTransient<IAttendanceCalculationsServ, AttendanceCalculationsServ>();
            #endregion "A series"
            #region "C series"
            //[NOTE:Common]
            services.AddTransient<ICommonServ, CommonServ>();
            services.AddTransient<ICommonFunctionsServ, CommonFunctionsServ>();

            //[NOTE:Classes]
            services.AddScoped(typeof(IClassesRepo<>), typeof(ClassesRepo<>));
            services.AddTransient<IClassesServ, ClassesServ>();

            //[NOTE:ClasseTimeSchedules]
            services.AddScoped(typeof(IClassTimeSchedulesRepo<>), typeof(ClassTimeSchedulesRepo<>));
            services.AddTransient<IClassTimeSchedulesServ, ClassTimeSchedulesServ>();

            //[NOTE:ClasseTimeSchedulesClassTimeScheduleActionDates]
            services.AddScoped(typeof(IClassTimeScheduleActionDatesRepo<>), typeof(ClassTimeScheduleActionDatesRepo<>));
            services.AddTransient<IClassTimeScheduleActionDatesServ, ClassTimeScheduleActionDatesServ>();
           
            //[NOTE:Counties]
            services.AddScoped(typeof(ICountriesRepo<>), typeof(CountriesRepo<>));
            services.AddTransient<ICountriesServ, CountriesServ>();

            //[NOTE:COM_Genders]
            services.AddScoped(typeof(ICOM_GendersRepo<>), typeof(COM_GendersRepo<>));

            //[NOTE:COM_RegistrationUserTypes]
            services.AddScoped(typeof(ICOM_RegistrationUserTypesRepo<>), typeof(COM_RegistrationUserTypesRepo<>));
            services.AddTransient<ICOM_RegistrationUserTypesServ, COM_RegistrationUserTypesServ>();

            #endregion "C series"
            #region "D series"
            //Departments
            services.AddScoped(typeof(IDepartmentsRepo<>), typeof(DepartmentsRepo<>));
            services.AddTransient<IDepartmentsServ, DepartmentsServ>();
            //Distribution Mark
            services.AddScoped(typeof(IDistributionMarksRepo<>), typeof(DistributionMarksRepo<>));
            services.AddTransient<IDistributionMarksServ, DistributionMarksServ>();
            //DistributionMarkActionDates
            services.AddScoped(typeof(IDistributionMarkActionDatesRepo<>), typeof(DistributionMarkActionDatesRepo<>));
            services.AddTransient<IDistributionMarkActionDatesServ, DistributionMarkActionDatesServ>();

            #endregion "D series"
            #region "E series"
            //Employees
            services.AddScoped(typeof(IEmployeesRepo<>), typeof(EmployeesRepo<>));
            services.AddTransient<IEmployeesServ, EmployeesServ>();

            //EmployeeDetails
            services.AddScoped(typeof(IEmployeeDetailsRepo<>), typeof(EmployeeDetailsRepo<>));
            services.AddTransient<IEmployeeDetailsServ, EmployeeDetailsServ>();

            //EmployeeDesignations
            services.AddScoped(typeof(IEmployeeDesignationsRepo<>), typeof(EmployeeDesignationsRepo<>));
            services.AddTransient<IEmployeeDesignationsServ, EmployeeDesignationsServ>();

            //ExamTypes
            services.AddScoped(typeof(IExamTypesRepo<>), typeof(ExamTypesRepo<>));
            services.AddTransient<IExamTypesServ, ExamTypesServ>();

            //EmployeeTypes
            services.AddScoped(typeof(IEmployeeTypesRepo<>), typeof(EmployeeTypesRepo<>));
            services.AddTransient<IEmployeeTypesServ, EmployeeTypesServ>();

            //EmployeeTypeCategories
            services.AddScoped(typeof(IEmployeeTypeCategoriesRepo<>), typeof(EmployeeTypeCategoriesRepo<>));
            services.AddTransient<IEmployeeTypeCategoriesServ, EmployeeTypeCategoriesServ>();

            #endregion "E series"
            #region "G series"
            //Genders
            services.AddScoped(typeof(ICOM_GendersRepo<>), typeof(COM_GendersRepo<>));
            services.AddTransient<ICOM_GendersServ, COM_GendersServ>();
            //GradeTypes
            services.AddScoped(typeof(IGradeTypesRepo<>), typeof(GradeTypesRepo<>));
            services.AddTransient<IGradeTypesServ, GradeTypesServ>();
            #endregion "G series" 
            #region "I series"
            //InsCategory
            services.AddScoped(typeof(IInsCategoriesRepo<>), typeof(InsCategoriesRepo<>));
            services.AddTransient<IInsCategoriesServ, InsCategoriesServ>();

            //InsPages
            services.AddScoped(typeof(IInsPagesRepo<>), typeof(InsPagesRepo<>));
            services.AddTransient<IInsPagesServ, InsPagesServ>();

            //InsPageDetails
            services.AddScoped(typeof(IInsPageDetailsRepo<>), typeof(InsPageDetailsRepo<>));
            services.AddTransient<IInsPageDetailsServ, InsPageDetailsServ>();

            //InstitutionLinks
            services.AddScoped(typeof(IInstitutionLinksRepo<>), typeof(InstitutionLinksRepo<>));
            services.AddTransient<IInstitutionLinksServ, InstitutionLinksServ>();
            #endregion "I series"
            #region "M series"
            //MarkTypes
            services.AddScoped(typeof(IMarkTypesRepo<>), typeof(MarkTypesRepo<>));
            services.AddTransient<IMarkTypesServ, MarkTypesServ>();
            #endregion "M series"
            #region "O series"
            //Licenses
            services.AddScoped(typeof(IOE_LicensesRepo<>), typeof(OE_LicensesRepo<>));
            services.AddTransient<IOE_LicensesServ, OE_LicensesServ>();

            //StaffTypes
            services.AddScoped(typeof(IOE_StaffTypesRepo<>), typeof(OE_StaffTypesRepo<>));
            services.AddTransient<IOE_StaffTypesServ, OE_StaffTypesServ>();

            //Staffs
            services.AddScoped(typeof(IOE_StaffsRepo<>), typeof(OE_StaffsRepo<>));
            services.AddTransient<IOE_StaffsServ, OE_StaffsServ>();

            services.AddScoped(typeof(IOE_InstitutionsRepo<>), typeof(OE_InstitutionsRepo<>));
            services.AddTransient<IOE_InstitutionsServ, OE_InstitutionsServ>();

            #endregion "O series"
            #region "R series"
            //RegistrationGroups
            services.AddScoped(typeof(IRegistrationGroupsRepo<>), typeof(RegistrationGroupsRepo<>));
            services.AddTransient<IRegistrationGroupsServ, RegistrationGroupsServ>();

            //RegistrationItemTypes
            services.AddScoped(typeof(IRegistrationItemTypesRepo<>), typeof(RegistrationItemTypesRepo<>));
            services.AddTransient<IRegistrationItemTypesServ, RegistrationItemTypesServ>();

            //RegistrationItems
            services.AddScoped(typeof(IRegistrationItemsRepo<>), typeof(RegistrationItemsRepo<>));
            services.AddTransient<IRegistrationItemsServ, RegistrationItemsServ>();

            //Results
            services.AddScoped(typeof(IResultsRepo<>), typeof(ResultsRepo<>));
            services.AddTransient<IResultsServ, ResultsServ>();
            #endregion "R series"                     
            #region "S series"
            //Subjects
            services.AddScoped(typeof(ISubjectsRepo<>), typeof(SubjectsRepo<>));
            services.AddTransient<ISubjectsServ, SubjectsServ>();

            //Students + StudentDetailsClassWise
            services.AddScoped(typeof(IStudentsRepo<>), typeof(StudentsRepo<>));
            services.AddScoped(typeof(IStudentPromotionsRepo<>), typeof(StudentPromotionsRepo<>));
            services.AddTransient<IStudentsServ, StudentsServ>();

            //SubjectTypes
            services.AddScoped(typeof(ISubjectTypesRepo<>), typeof(SubjectTypesRepo<>));
            services.AddTransient<ISubjectTypesServ, SubjectTypesServ>();

            services.AddScoped(typeof(IStudentDetailsRepo<>), typeof(StudentDetailsRepo<>));
            services.AddTransient<IStudentDetailsServ, StudentDetailsServ>();


            #endregion "S series"
            #region "U series"
            //Users
            services.AddScoped(typeof(IOE_UsersRepo<>), typeof(OE_UsersRepo<>));
            services.AddTransient<IOE_UsersServ, OE_UsersServ>();

            //UserAuthentications
            services.AddScoped(typeof(IOE_UserAuthenticationsRepo<>), typeof(OE_UserAuthenticationsRepo<>));
            services.AddTransient<IOE_UserAuthenticationsServ, OE_UserAuthenticationsServ>();

            //Users
            services.AddScoped(typeof(IOE_InstitutionsRepo<>), typeof(OE_InstitutionsRepo<>));
            services.AddTransient<IOE_InstitutionsServ, OE_InstitutionsServ>();

            #endregion "U series"

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseSession();


            app.UseMvc(routes =>
            {
                routes.MapRoute(
                  name: "areaRoute",
                  template: "{area:exists}/{controller=OE_Users}/{action=Index}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Login}/{id?}");
            });

            RotativaConfiguration.Setup(env,"DataDictionary\\Rotativa");

        }
    }
}
