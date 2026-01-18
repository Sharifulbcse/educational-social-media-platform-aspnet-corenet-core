
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class RegistrationGroupsMap
    {
        public RegistrationGroupsMap(EntityTypeBuilder<RegistrationGroups> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.RegistrationUserTypeId);
        }
    }
}
