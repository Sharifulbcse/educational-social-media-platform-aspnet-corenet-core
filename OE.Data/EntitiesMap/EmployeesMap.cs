
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace OE.Data
{
    public class EmployeesMap
    {
        public EmployeesMap(EntityTypeBuilder<Employees> entityBuilder)
        {
            entityBuilder.Property(t => t.DOB);
            entityBuilder.Property(t => t.JoiningDate);
            entityBuilder.Property(t => t.GenderId);
            entityBuilder.Property(t => t.UserId);
            entityBuilder.Property(t => t.InstitutionId);
            entityBuilder.Property(t => t.InsId);
            
            entityBuilder.Property(t => t.FirstName);
            entityBuilder.Property(t => t.LastName);
            entityBuilder.Property(t => t.IP300X200);
            entityBuilder.Property(t => t.PresentAddress);
            entityBuilder.Property(t => t.PermanentAddress);
            entityBuilder.Property(t => t.ContactNo);
            entityBuilder.Property(t => t.EmailAddress);

        }
    }
}
