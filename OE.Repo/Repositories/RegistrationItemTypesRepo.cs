using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;
using OE.Data;


namespace OE.Repo
{
    public class RegistrationItemTypesRepo<T> : IRegistrationItemTypesRepo<T> where T: COM_RegistrationItemTypes
    {
        private readonly OurEduMediaContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public RegistrationItemTypesRepo(OurEduMediaContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity is not save");
            }
            entities.Add(entity);
            context.SaveChanges();
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity is not update");
            }
            entities.Update(entity);
            context.SaveChanges();
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity is not update");
            }
            entities.Remove(entity);
            context.SaveChanges();
        }

    }
}
