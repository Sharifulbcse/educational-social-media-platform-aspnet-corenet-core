
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class StudentsMap
    {
        public StudentsMap(EntityTypeBuilder<Students> entityBuilder)
        {           
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.UserId);
            entityBuilder.Property(t => t.ClassId);
            entityBuilder.Property(t => t.GenderId);
            entityBuilder.Property(t => t.Name);
            entityBuilder.Property(t => t.IP300X200);
            entityBuilder.Property(t => t.PresentAddress);
            entityBuilder.Property(t => t.PermanentAddress);
            entityBuilder.Property(t => t.DOB);
            entityBuilder.Property(t => t.AdmittedYear);
            
        }
    }
}
