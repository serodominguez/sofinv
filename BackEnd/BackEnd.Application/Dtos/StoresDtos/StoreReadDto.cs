namespace BackEnd.Application.Dtos.StoresDtos
{
    public class StoreReadDto
    {
        public int PK_STORE { get; set; }
        public string? STORE_NAME { get; set; }
        public string? MANAGER { get; set; }
        public string? ADDRESS { get; set; }
        public string? PHONE { get; set; }
        public string? CITY { get; set; }
        public string? EMAIL { get; set; }
        public string? TYPE { get; set; }
        public string? STATUS { get; set; }
        public string? CREATION_DATE { get; set; }
    }
}
