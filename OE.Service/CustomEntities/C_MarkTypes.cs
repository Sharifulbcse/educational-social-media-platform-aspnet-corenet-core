namespace OE.Service.CustomEntitiesServ
{
    public class C_MarkTypes
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long InstitutionId { get; set; }

        //[NOTE: Extra Fields: From Distribution Mark Entity]
        public long BreakDownInP { get; set; }

    }
}

