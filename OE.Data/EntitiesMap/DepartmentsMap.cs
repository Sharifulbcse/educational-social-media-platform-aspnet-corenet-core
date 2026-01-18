
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class DepartmentsMap
    {
        public DepartmentsMap(EntityTypeBuilder<Departments> entityBuilder)
        {           
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.InstitutionId);
        }
    }
}
