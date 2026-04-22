namespace api_clase.DTOs
{
    /// <summary>
    /// Data Transfer Object para Cliente (sin exponer el Password)
    /// Se usa en las respuestas de la API
    /// </summary>
    public class CustomerDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    /// <summary>
    /// DTO para crear/actualizar clientes (sin incluir campos generados automáticamente)
    /// </summary>
    public class CreateUpdateCustomerDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "CLIENT";
    }
}
