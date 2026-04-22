# Documentación del Proyecto: API REST de Tienda de Teléfonos

**Fecha:** 22 de Abril de 2026  
**Nivel:** 2º Desarrollo de Aplicaciones Web (DAW)  
**Asignatura:** Entorno Servidor

---

## Índice

1. [Descripción General](#descripción-general)
2. [Arquitectura del Proyecto](#arquitectura-del-proyecto)
3. [Tecnologías Utilizadas](#tecnologías-utilizadas)
4. [Estructura de Directorios](#estructura-de-directorios)
5. [Base de Datos](#base-de-datos)
6. [Endpoints de la API](#endpoints-de-la-api)
7. [Instalación y Configuración](#instalación-y-configuración)
8. [Ejecución del Proyecto](#ejecución-del-proyecto)
9. [Flujo de Datos](#flujo-de-datos)
10. [Patrones de Diseño](#patrones-de-diseño)
11. [Migraciones y Seeding](#migraciones-y-seeding)

---

## Descripción General

Esta es una **API REST** desarrollada en **ASP.NET Core** que gestiona:
- **Clientes** (Customers): registro, autenticación, actualización y eliminación
- **Teléfonos** (Phones): catálogo de productos, stock y compras

La API utiliza un patrón de **3 capas**:
1. **Capa de Presentación** (Controllers)
2. **Capa de Lógica de Negocio** (Services/Repositories)
3. **Capa de Datos** (Entity Framework Core + MariaDB)

---

## Arquitectura del Proyecto

### Diagrama de Capas

```
┌─────────────────────────────────────────────────────┐
│               PRESENTACIÓN (API)                    │
│           CustomersController                       │
│           PhonesController                          │
│         (Reciben requests HTTP)                     │
└─────────────────┬───────────────────────────────────┘
                  │
┌─────────────────┴───────────────────────────────────┐
│              LÓGICA DE NEGOCIO                       │
│      ICustomerRepository                            │
│      IPhoneRepository                               │
│      (Define contrato de operaciones)               │
└─────────────────┬───────────────────────────────────┘
                  │
┌─────────────────┴───────────────────────────────────┐
│          IMPLEMENTACIÓN DE SERVICIOS                │
│      CustomerRepository                             │
│      PhoneRepository                                │
│      (Implementa las operaciones)                   │
└─────────────────┬───────────────────────────────────┘
                  │
┌─────────────────┴───────────────────────────────────┐
│              CAPA DE DATOS                          │
│        ApplicationDbContext (EF Core)               │
│          MariaDB / MySQL                            │
└─────────────────────────────────────────────────────┘
```

---

## Tecnologías Utilizadas

| Componente | Tecnología | Versión |
|-----------|-----------|---------|
| **Framework** | ASP.NET Core | .NET 10.0 |
| **Base de Datos** | MariaDB/MySQL | 11 |
| **ORM** | Entity Framework Core | Últimas versiones |
| **Mapeo de Objetos** | AutoMapper | 12.0.1 |
| **Contenedorización** | Docker | Últimas versiones |
| **Lenguaje** | C# | 12 |
| **API Documentation** | Swagger/OpenAPI | Integrado |

---

## Estructura de Directorios

```
aa2dwec/
├── Controllers/                 # Controladores (Endpoints de API)
│   ├── CustomersController.cs   # GET, POST, PUT, DELETE clientes
│   └── PhonesController.cs      # GET, POST, PUT, DELETE teléfonos
│
├── Models/                      # Entidades de Base de Datos
│   ├── ApplicationDbContext.cs  # Contexto de EF Core + Seeding
│   ├── Customer.cs              # Modelo de Cliente
│   └── Phone.cs                 # Modelo de Teléfono
│
├── DTOs/                        # Data Transfer Objects
│   ├── CustomerDTO.cs           # DTO para respuestas de cliente
│   ├── PhoneDTO.cs              # DTO para respuestas de teléfono
│   └── PurchaseRequestDTO.cs    # DTO para compras
│
├── Services/                    # Lógica de Negocio
│   ├── ICustomerRepository.cs   # Interfaz de cliente
│   ├── CustomerRepository.cs    # Implementación cliente
│   ├── IPhoneRepository.cs      # Interfaz de teléfono
│   └── PhoneRepository.cs       # Implementación teléfono
│
├── Mappings/                    # Configuración de AutoMapper
│   └── MappingProfile.cs        # Mapeos Modelo ↔ DTO
│
├── Migrations/                  # Migraciones de EF Core
│   ├── 20260421113537_InitialCreate.cs
│   ├── 20260421115206_SeedInitialData.cs
│   └── ApplicationDbContextModelSnapshot.cs
│
├── Properties/                  # Configuración del proyecto
│   └── launchSettings.json      # Puertos y perfiles de ejecución
│
├── appsettings.json             # Configuración general
├── appsettings.Development.json # Configuración de desarrollo
├── Program.cs                   # Configuración de Startup
├── api clase.csproj             # Archivo del proyecto
├── docker-compose.yml           # Orquestación de contenedores
└── DOCUMENTACION.md             # Este archivo
```

---

## Base de Datos

### Diagrama E-R

```
┌──────────────────┐      ┌──────────────────┐
│    Customers     │      │     Phones       │
├──────────────────┤      ├──────────────────┤
│ PK  Id (int)     │      │ PK  Id (int)     │
│     Name (str)   │      │     Brand (str)  │
│     Email (str)  │      │     Model (str)  │
│     Password(str)│      │     Price (dec)  │
│     Role (str)   │      │     Stock (int)  │
│     CreatedAt    │      │     ReleaseDate  │
│     IsActive(bool)       │     IsActive(bool)
└──────────────────┘      └──────────────────┘
```

### Tablas

#### Tabla: `Customers`

| Campo | Tipo | Longitud | Restricciones | Descripción |
|-------|------|----------|---------------|-------------|
| **Id** | INT | - | PRIMARY KEY, AUTO_INCREMENT | Identificador único |
| **Name** | VARCHAR | 50 | NOT NULL | Nombre del cliente |
| **Email** | VARCHAR | 100 | NOT NULL | Email único para autenticación |
| **Password** | VARCHAR | 255 | NOT NULL | Contraseña (idealmente hasheada) |
| **Role** | VARCHAR | 20 | NOT NULL | "ADMIN" o "CLIENT" |
| **CreatedAt** | DATETIME | - | NOT NULL | Fecha de creación |
| **IsActive** | BOOLEAN | - | NOT NULL (default: true) | Soft delete flag |

#### Tabla: `Phones`

| Campo | Tipo | Longitud | Restricciones | Descripción |
|-------|------|----------|---------------|-------------|
| **Id** | INT | - | PRIMARY KEY, AUTO_INCREMENT | Identificador único |
| **Brand** | VARCHAR | 50 | NOT NULL | Marca del teléfono |
| **Model** | VARCHAR | 50 | NOT NULL | Modelo del teléfono |
| **Price** | DECIMAL | 18,2 | NOT NULL | Precio del producto |
| **Stock** | INT | - | NOT NULL | Cantidad disponible |
| **ReleaseDate** | DATETIME | - | NOT NULL | Fecha de lanzamiento |
| **IsActive** | BOOLEAN | - | NOT NULL (default: true) | Soft delete flag |

---

## Endpoints de la API

### Base URL
```
http://localhost:5000/api
```

### Autenticación
⚠️ **Nota:** La autenticación está deshabilitada en desarrollo. En producción se recomienda JWT o sesiones.

### Clientes (Customers)

#### 1. Obtener todos los clientes activos
```http
GET /api/customers
```
**Respuesta:**
```json
[
  {
    "id": 1,
    "name": "Juan Pérez",
    "email": "juan@email.com",
    "role": "ADMIN",
    "createdAt": "2024-04-01T00:00:00",
    "isActive": true
  }
]
```

#### 2. Obtener cliente por ID
```http
GET /api/customers/{id}
```
**Ejemplo:**
```http
GET /api/customers/1
```

#### 3. Crear nuevo cliente
```http
POST /api/customers
Content-Type: application/json

{
  "name": "Pedro García",
  "email": "pedro@email.com",
  "password": "secure123",
  "role": "CLIENT"
}
```
**Respuesta (201 Created):**
```json
{
  "id": 4,
  "name": "Pedro García",
  "email": "pedro@email.com",
  "role": "CLIENT",
  "createdAt": "2026-04-22T10:30:00",
  "isActive": true
}
```

#### 4. Actualizar cliente
```http
PUT /api/customers/{id}
Content-Type: application/json

{
  "name": "Pedro García Actualizado",
  "email": "pedro.nuevo@email.com",
  "password": "newpass456",
  "role": "CLIENT"
}
```

#### 5. Eliminar cliente (Soft Delete)
```http
DELETE /api/customers/{id}
```
**Efecto:** Marca el cliente con `IsActive = false` sin eliminar el registro

---

### Teléfonos (Phones)

#### 1. Obtener todos los teléfonos
```http
GET /api/phones
```

#### 2. Obtener teléfono por ID
```http
GET /api/phones/{id}
```

#### 3. Crear nuevo teléfono
```http
POST /api/phones
Content-Type: application/json

{
  "brand": "Xiaomi",
  "model": "14 Ultra",
  "price": 599.99,
  "stock": 20,
  "releaseDate": "2024-03-15T00:00:00"
}
```

#### 4. Actualizar teléfono
```http
PUT /api/phones/{id}
Content-Type: application/json

{
  "brand": "Xiaomi",
  "model": "14 Ultra",
  "price": 549.99,
  "stock": 18
}
```

#### 5. Eliminar teléfono
```http
DELETE /api/phones/{id}
```

#### 6. Comprar teléfono (Reducir stock)
```http
POST /api/phones/{id}/purchase
Content-Type: application/json

{
  "quantity": 2
}
```
**Respuesta:**
```json
{
  "message": "Compra realizada exitosamente. Stock restante: 48",
  "purchasedQuantity": 2,
  "phone": { ... }
}
```

#### 7. Buscar teléfono por marca
```http
GET /api/phones/search/byBrand?brand=Apple
```

---

## Instalación y Configuración

### Requisitos Previos

- **.NET 10 SDK**: Descargar desde https://dotnet.microsoft.com/download
- **Docker y Docker Compose**: https://www.docker.com/
- **Visual Studio Code** o **Visual Studio Community**
- **DBeaver** (opcional, para visualizar BD)

### Pasos de Instalación

#### 1. Clonar el repositorio
```bash
git clone <url-repo>
cd aa2dwec
```

#### 2. Restaurar dependencias
```bash
dotnet restore
```

#### 3. Iniciar Base de Datos con Docker
```bash
docker-compose up -d
```
Esto inicia un contenedor de MariaDB con:
- **Usuario:** root / **Contraseña:** root123
- **Base de Datos:** mi_api_db
- **Puerto:** 3306

#### 4. Aplicar migraciones
```bash
dotnet ef database update
```

Este comando:
1. Crea la BD si no existe
2. Ejecuta todas las migraciones pendientes
3. Inserta los datos iniciales (seeding)

#### 5. Compilar el proyecto
```bash
dotnet build
```

---

## Ejecución del Proyecto

### Opción 1: Desde Visual Studio Code
```bash
dotnet watch run
```
- Ejecuta en modo debug
- Auto-reinicia si hay cambios

### Opción 2: Desde línea de comandos
```bash
dotnet run
```

### Opción 3: Desde tareas de VS Code
Presiona `Ctrl+Shift+B` y selecciona "build"

### Acceder a la API
- **Swagger UI:** http://localhost:5000/swagger/index.html
- **API Base:** http://localhost:5000/api/

---

## Flujo de Datos

### Flujo de una petición GET

```
1. Cliente (Swagger/Postman)
   └─ GET /api/customers/1
      │
      │
2. CustomersController.GetCustomerById(1)
   └─ Valida y extrae el ID
      │
      │
3. ICustomerRepository.GetById(1)
   └─ Interfaz define el contrato
      │
      │
4. CustomerRepository.GetById(1)
   ├─ Ejecuta: context.Customers
   │          .FirstOrDefault(c => c.Id == 1 && c.IsActive == true)
   │
   └─ Retorna objeto Customer o null
      │
      │
5. AutoMapper.Map<CustomerDTO>(customer)
   └─ Convierte Customer a CustomerDTO
      (Oculta Password)
      │
      │
6. Return Ok(customerDTO)
   └─ Serializa a JSON y envía respuesta 200 OK
```

### Flujo de una petición POST

```
1. Cliente (Swagger/Postman)
   └─ POST /api/customers
      { "name": "Juan", "email": "juan@email.com", ... }
      │
      │
2. CustomersController.CreateCustomer(CreateUpdateCustomerDTO)
   ├─ Valida ModelState
   │
   └─ Se mapea DTO a Customer
      │
      │
3. AutoMapper.Map<Customer>(customerDTO)
   ├─ Crea nuevo objeto Customer
   ├─ Asigna: Name, Email, Password, Role
   ├─ Auto-asigna: CreatedAt = DateTime.Now
   │                IsActive = true
   │
   └─ Retorna Customer configurado
      │
      │
4. ICustomerRepository.Add(customer)
   └─ Implementación en CustomerRepository
      │
      │
5. CustomerRepository.Add(customer)
   ├─ Valida CreatedAt e IsActive
   ├─ context.Customers.Add(customer)
   ├─ context.SaveChanges()
   │  ├─ EF Core genera INSERT SQL
   │  ├─ Ejecuta en MariaDB
   │  └─ Base de datos retorna ID generado
   │
   └─ Retorna al controlador
      │
      │
6. Mapear a DTO para respuesta
   └─ AutoMapper.Map<CustomerDTO>(customer)
      │
      │
7. Return CreatedAtAction(...)
   └─ Respuesta 201 Created + Location header
```

---

## Patrones de Diseño

### 1. Patrón Repository

**Objetivo:** Abstrae el acceso a datos

```csharp
// Interfaz (contrato)
public interface ICustomerRepository
{
    Customer GetById(int id);
    void Add(Customer customer);
    void Update(int id, Customer customer);
    void Delete(int id);
}

// Implementación
public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;
    
    public void Add(Customer customer)
    {
        _context.Customers.Add(customer);
        _context.SaveChanges();
    }
}
```

**Ventajas:**
- Fácil de testear
- Desacoplamiento
- Cambiar BD sin afectar controladores

### 2. Patrón DTO (Data Transfer Object)

**Objetivo:** Separar modelo de dominio de respuesta API

```csharp
// Modelo (con Password)
public class Customer
{
    public int Id { get; set; }
    public string Password { get; set; }
}

// DTO (sin Password - seguridad)
public class CustomerDTO
{
    public int Id { get; set; }
    // Password NO se expone
}
```

**Ventajas:**
- Seguridad (no expone datos sensibles)
- Control de serialización
- Validación en entrada

### 3. Inyección de Dependencias (DI)

**Configuración en Program.cs:**
```csharp
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddAutoMapper(typeof(MappingProfile));
```

**Uso en Controlador:**
```csharp
public CustomersController(
    ICustomerRepository repo,  // ← Inyectado automáticamente
    IMapper mapper)
{
    _customerRepository = repo;
    _mapper = mapper;
}
```

### 4. Soft Delete

**Estrategia:** No eliminar registros, marcar como inactivos

```csharp
// DELETE no elimina físicamente
public void Delete(int id)
{
    var customer = GetById(id);
    if (customer != null)
    {
        customer.IsActive = false;  // Marcar como inactivo
        _context.Customers.Update(customer);
        _context.SaveChanges();
    }
}

// SELECT filtra activos automáticamente
public Customer GetById(int id)
{
    return _context.Customers
        .FirstOrDefault(c => c.Id == id && c.IsActive);
}
```

**Ventajas:**
- Historial completo
- Recuperable si es error
- Cumple regulaciones de datos

---

## Migraciones y Seeding

### ¿Qué es una Migración?

Una migración es un archivo que registra **cambios en el esquema de BD**. EF Core las genera automáticamente.

### Migraciones Creadas

#### 1. `20260421113537_InitialCreate`
- Crea tabla `Customers` con campos
- Crea tabla `Phones` con campos
- Define claves primarias

#### 2. `20260421115206_SeedInitialData`
- Inserta 3 clientes de ejemplo
- Inserta 4 teléfonos de ejemplo

### Datos Iniciales (Seeding)

En `ApplicationDbContext.OnModelCreating()`:

```csharp
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
    // ... más clientes
);

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
    // ... más teléfonos
);
```

### Comandos de Migraciones

#### Ver migraciones aplicadas
```bash
dotnet ef migrations list
```

#### Crear nueva migración
```bash
dotnet ef migrations add NombreMigracion
```

#### Revertir última migración
```bash
dotnet ef database update NombreMigracionAnterior
```

#### Actualizar BD a última migración
```bash
dotnet ef database update
```

---

## Queries SQL Generadas

### INSERT Customer

```sql
INSERT INTO Customers (Name, Email, Password, Role, CreatedAt, IsActive)
VALUES ('Juan Pérez', 'juan@email.com', 'password123', 'ADMIN', 
        '2024-04-01 00:00:00', true);
```

### SELECT Customers Activos

```sql
SELECT * FROM Customers 
WHERE IsActive = 1;
```

### SOFT DELETE

```sql
UPDATE Customers 
SET IsActive = 0 
WHERE Id = 1;
```

---

## Validación de Datos

### En Models

```csharp
public class Customer
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
}
```

### En DTOs

```csharp
public class CreateUpdateCustomerDTO
{
    [Required]
    public string Name { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
}
```

---

## Troubleshooting

### Error: "Connection refused" para BD
**Solución:**
```bash
docker-compose up -d
docker ps  # Verificar que mariadb está corriendo
```

### Error: Migración no aplicada
**Solución:**
```bash
dotnet ef database update
```

### Error: "Entity type 'Customer' is not mapped"
**Solución:** Asegúrate que el DbSet esté en `ApplicationDbContext`:
```csharp
public DbSet<Customer> Customers { get; set; }
```

### El cliente creado no aparece en GET
**Solución:** Limpia la BD de registros con `IsActive = false`:
```sql
DELETE FROM Customers WHERE IsActive = 0;
```

---

## Próximas Mejoras (Trabajo Futuro)

1. **Seguridad:**
   - Implementar JWT para autenticación
   - Hash de contraseñas con bcrypt
   - CORS configurado

2. **Validación:**
   - Custom validators
   - Manejo de errores centralizado

3. **Performance:**
   - Caché con Redis
   - Paginación en listados
   - Índices en BD

4. **Deployment:**
   - Dockerizar la API
   - Kubernetes manifests
   - CI/CD con GitHub Actions

---

## Conclusión

Esta API es un ejemplo completo de:
- ✅ Arquitectura en capas
- ✅ Patrones SOLID
- ✅ Entity Framework Core con migraciones
- ✅ Inyección de dependencias
- ✅ Separación de responsabilidades
- ✅ Soft delete para conservar datos

**Ideal para aprender sobre desarrollo backend profesional en .NET Core.**

---

**Realizado por:** [Tu Nombre]  
**Fecha:** 22 de Abril de 2026  
**Institución:** IES [Tu Instituto]
