namespace BackEnd.Application.Dtos.DetailsIssueDtos
{
    public class DetailIssueReadDto
    {
        public string? CODE { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? MATERIAL { get; set; }
        public string? COLOUR { get; set; }
        public string? BRAND_NAME { get; set; }
        public string? CATEGORY_NAME { get; set; }
        public int QUANTITY { get; set; }
        public int PRICE { get; set; }
    }
}
