using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.BrandsDtos
{
    public class BrandCreateDto
    {
        [Required(ErrorMessage = "The brand name cannot be empty.")]
        [StringLength(40, ErrorMessage = "The brand name must be 1 to 40 characters long.", MinimumLength = 1)]
        public required string BRAND_NAME { get; set; }
    }
}
