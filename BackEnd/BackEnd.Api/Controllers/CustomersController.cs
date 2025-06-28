using BackEnd.Application.Dtos.CustomersDtos;
using BackEnd.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackEnd.Api.Controllers
{
    [Authorize(Roles = "ADMINISTRADORES, ALMACENEROS, OPERARIOS")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly IConfiguration _config;

        public CustomersController(CustomerService customerService, IConfiguration config)
        {
            _customerService = customerService;
            _config = config;
        }

        [HttpPost]
        public async Task<ActionResult> CreateCustomer([FromBody] CustomerCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _customerService.CreateCustomer(createDto);

                if (result == "Cliente creado exitosamente.")
                {
                    return CreatedAtAction(nameof(CreateCustomer), createDto);
                }

                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<ActionResult> ReadCustomers()
        {
            IEnumerable<CustomerReadDto> customers = await Task.Run(() => _customerService.ReadCustomers());
            return Ok(customers);
        }

        [HttpGet("{text}")]
        public async Task<ActionResult<CustomerReadDto>> SearchCustomer(string text)
        {
            if (text == null)
            {
                return BadRequest();
            }

            var customers = await _customerService.SearchCustomer(text);
            if (customers == null)
            {
                return NotFound();
            }
            return Ok(customers);
        }

        [HttpGet]
        public async Task<ActionResult> SelectCustomers()
        {
            IEnumerable<CustomerSelectDto> customers = await Task.Run(() => _customerService.SelectCustomers());
            return Ok(customers);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCustomer([FromBody] CustomerUpdateDto updateDto)
        {
            if (updateDto.PK_CUSTOMER <= 0)
            {
                return BadRequest(new { message = "El cliente no es valida." });
            }

            try
            {
                var result = await _customerService.UpdateCustomer(updateDto);
                if (result == "Actualización del cliente exitosamente.")
                {
                    return NoContent();
                }
                return BadRequest(new { message = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An unexpected error occurred: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> EnabledCustomer(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _customerService.EnabledCustomer(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> DisabledCustomer(int id)
        {
            if (id <= 0)
                return BadRequest();
            await _customerService.DisabledCustomer(id);
            return NoContent();
        }
    }
}
