
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class DistributionMarkActionDatesMap
    {
        public DistributionMarkActionDatesMap(EntityTypeBuilder<DistributionMarkActionDates> entityBuilder)
        {
            entityBuilder.Property(t => t.EffectiveStartDate);
            entityBuilder.Property(t => t.EffectiveEndDate);
            entityBuilder.Property(t => t.IsActive);

        }
    }
}
