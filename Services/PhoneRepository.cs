using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Services
{
    /// <summary>
    /// Implementación del repositorio de teléfonos en memoria.
    /// En producción, esto sería una clase que acceda a una base de datos.
    /// </summary>
    public class PhoneRepository : IPhoneRepository
    {
        // Almacenamiento temporal en memoria
        private static List<Phone> phones = new()
        {
            new Phone(1, "Apple", "iPhone 15", 999.99m, 50),
            new Phone(2, "Samsung", "Galaxy S24", 899.99m, 40),
            new Phone(3, "Google", "Pixel 8", 799.99m, 30)
        };

        public IEnumerable<Phone> GetAllActive()
        {
            return phones.Where(p => p.IsActive).ToList();
        }

        public Phone GetById(int id)
        {
            return phones.FirstOrDefault(p => p.Id == id && p.IsActive);
        }

        public void Add(Phone phone)
        {
            phone.Id = phones.Max(p => p.Id) + 1;
            phones.Add(phone);
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
            }
        }

        public void Delete(int id)
        {
            var phone = GetById(id);
            if (phone != null)
            {
                phone.IsActive = false;
            }
        }
    }
}
