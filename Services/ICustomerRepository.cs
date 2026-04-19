using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Services
{
    /// <summary>
    /// Interfaz que define el contrato para las operaciones de clientes.
    /// Esto es parte del patrón de Inyección de Dependencias.
    /// </summary>
    public interface ICustomerRepository
    {
        IEnumerable<Customer> GetAllActive();
        Customer GetById(int id);
        void Add(Customer customer);
        void Update(int id, Customer customerUpdate);
        void Delete(int id);
    }
}
