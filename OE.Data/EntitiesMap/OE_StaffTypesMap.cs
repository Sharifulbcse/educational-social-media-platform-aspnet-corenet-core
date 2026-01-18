
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class OE_StaffTypesMap
    {
        public OE_StaffTypesMap(EntityTypeBuilder<OE_StaffTypes> entityBuilder)
        {          
            entityBuilder.Property(t => t.Id);
            entityBuilder.Property(t => t.Name);
        }
    }
}
