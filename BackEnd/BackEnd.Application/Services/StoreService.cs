using BackEnd.Application.Dtos.StoresDtos;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class StoreService
    {
        private readonly IStoresRepository _storeRepository;

        public StoreService(IStoresRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<IEnumerable<StoreReadDto>> ReadStores()
        {
            var stores = await _storeRepository.ReadStores();
            return stores.Select(s => new StoreReadDto
            {
                PK_STORE = s.PK_STORE,
                STORE_NAME = s.STORE_NAME,
                MANAGER = s.MANAGER,
                ADDRESS = s.ADDRESS,
                PHONE = s.PHONE?.ToString(),
                CITY = s.CITY,
                EMAIL = s.EMAIL,
                TYPE = s.TYPE,
                STATUS = s.STATUS,
                CREATION_DATE = s.CREATION_DATE.HasValue ? s.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<IEnumerable<StoreReadDto>> SearchStore(string text)
        {
            var stores = await _storeRepository.SearchStore(text);
            return stores.Select(s => new StoreReadDto
            {
                PK_STORE = s.PK_STORE,
                STORE_NAME = s.STORE_NAME,
                MANAGER = s.MANAGER,
                ADDRESS = s.ADDRESS,
                PHONE = s.PHONE?.ToString(),
                CITY = s.CITY,
                EMAIL = s.EMAIL,
                TYPE = s.TYPE,
                STATUS = s.STATUS,
                CREATION_DATE = s.CREATION_DATE.HasValue ? s.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<IEnumerable<StoreSelectDto>> SelectStores()
        {
            var stores = await _storeRepository.SelectStores();
            return stores.Select(s => new StoreSelectDto
            {
                PK_STORE = s.PK_STORE,
                STORE_NAME = s.STORE_NAME
            });
        }

        public async Task<string> CreateStore(StoreCreateDto createDto)
        {
            var store = new Stores
            {
                STORE_NAME = createDto.STORE_NAME,
                MANAGER = createDto.MANAGER,
                ADDRESS = createDto.ADDRESS,
                PHONE = string.IsNullOrWhiteSpace(createDto.PHONE) ? (int?)null : int.Parse(createDto.PHONE),
                CITY = createDto.CITY,
                EMAIL = createDto.EMAIL,
            };

            return await _storeRepository.CreateStore(store);
        }

        public async Task<string> UpdateStore(StoreUpdateDto updateDto)
        {
            var store = new Stores
            {
                PK_STORE = updateDto.PK_STORE,
                STORE_NAME = updateDto.STORE_NAME,
                MANAGER = updateDto.MANAGER,
                ADDRESS = updateDto.ADDRESS,
                PHONE = string.IsNullOrWhiteSpace(updateDto.PHONE) ? (int?)null : int.Parse(updateDto.PHONE),
                CITY = updateDto.CITY,
                EMAIL = updateDto.EMAIL,
            };

            return await _storeRepository.UpdateStore(store);
        }

        public async Task EnabledStore(int id)
        {
            await _storeRepository.EnabledStore(id);
        }

        public async Task DisabledStore(int id)
        {
            await _storeRepository.DisabledStore(id);
        }
    }
}
