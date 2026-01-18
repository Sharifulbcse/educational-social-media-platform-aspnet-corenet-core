
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class InsCategoriesMap
    {
        public InsCategoriesMap(EntityTypeBuilder<InsCategories> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.CountryId);
        }
    }
}
