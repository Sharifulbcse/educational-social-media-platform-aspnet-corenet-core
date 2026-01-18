using System.Collections.Generic;
using OE.Data;

namespace OE.Repo
{
    public interface IEmployeeTypesRepo<T> where T : BaseEntity
    {
        T Get(long id);
        long GetLastId();
        IEnumerable<T> GetAll();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}

