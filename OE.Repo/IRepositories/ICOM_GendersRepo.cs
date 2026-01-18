
using System.Collections.Generic;
using OE.Data;

namespace OE.Repo
{
    public interface ICOM_GendersRepo<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        T Get(long id);
    }
}

