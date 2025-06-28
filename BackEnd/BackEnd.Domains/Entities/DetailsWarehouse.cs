namespace BackEnd.Domains.Entities
{
    public class DetailsWarehouse
    {
        public int PK_WAREHOUSE { get; set; }
        public int PK_PRODUCT { get; set; }
        public int PRICE { get; set; }
        public int STOCK { get; set; }

        public Products? products { get; set; }


        public string CODE
        {
            get { return products?.CODE ?? string.Empty; }
        }

        public string DESCRIPTION
        {
            get { return products?.DESCRIPTION ?? string.Empty; }
        }
        public string MATERIAL
        {
            get { return products?.MATERIAL ?? string.Empty; }
        }
        public string COLOUR
        {
            get { return products?.COLOUR ?? string.Empty; }
        }

        public string CATEGORY_NAME
        {
            get { return products?.CATEGORY_NAME ?? string.Empty; }
        }

        public string BRAND_NAME
        {
            get { return products?.BRAND_NAME ?? string.Empty; }
        }
    }
}
