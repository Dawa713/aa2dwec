using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api_clase.Models
{
    /// Clase que representa un cliente/usuario de la tienda.
    /// Almacena la información personal y de autenticación del usuario.
    [Table("Customers")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; } // ADMIN o CLIENT

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Constructor vacío requerido por EF Core
        public Customer()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        // Constructor con parámetros
        public Customer(string name, string email, string password, string role = "CLIENT")
        {
            Name = name;
            Email = email;
            Password = password;
            Role = role;
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        /// Método privado para validar el formato del email.
        /// Comprueba que contenga @ y al menos un punto (.).
        private bool EsEmailValido(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }
    }
}