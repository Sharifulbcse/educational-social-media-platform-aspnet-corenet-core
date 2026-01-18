using System.Collections.Generic;
using OE.Data;

namespace OE.Repo
{
    public interface IInsPagesRepo<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
