using OE.Data;
using OE.Service.CustomEntitiesServ;
using System.Collections.Generic;

namespace OE.Service.ServiceModels
{
    public class GetStudentDetails : MessageModel
    {
        public Students Students { get; set; }
        public Classes Classes { get; set; }
        public COM_Genders Genders { get; set; }
        public StudentPromotions StudentPromotions { get; set; }
        public OE_Users Users { get; set; }
        public List<RegistrationGroups> _RegistrationGroups { get; set; }
        public List<RegistrationItems> _RegistrationItems { get; set; }
        public List<C_StudentDetails> _StudentDetails { get; set; }       
    }
}
