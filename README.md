# Examen Segundo Parcial: Aplicación Web Northwind

Este proyecto consiste en el desarrollo de una aplicación web utilizando ASP.NET Core MVC, Entity Framework Core y PostgreSQL bajo el enfoque Database First, utilizando la base de datos Northwind.

## Requisitos Previos
* .NET SDK 8.0 o superior
* Servidor de base de datos PostgreSQL activo con la base de datos `northwind` restaurada.

## Configuración y Ejecución

### 1. Conexión de Base de Datos
La cadena de conexión se encuentra configurada en el archivo `appsettings.json` apuntando al servidor PostgreSQL local:
```json
"DefaultConnection": "Host=localhost;Port=5432;Database=northwind;Username=postgres;Password=postgres"
```

### 2. Generación de Modelos (Database First)
Para mapear la estructura de la base de datos Northwind al proyecto, se ejecutó el siguiente comando de scaffolding de Entity Framework Core:
```bash
dotnet ef dbcontext scaffold "Host=localhost;Port=5432;Database=northwind;Username=postgres;Password=postgres" Npgsql.EntityFrameworkCore.PostgreSQL -o Models -c NorthwindContext --context-dir Data --force --no-onconfiguring --schema public
```

### 3. Ejecución del Proyecto
Para ejecutar la aplicación en el entorno de desarrollo local:
```bash
dotnet run --project NorthwindApp
```

La aplicación estará disponible por defecto en `http://localhost:5000` o `https://localhost:5001`.

## Funcionalidades Implementadas

### Módulos Principales
* **Productos (Products):** Operaciones CRUD completas y validación de campos obligatorios.
* **Categorías y Proveedores:** Vistas para el manejo de catálogos relacionados a los productos.
* **Pedidos (Orders):** Listado de transacciones históricas registradas en el sistema.

### Consultas LINQ
En la sección de productos se han implementado vistas específicas para las siguientes consultas:
1. Búsqueda interactiva de productos por coincidencia en el nombre.
2. Listado de los 10 productos con los precios unitarios más altos.
3. Cantidad total de productos agrupados por su correspondiente categoría.
4. Cantidad total de productos agrupados por su proveedor asignado.
5. Filtrado detallado de productos suministrados por un proveedor específico.

### Autenticación y Autorización (Identity)
Se integró el sistema de Identity para gestionar accesos diferenciados:
* **Roles:** Admin y Employee.
* **Acceso de Administración:** Solo los usuarios con rol de administrador pueden realizar acciones de creación, modificación o eliminación de datos.
* **Usuario Administrador por Defecto:**
  * **Correo Electrónico:** admin@northwind.com
  * **Contraseña:** Admin123!

## Publicación
La compilación final y optimizada para producción fue generada en el directorio `NorthwindApp/Publicacion` utilizando el comando:
```bash
dotnet publish -c Release -o Publicacion
```
La carpeta de publicación incluye los ejecutables, dependencias, vistas precompiladas y recursos estáticos requeridos para la puesta en marcha.
