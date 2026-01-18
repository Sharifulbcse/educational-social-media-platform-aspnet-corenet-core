
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class ExamTypesMap
    {
        public ExamTypesMap(EntityTypeBuilder<ExamTypes> entityBuilder)
        {
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.InsId);
            entityBuilder.Property(t => t.BreakDownInP);
            entityBuilder.Property(t => t.IsLastExam);
            entityBuilder.Property(t => t.Sorting);
        }
    }
}
