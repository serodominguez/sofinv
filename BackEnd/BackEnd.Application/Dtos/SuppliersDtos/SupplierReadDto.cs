namespace BackEnd.Application.Dtos.SuppliersDtos
{
    public class SupplierReadDto
    {
        public int PK_SUPPLIER { get; set; }
        public string? COMPANY_NAME { get; set; }
        public string? CONTACT { get; set; }
        public int PHONE { get; set; }
        public string? EMAIL { get; set; }
        public string? STATUS { get; set; }
        public string? CREATION_DATE { get; set; }
    }
}
