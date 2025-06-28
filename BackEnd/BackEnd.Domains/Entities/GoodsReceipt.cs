namespace BackEnd.Domains.Entities
{
    public class GoodsReceipt
    {
        public int PK_RECEIPT { get; set; }
        public string? CODE { get; set; }
        public DateTime? DATE_PURCHASE { get; set; }
        public DateTime? DATE_REGISTRATION { get; set; }
        public string? TYPE_RECEIPT { get; set; }
        public string? TYPE_DOCUMENT { get; set; }
        public string? DOCUMENT_NUMBER { get; set; }
        public string? ANNOTATIONS { get; set; }
        public string? STATUS { get; set; }
        public int PK_SUPPLIER { get; set; }
        public int PK_USER { get; set; }
        public int PK_WAREHOUSE { get; set; }

        public Stores? stores { get; set; }
        public Suppliers? suppliers { get; set; }
        public Users? users { get; set; }
        public Warehouses? warehouses { get; set; }
        public List<DetailsReceipt>? details { get; set; }

        public string COMPANY_NAME
        {
            get { return suppliers?.COMPANY_NAME ?? string.Empty; }
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
