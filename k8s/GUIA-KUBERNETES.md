# Guía de Despliegue en Kubernetes

## 📚 Índice
1. [Conceptos Básicos](#conceptos-básicos)
2. [Requisitos Previos](#requisitos-previos)
3. [Arquitectura Kubernetes](#arquitectura-kubernetes)
4. [Paso a Paso del Despliegue](#paso-a-paso-del-despliegue)
5. [Verificación](#verificación)
6. [Troubleshooting](#troubleshooting)
7. [Scaling y Configuración](#scaling-y-configuración)

---

## Conceptos Básicos

### ¿Qué es Kubernetes?

Kubernetes (K8s) es un **orquestador de contenedores** que automatiza:
- 🚀 Despliegue de aplicaciones
- 📈 Escalado automático
- 🔄 Recuperación ante fallos
- 🌐 Balanceo de carga
- 🔒 Gestión de secretos

### Conceptos Clave

| Término | Definición |
|---------|-----------|
| **Pod** | La unidad más pequeña: contenedor + configuración |
| **Deployment** | Gestiona pods (replicas, actualizaciones) |
| **Service** | Expone pods con DNS/IP estable |
| **StatefulSet** | Como Deployment pero para datos persistentes (BD) |
| **ConfigMap** | Variables de configuración en texto plano |
| **Secret** | Variables sensibles (contraseñas) encriptadas |
| **PVC** | Almacenamiento persistente |
| **Namespace** | Aislamiento de recursos |

---

## Requisitos Previos

### 1. Instalar Kubernetes Local

#### Opción A: Minikube (Recomendado para aprender)
```bash
# Descargar: https://minikube.sigs.k8s.io/docs/start/

# En Windows (PowerShell como Admin)
choco install minikube

# Iniciar cluster
minikube start --driver=docker --memory=4096 --cpus=2

# Verificar
minikube status

# Dashboard (abrir interfaz visual)
minikube dashboard
```

#### Opción B: Docker Desktop (Si ya lo tienes)
1. Abrir Docker Desktop
2. Settings → Kubernetes → Enable Kubernetes
3. Esperar a que inicie

#### Opción C: Cluster Real (AWS/Azure/GCP)
Usar `kubeadm` o servicios managed de cloud

### 2. Instalar Herramientas

```bash
# kubectl - Herramienta CLI de K8s
# Windows: Descargarlua desde https://kubernetes.io/docs/tasks/tools/

# Verificar versión
kubectl version --client

# Verificar conexión al cluster
kubectl cluster-info
```

### 3. Preparar la Imagen Docker

```bash
# 1. Construir la imagen
docker build -t tu-usuario/api-tienda:latest .

# 2. Pushear a Docker Hub
docker login
docker push tu-usuario/api-tienda:latest

# O si usas minikube (sin pushear):
minikube image load tu-usuario/api-tienda:latest
```

---

## Arquitectura Kubernetes

```
┌─────────────────────────────────────────────┐
│         KUBERNETES CLUSTER                  │
│                                             │
│  ┌──────────────────────────────────────┐  │
│  │      NAMESPACE: tienda-api          │  │
│  │                                     │  │
│  │  ┌─────────────────────────────┐    │  │
│  │  │   Deployment: api-tienda    │    │  │
│  │  │  ┌─────────┐  ┌─────────┐  │    │  │
│  │  │  │ Pod 1   │  │ Pod 2   │  │    │  │
│  │  │  │ :5000   │  │ :5000   │  │    │  │
│  │  │  └─────────┘  └─────────┘  │    │  │
│  │  └─────────────────────────────┘    │  │
│  │           ▲                          │  │
│  │           │                          │  │
│  │  ┌────────────────────────────┐     │  │
│  │  │ Service: api-tienda        │     │  │
│  │  │ (Load Balancer)            │     │  │
│  │  │ Puerto 80 → 5000           │     │  │
│  │  └────────────────────────────┘     │  │
│  │                                     │  │
│  │  ┌─────────────────────────────┐    │  │
│  │  │  StatefulSet: mariadb       │    │  │
│  │  │  ┌──────────────────────┐   │    │  │
│  │  │  │ Pod mariadb-0 :3306  │   │    │  │
│  │  │  │ │ /var/lib/mysql ────┼─────┐ │  │
│  │  │  └──────────────────────┘   │ │ │  │
│  │  │         │                   │ │ │  │
│  │  │         │ (mariadb service) │ │ │  │
│  │  └─────────┼───────────────────┼─┼─┘  │
│  │            │                   │ │     │
│  │  ┌─────────────────────────────┼─┼─┐  │
│  │  │ PVC: mariadb-pvc            │ │ │  │
│  │  │ (10Gi storage)              │ │ │  │
│  │  └─────────────────────────────┼─┘─┘  │
│  │                                │       │
│  └────────────────────────────────┼───────┘
│                                   │
└───────────────────────────────────┼───────
                                    │
                            ┌───────▼────────┐
                            │ Cliente HTTP   │
                            │ localhost:80   │
                            └────────────────┘
```

---

## Paso a Paso del Despliegue

### PASO 1: Verificar Conexión a Kubernetes

```bash
# Ver estado del cluster
kubectl cluster-info

# Ver nodos disponibles
kubectl get nodes

# Ver namespaces existentes
kubectl get namespaces
```

**Salida esperada:**
```
NAME              STATUS   ROLES    AGE
minikube          Ready    master   10m
```

---

### PASO 2: Crear Namespace

```bash
# Aplicar el namespace
kubectl apply -f k8s/01-namespace.yaml

# Verificar que se creó
kubectl get namespaces

# Ver detalles
kubectl describe namespace tienda-api
```

---

### PASO 3: Crear Secretos (Contraseñas)

```bash
# Aplicar secretos
kubectl apply -f k8s/02-mariadb-secret.yaml

# Verificar
kubectl get secrets -n tienda-api

# Ver contenido (encodificado en base64)
kubectl get secret mariadb-secret -n tienda-api -o yaml
```

---

### PASO 4: Crear ConfigMaps (Configuración)

```bash
# ConfigMap de MariaDB
kubectl apply -f k8s/03-mariadb-configmap.yaml

# ConfigMap de API
kubectl apply -f k8s/07-api-configmap.yaml

# Verificar
kubectl get configmaps -n tienda-api
```

---

### PASO 5: Crear Almacenamiento Persistente

```bash
# PVC para datos de MariaDB
kubectl apply -f k8s/04-mariadb-pvc.yaml

# Verificar
kubectl get pvc -n tienda-api
kubectl get pv
```

---

### PASO 6: Desplegar MariaDB

```bash
# StatefulSet de MariaDB
kubectl apply -f k8s/05-mariadb-statefulset.yaml

# Esperar a que esté listo (Ready 1/1)
kubectl get statefulset -n tienda-api -w

# Ver pods
kubectl get pods -n tienda-api

# Ver logs
kubectl logs -n tienda-api mariadb-0
```

**Esperar hasta que muestre:**
```
NAME        READY   AGE
mariadb-0   1/1     2m
```

---

### PASO 7: Exponer MariaDB con Service

```bash
# Service para MariaDB
kubectl apply -f k8s/06-mariadb-service.yaml

# Verificar
kubectl get services -n tienda-api
```

---

### PASO 8: Desplegar API

```bash
# Deployment de la API
kubectl apply -f k8s/08-api-deployment.yaml

# Ver pods (esperar a que estén Running)
kubectl get pods -n tienda-api -w

# Ver replicas
kubectl get deployment -n tienda-api
```

**Esperar hasta que muestre:**
```
NAME         READY   UP-TO-DATE   AVAILABLE
api-tienda   2/2     2            2
```

---

### PASO 9: Exponer API con Service

```bash
# Service para API
kubectl apply -f k8s/09-api-service.yaml

# Ver servicios
kubectl get services -n tienda-api
```

---

### PASO 10: Acceder a la API

#### Si usas Minikube:
```bash
# Ver IP de minikube
minikube ip
# Resultado: 192.168.49.2

# Abrir tunel (en otra terminal)
minikube tunnel

# Acceder en el navegador
http://localhost/swagger/index.html
```

#### Si usas Docker Desktop:
```bash
# Directamente accesible
http://localhost/swagger/index.html
```

#### Si usas cluster real:
```bash
# Ver IP del LoadBalancer
kubectl get svc -n tienda-api api-tienda

# Esperar a que tenga IP externa asignada
# Acceder con esa IP
```

---

## Verificación

### Ver todos los recursos en el namespace

```bash
# Todos los recursos
kubectl get all -n tienda-api

# Formato detallado
kubectl get all -n tienda-api -o wide
```

### Ver logs de los pods

```bash
# Logs de API
kubectl logs -n tienda-api deployment/api-tienda

# Logs en tiempo real
kubectl logs -n tienda-api deployment/api-tienda -f

# Logs de un pod específico
kubectl logs -n tienda-api api-tienda-xxxxx

# Logs de MariaDB
kubectl logs -n tienda-api mariadb-0
```

### Ejecutar comandos en un pod

```bash
# Entrar en el pod de MariaDB
kubectl exec -it -n tienda-api mariadb-0 -- bash

# Ver BD
mysql -u root -p
# Contraseña: root123
SHOW DATABASES;
USE mi_api_db;
SELECT * FROM Customers;
```

### Verificar health checks

```bash
# Ver estado detallado de pods
kubectl describe pod -n tienda-api api-tienda-xxxxx

# Ver eventos del namespace
kubectl get events -n tienda-api --sort-by='.lastTimestamp'
```

---

## Troubleshooting

### Pod no inicia (Pending)

```bash
# Describir el pod para ver por qué
kubectl describe pod -n tienda-api api-tienda-xxxxx

# Causas comunes:
# - Imagen no encontrada
# - PVC no existe
# - Recursos insuficientes
```

**Solución:**
```bash
# Verificar que la imagen existe
docker image ls | grep api-tienda

# Si no existe, construir y pushear
docker build -t tu-usuario/api-tienda:latest .
docker push tu-usuario/api-tienda:latest
```

### Pod en estado CrashLoopBackOff

Significa que se inicia y luego se detiene.

```bash
# Ver logs para entender por qué
kubectl logs -n tienda-api api-tienda-xxxxx --previous

# Causas comunes:
# - Connection string incorrecta
# - BD no está lista
# - Error en aplicación .NET
```

### No puede conectar a MariaDB

```bash
# Verificar que MariaDB está corriendo
kubectl get pods -n tienda-api mariadb-0

# Verificar el service
kubectl get svc -n tienda-api mariadb

# Probar conectividad desde API pod
kubectl exec -it -n tienda-api api-tienda-xxxxx -- sh

# Dentro del pod:
nslookup mariadb
curl mariadb:3306
```

### LoadBalancer no obtiene IP externa

```bash
# Si usas minikube:
minikube tunnel

# Si usas Docker Desktop:
# Esperar a que Docker asigne la IP

# Ver estado
kubectl get svc -n tienda-api api-tienda -w
```

---

## Scaling y Configuración

### Escalar Deployment (más replicas)

```bash
# Escalar a 5 replicas
kubectl scale deployment api-tienda -n tienda-api --replicas=5

# Verificar
kubectl get deployment -n tienda-api
```

### Actualizar imagen

```bash
# Cambiar la imagen de forma manual
kubectl set image deployment/api-tienda \
  api=tu-usuario/api-tienda:v2.0 \
  -n tienda-api

# Ver rollout
kubectl rollout status deployment/api-tienda -n tienda-api
```

### Revertir actualización

```bash
# Ver historial
kubectl rollout history deployment/api-tienda -n tienda-api

# Revertir a revisión anterior
kubectl rollout undo deployment/api-tienda -n tienda-api
```

### Usar Kustomize para aplicar todo

```bash
# Aplicar todos los manifests en orden
kubectl apply -k k8s/

# Verificar
kubectl get all -n tienda-api
```

---

## Comandos Útiles

```bash
# Eliminar namespace (borra todo)
kubectl delete namespace tienda-api

# Ver estadísticas de recursos
kubectl top nodes
kubectl top pods -n tienda-api

# Abrir puerto-forward (acceder sin Service)
kubectl port-forward -n tienda-api svc/api-tienda 8080:80

# Copiar archivo del pod
kubectl cp -n tienda-api mariadb-0:/var/lib/mysql/mi_api_db.sql ./backup.sql

# Ver YAML generado
kubectl get deployment -n tienda-api api-tienda -o yaml
```

---

## Resumen del Flujo

```
1. Cliente (Navegador)
   ↓
2. Service (LoadBalancer: localhost:80)
   ↓
3. Balanceo de carga → Pod 1 o Pod 2
   ↓
4. API .NET Core (:5000)
   ↓
5. Conecta a: mariadb:3306
   ↓
6. MariaDB (Base de datos)
```

Cada componente es **independiente** y se **recupera automáticamente** si falla.

---

## Próximos Pasos

1. **Autoscaling**: Agregar HPA (Horizontal Pod Autoscaler)
2. **Ingress**: Configurar dominio personalizado
3. **TLS/HTTPS**: Certificados SSL con cert-manager
4. **Monitoring**: Prometheus + Grafana
5. **Logging**: ELK Stack

---

**¡Felicidades! Tu API está corriendo en Kubernetes! 🎉**
