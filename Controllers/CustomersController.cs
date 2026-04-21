using Microsoft.AspNetCore.Mvc;
using ConsolePhoneStore.Models;
using ConsolePhoneStore.Services;
using ConsolePhoneStore.DTOs;
using AutoMapper;

namespace ConsolePhoneStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        // Inyección de dependencias: El servicio se inyecta a través del constructor
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerRepository customerRepository, IMapper mapper)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// GET: api/customers - Obtiene todos los clientes
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<CustomerDTO>> GetAllCustomers()
        {
            var customers = _customerRepository.GetAllActive();
            // Mapear Customer a CustomerDTO (no expone el Password)
            var customersDTO = _mapper.Map<IEnumerable<CustomerDTO>>(customers);
            return Ok(customersDTO);
        }

        /// <summary>
        /// GET: api/customers/{id} - Obtiene un cliente por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<CustomerDTO> GetCustomerById(int id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
                return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

            // Mapear Customer a CustomerDTO
            var customerDTO = _mapper.Map<CustomerDTO>(customer);
            return Ok(customerDTO);
        }

        /// <summary>
        /// POST: api/customers - Crea un nuevo cliente
        /// </summary>
        [HttpPost]
        public ActionResult<CustomerDTO> CreateCustomer([FromBody] CreateUpdateCustomerDTO customerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO a Customer
            var customer = _mapper.Map<Customer>(customerDTO);
            _customerRepository.Add(customer);

            // Mapear el customer creado a CustomerDTO para la respuesta
            var responseDTO = _mapper.Map<CustomerDTO>(customer);
            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, responseDTO);
        }

        /// <summary>
        /// PUT: api/customers/{id} - Actualiza un cliente
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] CreateUpdateCustomerDTO customerUpdateDTO)
        {
            var existingCustomer = _customerRepository.GetById(id);
            if (existingCustomer == null)
                return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

            // Crear un nuevo objeto Customer con los datos actualizados
            var updatedCustomer = _mapper.Map<Customer>(customerUpdateDTO);
            updatedCustomer.Id = id; // Mantener el mismo ID
            updatedCustomer.CreatedAt = existingCustomer.CreatedAt; // Mantener fecha de creación
            updatedCustomer.IsActive = existingCustomer.IsActive; // Mantener estado activo

            _customerRepository.Update(id, updatedCustomer);

            var responseDTO = _mapper.Map<CustomerDTO>(updatedCustomer);

            return Ok(new { message = "Cliente actualizado correctamente", data = responseDTO });
        }

        /// <summary>
        /// DELETE: api/customers/{id} - Desactiva un cliente
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = _customerRepository.GetById(id);
            if (customer == null)
                return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

            _customerRepository.Delete(id);
            return Ok(new { message = "Cliente eliminado correctamente" });
        }
    }
}
