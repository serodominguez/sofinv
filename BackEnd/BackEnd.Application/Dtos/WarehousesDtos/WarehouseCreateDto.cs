using System.ComponentModel.DataAnnotations;

namespace BackEnd.Application.Dtos.WarehousesDtos
{
    public class WarehouseCreateDto
    {
        public required string TYPE_WAREHOUSE { get; set; }
        [Required]
        public int PK_STORE { get; set; }
    }
}
