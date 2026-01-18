
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class ResultSheet
    {
        public Results results { get; set; }
        public Subjects subjects { get; set; }
        public Classes classes { get; set; }
        public ExamTypes examTypes { get; set; }
        public MarkTypes markTypes { get; set; }
        public Employees employees { get; set; }
        public Students students { get; set; }
    }
}
