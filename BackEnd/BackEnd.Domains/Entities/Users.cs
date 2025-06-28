namespace BackEnd.Domains.Entities
{
    public class Users
    {
        public int PK_USER { get; set; }
        public string? USER_NAME { get; set; }
        public byte[]? PASSWORD_HASH { get; set; }
        public byte[]? PASSWORD_SALT { get; set; }
        public string? NAMES { get; set; }
        public string? LAST_NAMES { get; set; }
        public string? IDENTIFICATION { get; set; }
        public int? PHONE { get; set; }
        public string? STATUS { get; set; }
        public int PK_ROLE { get; set; }
        public int PK_STORE { get; set; }
        public DateTime? CREATION_DATE { get; set; }

        public Roles? roles { get; set; }
        public Stores? stores { get; set; }

        public string ROLE_NAME
        {
            get { return roles?.ROLE_NAME ?? string.Empty; }
        }

        public string STORE_NAME
        {
            get { return stores?.STORE_NAME ?? string.Empty; }
        }
    }
}
