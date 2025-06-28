using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.ProductsDtos
{
    public class ProductUpdateDto
    {
        [Required]
        public int PK_PRODUCT { get; set; }
        public string? CODE { get; set; }
        [Required(ErrorMessage = "The description cannot be empty")]
        [StringLength(50, ErrorMessage = "The description must be from 1 to 50 characters long.", MinimumLength = 1)]
        public required string DESCRIPTION { get; set; }
        [Required(ErrorMessage = "The measure cannot be empty")]
        [StringLength(15, ErrorMessage = "The measurement must be 1 to 15 characters long.", MinimumLength = 1)]
        public required string MEASUREMENT { get; set; }
        public string? MATERIAL { get; set; }
        public string? COLOUR { get; set; }
        public int PK_BRAND { get; set; }
        public int PK_CATEGORY { get; set; }
    }
}
