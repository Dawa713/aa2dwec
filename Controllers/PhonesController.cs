using Microsoft.AspNetCore.Mvc;
using ConsolePhoneStore.Models;
using ConsolePhoneStore.Services;
using ConsolePhoneStore.DTOs;
using AutoMapper;

namespace ConsolePhoneStore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PhonesController : ControllerBase
    {
        // Inyección de dependencias: El servicio se inyecta a través del constructor
        private readonly IPhoneRepository _phoneRepository;
        private readonly IMapper _mapper;

        public PhonesController(IPhoneRepository phoneRepository, IMapper mapper)
        {
            _phoneRepository = phoneRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// GET: api/phones - Obtiene todos los teléfonos
        /// </summary>
        [HttpGet]
        public ActionResult<IEnumerable<PhoneDTO>> GetAllPhones()
        {
            var phones = _phoneRepository.GetAllActive();
            // Mapear Phone a PhoneDTO
            var phonesDTO = _mapper.Map<IEnumerable<PhoneDTO>>(phones);
            return Ok(phonesDTO);
        }

        /// <summary>
        /// GET: api/phones/{id} - Obtiene un teléfono por ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<PhoneDTO> GetPhoneById(int id)
        {
            var phone = _phoneRepository.GetById(id);
            if (phone == null)
                return NotFound(new { message = $"Teléfono con ID {id} no encontrado" });

            // Mapear Phone a PhoneDTO
            var phoneDTO = _mapper.Map<PhoneDTO>(phone);
            return Ok(phoneDTO);
        }

        /// <summary>
        /// POST: api/phones - Crea un nuevo teléfono
        /// </summary>
        [HttpPost]
        public ActionResult<PhoneDTO> CreatePhone([FromBody] CreateUpdatePhoneDTO phoneDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Mapear DTO a Phone
            var phone = _mapper.Map<Phone>(phoneDTO);
            _phoneRepository.Add(phone);

            // Mapear el phone creado a PhoneDTO para la respuesta
            var responseDTO = _mapper.Map<PhoneDTO>(phone);
            return CreatedAtAction(nameof(GetPhoneById), new { id = phone.Id }, responseDTO);
        }

        /// <summary>
        /// PUT: api/phones/{id} - Actualiza un teléfono
        /// </summary>
        [HttpPut("{id}")]
        public IActionResult UpdatePhone(int id, [FromBody] CreateUpdatePhoneDTO phoneUpdateDTO)
        {
            var phone = _phoneRepository.GetById(id);
            if (phone == null)
                return NotFound(new { message = $"Teléfono con ID {id} no encontrado" });

            // Mapear DTO a Phone para la actualización
            _mapper.Map(phoneUpdateDTO, phone);
            _phoneRepository.Update(id, phone);

            var updatedPhone = _phoneRepository.GetById(id);
            var responseDTO = _mapper.Map<PhoneDTO>(updatedPhone);

            return Ok(new { message = "Teléfono actualizado correctamente", data = responseDTO });
        }

        /// <summary>
        /// DELETE: api/phones/{id} - Desactiva un teléfono
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult DeletePhone(int id)
        {
            var phone = _phoneRepository.GetById(id);
            if (phone == null)
                return NotFound(new { message = $"Teléfono con ID {id} no encontrado" });

            _phoneRepository.Delete(id);
            return Ok(new { message = "Teléfono eliminado correctamente" });
        }

        /// <summary>
        /// POST: api/phones/{id}/purchase - Compra un teléfono (reduce stock)
        /// </summary>
        [HttpPost("{id}/purchase")]
        public IActionResult PurchasePhone(int id, [FromBody] PurchaseRequestDTO purchaseRequest)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var phone = _phoneRepository.GetById(id);
            if (phone == null)
                return NotFound(new { message = $"Teléfono con ID {id} no encontrado" });

            if (!phone.IsActive)
                return BadRequest(new { message = "El teléfono no está disponible para compra" });

            try
            {
                // Usar el método ReduceStock del modelo Phone
                phone.ReduceStock(purchaseRequest.Quantity);

                // Actualizar el teléfono en el repositorio con el stock reducido
                _phoneRepository.Update(id, phone);

                // Mapear a DTO para la respuesta
                var phoneDTO = _mapper.Map<PhoneDTO>(phone);

                return Ok(new
                {
                    message = $"Compra realizada exitosamente. Stock restante: {phone.Stock}",
                    purchasedQuantity = purchaseRequest.Quantity,
                    phone = phoneDTO
                });
            }
            catch (ArgumentOutOfRangeException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// GET: api/phones/search/byBrand?brand=Apple - Busca teléfonos por marca
        /// </summary>
        [HttpGet("search/byBrand")]
        public ActionResult<IEnumerable<PhoneDTO>> SearchByBrand([FromQuery] string brand)
        {
            if (string.IsNullOrWhiteSpace(brand))
                return BadRequest(new { message = "La marca es requerida" });

            var result = _phoneRepository.GetAllActive()
                .Where(p => p.Brand.Contains(brand, StringComparison.OrdinalIgnoreCase)).ToList();
            // Mapear resultados a DTOs
            var resultDTO = _mapper.Map<IEnumerable<PhoneDTO>>(result);
            return Ok(resultDTO);
        }

        /// <summary>
        /// GET: api/phones/search/byPrice?minPrice=500&maxPrice=1000 - Busca por rango de precio
        /// </summary>
        [HttpGet("search/byPrice")]
        public ActionResult<IEnumerable<PhoneDTO>> SearchByPrice([FromQuery] decimal minPrice = 0, [FromQuery] decimal maxPrice = decimal.MaxValue)
        {
            var result = _phoneRepository.GetAllActive()
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice).ToList();
            // Mapear resultados a DTOs
            var resultDTO = _mapper.Map<IEnumerable<PhoneDTO>>(result);
            return Ok(resultDTO);
        }
    }
}
