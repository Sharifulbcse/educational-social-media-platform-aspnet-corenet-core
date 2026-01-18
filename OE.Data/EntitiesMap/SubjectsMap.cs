
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class SubjectsMap
    {
        public SubjectsMap(EntityTypeBuilder<Subjects> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.ClassId);            
            entityBuilder.Property(t => t.SubjectTypeId);
        }
    }
}
