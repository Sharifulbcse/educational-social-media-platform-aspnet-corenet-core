using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class InsPagesMap
    {
        public InsPagesMap(EntityTypeBuilder<InsPages> entityBuilder)
        {
            entityBuilder.Property(t => t.Title);
            entityBuilder.Property(t => t.IP300X200);
            entityBuilder.Property(t => t.IP600X400);
        }
    }
}
