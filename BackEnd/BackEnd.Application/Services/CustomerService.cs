using BackEnd.Application.Dtos.CustomersDtos;
using BackEnd.Domains.Entities;
using BackEnd.Domains.Interfaces;

namespace BackEnd.Application.Services
{
    public class CustomerService
    {
        private readonly ICustomersRepository _customerRepository;

        public CustomerService(ICustomersRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<IEnumerable<CustomerReadDto>> ReadCustomers()
        {
            var customers = await _customerRepository.ReadCustomers();
            return customers.Select(c => new CustomerReadDto
            {
                PK_CUSTOMER = c.PK_CUSTOMER,
                NAMES = c.NAMES,
                LAST_NAMES = c.LAST_NAMES,
                IDENTIFICATION = c.IDENTIFICATION,
                PHONE = c.PHONE?.ToString(),
                STATUS = c.STATUS,
                CREATION_DATE = c.CREATION_DATE.HasValue ? c.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }

        public async Task<IEnumerable<CustomerReadDto>> SearchCustomer(string text)
        {
            var customers = await _customerRepository.SearchCustomer(text);
            return customers.Select(c => new CustomerReadDto
            {
                PK_CUSTOMER = c.PK_CUSTOMER,
                NAMES = c.NAMES,
                LAST_NAMES = c.LAST_NAMES,
                IDENTIFICATION = c.IDENTIFICATION,
                PHONE = c.PHONE?.ToString(),
                STATUS = c.STATUS,
                CREATION_DATE = c.CREATION_DATE.HasValue ? c.CREATION_DATE.Value.ToString("dd/MM/yyyy HH:mm") : null
            });
        }
        public async Task<IEnumerable<CustomerSelectDto>> SelectCustomers()
        {
            var customers = await _customerRepository.SelectCustomers();
            return customers.Select(c => new CustomerSelectDto
            {
                PK_CUSTOMER = c.PK_CUSTOMER,
                NAMES = c.NAMES
            });
        }

        public async Task<string> CreateCustomer(CustomerCreateDto createDto)
        {
            var customer = new Customers
            {
                NAMES = createDto.NAMES,
                LAST_NAMES = createDto.LAST_NAMES,
                IDENTIFICATION = string.IsNullOrWhiteSpace(createDto.IDENTIFICATION) ? null : createDto.IDENTIFICATION,
                PHONE = string.IsNullOrWhiteSpace(createDto.PHONE) ? (int?)null : int.Parse(createDto.PHONE)
            };

            return await _customerRepository.CreateCustomer(customer);

        }

        public async Task<string> UpdateCustomer(CustomerUpdateDto updateDto)
        {
            var customer = new Customers
            {
                PK_CUSTOMER = updateDto.PK_CUSTOMER,
                NAMES = updateDto.NAMES,
                LAST_NAMES = updateDto.LAST_NAMES,
                IDENTIFICATION = string.IsNullOrWhiteSpace(updateDto.IDENTIFICATION) ? null : updateDto.IDENTIFICATION,
                PHONE = string.IsNullOrWhiteSpace(updateDto.PHONE) ? (int?)null : int.Parse(updateDto.PHONE)
            };

            return await _customerRepository.UpdateCustomer(customer);
        }

        public async Task EnabledCustomer(int id)
        {
            await _customerRepository.EnabledCustomer(id);
        }

        public async Task DisabledCustomer(int id)
        {
            await _customerRepository.DisabledCustomer(id);
        }
    }
}
