using ConsolePhoneStore.Models;

namespace ConsolePhoneStore.Services
{
    /// <summary>
    /// Implementación del repositorio de clientes en memoria.
    /// En producción, esto sería una clase que acceda a una base de datos.
    /// </summary>
    public class CustomerRepository : ICustomerRepository
    {
        // Almacenamiento temporal en memoria
        private static List<Customer> customers = new()
        {
            new Customer(1, "Juan", "juan@email.com", "password123", "ADMIN"),
            new Customer(2, "Maria", "maria@email.com", "pass1234", "CLIENT")
        };

        public IEnumerable<Customer> GetAllActive()
        {
            return customers.Where(c => c.IsActive).ToList();
        }

        public Customer GetById(int id)
        {
            return customers.FirstOrDefault(c => c.Id == id && c.IsActive);
        }

        public void Add(Customer customer)
        {
            customer.Id = customers.Max(c => c.Id) + 1;
            customers.Add(customer);
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
            }
        }

        public void Delete(int id)
        {
            var customer = GetById(id);
            if (customer != null)
            {
                customer.IsActive = false;
            }
        }
    }
}
