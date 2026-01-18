
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace OE.Data
{
    public class EmployeeDetailsMap
    {
        public EmployeeDetailsMap(EntityTypeBuilder<EmployeeDetails> entityBuilder)
        {
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.EmployeeId);
            entityBuilder.Property(t => t.RegistrationItemId);
            entityBuilder.Property(t => t.StringValue);
            entityBuilder.Property(t => t.WholeValue);
            entityBuilder.Property(t => t.FloatValue);
            entityBuilder.Property(t => t.DateValue);
            entityBuilder.Property(t => t.FilePathValue);
            entityBuilder.Property(t => t.ImagePathValue);
            entityBuilder.Property(t => t.BitValue);
            entityBuilder.Property(t => t.TextAreaValue);
            entityBuilder.Property(t => t.InsId);
        }
    }
}


