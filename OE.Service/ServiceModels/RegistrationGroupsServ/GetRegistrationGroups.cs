using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetRegistrationGroups
    {
        public COM_RegistrationUserTypes COM_RegistrationUserTypes { get; set; }
        public RegistrationGroups RegistrationGroups { get; set; }
    }
}
