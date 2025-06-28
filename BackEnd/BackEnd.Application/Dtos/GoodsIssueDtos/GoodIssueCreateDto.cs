using System.ComponentModel.DataAnnotations;
using BackEnd.Application.Dtos.DetailsIssueDtos;

namespace BackEnd.Application.Dtos.GoodsIssueDtos
{
    public class GoodIssueCreateDto
    {
        [Required]
        public DateTime DATE_SALE { get; set; }
        [Required]
        public string? TYPE_ISSUE { get; set; }
        [Required]
        public string? TYPE_DOCUMENT { get; set; }
        [StringLength(30, ErrorMessage = "Type of document must be 30 characters long.")]
        public string? DOCUMENT_NUMBER { get; set; }
        [StringLength(80, ErrorMessage = "Annotations must be 80 characters long.")]
        public string? ANNOTATIONS { get; set; }
        [Required]
        public int PK_CUSTOMER { get; set; }
        [Required]
        public int PK_USER { get; set; }
        [Required]
        public int PK_WAREHOUSE { get; set; }

        public List<DetailIssueCreateDto>? details { get; set; }
    }
}
