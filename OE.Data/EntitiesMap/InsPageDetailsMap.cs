using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class InsPageDetailsMap
    {
        public InsPageDetailsMap(EntityTypeBuilder<InsPageDetails> entityBuilder)
        {
            entityBuilder.Property(t => t.InsPageId);
            entityBuilder.Property(t => t.Title);
            entityBuilder.Property(t => t.Description);
            entityBuilder.Property(t => t.Sorting);
        }
    }
}
