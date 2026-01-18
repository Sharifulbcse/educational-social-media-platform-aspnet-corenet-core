
using System;

namespace OE.Data
{
    public class EmployeeDetails : BaseEntity
    {
        public long InstitutionId { get; set; }
        public long EmployeeId { get; set; }
        public long RegistrationItemId { get; set; }
        public string StringValue { get; set; }
        public long? WholeValue { get; set; }
        public double? FloatValue { get; set; }
        public DateTime? DateValue { get; set; }
        public string FilePathValue { get; set; }
        public string ImagePathValue { get; set; }
        public bool? BitValue { get; set; }
        public string TextAreaValue { get; set; }
        public long InsId { get; set; }
        public bool? IsActive { get; set; }

    }
}


