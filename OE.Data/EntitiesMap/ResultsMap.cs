
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class ResultsMap
    {
        public ResultsMap(EntityTypeBuilder<Results> entityBuilder)
        {
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.StudentId);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.EmployeeId);
            entityBuilder.Property(t => t.ExamTypeId);
            entityBuilder.Property(t => t.SubjectId);
            entityBuilder.Property(t => t.MarkTypeId);
            entityBuilder.Property(t => t.Mark);
            entityBuilder.Property(t => t.Year);
        }
    }
}
