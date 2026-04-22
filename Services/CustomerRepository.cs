using api_clase.Models;
using api_clase.Data;

namespace api_clase.Services
{
    /// <summary>
    /// Implementación del repositorio de clientes usando Entity Framework Core.
    /// Lee y escribe datos desde la base de datos MariaDB.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Customer> GetAllActive()
        {
            return _context.Customers.Where(c => c.IsActive).ToList();
        }

        public Customer GetById(int id)
        {
            return _context.Customers.FirstOrDefault(c => c.Id == id && c.IsActive);
        }

        public void Add(Customer customer)
        {
            // Asegurar que se asignen los valores por defecto si no están establecidos
            if (customer.CreatedAt == default)
                customer.CreatedAt = DateTime.Now;
            if (!customer.IsActive)
                customer.IsActive = true;
            
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(int id, Customer customerUpdate)
        {
            var customer = GetById(id);
            if (customer != null)
            {
                customer.Name = customerUpdate.Name ?? customer.Name;
                customer.Email = customerUpdate.Email ?? customer.Email;
                customer.Password = customerUpdate.Password ?? customer.Password;
                customer.Role = customerUpdate.Role ?? customer.Role;
                _context.Customers.Update(customer);
                _context.SaveChanges();
            }
        }

        public void Delete(int id)
        {
            var customer = _context.Customers.FirstOrDefault(c => c.Id == id);
            if (customer != null)
            {
                customer.IsActive = false;
                _context.Customers.Update(customer);
                _context.SaveChanges();
            }
        }
    }
}
