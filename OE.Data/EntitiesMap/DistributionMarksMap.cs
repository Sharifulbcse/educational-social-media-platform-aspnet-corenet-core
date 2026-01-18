
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class DistributionMarksMap
    {
        public DistributionMarksMap(EntityTypeBuilder<DistributionMarks> entityBuilder)
        {
            entityBuilder.Property(t => t.SubjectId);
            entityBuilder.Property(t => t.MarkTypeId);
            entityBuilder.Property(t => t.BreakDownInP);
            //entityBuilder.Property(t => t.EffectiveStartDate);
            //entityBuilder.Property(t => t.EffectiveEndDate);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.InsId);

        }
    }
}
