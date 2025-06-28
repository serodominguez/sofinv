using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.DetailsReceiptDtos
{
    public class DetailReceiptCreateDto
    {
        [Required]
        public int PK_PRODUCT { get; set; }
        [Required]
        public int QUANTITY { get; set; }
        [Required]
        public int COST { get; set; }
    }
}
