
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class GradeTypesMap
    {
        public GradeTypesMap(EntityTypeBuilder<GradeTypes> entityBuilder)
        {
            
            entityBuilder.Property(t => t.StartMark);
            entityBuilder.Property(t => t.EndMark);
            entityBuilder.Property(t => t.Grade);            
            entityBuilder.Property(t => t.GPA);
            entityBuilder.Property(t => t.GPAOutOf);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.InsId);
        }
    }
}
