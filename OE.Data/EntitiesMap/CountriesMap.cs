
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class CountriesMap
    {
        public CountriesMap(EntityTypeBuilder<Countries> entityBuilder)
        {
            entityBuilder.Property(t => t.Name);
        }
    }
}
