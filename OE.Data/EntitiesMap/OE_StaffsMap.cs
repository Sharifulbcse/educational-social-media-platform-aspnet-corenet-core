
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class OE_StaffsMap
    {
        public OE_StaffsMap(EntityTypeBuilder<OE_Staffs> entityBuilder)
        {
            entityBuilder.HasKey(t => t.Id);
            entityBuilder.Property(t => t.StaffTypeId);
            entityBuilder.Property(t => t.FirstName);
            entityBuilder.Property(t => t.LastName);
            entityBuilder.Property(t => t.IP300X200);
            entityBuilder.Property(t => t.IP600X400);
            entityBuilder.Property(t => t.Designation);
            entityBuilder.Property(t => t.PresentAddress);
            entityBuilder.Property(t => t.PermanentAddress);
            entityBuilder.Property(t => t.Contact);
            entityBuilder.Property(t => t.Email);
        }
    }
}
