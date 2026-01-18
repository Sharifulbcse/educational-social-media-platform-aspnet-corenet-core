using OE.Data;
using OE.Repo;
using System.Collections.Generic;
using System.Linq;

namespace OE.Service
{
    public class AttendanceCalculationsServ : IAttendanceCalculationsServ
    {

        #region "Variables"
        private readonly IAttendanceCalculationsRepo<AttendanceCalculations> _attendanceCalculationsRepo;
        #endregion "Variables"

        #region "Constructor"
        public AttendanceCalculationsServ(
        IAttendanceCalculationsRepo<AttendanceCalculations> attendanceCalculationsRepo
            )
        {
            _attendanceCalculationsRepo = attendanceCalculationsRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"        
        public IEnumerable<AttendanceCalculations> GetAttendanceCalculations(long institutionId)
        {
            var getAll = _attendanceCalculationsRepo.GetAll();
            var returnQry = from e in getAll
                            where e.InstitutionId == institutionId
                            select e;
            return returnQry;
        }

        public AttendanceCalculations GetAttendanceCalculationById(long id)
        {
            var getAll = _attendanceCalculationsRepo.GetAll();
            var returnQry = (from e in getAll
                             where e.Id == id
                             select e).SingleOrDefault();
            return returnQry;
        }

        #endregion "Get Methods"

        #region "Insert Update Delete Methods"

        public void InsertAttendanceCalculation(AttendanceCalculations attendanceCalculations)
        {
            _attendanceCalculationsRepo.Insert(attendanceCalculations);
        }
        public void UpdateAttendanceCalculation(AttendanceCalculations attendanceCalculations)
        {
            _attendanceCalculationsRepo.Update(attendanceCalculations);
        }
        public void DeleteAttendanceCalculation(AttendanceCalculations attendanceCalculations)
        {
            _attendanceCalculationsRepo.Delete(attendanceCalculations);
        }
        #endregion "Insert Update Delete Methods"


    }
}


