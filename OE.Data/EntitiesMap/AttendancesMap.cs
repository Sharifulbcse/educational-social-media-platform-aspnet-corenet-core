using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class AttendancesMap
    {
        public AttendancesMap(EntityTypeBuilder<Attendances> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.StudentId);
            entityBuilder.Property(t => t.ClassTimeScheduleId);
            entityBuilder.Property(t => t.EmployeeId);
            entityBuilder.Property(t => t.AssignedCourseId);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.AssignedSectionId);
            entityBuilder.Property(t => t.AttendanceDate);
            entityBuilder.Property(t => t.IsPresent);
            entityBuilder.Property(t => t.InsId);
        }
    }
}

