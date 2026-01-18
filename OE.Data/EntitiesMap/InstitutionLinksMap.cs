using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class InstitutionLinksMap
    {
        public InstitutionLinksMap(EntityTypeBuilder<InstitutionLinks> entityBuilder)
        {

            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.Url);
            entityBuilder.Property(t => t.IP24X24);

        }
    }
}
