# 🛒 API REST de Tienda de Teléfonos

**Proyecto de Desarrollo de Aplicaciones Web (2º DAW) - Entorno Servidor**

---

## 📋 Descripción

API REST completa desarrollada en **ASP.NET Core 10** que gestiona:
- **👥 Clientes**: Registro, autenticación, actualización y eliminación
- **📱 Teléfonos**: Catálogo de productos, stock, y compras

Implementa patrones profesionales como **Repository Pattern**, **DTOs**, **Inyección de Dependencias** y **Migraciones de BD**.

---

## 🚀 Inicio Rápido

### 1. Clonar y Configurar

```bash
# Clonar en rama develop
git clone -b develop <url-repo>
cd aa2dwec

# Restaurar dependencias
dotnet restore
```

### 2. Ejecutar Base de Datos

```bash
# Iniciar MariaDB con Docker
docker-compose up -d

# Aplicar migraciones
dotnet ef database update
```

### 3. Ejecutar API

```bash
# Opción A: Con watch (auto-reinicia)
dotnet watch run

# Opción B: Ejecución normal
dotnet run
```

### 4. Acceder

- **Swagger UI**: http://localhost:5000/swagger/index.html
- **API**: http://localhost:5000/api/

---

## 📚 Documentación

### [1. DOCUMENTACION.md](./DOCUMENTACION.md) - Guía Completa del Proyecto
Contiene:
- ✅ Arquitectura en capas
- ✅ Descripción de endpoints
- ✅ Diagrama de base de datos
- ✅ Flujo de datos (GET, POST, PUT, DELETE)
- ✅ Patrones de diseño
- ✅ Migraciones y seeding

**Ideal para presentar ante el profesor.**

### [2. k8s/GUIA-KUBERNETES.md](./k8s/GUIA-KUBERNETES.md) - Despliegue en Kubernetes
Paso a paso para desplegar en K8s:
- 📦 Conceptos básicos de Kubernetes
- 🔧 Instalación de herramientas (Minikube, kubectl)
- 🚀 Despliegue paso a paso
- ✅ Verificación y troubleshooting
- 📊 Scaling y configuración

---

## 🏗️ Estructura del Proyecto

```
aa2dwec/
├── Controllers/                    # Endpoints (CustomersController, PhonesController)
├── Models/                         # Entidades (Customer, Phone, ApplicationDbContext)
├── DTOs/                           # Data Transfer Objects (sin datos sensibles)
├── Services/                       # Interfaces y Repositorios
├── Mappings/                       # Configuración de AutoMapper
├── Migrations/                     # Migraciones EF Core
│
├── Dockerfile                      # Para containerizar
├── docker-compose.yml              # Base de datos local
│
├── k8s/                            # 🆕 Manifests Kubernetes
│   ├── 01-namespace.yaml
│   ├── 02-mariadb-secret.yaml
│   ├── 03-mariadb-configmap.yaml
│   ├── 04-mariadb-pvc.yaml
│   ├── 05-mariadb-statefulset.yaml
│   ├── 06-mariadb-service.yaml
│   ├── 07-api-configmap.yaml
│   ├── 08-api-deployment.yaml
│   ├── 09-api-service.yaml
│   ├── kustomization.yaml
│   └── GUIA-KUBERNETES.md
│
├── DOCUMENTACION.md                # Documentación completa
└── README.md                       # Este archivo
```

---

## 🔧 Tecnologías

| Componente | Tecnología |
|-----------|-----------|
| **Framework** | ASP.NET Core 10 |
| **Base de Datos** | MariaDB 11 |
| **ORM** | Entity Framework Core |
| **Mapeo** | AutoMapper |
| **API Docs** | Swagger/OpenAPI |
| **Containerización** | Docker |
| **Orquestación** | Kubernetes |

---

## 📊 Endpoints Principales

### Clientes
```http
GET    /api/customers              # Listar todos
GET    /api/customers/{id}         # Por ID
POST   /api/customers              # Crear
PUT    /api/customers/{id}         # Actualizar
DELETE /api/customers/{id}         # Eliminar (soft delete)
```

### Teléfonos
```http
GET    /api/phones                 # Listar todos
GET    /api/phones/{id}            # Por ID
POST   /api/phones                 # Crear
PUT    /api/phones/{id}            # Actualizar
DELETE /api/phones/{id}            # Eliminar
POST   /api/phones/{id}/purchase   # Comprar (reducir stock)
GET    /api/phones/search/byBrand  # Buscar por marca
```

---

## 🐳 Despliegue con Docker

### Construir imagen
```bash
docker build -t api-tienda:latest .
```

### Ejecutar contenedor
```bash
docker run -p 5000:5000 \
  -e "ConnectionStrings__DefaultConnection=Server=mariadb;..." \
  api-tienda:latest
```

---

## ☸️ Despliegue con Kubernetes

### Requisitos
- Cluster Kubernetes (Minikube, Docker Desktop o cloud)
- `kubectl` CLI
- Imagen Docker disponible

### Despliegue
```bash
# Opción 1: Aplicar manifests uno por uno
kubectl apply -f k8s/01-namespace.yaml
kubectl apply -f k8s/02-mariadb-secret.yaml
# ... (ver GUIA-KUBERNETES.md para paso a paso)

# Opción 2: Aplicar todo con Kustomize
kubectl apply -k k8s/

# Ver estado
kubectl get all -n tienda-api
```

**📖 Ver [k8s/GUIA-KUBERNETES.md](./k8s/GUIA-KUBERNETES.md) para guía detallada.**

---

## 🐛 Problemas Comunes y Soluciones

### "Cliente creado pero GET por ID no lo encuentra"
**Solución:** Verificar que `IsActive = true`. Ver [DOCUMENTACION.md](./DOCUMENTACION.md#soft-delete)

### "Connection refused" para BD
**Solución:**
```bash
docker-compose up -d
docker ps  # Verificar que mariadb esté corriendo
```

### Migraciones no aplicadas
```bash
dotnet ef database update
```

### Imagen Docker no encontrada en K8s
```bash
# Construir y pushear
docker build -t tu-usuario/api-tienda:latest .
docker push tu-usuario/api-tienda:latest

# Actualizar en manifests K8s
```

---

## 🎯 Para la Presentación del Profesor

### Materiales Preparados

1. **Documentación Técnica**: [DOCUMENTACION.md](./DOCUMENTACION.md)
   - Explicar arquitectura en capas
   - Mostrar endpoints y queries SQL
   - Comentar patrones SOLID

2. **Live Demo Swagger**
   - Crear cliente
   - Actualizar cliente
   - Obtener por ID
   - Eliminar (soft delete)

3. **Database View (DBeaver)**
   - Mostrar tablas Customers y Phones
   - Verificar registros creados
   - Mostrar soft delete (IsActive = 0)

4. **Docker & Kubernetes** (opcional)
   - Mostrar contenerización
   - Explicar manifests K8s
   - Demo de despliegue

### Preguntas Probables del Profesor

**Q: ¿Cómo se guardan los datos?**
A: Usamos Entity Framework Core con migraciones. Los cambios se rastrean en `Migrations/` y se aplican automáticamente.

**Q: ¿Por qué usan DTOs?**
A: Para seguridad (no exponemos Password) y control de serialización.

**Q: ¿Cómo se escala?**
A: Con Kubernetes: más replicas en el Deployment, MariaDB como StatefulSet.

**Q: ¿Qué pasa si la BD falla?**
A: El PVC mantiene datos. StatefulSet reinicia el pod automáticamente.

---

## 🌱 Próximas Mejoras (Futuro)

- [ ] Autenticación JWT
- [ ] Hash de contraseñas (bcrypt)
- [ ] CORS configurado
- [ ] Paginación en listados
- [ ] Caché Redis
- [ ] Logging centralizado
- [ ] Monitoreo Prometheus
- [ ] CI/CD GitHub Actions

---

## 📝 Ramas Git

- **main**: Producción (estable)
- **develop**: Desarrollo (actual)
  - Contiene todas las características
  - Documentación completa
  - Manifests Kubernetes

```bash
# Ver ramas
git branch -a

# Cambiar a develop
git checkout develop

# Hacer pull de cambios
git pull origin develop
```

---

## 🤝 Contribuciones

Este proyecto es educativo. Para mejoras:
1. Crear rama `feature/nombre`
2. Hacer cambios
3. Hacer commit descriptivo
4. Push y crear Pull Request

```bash
git checkout -b feature/nueva-funcionalidad
# ... cambios ...
git add .
git commit -m "feat: Descripción de cambio"
git push origin feature/nueva-funcionalidad
```

---

## 📖 Recursos Útiles

- [ASP.NET Core Docs](https://docs.microsoft.com/es-es/dotnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/es-es/ef/core/)
- [Kubernetes Docs](https://kubernetes.io/docs/)
- [Docker Docs](https://docs.docker.com/)

---

## ✅ Checklist de Funcionalidad

- [x] Base de datos con MariaDB
- [x] Modelos y DTOs
- [x] Endpoints CRUD completos
- [x] Migraciones y seeding
- [x] Repository Pattern
- [x] Inyección de Dependencias
- [x] Swagger/OpenAPI
- [x] Soft Delete
- [x] AutoMapper
- [x] Docker
- [x] Kubernetes
- [x] Documentación completa

---

## 📄 Licencia

Proyecto educativo - IES [Tu Instituto] 2026

---

**Realizado por:** [Tu Nombre]  
**Fecha:** 22 de Abril de 2026  
**Institución:** 2º Desarrollo de Aplicaciones Web (DAW)

---

## 🎯 Próximos Pasos

1. **Para aprender**: Leer [DOCUMENTACION.md](./DOCUMENTACION.md)
2. **Para desplegar**: Leer [k8s/GUIA-KUBERNETES.md](./k8s/GUIA-KUBERNETES.md)
3. **Para debuggear**: Ver sección de Troubleshooting
4. **Para presentar**: Preparar demo en Swagger + mostrar código

¡**Buena suerte! 🚀**
