using BackEnd.Application.Dtos.SuppliersDtos;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class SupplierService
    {
        private readonly ISuppliersRepository _supplierRepository;

        public SupplierService(ISuppliersRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<SupplierReadDto>> ReadSupplier()
        {
            var suppliers = await _supplierRepository.ReadSuppliers();
            return suppliers.Select(s => new SupplierReadDto
            {
                PK_SUPPLIER = s.PK_SUPPLIER,
                COMPANY_NAME = s.COMPANY_NAME,
                CONTACT = s.CONTACT,
                PHONE = s.PHONE,
                EMAIL = s.EMAIL,
                STATUS = s.STATUS,
                CREATION_DATE = s.CREATION_DATE.HasValue ? s.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<IEnumerable<SupplierReadDto>> SearchSupplier(string text)
        {
            var supplier = await _supplierRepository.SearchSupplier(text);
            return supplier.Select(s => new SupplierReadDto
            {
                PK_SUPPLIER = s.PK_SUPPLIER,
                COMPANY_NAME = s.COMPANY_NAME,
                CONTACT = s.CONTACT,
                PHONE = s.PHONE,
                EMAIL = s.EMAIL,
                STATUS = s.STATUS,
                CREATION_DATE = s.CREATION_DATE.HasValue ? s.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }
        public async Task<IEnumerable<SupplierSelectDto>> SelectSuppliers()
        {
            var suppliers = await _supplierRepository.SelectSuppliers();
            return suppliers.Select(s => new SupplierSelectDto
            {
                PK_SUPPLIER = s.PK_SUPPLIER,
                COMPANY_NAME = s.COMPANY_NAME
            });
        }

        public async Task<string> CreateSupplier(SupplierCreateDto createDto)
        {
            var supplier = new Suppliers
            {
                COMPANY_NAME = createDto.COMPANY_NAME,
                CONTACT = createDto.CONTACT,
                PHONE = createDto.PHONE,
                EMAIL = createDto.EMAIL
            };

            return await _supplierRepository.CreateSupplier(supplier);
        }

        public async Task<string> UpdateSupplier(SupplierUpdateDto updateDto)
        {
            var supplier = new Suppliers
            {
                PK_SUPPLIER = updateDto.PK_SUPPLIER,
                COMPANY_NAME = updateDto.COMPANY_NAME,
                CONTACT = updateDto.CONTACT,
                PHONE = updateDto.PHONE,
                EMAIL = updateDto.EMAIL
            };

            return await _supplierRepository.UpdateSupplier(supplier);
        }

        public async Task EnabledSupplier(int id)
        {
            await _supplierRepository.EnabledSupplier(id);
        }

        public async Task DisabledSupplier(int id)
        {
            await _supplierRepository.DisabledSupplier(id);
        }
    }
}
