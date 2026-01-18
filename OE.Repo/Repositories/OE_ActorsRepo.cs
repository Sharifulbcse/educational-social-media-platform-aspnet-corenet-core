
using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;

using Microsoft.EntityFrameworkCore;

namespace OE.Repo
{
    
    public class OE_ActorsRepo<T> : IOE_ActorsRepo<T> where T : OE_Actors
    {
        private readonly OurEduMediaContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;

        public OE_ActorsRepo(OurEduMediaContext context)
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
