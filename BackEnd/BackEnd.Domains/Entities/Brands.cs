namespace BackEnd.Domains.Entities
{
    public partial class Brands
    {
        public int PK_BRAND { get; set; }
        public string? BRAND_NAME { get; set; }
        public string? STATUS {  get; set; }
        public DateTime? CREATION_DATE { get; set; }
    }
}
