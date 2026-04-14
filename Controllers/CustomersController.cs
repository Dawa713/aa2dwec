using Microsoft.AspNetCore.Mvc;
using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        // Almacenamiento temporal en memoria (en producción sería una BD)
        private static List<Customer> customers = new()
        {
            new Customer(1, "Juan", "juan@email.com", "password123", "ADMIN"),
            new Customer(2, "Maria", "maria@email.com", "pass1234", "CLIENT")
        };

        /// <summary>
        /// GET: api/customers - Obtiene todos los clientes
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Customer>> GetAllCustomers()
        {
            return Ok(customers.Where(c => c.IsActive).ToList());
        }

        /// <summary>
        /// GET: api/customers/{id} - Obtiene un cliente por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Customer> GetCustomerById(int id)
        {
            var customer = customers.FirstOrDefault(c => c.Id == id && c.IsActive);
            if (customer == null)
                return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

            return Ok(customer);
        }

        /// <summary>
        /// POST: api/customers - Crea un nuevo cliente
        /// </summary>
        [HttpPost]
        public ActionResult<Customer> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Asignar nuevo ID
            customer.Id = customers.Max(c => c.Id) + 1;
            customers.Add(customer);

            return CreatedAtAction(nameof(GetCustomerById), new { id = customer.Id }, customer);
        }

        /// <summary>
        /// PUT: api/customers/{id} - Actualiza un cliente
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdateCustomer(int id, [FromBody] Customer customerUpdate)
        {
            var customer = customers.FirstOrDefault(c => c.Id == id && c.IsActive);
            if (customer == null)
                return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

            customer.Name = customerUpdate.Name ?? customer.Name;
            customer.Email = customerUpdate.Email ?? customer.Email;
            customer.Password = customerUpdate.Password ?? customer.Password;
            customer.Role = customerUpdate.Role ?? customer.Role;

            return Ok(new { message = "Cliente actualizado correctamente", data = customer });
        }

        /// <summary>
        /// DELETE: api/customers/{id} - Desactiva un cliente
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeleteCustomer(int id)
        {
            var customer = customers.FirstOrDefault(c => c.Id == id && c.IsActive);
            if (customer == null)
                return NotFound(new { message = $"Cliente con ID {id} no encontrado" });

            customer.IsActive = false;
            return Ok(new { message = "Cliente eliminado correctamente" });
        }
    }
}
