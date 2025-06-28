using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.CategoriesDtos
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "The category name cannot be empty.")]
        [StringLength(40, ErrorMessage = "The category name must be 1 to 40 characters long.", MinimumLength = 1)]
        public required string CATEGORY_NAME { get; set; }
    }
}
