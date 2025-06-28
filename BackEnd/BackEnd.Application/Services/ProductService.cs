using BackEnd.Application.Dtos.ProductsDtos;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class ProductService
    {
        private readonly IProductsRepository _productRepository;

        public ProductService(IProductsRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductReadDto>> ReadProducts()
        {
            var products = await _productRepository.ReadProducts();
            return products.Select(p => new ProductReadDto
            {
                PK_PRODUCT = p.PK_PRODUCT,
                CODE = p.CODE,
                DESCRIPTION = p.DESCRIPTION,
                MATERIAL = p.MATERIAL,
                COLOUR = p.COLOUR,
                MEASUREMENT = p.MEASUREMENT,
                STATUS = p.STATUS,
                PK_BRAND = p.PK_BRAND,
                BRAND_NAME = p.brands?.BRAND_NAME,
                PK_CATEGORY = p.PK_CATEGORY,
                CATEGORY_NAME = p.categories?.CATEGORY_NAME,
                CREATION_DATE = p.CREATION_DATE.HasValue ? p.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<IEnumerable<ProductReadDto>> SearchProduct(string text)
        {
            var products = await _productRepository.SearchProduct(text);
            return products.Select(p => new ProductReadDto
            {
                PK_PRODUCT = p.PK_PRODUCT,
                CODE = p.CODE,
                DESCRIPTION = p.DESCRIPTION,
                MATERIAL = p.MATERIAL,
                COLOUR = p.COLOUR,
                MEASUREMENT = p.MEASUREMENT,
                STATUS = p.STATUS,
                PK_BRAND = p.PK_BRAND,
                BRAND_NAME = p.brands?.BRAND_NAME,
                PK_CATEGORY = p.PK_CATEGORY,
                CATEGORY_NAME = p.categories?.CATEGORY_NAME,
                CREATION_DATE = p.CREATION_DATE.HasValue ? p.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<string> CreateProduct(ProductCreateDto createDto)
        {
            if (createDto == null)
            {
                throw new ArgumentNullException(nameof(createDto));
            }

            var product = new Products
            {
                CODE = createDto.CODE,
                DESCRIPTION = createDto.DESCRIPTION,
                MEASUREMENT = createDto.MEASUREMENT,
                MATERIAL = createDto.MATERIAL,
                COLOUR = createDto.COLOUR,
                PK_BRAND = createDto.PK_BRAND,
                PK_CATEGORY = createDto.PK_CATEGORY,
                storing = new List<DetailsWarehouse>
                {
                    new DetailsWarehouse { PRICE = createDto.PRICE }
                }
            };

            if (product.storing == null || !product.storing.Any())
            {
                throw new InvalidOperationException("Storing es nulo o vacío después del mapeo.");
            }

            var warehouse = product.storing.First();

            if (warehouse.PRICE != createDto.PRICE)
            {
                throw new InvalidOperationException($"El precio mapeado es {warehouse.PRICE}, pero se esperaba {createDto.PRICE}.");
            }

            return await _productRepository.CreateProduct(product);
        }

        public async Task<string> UpdateProduct(ProductUpdateDto updateDto)
        {
            var product = new Products
            {
                PK_PRODUCT = updateDto.PK_PRODUCT,
                CODE = updateDto.CODE,
                DESCRIPTION = updateDto.DESCRIPTION,
                MEASUREMENT = updateDto.MEASUREMENT,
                MATERIAL = updateDto.MATERIAL,
                COLOUR = updateDto.COLOUR,
                PK_BRAND = updateDto.PK_BRAND,
                PK_CATEGORY = updateDto.PK_CATEGORY,
            };

            return await _productRepository.UpdateProduct(product);
        }

        public async Task EnabledProduct(int id)
        {
            await _productRepository.EnabledProduct(id);
        }

        public async Task DisabledProduct(int id)
        {
            await _productRepository.DisabledProduct(id);
        }

        //public async Task CreateProduct(ProductCreateDto createDto)
        //{
        //    var product = ProductProfile.MapToEntity(createDto);
        //    await _productRepository.CreateProduct(product);
        //}
    }
}
