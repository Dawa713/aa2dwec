namespace ConsolePhoneStore.DTOs
{
    /// <summary>
    /// DTO para solicitudes de compra de teléfonos
    /// </summary>
    public class PurchaseRequestDTO
    {
        /// <summary>
        /// Cantidad de teléfonos a comprar
        /// </summary>
        public int Quantity { get; set; }
    }
}