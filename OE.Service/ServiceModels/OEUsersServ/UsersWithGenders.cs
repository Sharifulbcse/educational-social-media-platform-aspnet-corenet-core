
using OE.Data;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class UsersWithGenders
    {
        public COM_Genders Genders { get; set; }
        public OE_Users Users { get; set; }
    }
}
