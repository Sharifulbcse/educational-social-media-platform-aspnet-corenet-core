
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class SubjectTypesMap
    {
        public SubjectTypesMap(EntityTypeBuilder<SubjectTypes> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.IsActive);
            entityBuilder.Property(t => t.InstitutionId);
        }
    }
}
