# Frontend - Microservicio de Catálogos

Frontend ASP.NET Core MVC (.NET 8) que consume el API backend de catálogos ubicado en [https://github.com/MinorSancho18/micro-catalogos](https://github.com/MinorSancho18/micro-catalogos).

## 🎯 Características

- ✅ CRUD completo para **Clientes**
- ✅ CRUD completo para **Vehículos** (con catálogo de categorías)
- ✅ CRUD completo para **Extras**
- ✅ Autenticación JWT automática
- ✅ Clean Architecture (Domain, Application, Infrastructure, Web)
- ✅ UI con Bootstrap 5, jQuery, DataTables y SweetAlert2

## 📋 Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Backend API corriendo en `https://localhost:7001` (o la URL configurada)

## 🚀 Ejecución

### 1. Clonar el repositorio
```bash
git clone https://github.com/MinorSancho18/micro-catalogos-fe.git
cd micro-catalogos-fe
```

### 2. Configurar appsettings.json
Editar el archivo `src/Frontend.Web/appsettings.json` para configurar la URL del backend:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7001",
    "AuthCode": "PROCOMER-2024-SECURE-API-TOKEN-XYZ123"
  }
}
```

### 3. Compilar y ejecutar
```bash
cd src/Frontend.Web
dotnet restore
dotnet build
dotnet run
```

### 4. Abrir en el navegador
Abrir: `https://localhost:5001` o `http://localhost:5000`

## 🏗️ Arquitectura

El proyecto sigue los principios de **Clean Architecture** con la siguiente estructura:

```
src/
├── Frontend.Domain/          # Modelos de dominio (puede estar vacío)
├── Frontend.Application/     # Lógica de aplicación
│   ├── DTOs/                # Data Transfer Objects
│   ├── Interfaces/          # Interfaces de servicios
│   └── Services/            # Implementación de servicios API
├── Frontend.Infrastructure/  # Configuración e HTTP
│   └── Configuration/       # ApiSettings
└── Frontend.Web/            # Aplicación MVC
    ├── Controllers/         # Controladores MVC
    ├── Views/              # Vistas Razor
    └── wwwroot/            # Archivos estáticos (JS, CSS)
```

### Dependencias entre proyectos

```
Frontend.Web
  → Frontend.Application
  → Frontend.Infrastructure

Frontend.Application
  → Frontend.Domain

Frontend.Infrastructure
  → Frontend.Application
```

## 📦 Funcionalidades Implementadas

### Clientes
- Listar todos los clientes
- Ver detalle de un cliente
- Crear nuevo cliente
- Editar cliente existente
- Eliminar cliente

**Campos:**
- ID Cliente
- Nombre
- Número de Cédula

### Vehículos
- Listar todos los vehículos
- Ver detalle de un vehículo
- Crear nuevo vehículo
- Editar vehículo existente
- Eliminar vehículo
- Seleccionar categoría de vehículo desde catálogo

**Campos:**
- ID Vehículo
- Descripción
- Categoría (con código y descripción)
- Costo

### Extras
- Listar todos los extras
- Ver detalle de un extra
- Crear nuevo extra
- Editar extra existente
- Eliminar extra

**Campos:**
- ID Extra
- Descripción
- Costo

## 🔐 Autenticación JWT

El frontend obtiene automáticamente un token JWT del backend al iniciar la aplicación. Este token se utiliza en todas las llamadas subsiguientes al API.

**Nota importante:** El JWT implementado es un token técnico para consumir el API. NO representa autenticación de usuarios finales.

## 🛠️ Tecnologías Utilizadas

- **Backend Framework:** ASP.NET Core 8.0 MVC
- **Frontend UI:**
  - Bootstrap 5
  - jQuery 3.x
  - DataTables.net
  - SweetAlert2
- **Arquitectura:** Clean Architecture
- **HTTP Client:** HttpClient con DI
- **Configuración:** IOptions pattern

## 📝 Notas Técnicas

- El frontend es completamente independiente del backend
- La comunicación es exclusivamente vía HTTP/REST
- No se comparte código ni solución con el backend
- Compatible con cualquier versión de .NET 8.x
- No incluye Docker ni global.json

## 🔍 Estructura de Archivos Clave

### Controllers
- `HomeController.cs` - Página de inicio
- `ClientesController.cs` - CRUD de clientes
- `VehiculosController.cs` - CRUD de vehículos y categorías
- `ExtrasController.cs` - CRUD de extras

### Views
- `Views/Clientes/Index.cshtml` - Vista de clientes
- `Views/Vehiculos/Index.cshtml` - Vista de vehículos
- `Views/Extras/Index.cshtml` - Vista de extras

### JavaScript
- `wwwroot/js/clientes.js` - Lógica de clientes
- `wwwroot/js/vehiculos.js` - Lógica de vehículos
- `wwwroot/js/extras.js` - Lógica de extras

### Services
- `JwtTokenService.cs` - Obtención y gestión del token JWT
- `ClientesApiService.cs` - Consumo del API de clientes
- `VehiculosApiService.cs` - Consumo del API de vehículos
- `ExtrasApiService.cs` - Consumo del API de extras
- `CategoriasApiService.cs` - Consumo del catálogo de categorías

## 📄 Licencia

Este proyecto es parte de un ejercicio académico/práctico.
