namespace BackEnd.Application.Dtos.UsersDtos
{
    public class UserCredentialsDto
    {
        public int PK_USER { get; set; }
        public string? USER_NAME { get; set; }
        public byte[]? PASSWORD_HASH { get; set; }
        public byte[]? PASSWORD_SALT { get; set; }
        public int PK_ROLE { get; set; }
        public int PK_STORE { get; set; }
        public int PK_WAREHOUSE { get; set; }
        public string? ROLE_NAME { get; set; }
        public string? STORE_NAME { get; set; }
    }
}
