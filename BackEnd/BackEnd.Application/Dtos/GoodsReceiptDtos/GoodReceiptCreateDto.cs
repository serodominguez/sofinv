using System.ComponentModel.DataAnnotations;
using BackEnd.Application.Dtos.DetailsReceiptDtos;

namespace BackEnd.Application.Dtos.GoodsReceiptDtos
{
    public class GoodReceiptCreateDto
    {
        [Required]
        public DateTime DATE_PURCHASE { get; set; }
        [Required]
        public string? TYPE_RECEIPT { get; set; }
        [Required]
        public string? TYPE_DOCUMENT { get; set; }
        [StringLength(30, ErrorMessage = "Type of document must be 30 characters long.")]
        public string? DOCUMENT_NUMBER { get; set; }
        [StringLength(80, ErrorMessage = "Annotations must be 80 characters long.")]
        public string? ANNOTATIONS { get; set; }
        [Required]
        public int PK_SUPPLIER { get; set; }
        [Required]
        public int PK_USER { get; set; }
        [Required]
        public int PK_WAREHOUSE { get; set; }

        public List<DetailReceiptCreateDto>? details { get; set; }
    }
}
