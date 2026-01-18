using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class AttendanceCalculationsMap
    {
        public AttendanceCalculationsMap(EntityTypeBuilder<AttendanceCalculations> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.StudentId);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.AssignedCourseId);
            entityBuilder.Property(t => t.AssignedSectionId);
            entityBuilder.Property(t => t.TotalAttendance);
            entityBuilder.Property(t => t.TotalClass);
            entityBuilder.Property(t => t.Year);
            entityBuilder.Property(t => t.InsId);
        }
    }
}


