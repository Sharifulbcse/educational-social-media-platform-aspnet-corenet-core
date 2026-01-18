using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class COM_RegistrationUserTypesMap
    {
        public COM_RegistrationUserTypesMap(EntityTypeBuilder<COM_RegistrationUserTypes> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
        }
    }
}
