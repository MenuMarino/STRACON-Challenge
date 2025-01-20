# Portal de proveedores

## Prerrequisitos

- Node v20.11.1
- Angular 19.1.2
- SQL Server
- .NET 8

## Configuracion

- Frontend

  - En el archivo `SupplierPortal/src/environment.ts` se encuentra la variable `apiBaseUrl` configurada al url del api (por defecto `http://localhost:5108/api`)

- Backend
  - En el archivo `SupplierPortalAPI/appsettings.json` se deben actualizar las lineas:
    - `<LOCAL_SERVER>` con el servidor local de SQL
    - `<JWT_KEY>` con el secret para crear los tokens utilizados en la pagina y los requests.
  - En el archivo `SupplierPortalAPI/appsettings.json` se encuentra la variable `AllowedOrigins` configurada al url del api (por defecto `http://localhost:4200`)

## Ejecucion

- Backend

  - Se debe ejecutar los siguientes comandos en la carpeta `SupplierPortalAPI/`
    - `dotnet ef database update`
    - `dotnet run`

- Frontend
  - Se deben ejecutar los siguientes comandos en la carpeta `SupplierPortal/`
    - `npm install`
    - `ng serve`

## Documentacion

Las rutas del API se pueden visualizar [aqui](`http://localhost:5108/swagger/index.html`)
