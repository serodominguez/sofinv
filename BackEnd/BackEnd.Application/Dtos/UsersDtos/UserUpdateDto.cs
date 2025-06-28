using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.UsersDtos
{
    public class UserUpdateDto
    {
        [Required]
        public int PK_USER { get; set; }

        [Required(ErrorMessage = "The user name cannot be empty.")]
        [StringLength(25, ErrorMessage = "The user name must be 1 to 25 characters long.", MinimumLength = 1)]
        public string? USER_NAME { get; set; }

        [Required(ErrorMessage = "The names cannot be empty.")]
        [StringLength(30, ErrorMessage = "The names must be 1 to 30 characters long.", MinimumLength = 1)]
        public string? NAMES { get; set; }

        [Required(ErrorMessage = "The surnames cannot be empty.")]
        [StringLength(30, ErrorMessage = "The surnames must be 1 to 30 characters long.", MinimumLength = 1)]
        public string? LAST_NAMES{ get; set; }
        public string? IDENTIFICATION { get; set; }
        public string? PHONE { get; set; }
        public int PK_ROLE { get; set; }
        public int PK_STORE { get; set; }
        public string? PASSWORD { get; set; }
        public bool? UPDATEPASSWORD { get; set; }
    }
}
