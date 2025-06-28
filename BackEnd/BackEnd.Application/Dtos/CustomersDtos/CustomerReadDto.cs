namespace BackEnd.Application.Dtos.CustomersDtos
{
    public class CustomerReadDto
    {
        public int PK_CUSTOMER { get; set; }
        public string? NAMES { get; set; }
        public string? LAST_NAMES { get; set; }
        public string? IDENTIFICATION { get; set; }
        public string? PHONE { get; set; }
        public string? STATUS { get; set; }
        public string? CREATION_DATE { get; set; }
    }
}
