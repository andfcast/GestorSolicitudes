# GestorSolicitudes
Aplicación de gestión de solicitudes

## 🚀 Decisiones Tomadas en el Diseño

1. **Autenticación y Seguridad (JWT):** Se implementó autenticación basada en JSON Web Tokens (JWT) junto con un algoritmo de encriptación de contraseñas de una sola vía (hashing seguro), asegurando que las credenciales no se almacenen ni se expongan en texto plano.
2. **Consumo de Servicios en Frontend:** Tras una autenticación exitosa, el token generado es almacenado y adherido automáticamente a cada petición HTTP subsecuente desde el frontend para validar el acceso autorizado mediante guardias de rutas (`roleGuard`).
3. **Manejo de Catálogos (Roles, Estados y Prioridades):** Se decidió modelarlos como `enums` (enumeraciones) en .NET en lugar de tablas maestras independientes en la base de datos. Esto optimiza el rendimiento al evitar `JOINs` innecesarios en consultas masivas del dashboard, manteniendo un modelo relacional limpio y acoplando su comportamiento directamente a las reglas de negocio.
4. **Gestión de Usuarios y Roles de Acceso:** 
   * Se configuró un sistema de rutas hijas protegidas bajo un layout principal (`DashboardLayout`).
   * Se reutilizó inteligentemente el componente `MisSolicitudes` tanto para agentes como para administradores, condicionando su comportamiento y visualización mediante una validación de rol (`isAdmin`).
5. **Flujo de Asignación de Responsables (HU-06):** 
   * Las solicitudes creadas por un administrador nacen con estado **Nueva**.
   * Mediante un modal flotante e intuitivo integrado en la misma vista, el administrador puede seleccionar un agente de una lista desplegable y asignar la responsabilidad, transicionando automáticamente la solicitud al estado **Asignada**.
6. **Manejo Global de Errores:** Se implementó un `GlobalExceptionHandler` centralizado para capturar excepciones de forma transversal, retornando códigos HTTP estandarizados (`400`, `401`, `404`, `500`).
7. **Enfoque de Base de Datos (Code-First):** Se utilizó el enfoque *Code-First* mediante Entity Framework Core para agilizar el diseño y la evolución del esquema relacional.
8. **Pruebas Unitarias:** Se cubrieron los escenarios principales de las Historias de Usuario en el backend; sin embargo, las pruebas unitarias (*Unit Tests*) quedaron excluidas del alcance actual por factores de tiempo.
9. **Stack Tecnológico:** 
   * **Backend:** .NET 8 (Forzado a correr exclusivamente bajo HTTPS).
   * **Frontend:** Angular 
   * **Base de Datos:** SQL Server
10. **Librerías Gráficas y Notificaciones:** Se integraron Angular Material y SweetAlert para estilar los componentes de la interfaz y gestionar las notificaciones visuales.
11. **Arquitectura del Backend:** Se estructuró bajo los principios de **Clean Architecture**, dividiendo la solución en capas desacopladas: Presentación, Aplicación, Dominio e Infraestructura.

---

## ⚙️ Pasos para Ejecución del Backend

1. Descargar o clonar el repositorio de las fuentes.
2. Abrir la carpeta del proyecto backend (`GestorSolicitudesBack`) y ubicar el proyecto principal de la API (`GestorSolicitudes.API`).
3. Buscar el archivo `appsettings.json`, ubicar la sección `"ConnectionStrings"` y configurar la cadena de conexión apuntando a tu servidor local de **SQL Server**.
4. Abrir una terminal y ubicarse en la ruta del proyecto backend.
5. Restaurar las dependencias y paquetes NuGet ejecutando:
   ```bash
   dotnet restore
   ```
6. Aplicar las migraciones de Entity Framework ejecutando el siguiente comando:
   ```bash
   dotnet ef database update
   ```
7. Ejecutar la API:
   ```bash
   dotnet run
   ```
## 💻 Pasos para Ejecución del Frontend

1. Abrir una terminal en la raíz del proyecto de Angular.
2. Instalar los paquetes necesarios ejecutando el comando:
   ```bash
   npm install
   ```
3. Comprobar que los archivos de entorno (environments/environment.ts) apunten correctamente a la URL base de la API de .NET (https://localhost:7154/).

4. Arrancar la aplicación con el comando:
   ```bash
   ng serve -o
   ```
   La aplicación se abrirá automáticamente en http://localhost:4200
   
## Datos iniciales

Hay un usuario con el perfil Administrador cuyo nombre de usuario es admin y el pass es Admin123!
Hay tres usuarios con el perfil Agente cuyos nombres de usuario son agente1,agente2 y agente3 y el pass es Agente123!