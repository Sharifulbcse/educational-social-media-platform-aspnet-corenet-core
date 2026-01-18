
using System.Collections.Generic;
using OE.Data;

namespace OE.Repo
{
    public interface ICOM_RegistrationUserTypesRepo<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
    }
}
