
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class OE_UsersMap
    {
        public OE_UsersMap(EntityTypeBuilder<OE_Users> entityBuilder)
        {
            entityBuilder.Property(t => t.FirstName);
            entityBuilder.Property(t => t.LastName);
            entityBuilder.Property(t => t.IP300X200);
            entityBuilder.Property(t => t.IP600X400);
            entityBuilder.Property(t => t.EmailAddress);
            entityBuilder.Property(t => t.ContactNo);
            entityBuilder.Property(t => t.DateOfBirth);
            entityBuilder.Property(t => t.GenderId);
            entityBuilder.Property(t => t.UserLoginId);
            entityBuilder.Property(t => t.Password);
            entityBuilder.Property(t => t.IsForgetPassword);
        }
    }
}
