using Microsoft.EntityFrameworkCore;
using api_clase.Models;

namespace api_clase.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets para las entidades
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Phone> Phones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Datos iniciales para Customers
            modelBuilder.Entity<Customer>().HasData(
                new Customer
                {
                    Id = 1,
                    Name = "Juan Pérez",
                    Email = "juan@email.com",
                    Password = "password123",
                    Role = "ADMIN",
                    CreatedAt = new DateTime(2024, 4, 1),
                    IsActive = true
                },
                new Customer
                {
                    Id = 2,
                    Name = "María García",
                    Email = "maria@email.com",
                    Password = "pass1234",
                    Role = "CLIENT",
                    CreatedAt = new DateTime(2024, 4, 11),
                    IsActive = true
                },
                new Customer
                {
                    Id = 3,
                    Name = "Carlos López",
                    Email = "carlos@email.com",
                    Password = "secure456",
                    Role = "CLIENT",
                    CreatedAt = new DateTime(2024, 4, 21),
                    IsActive = true
                }
            );

            // Datos iniciales para Phones
            modelBuilder.Entity<Phone>().HasData(
                new Phone
                {
                    Id = 1,
                    Brand = "Apple",
                    Model = "iPhone 15",
                    Price = 999.99m,
                    Stock = 50,
                    ReleaseDate = new DateTime(2024, 3, 21),
                    IsActive = true
                },
                new Phone
                {
                    Id = 2,
                    Brand = "Samsung",
                    Model = "Galaxy S24",
                    Price = 899.99m,
                    Stock = 40,
                    ReleaseDate = new DateTime(2024, 2, 21),
                    IsActive = true
                },
                new Phone
                {
                    Id = 3,
                    Brand = "Google",
                    Model = "Pixel 8",
                    Price = 799.99m,
                    Stock = 30,
                    ReleaseDate = new DateTime(2024, 1, 21),
                    IsActive = true
                },
                new Phone
                {
                    Id = 4,
                    Brand = "OnePlus",
                    Model = "12",
                    Price = 699.99m,
                    Stock = 25,
                    ReleaseDate = new DateTime(2023, 12, 21),
                    IsActive = true
                }
            );
        }
    }
}