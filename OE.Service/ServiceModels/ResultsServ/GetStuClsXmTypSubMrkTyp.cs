
using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetStuClsXmTypSubMrkTyp
    {
        public Results Results { get; set; }
        public Students Students { get; set; }
        public Classes Classes { get; set; }
        public ExamTypes ExamTypes { get; set; }
        public Subjects Subjects { get; set; }
        public MarkTypes MarkTypes { get; set; }
    }
}
