namespace BackEnd.Application.Dtos.UsersDtos
{
    public class UserReadDto
    {
        public int PK_USER { get; set; }
        public string? USER_NAME { get; set; }
        public byte[]? PASSWORD_HASH { get; set; }
        public string? NAMES { get; set; }
        public string? LAST_NAMES { get; set; }
        public string? IDENTIFICATION { get; set; }
        public string? PHONE { get; set; }
        public string? STATUS { get; set; }
        public int PK_ROLE { get; set; }
        public int PK_STORE { get; set; }
        public string? CREATION_DATE { get; set; }

        public string? ROLE_NAME { get; set; }
        public string? STORE_NAME { get; set; }
    }
}
