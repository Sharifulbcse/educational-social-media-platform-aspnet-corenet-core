
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
   
    public class OE_ActorsMap
    {
        public OE_ActorsMap(EntityTypeBuilder<OE_Actors> entityBuilder)
        {            
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.OrderNo);
        }
       
    }
}
