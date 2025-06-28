namespace BackEnd.Application.Dtos.GoodsReceiptDtos
{
    public class GoodReceiptReadDto
    {
        public int PK_RECEIPT { get; set; }
        public string? CODE { get; set; }
        public string? DATE_PURCHASE { get; set; }
        public string? DATE_REGISTRATION { get; set; }
        public string? TYPE_RECEIPT { get; set; }
        public string? TYPE_DOCUMENT { get; set; }
        public string? DOCUMENT_NUMBER { get; set; }
        public string? ANNOTATIONS { get; set; }
        public string? STATUS { get; set; }
        public int PK_SUPPLIER { get; set; }
        public int PK_USER { get; set; }
        public int PK_WAREHOUSE { get; set; }
        public string? COMPANY_NAME { get; set; }
        public string? USER_NAME { get; set; } 
        public string? STORE_NAME { get; set; }
    }
}
