namespace ConsolePhoneStore.DTOs
{
    /// <summary>
    /// Data Transfer Object para Teléfono
    /// Se usa en las respuestas de la API
    /// </summary>
    public class PhoneDTO
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO para crear/actualizar teléfonos (sin incluir campos generados automáticamente)
    /// </summary>
    public class CreateUpdatePhoneDTO
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
