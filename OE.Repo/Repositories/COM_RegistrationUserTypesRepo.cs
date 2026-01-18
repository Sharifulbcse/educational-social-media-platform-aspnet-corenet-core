
using System.Linq;
using System.Collections.Generic;

using OE.Data;

using Microsoft.EntityFrameworkCore;

namespace OE.Repo
{
    public class COM_RegistrationUserTypesRepo<T> : ICOM_RegistrationUserTypesRepo<T> where T : COM_RegistrationUserTypes
    {
        private readonly OurEduMediaContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public COM_RegistrationUserTypesRepo(OurEduMediaContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }
    }
}
