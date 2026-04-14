using Microsoft.AspNetCore.Mvc;
using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhonesController : ControllerBase
    {
        // Almacenamiento temporal en memoria (en producción sería una BD)
        private static List<Phone> phones = new()
        {
            new Phone(1, "Apple", "iPhone 15", 999.99m, 50),
            new Phone(2, "Samsung", "Galaxy S24", 899.99m, 40),
            new Phone(3, "Google", "Pixel 8", 799.99m, 30)
        };

        /// <summary>
        /// GET: api/phones - Obtiene todos los teléfonos
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<Phone>> GetAllPhones()
        {
            return Ok(phones.Where(p => p.IsActive).ToList());
        }

        /// <summary>
        /// GET: api/phones/{id} - Obtiene un teléfono por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<Phone> GetPhoneById(int id)
        {
            var phone = phones.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (phone == null)
                return NotFound(new { message = $"Teléfono con ID {id} no encontrado" });

            return Ok(phone);
        }

        /// <summary>
        /// POST: api/phones - Crea un nuevo teléfono
        /// </summary>
        [HttpPost]
        public ActionResult<Phone> CreatePhone([FromBody] Phone phone)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Asignar nuevo ID
            phone.Id = phones.Max(p => p.Id) + 1;
            phones.Add(phone);

            return CreatedAtAction(nameof(GetPhoneById), new { id = phone.Id }, phone);
        }

        /// <summary>
        /// PUT: api/phones/{id} - Actualiza un teléfono
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdatePhone(int id, [FromBody] Phone phoneUpdate)
        {
            var phone = phones.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (phone == null)
                return NotFound(new { message = $"Teléfono con ID {id} no encontrado" });

            phone.Brand = phoneUpdate.Brand ?? phone.Brand;
            phone.Model = phoneUpdate.Model ?? phone.Model;
            phone.Price = phoneUpdate.Price > 0 ? phoneUpdate.Price : phone.Price;
            phone.Stock = phoneUpdate.Stock >= 0 ? phoneUpdate.Stock : phone.Stock;

            return Ok(new { message = "Teléfono actualizado correctamente", data = phone });
        }

        /// <summary>
        /// DELETE: api/phones/{id} - Desactiva un teléfono
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeletePhone(int id)
        {
            var phone = phones.FirstOrDefault(p => p.Id == id && p.IsActive);
            if (phone == null)
                return NotFound(new { message = $"Teléfono con ID {id} no encontrado" });

            phone.IsActive = false;
            return Ok(new { message = "Teléfono eliminado correctamente" });
        }

        /// <summary>
        /// GET: api/phones/search/byBrand?brand=Apple - Busca teléfonos por marca
        /// </summary>
        [HttpGet("search/byBrand")]
        public ActionResult<IEnumerable<Phone>> SearchByBrand([FromQuery] string brand)
        {
            if (string.IsNullOrWhiteSpace(brand))
                return BadRequest(new { message = "La marca es requerida" });

            var result = phones.Where(p => p.IsActive && p.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase)).ToList();
            return Ok(result);
        }

        /// <summary>
        /// GET: api/phones/search/byPrice?minPrice=500&maxPrice=1000 - Busca por rango de precio
        /// </summary>
        [HttpGet("search/byPrice")]
        public ActionResult<IEnumerable<Phone>> SearchByPrice([FromQuery] decimal minPrice = 0, [FromQuery] decimal maxPrice = decimal.MaxValue)
        {
            var result = phones.Where(p => p.IsActive && p.Price >= minPrice && p.Price <= maxPrice).ToList();
            return Ok(result);
        }
    }
}
