namespace BackEnd.Application.Dtos.GoodsIssueDtos
{
    public class GoodIssueReadDto
    {
        public int PK_ISSUE { get; set; }
        public string? CODE { get; set; }
        public string? DATE_SALE { get; set; }
        public string? DATE_REGISTRATION { get; set; }
        public string? TYPE_ISSUE { get; set; }
        public string? TYPE_DOCUMENT { get; set; }
        public string? DOCUMENT_NUMBER { get; set; }
        public string? ANNOTATIONS { get; set; }
        public string? STATUS { get; set; }
        public int PK_CUSTOMER { get; set; }
        public int PK_USER { get; set; }
        public int PK_WAREHOUSE { get; set; }
        public string? NAMES { get; set; }
        public string? USER_NAME { get; set; }
        public string? STORE_NAME { get; set; }
    }
}
