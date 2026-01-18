
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class AssignedTeachersMap
    {
        public AssignedTeachersMap(EntityTypeBuilder<AssignedTeachers> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Year);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.AssignedCourseId);
            entityBuilder.Property(t => t.AssignedSectionId);
            entityBuilder.Property(t => t.EmployeeId);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.InsId);
        }
    }
}

