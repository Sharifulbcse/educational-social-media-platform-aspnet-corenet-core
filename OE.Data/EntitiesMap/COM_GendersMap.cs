
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class COM_GendersMap
    {
        public COM_GendersMap(EntityTypeBuilder<COM_Genders> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
        }
    }
}
