using api_clase.Models;
using api_clase.Data;

namespace api_clase.Services
{
    /// <summary>
    /// Implementación del repositorio de teléfonos usando Entity Framework Core.
    /// Lee y escribe datos desde la base de datos MariaDB.
    /// </summary>
    public class PhoneRepository : IPhoneRepository
    {
        private readonly ApplicationDbContext _context;

        public PhoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Phone> GetAllActive()
        {
            return _context.Phones.Where(p => p.IsActive).ToList();
        }

        public Phone GetById(int id)
        {
            return _context.Phones.FirstOrDefault(p => p.Id == id && p.IsActive);
        }

        public void Add(Phone phone)
        {
            // Asegurar que se asignen los valores por defecto si no están establecidos
            if (!phone.IsActive)
                phone.IsActive = true;
            
            _context.Phones.Add(phone);
            _context.SaveChanges();
        }

        public void Update(int id, Phone phoneUpdate)
        {
            var phone = GetById(id);
            if (phone != null)
            {
                phone.Brand = phoneUpdate.Brand ?? phone.Brand;
                phone.Model = phoneUpdate.Model ?? phone.Model;
                phone.Price = phoneUpdate.Price > 0 ? phoneUpdate.Price : phone.Price;
                phone.Stock = phoneUpdate.Stock >= 0 ? phoneUpdate.Stock : phone.Stock;
                _context.Phones.Update(phone);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var phone = GetById(id);
            if (phone != null)
            {
                phone.IsActive = false;
                _context.Phones.Update(phone);
                _context.SaveChanges();
            }
        }
    }
}
