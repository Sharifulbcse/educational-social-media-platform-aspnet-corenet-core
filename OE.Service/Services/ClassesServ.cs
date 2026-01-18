
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{
    public class ClassesServ : IClassesServ
    {
        #region "Variables"
        private IClassesRepo<Classes> _classesRepo;
        private IDepartmentsRepo<Departments> _departmentsRepo;
        private IInsCategoriesRepo<InsCategories> _insCategoryRepo;
        private IAssignedTeachersRepo<AssignedTeachers> _assignedTeachersRepo;
        #endregion "Variables"

        #region "Constructor"
        public ClassesServ(
            IClassesRepo<Classes> classesRepo, 
            IDepartmentsRepo<Departments> departmentsRepo,
            IInsCategoriesRepo<InsCategories> insCategoryRepo,
             IAssignedTeachersRepo<AssignedTeachers> assignedTeachersRepo
            )
        {
            _classesRepo = classesRepo;
            _insCategoryRepo = insCategoryRepo;
            _departmentsRepo = departmentsRepo;
            _assignedTeachersRepo = assignedTeachersRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"

        public Classes GetClassById(Int64 id)
        {
            //[NOTE: get all departments]
            var queryAll = _classesRepo.GetAll();
            var query = (from e in queryAll                             
                         where e.Id == id                         
                         select e).SingleOrDefault();
            return query;
        }
        
        public string ClassName(long institutionId, long id)
        {
            var queryAll = _classesRepo.GetAll();
            var query = (from c in queryAll
                         where c.Id == id && c.InstitutionId == institutionId
                         select c.Name).SingleOrDefault();
            return query;
        }
               
        public IEnumerable<Classes> GetClasses(long institutionId)        
        {            
            var classes = _classesRepo.GetAll().ToList();
            var query = from c in classes
                        where c.InstitutionId == institutionId
                        orderby c.Sorting
                        select c;
            return query;        
        }
        #endregion "Get Methods"

        #region "Insert Update Delete Methods"
        public string InsertClasses(InsertClasses obj)
        {
            string returnResult = (dynamic)null;
            try
            {
                if (obj != null)
                {
                    //[Note: insert 'Departments' table]
                    if (obj.classes != null)
                    {
                        var classes = new Classes()
                        {
                            InstitutionId = obj.classes.InstitutionId,
                            Name = obj.classes.Name,
                            Sorting = obj.classes.Sorting,
                            AddedBy = obj.classes.AddedBy,
                            AddedDate = obj.classes.AddedDate
                        };

                        _classesRepo.Insert(classes);
                        returnResult = "Saved";

                    }
                }
            }
            catch (Exception ex)
            {
                returnResult = "ERROR102:ClassesServ/InsertClasses - " + ex.Message;
            }

            return returnResult;

        }

        public void UpdateClasses(Classes classes)
        {
            _classesRepo.Update(classes);
        }
        
        public DeleteClasses DeleteClasses(DeleteClasses obj)
        {
            var returnModel = new DeleteClasses();           
            try
            {
                if (obj.Id > 0)
                {
                    var classes = _classesRepo.Get(obj.Id);
                    if (classes != null)
                    {
                        _classesRepo.Delete(classes);
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
                    returnModel.Message = "ERROR102:ClassesServ/DeleteClasses - " + ex.Message;
                    returnModel.SuccessIndicator = false;
                }                
            }            
            return returnModel; 
        }
        #endregion "Insert Update Delete Methods"

        #region "Dropdown Methods"
        public IEnumerable<dropdown_Classes> Dropdown_Classes(long institutionId)
        {
            var queryAll = _classesRepo.GetAll().ToList();            
            var getCls = from c in queryAll
                         where c.InstitutionId == institutionId
                         orderby c.Sorting
                         select c;
            

            var queryResult = new List<dropdown_Classes>();

            foreach (var item in getCls)            
            {
                var d = new dropdown_Classes()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                queryResult.Add(d);
            }

            return queryResult;

        }
        public IEnumerable<dropdown_Classes> Dropdown_Classes(long institutionId, long empId, long year)
        {
            var queryClass = _classesRepo.GetAll().ToList();
            var queryAgnTeacher = _assignedTeachersRepo.GetAll().ToList();
            var getClass = (from c in queryClass
                            join at in queryAgnTeacher on c.Id equals at.ClassId
                            where at.InstitutionId == institutionId && at.EmployeeId == empId && at.Year.Year == year
                            select new { c }).Distinct();
            var queryResult = new List<dropdown_Classes>();

            foreach (var item in getClass)
            {
                var d = new dropdown_Classes()
                {
                    Id = item.c.Id,
                    Name = item.c.Name
                };
                queryResult.Add(d);
            }
            return queryResult;

        }
        #endregion "Dropdown Methods"
    }
}
