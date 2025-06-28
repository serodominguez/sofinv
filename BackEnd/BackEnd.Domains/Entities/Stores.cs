using System.Data;

namespace BackEnd.Domains.Entities
{
    public class Stores
    {
        public int PK_STORE { get; set; }
        public string? STORE_NAME { get; set; }
        public string? MANAGER { get; set; }
        public string? ADDRESS { get; set; }
        public int? PHONE { get; set; }
        public string? CITY { get; set; }
        public string? EMAIL { get; set; }
        public string? TYPE { get; set; }
        public string? STATUS { get; set; }
        public DateTime? CREATION_DATE { get; set; }

        public Warehouses? warehouses { get; set; }

        public int PK_WAREHOUSE 
        { 
            get { return warehouses?.PK_WAREHOUSE ?? 0; }
        }

        public string WAREHOUSE_NAME
        {
            get { return warehouses?.WAREHOUSE_NAME ?? string.Empty; }
        }
    }
}
