namespace OE.Web.Models
{
    public class IndexLoginVM
    {
        public string InstitutionName { get; set; }
        public string Logo { get; set; }

        //[NOTE: Extra field from OE_Users]
        public string UserLoginId { get; set; }
        public string Password { get; set; }
       
    }
}
