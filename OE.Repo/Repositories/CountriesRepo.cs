
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;

using Microsoft.EntityFrameworkCore;

namespace OE.Repo
{
    public class CountriesRepo<T> : ICountriesRepo<T> where T : Countries
    {
        private readonly OurEduMediaContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public CountriesRepo(OurEduMediaContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }
        public long GetLastId()
        {
            long lastId = GetAll().Count() == 0 ? 0 : GetAll().Last().Id;
            return lastId;
        }
        public void Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity is not save");
            }
            entity.Id = GetLastId() + 1;
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
