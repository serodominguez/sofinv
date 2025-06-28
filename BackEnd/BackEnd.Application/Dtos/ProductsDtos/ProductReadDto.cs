namespace BackEnd.Application.Dtos.ProductsDtos
{
    public class ProductReadDto
    {
        public int PK_PRODUCT { get; set; }
        public string? CODE { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? MATERIAL { get; set; }
        public string? COLOUR { get; set; }
        public string? MEASUREMENT { get; set; }
        public string? STATUS { get; set; }
        public int? PK_BRAND { get; set; }
        public string? BRAND_NAME { get; set; }
        public int PK_CATEGORY { get; set; }
        public string? CATEGORY_NAME { get; set; }
        public string? CREATION_DATE { get; set; }
    }
}
