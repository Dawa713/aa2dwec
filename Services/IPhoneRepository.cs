using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Services
{
    /// <summary>
    /// Interfaz que define el contrato para las operaciones de teléfonos.
    /// Esto es parte del patrón de Inyección de Dependencias.
    /// </summary>
    public interface IPhoneRepository
    {
        IEnumerable<Phone> GetAllActive();
        Phone GetById(int id);
        void Add(Phone phone);
        void Update(int id, Phone phoneUpdate);
        void Delete(int id);
    }
}
