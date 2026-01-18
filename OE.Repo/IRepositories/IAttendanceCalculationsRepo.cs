using OE.Data;
using System.Collections.Generic;

namespace OE.Repo
{
    public interface IAttendanceCalculationsRepo<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(long id);
        long GetLastId();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}


