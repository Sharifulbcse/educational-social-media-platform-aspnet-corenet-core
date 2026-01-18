
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class ClassTimeScheduleActionDatesMap
    {
        public ClassTimeScheduleActionDatesMap(EntityTypeBuilder<ClassTimeScheduleActionDates> entityBuilder)
        {
            entityBuilder.Property(t => t.EffectiveStartDate);
            entityBuilder.Property(t => t.EffectiveEndDate);
            entityBuilder.Property(t => t.Sorting);
            entityBuilder.Property(t => t.IsActive);

        }
    }
}
