namespace BackEnd.Application.Dtos.DetailsWarehouseDtos
{
    public class DetailWarehouseManageDto
    {
        public int PK_WAREHOUSE { get; set; }
        public int PK_PRODUCT { get; set; }
        public int STOCK { get; set; }
        public int PRICE { get; set; }
        public string? CODE { get; set; }
        public string? DESCRIPTION { get; set; } 
        public string? MATERIAL { get; set; }
        public string? COLOUR { get; set; }
        public string? BRAND_NAME { get; set; }
        public string? CATEGORY_NAME { get; set; }
    }
}
