
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace OE.Data
{
    public class EmployeeDesignationsMap
    {
        public EmployeeDesignationsMap(EntityTypeBuilder<EmployeeDesignations> entityBuilder)
        {
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.EmployeeId);
            entityBuilder.Property(t => t.EmployeeTypeId);
            entityBuilder.Property(t => t.EmployeeTypeCategoryId);
            entityBuilder.Property(t => t.StartDate);
            entityBuilder.Property(t => t.EndDate);
            entityBuilder.Property(t => t.InsId);
        }
    }
}



