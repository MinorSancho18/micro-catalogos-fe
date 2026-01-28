# Project Structure - Micro Catálogos Frontend

## Directory Tree

```
micro-catalogos-fe/
├── README.md                           # Project documentation
├── SECURITY.md                         # Security considerations and recommendations
├── Frontend.sln                        # Visual Studio Solution file
├── .gitignore                          # Git ignore rules
└── src/
    ├── Frontend.Domain/                # Domain layer (Clean Architecture)
    │   └── Frontend.Domain.csproj
    │
    ├── Frontend.Application/           # Application layer (Business logic)
    │   ├── Configuration/
    │   │   └── ApiSettings.cs          # API configuration settings
    │   ├── DTOs/
    │   │   ├── ClienteDto.cs           # Client data transfer object
    │   │   ├── VehiculoDto.cs          # Vehicle data transfer object
    │   │   ├── ExtraDto.cs             # Extra data transfer object
    │   │   ├── CategoriaVehiculoDto.cs # Vehicle category DTO
    │   │   └── TokenResponse.cs        # JWT token response DTO
    │   ├── Interfaces/
    │   │   ├── IJwtTokenService.cs     # JWT service interface
    │   │   ├── IClientesApiService.cs  # Clients API interface
    │   │   ├── IVehiculosApiService.cs # Vehicles API interface
    │   │   ├── IExtrasApiService.cs    # Extras API interface
    │   │   └── ICategoriasApiService.cs# Categories API interface
    │   ├── Services/
    │   │   ├── JwtTokenService.cs      # JWT token management
    │   │   ├── ClientesApiService.cs   # Clients API implementation
    │   │   ├── VehiculosApiService.cs  # Vehicles API implementation
    │   │   ├── ExtrasApiService.cs     # Extras API implementation
    │   │   └── CategoriasApiService.cs # Categories API implementation
    │   └── Frontend.Application.csproj
    │
    ├── Frontend.Infrastructure/        # Infrastructure layer
    │   ├── Configuration/              # (Empty - ApiSettings moved to Application)
    │   ├── Http/                       # (Reserved for future HTTP handlers)
    │   └── Frontend.Infrastructure.csproj
    │
    └── Frontend.Web/                   # Presentation layer (MVC)
        ├── Controllers/
        │   ├── HomeController.cs       # Home page controller
        │   ├── ClientesController.cs   # Clients CRUD controller
        │   ├── VehiculosController.cs  # Vehicles CRUD controller
        │   └── ExtrasController.cs     # Extras CRUD controller
        ├── Views/
        │   ├── Shared/
        │   │   ├── _Layout.cshtml      # Main layout template
        │   │   ├── Error.cshtml        # Error page
        │   │   └── _ValidationScriptsPartial.cshtml
        │   ├── Home/
        │   │   ├── Index.cshtml        # Landing page
        │   │   └── Privacy.cshtml      # Privacy page
        │   ├── Clientes/
        │   │   └── Index.cshtml        # Clients management page
        │   ├── Vehiculos/
        │   │   └── Index.cshtml        # Vehicles management page
        │   ├── Extras/
        │   │   └── Index.cshtml        # Extras management page
        │   ├── _ViewStart.cshtml
        │   └── _ViewImports.cshtml
        ├── wwwroot/
        │   ├── css/
        │   │   └── site.css            # Custom styles
        │   ├── js/
        │   │   ├── site.js             # General site scripts
        │   │   ├── clientes.js         # Clients CRUD logic
        │   │   ├── vehiculos.js        # Vehicles CRUD logic
        │   │   └── extras.js           # Extras CRUD logic
        │   ├── lib/                    # Client-side libraries
        │   │   ├── bootstrap/          # Bootstrap 5
        │   │   ├── jquery/             # jQuery 3.x
        │   │   └── jquery-validation/  # Validation libraries
        │   └── favicon.ico
        ├── Models/
        │   └── ErrorViewModel.cs       # Error view model
        ├── Properties/
        │   └── launchSettings.json     # Launch configuration
        ├── Program.cs                  # Application entry point & DI config
        ├── appsettings.json            # Application configuration
        ├── appsettings.Development.json# Development configuration
        └── Frontend.Web.csproj
```

## Project Dependencies

```
Frontend.Web
  → Frontend.Application
  → Frontend.Infrastructure

Frontend.Application
  → Frontend.Domain
  → Microsoft.Extensions.Options (NuGet)

Frontend.Infrastructure
  → Frontend.Application

Frontend.Domain
  (No dependencies)
```

## Technology Stack

### Backend
- ASP.NET Core 8.0 MVC
- HttpClient for API consumption
- Dependency Injection
- IOptions pattern for configuration

### Frontend
- Bootstrap 5 (UI framework)
- jQuery 3.x (DOM manipulation)
- DataTables 1.13.7 (Data grids)
- SweetAlert2 11 (Notifications)
- Bootstrap Icons 1.11.2

### Architecture
- Clean Architecture
- Repository pattern (via API services)
- DTO pattern for data transfer

## Key Features

1. **JWT Authentication** - Automatic token retrieval and injection
2. **CRUD Operations** - Complete Create, Read, Update, Delete for all entities
3. **Responsive Design** - Mobile-friendly Bootstrap 5 UI
4. **Error Handling** - Comprehensive client and server-side error handling
5. **Data Tables** - Sortable, searchable, paginated tables
6. **Modal Dialogs** - Bootstrap modals for forms and details
7. **AJAX Communication** - Asynchronous API calls for better UX

## Build & Run

```bash
# Build the solution
dotnet build Frontend.sln

# Run the application
cd src/Frontend.Web
dotnet run

# Access at: https://localhost:5001
```

## Configuration

Edit `src/Frontend.Web/appsettings.json`:

```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7001",
    "AuthCode": "PROCOMER-2024-SECURE-API-TOKEN-XYZ123"
  }
}
```

## API Endpoints Consumed

### Authentication
- POST `/api/auth/token` - Obtain JWT token

### Clientes
- GET `/api/clientes` - List all clients
- GET `/api/clientes/{id}` - Get client by ID
- POST `/api/clientes` - Create client
- PUT `/api/clientes/{id}` - Update client
- DELETE `/api/clientes/{id}` - Delete client

### Vehículos
- GET `/api/vehiculos` - List all vehicles
- GET `/api/vehiculos/{id}` - Get vehicle by ID
- POST `/api/vehiculos` - Create vehicle
- PUT `/api/vehiculos/{id}` - Update vehicle
- DELETE `/api/vehiculos/{id}` - Delete vehicle
- GET `/api/categorias-vehiculo` - List vehicle categories

### Extras
- GET `/api/extras` - List all extras
- GET `/api/extras/{id}` - Get extra by ID
- POST `/api/extras` - Create extra
- PUT `/api/extras/{id}` - Update extra
- DELETE `/api/extras/{id}` - Delete extra
