
using System.Collections.Generic;
using OE.Data;

namespace OE.Repo
{  
    public interface IOE_ActorsRepo<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
    }
    
}
