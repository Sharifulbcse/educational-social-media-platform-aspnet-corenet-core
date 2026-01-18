
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class MarkTypesMap
    {
        public MarkTypesMap(EntityTypeBuilder<MarkTypes> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.IsActive);            
            entityBuilder.Property(t => t.InstitutionId);            
        }
    }
}
