namespace BackEnd.Domains.Entities
{
    public class Products
    {
        public int PK_PRODUCT { get; set; }
        public string? CODE { get; set; }
        public string? DESCRIPTION { get; set; }
        public string? MEASUREMENT { get; set; }
        public string? MATERIAL { get; set; }
        public string? COLOUR { get; set; }
        public string? STATUS { get; set; }
        public int PK_BRAND { get; set; }
        public int PK_CATEGORY { get; set; }
        public DateTime? CREATION_DATE { get; set; }


        public Brands? brands { get; set; }
        public Categories? categories { get; set; }
        public List<DetailsWarehouse>? storing { get; set; }

        public string BRAND_NAME
        {
            get { return brands?.BRAND_NAME ?? string.Empty; }
        }

        public string CATEGORY_NAME
        {
            get { return categories?.CATEGORY_NAME ?? string.Empty; }
        }
    }
}
