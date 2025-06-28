using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.StoresDtos
{
    public class StoreUpdateDto
    {
        [Required]
        public int PK_STORE { get; set; }
        [Required(ErrorMessage = "The store name cannot be empty")]
        [StringLength(30, ErrorMessage = "The store name must be 1 to 30 characters long.", MinimumLength = 1)]
        public required string STORE_NAME { get; set; }
        [Required(ErrorMessage = "The manager's name cannot be empty")]
        [StringLength(50, ErrorMessage = "The manager's name must be 1 to 50 characters long.", MinimumLength = 1)]
        public required string MANAGER { get; set; }
        [Required(ErrorMessage = "The address cannot be empty")]
        [StringLength(50, ErrorMessage = "The address must be 1 to 50 characters long.", MinimumLength = 1)]
        public required string ADDRESS { get; set; }
        public string? PHONE { get; set; }
        [Required(ErrorMessage = "The city cannot be empty")]
        [StringLength(20, ErrorMessage = "The city must be from 1 to 20 characters long.", MinimumLength = 1)]
        public required string CITY { get; set; }
        public string? EMAIL { get; set; }
    }
}
