namespace BackEnd.Domains.Entities
{
    public class Warehouses
    {
        public int PK_WAREHOUSE { get; set; }
        public string? WAREHOUSE_NAME { get; set; }
        public string? TYPE_WAREHOUSE { get; set; }
        public int PK_STORE { get; set; }
        public string? STATUS { get; set; }
        public DateTime? CREATION_DATE { get; set; }
    }
}
