using BackEnd.Application.Dtos.BrandsDtos;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class BrandService
    {
        private readonly IBrandsRepository _brandRepository;

        public BrandService(IBrandsRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<IEnumerable<BrandReadDto>> ReadBrands()
        {
            var brands = await _brandRepository.ReadBrands();
            return brands.Select(b => new BrandReadDto
            {
                PK_BRAND = b.PK_BRAND,
                BRAND_NAME = b.BRAND_NAME,
                STATUS = b.STATUS,
                CREATION_DATE = b.CREATION_DATE.HasValue ? b.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<IEnumerable<BrandReadDto>> SearchBrand(string text)
        {
            var brands = await _brandRepository.SearchBrand(text);
            return brands.Select(b => new BrandReadDto
            {
                PK_BRAND = b.PK_BRAND,
                BRAND_NAME = b.BRAND_NAME,
                STATUS = b.STATUS,
                CREATION_DATE = b.CREATION_DATE.HasValue ? b.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<IEnumerable<BrandSelectDto>> SelectBrands()
        {
            var brands = await _brandRepository.SelectBrands();
            return brands.Select(b => new BrandSelectDto
            {
                PK_BRAND = b.PK_BRAND,
                BRAND_NAME = b.BRAND_NAME
            });
        }

        public async Task<string> CreateBrand(BrandCreateDto createDto)
        {
            var brand = new Brands
            {
                BRAND_NAME = createDto.BRAND_NAME,
                //STATUS = "Active",
                //CREATION_DATE = DateTime.Now
            };
            return await _brandRepository.CreateBrand(brand);
        }

        public async Task<string> UpdateBrand(BrandUpdateDto updateDto)
        {
            var brand = new Brands
            {
                PK_BRAND = updateDto.PK_BRAND,
                BRAND_NAME = updateDto.BRAND_NAME,
            };
            return await _brandRepository.UpdateBrand(brand);
        }

        public async Task EnabledBrand(int id)
        {
            await _brandRepository.EnabledBrand(id);
        }

        public async Task DisabledBrand(int id)
        {
            await _brandRepository.DisabledBrand(id);
        }
    }
}
