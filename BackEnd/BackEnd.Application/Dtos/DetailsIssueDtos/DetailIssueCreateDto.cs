using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.DetailsIssueDtos
{
    public class DetailIssueCreateDto
    {
        [Required]
        public int PK_PRODUCT { get; set; }
        [Required]
        public int QUANTITY { get; set; }
        [Required]
        public int PRICE { get; set; }
    }
}
