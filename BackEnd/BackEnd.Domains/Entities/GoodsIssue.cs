namespace BackEnd.Domains.Entities
{
    public class GoodsIssue
    {
        public int PK_ISSUE { get; set; }
        public string? CODE { get; set; }
        public DateTime? DATE_SALE { get; set; }
        public DateTime? DATE_REGISTRATION { get; set; }
        public string? TYPE_ISSUE { get; set; }
        public string? TYPE_DOCUMENT { get; set; }
        public string? DOCUMENT_NUMBER { get; set; }
        public string? ANNOTATIONS { get; set; }
        public string? STATUS { get; set; }
        public int PK_CUSTOMER { get; set; }
        public int PK_USER { get; set; }
        public int PK_WAREHOUSE { get; set; }

        public Stores? stores { get; set; }
        public Customers? customers { get; set; }
        public Users? users { get; set; }
        public Warehouses? warehouses { get; set; }
        public List<DetailsIssue>? details { get; set; }

        public string NAMES
        {
            get { return customers?.NAMES ?? string.Empty; }
        }

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
