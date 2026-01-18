
using System.Collections.Generic;

namespace OE.Repo
{
    public interface IOE_StaffsRepo<T> 
    {
        IEnumerable<T> GetAll();
        long GetLastId();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);       
    }
}
