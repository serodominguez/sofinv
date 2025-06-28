using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.CategoriesDtos
{
    public class CategoryUpdateDto
    {
        [Required]
        public int PK_CATEGORY { get; set; }
        [StringLength(40, ErrorMessage = "The category name must be 1 to 40 characters long.", MinimumLength = 1)]
        public required string CATEGORY_NAME { get; set; }
    }
}
