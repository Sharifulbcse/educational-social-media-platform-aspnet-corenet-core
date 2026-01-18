
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class AssignedCoursesMap
    {
        public AssignedCoursesMap(EntityTypeBuilder<AssignedCourses> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.Year);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.AssignedSectionId);
            entityBuilder.Property(t => t.SubjectId);
            entityBuilder.Property(t => t.InstitutionId);
        }
    }
}

