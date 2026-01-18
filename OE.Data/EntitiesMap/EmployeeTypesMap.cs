using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class EmployeeTypesMap
    {
        public EmployeeTypesMap(EntityTypeBuilder<EmployeeTypes> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.InstitutionId);
        }
    }
}
