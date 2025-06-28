using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.SuppliersDtos
{
    public class SupplierUpdateDto
    {
        [Required]
        public int PK_SUPPLIER { get; set; }

        [Required(ErrorMessage = "The company name cannot be empty.")]
        [StringLength(50, ErrorMessage = "The company name must be 1 to 50 characters long.", MinimumLength = 1)]
        public required string COMPANY_NAME { get; set; }

        [Required(ErrorMessage = "The contact cannot be empty.")]
        [StringLength(50, ErrorMessage = "The contact must be 1 to 50 characters long.", MinimumLength = 1)]
        public required string CONTACT { get; set; }
        public int PHONE { get; set; }
        public string? EMAIL { get; set; }
    }
}
