
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class AssignedSectionsMap
    {
        public AssignedSectionsMap(EntityTypeBuilder<AssignedSections> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.Year);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.InstitutionId);
        }
    }
}

