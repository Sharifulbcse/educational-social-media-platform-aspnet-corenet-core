
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
   public class COM_RegistrationItemTypesMap
    {
        public COM_RegistrationItemTypesMap(EntityTypeBuilder<COM_RegistrationItemTypes> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
        }
    }
}
