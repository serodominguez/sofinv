namespace BackEnd.Domains.Entities
{
    public class IncomingTransfers
    {
        public int PK_INCOMING { get; set; }
        public string? CODE { get; set; }
        public DateTime? DATE { get; set; }
        public string? INPUT_TYPE { get; set; }
        public string? ANNOTATIONS { get; set; }
        public string? STATUS { get; set; }
        public int PK_STORE { get; set; }
        public int PK_WAREHOUSE { get; set; }
        public int PK_USER { get; set; }

        public Stores? stores { get; set; }
        public Users? users { get; set; }
        public Warehouses? warehouses { get; set; }

        public List<DetailsIncoming>? details { get; set; }

        public string STORE_NAME
        {
            get { return stores?.STORE_NAME ?? string.Empty; }
        }

        public string USER_NAME
        {
            get { return users?.USER_NAME ?? string.Empty; }
        }

        public string WAREHOUSE_NAME
        {
            get { return warehouses?.WAREHOUSE_NAME ?? string.Empty; }
        }
    }
}
