using BackEnd.Application.Dtos.DetailsWarehouseDtos;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class WarehouseService
    {
        private readonly IWarehousesRepository _warehousesRepository;

        public WarehouseService(IWarehousesRepository warehousesRepository)
        {
            _warehousesRepository = warehousesRepository;
        }

        public async Task<IEnumerable<DetailWarehouseManageDto>> SearchStock(string text, int id)
        {
            var details = await _warehousesRepository.SearchStock(text, id);
            return details.Select(d => new DetailWarehouseManageDto
            {
                PK_WAREHOUSE = d.PK_WAREHOUSE,
                PK_PRODUCT = d.PK_PRODUCT,
                STOCK = d.STOCK,
                PRICE = d.PRICE,
                CODE = d.CODE,
                DESCRIPTION = d.DESCRIPTION,
                MATERIAL = d.MATERIAL,
                COLOUR = d.COLOUR,
                BRAND_NAME = d.BRAND_NAME,
                CATEGORY_NAME = d.CATEGORY_NAME
            });
        }

        public async Task<IEnumerable<DetailProductDto>> SelectProduct(string text, int id)
        {
            var details = await _warehousesRepository.SelectProduct(text, id);
            return details.Select(d => new DetailProductDto
            {
                PK_PRODUCT = d.PK_PRODUCT,
                STOCK = d.STOCK,
                PRICE = d.PRICE,
                CODE = d.CODE,
                DESCRIPTION = d.DESCRIPTION,
                MATERIAL = d.MATERIAL,
                COLOUR = d.COLOUR,
                BRAND_NAME = d.BRAND_NAME,
                CATEGORY_NAME = d.CATEGORY_NAME
            });
        }
    }
}
