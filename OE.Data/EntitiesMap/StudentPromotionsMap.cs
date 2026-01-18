using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace OE.Data
{
    public class StudentPromotionsMap
    {
        public StudentPromotionsMap(EntityTypeBuilder<StudentPromotions> entityBuilder)
        {

            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.StudentId);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.RollNo);
            entityBuilder.Property(t => t.Year);
            entityBuilder.Property(t => t.InsId);
        }
    }
}
