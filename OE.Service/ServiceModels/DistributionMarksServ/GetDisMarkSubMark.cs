
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetDisMarkSubMark
    {
        public DistributionMarks DistributionMarks { get; set; }
        public Subjects Subjects { get; set; }
        public MarkTypes MarkTypes { get; set; }
        public Classes Classes { get; set; }
    }
}
