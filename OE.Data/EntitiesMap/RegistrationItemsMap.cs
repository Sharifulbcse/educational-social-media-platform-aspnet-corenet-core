
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class RegistrationItemsMap
    {
        public RegistrationItemsMap(EntityTypeBuilder<RegistrationItems> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.RegistrationGroupId);
            entityBuilder.Property(t => t.RegistrationItemTypeId);
            entityBuilder.Property(t => t.RegistrationUserTypeId);
        }
    }
}
