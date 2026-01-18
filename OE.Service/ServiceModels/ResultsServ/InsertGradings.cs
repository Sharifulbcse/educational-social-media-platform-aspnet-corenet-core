using OE.Data;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class InsertGradings
    {
        public List<Results> ResultList { get; set; }

        //[NOTE: Extra Fields]
        public long ExampTypeId { get; set; }
    }
}

