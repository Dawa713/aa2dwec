using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace api_clase.Models
{
    /// Clase que representa un teléfono móvil en el catálogo de la tienda.
    /// Contiene información sobre el producto como marca, modelo, precio y stock disponible.
    [Table("Phones")]
    public class Phone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Brand { get; set; }

        [Required]
        [StringLength(50)]
        public string Model { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        public int Stock { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // Constructor vacío requerido por EF Core
        public Phone()
        {
            IsActive = true;
        }

        // Constructor con parámetros
        public Phone(string brand, string model, decimal price, int stock, DateTime releaseDate, bool isActive = true)
        {
            Brand = brand;
            Model = model;
            Price = price;
            Stock = stock;
            ReleaseDate = releaseDate;
            IsActive = isActive;
        }

        /// Reduce el stock del teléfono cuando se realiza una compra.
        /// Valida que la cantidad sea válida antes de restar.
        public void ReduceStock(int quantity)
        {
            if (quantity <= 0 || quantity > Stock)
                throw new ArgumentOutOfRangeException("Cantidad inválida");

            Stock -= quantity;
        }
    }
}
