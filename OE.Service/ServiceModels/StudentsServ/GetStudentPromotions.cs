using OE.Data;

namespace OE.Service.ServiceModels
{
    public class GetStudentPromotions
    {
        public StudentPromotions StudentPromotions { get; set; }
        public Students Students { get; set; }
        public Classes Classes { get; set; }
    }
}
