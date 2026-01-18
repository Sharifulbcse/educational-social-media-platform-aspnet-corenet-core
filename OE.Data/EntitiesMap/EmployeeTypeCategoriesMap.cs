
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class EmployeeTypeCategoriesMap
    {
        public EmployeeTypeCategoriesMap(EntityTypeBuilder<EmployeeTypeCategories> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.EmployeeTypeId);
        }
    }
}
