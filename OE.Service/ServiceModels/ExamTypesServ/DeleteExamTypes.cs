using OE.Data;

namespace OE.Service.ServiceModels
{    
    public class DeleteExamTypes : MessageModel    
    {
        public ExamTypes ExamTypes { get; set; }
        public long Id { get; set; }
        

    }
}

