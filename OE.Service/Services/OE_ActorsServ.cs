using System;
using System.Linq;
using System.Collections.Generic;

using OE.Data;
using OE.Repo;
using OE.Service.ServiceModels;

namespace OE.Service
{  
    public class OE_ActorsServ : IOE_ActorsServ
    {
        #region "Variables"
        private IOE_ActorsRepo<OE_Actors> _actorsRepo;
        #endregion "Variables"

        #region "Constructor"
        public OE_ActorsServ(IOE_ActorsRepo<OE_Actors> actorsRepo)
        {
            _actorsRepo = actorsRepo;
        }
        #endregion "Constructor"

        #region "Get Methods"
        public IEnumerable<OE_Actors> GetActors()
        {            
            var queryAll = _actorsRepo.GetAll();
            var queryActors = from e in queryAll
                              orderby e.OrderNo ascending
                              select e;
            return queryActors;
        }
        #endregion "Get Methods"

        #region "Dropdown Methods"        

        public IEnumerable<dropdown_Actors> Dropdown_Actors(Int64 actorId)
        {
            var actorss = _actorsRepo.GetAll().ToList();
            var query = from a in actorss
                        orderby a.Name
                        select a;

            var filterQuery = (dynamic)null;
            if (actorId == 1)
            {
                filterQuery = query.Where(a => a.Id == 2 || a.Id == 3);
            }
            else if (actorId == 11)
            {
                filterQuery = query.Where(a => a.Id == 12 || a.Id == 13);
            }
            else
            {
                filterQuery = query;
            }
            if (actorId == 14)
            {
                filterQuery = query.Where(a => a.Id == 14);
            }

            //[NOTE: add new record]
            var queryResult = new List<dropdown_Actors>();            

            foreach (var item in filterQuery)
            {
                var d = new dropdown_Actors()
                {
                    Id = item.Id,
                    Name = item.Name
                };
                queryResult.Add(d);
            }
            return queryResult;

        }


        #endregion "Dropdown Methods"

    }
}
