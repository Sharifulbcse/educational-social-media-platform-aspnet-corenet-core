
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class OE_UserAuthenticationsMap
    {
        public OE_UserAuthenticationsMap(EntityTypeBuilder<OE_UserAuthentications> entityBuilder)
        {
            entityBuilder.Property(t => t.ActorId);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.UserId);
        }
    }
}
