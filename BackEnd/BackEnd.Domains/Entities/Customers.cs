namespace BackEnd.Domains.Entities
{
    public class Customers
    {
        public int PK_CUSTOMER { get; set; }
        public string? NAMES { get; set; }
        public string? LAST_NAMES { get; set; }
        public string? IDENTIFICATION { get; set; }
        public int? PHONE { get; set; }
        public string? STATUS { get; set; }
        public DateTime? CREATION_DATE { get; set; }
    }
}
