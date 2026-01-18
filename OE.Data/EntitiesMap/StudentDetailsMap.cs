
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class StudentDetailsMap
    {
        public StudentDetailsMap(EntityTypeBuilder<StudentDetails> entityBuilder)
        {
            
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.StudentId);
            entityBuilder.Property(t => t.RegistrationItemId);
            entityBuilder.Property(t => t.StringValue);
            entityBuilder.Property(t => t.WholeValue);
            entityBuilder.Property(t => t.FloatValue);
            entityBuilder.Property(t => t.DateValue);
            entityBuilder.Property(t => t.FilePathValue);
            entityBuilder.Property(t => t.ImagePathValue);
            entityBuilder.Property(t => t.BitValue);
            entityBuilder.Property(t => t.TextAreaValue);
           
        }
    }
}
