using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class ClassTimeSchedulesMap
    {
        public ClassTimeSchedulesMap(EntityTypeBuilder<ClassTimeSchedules> entityBuilder)
        {
            
            entityBuilder.Property(t => t.InstitutionId);
            
            entityBuilder.Property(t => t.ClassTimeScheduleActionDateId);
            
            entityBuilder.Property(t => t.ClassStartTime);
            entityBuilder.Property(t => t.ClassEndTime);
            entityBuilder.Property(t => t.Sorting);

            entityBuilder.Property(t => t.InsId);
            entityBuilder.Property(t => t.IsActive);
        }
    }
}

