using System.Collections.Generic;
using OE.Data;

namespace OE.Repo
{
    public interface IEmployeeDesignationsRepo<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        long GetLastId();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}



