using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.CustomersDtos
{
    public class CustomerUpdateDto
    {
        [Required]
        public int PK_CUSTOMER { get; set; }

        [Required(ErrorMessage = "The names cannot be empty.")]
        [StringLength(30, ErrorMessage = "The names must be 1 to 30 characters long.", MinimumLength = 1)]
        public string? NAMES { get; set; }

        [Required(ErrorMessage = "The surnames cannot be empty.")]
        [StringLength(30, ErrorMessage = "The surnames must be 1 to 30 characters long.", MinimumLength = 1)]
        public string? LAST_NAMES { get; set; }
        public string? IDENTIFICATION { get; set; }
        public string? PHONE { get; set; }
    }
}
