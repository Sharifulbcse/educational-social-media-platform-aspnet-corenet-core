
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class OE_LicensesMap
    {
        public OE_LicensesMap(EntityTypeBuilder<OE_Licenses> entityBuilder)
        {
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.DP);
            entityBuilder.Property(t => t.LicenseNumber);
            entityBuilder.Property(t => t.StartDate);
            entityBuilder.Property(t => t.EndDate);
        }
    }
}
