
# 📝 To‑Do List API – Minimal API en .NET 8

![Estado](https://img.shields.io/badge/Estado-🚧%20En%20desarrollo-yellow?style=flat)
![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue?style=flat&logo=visual-studio&logoColor=white)
![.NET 8](https://img.shields.io/badge/.NET-8.0-purple?style=flat)
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-blue)
![Serilog](https://img.shields.io/badge/Logging-Serilog-green)
![Swagger](https://img.shields.io/badge/API%20Docs-Swagger-orange)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-lightgrey)
![Se Busca Feedback](https://img.shields.io/badge/Se%20Busca-Feedback-brightgreen?style=flat)  

[🌐Inglés](README.en.md) | [🌐Español](README.es.md)

## 📌 Sobre el proyecto

Este proyecto fue desarrollado con el objetivo de **practicar las nuevas características de .NET 8 y Minimal API**, aplicando principios de **Clean Architecture** y utilizando herramientas modernas para persistencia, logging y documentación.

Sirve como ejemplo para mi **portfolio** y como base para futuros desarrollos más complejos.

  
## 🛠 Herramientas utilizadas

-  **Visual Studio Community 2022** – IDE

-  **.NET 8** con **Minimal API**

-  **Entity Framework Core** – ORM con persistencia en **SQL Server**

-  **Serilog** – Logging estructurado

-  **Swagger** – Documentación interactiva

-  **Postman** – Pruebas manuales de endpoints

-  **Clean Architecture** – Separación de capas y mantenibilidad


## ⚙️ Configuración e Instalación del proyecto

### 📋 Requisitos previos

Antes de comenzar, asegúrate de tener instalado:

- 🟣 [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)

- 🗃️ [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

---

### 📥 Clonar el repositorio

```git
git clone https://github.com/MarioEspinosaFiguerez/ToDoListAPI.git
```

---

### 📦 Restaurar dependencias (opcional)

Desde Visual Studio:

- Abre la solución

- Haz clic derecho sobre la solución en el Explorador de soluciones

- Selecciona **Restaurar Paquetes Nuget**
 
---


### 🔧 Configuración de la base de datos

Abre el archivo `appsettings.json` y modifica la sección `ConnectionStrings` con la connection string que corresponda con tu entorno:

#### 🔹Opción 1: SQL Server instalado localmente
```json
{
    "ConnectionStrings": {
	"Connection":"Server=localhost;Database=ToDoList;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
    }
}
```

####  🔹Opción 2: SQL Server LocalDB (ideal si no tienes SQL Server instalado)
```json
{
    "ConnectionStrings": {
	"Connection": "Server=(localdb)\\MSSQLLocalDB;Database=ToDoList;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
    }
}

```  
>⚠️ Recuerda instalar el componente **Almacenamiento y Procesamiento de Datos** en el Visual Studio Installer para usar LocalDB.
 ---

### 🚀 Aplicar migraciones

Abre la consola del Administrador de paquetes:

1. Ve a **Ve a Vista** > **Otras ventanas** > **Consola del Administrador de Paquetes**

2. En **Proyecto por Defecto**, selecciona `Infrastructure`

3. Ejecuta el siguiente comando:

```bash
Update-Database
```
---  

### 📮 Colección de Postman

En la carpeta `/Postman` encontrarás:

-  `ToDoListAPI.postman_collection.json` → Peticiones de la API.

-  `ToDoListAPI.postman_environment.json` → Variables de entorno para la API.

#### 📌 Cómo importarlas en Postman:

1. Abre **Postman**.

2. Ve a **Archivo** > **Importar**.

3. Selecciona el archivo `.json` correspondiente (colección o environment).

> ℹ️ La colección usa la variable `{{baseUrl}}` para la dirección de la API.
> Cambia su valor en el environment si tu puerto/localhost es distinto.

## 📑 Endpoints principales

### 👤 Users
| Método | Endpoint                                         | Descripción                              |
|--------|--------------------------------------------------|------------------------------------------|
| GET    | `/users`                                         | Obtiene todos los usuarios               |
| GET    | `/users/{id}`                                    | Obtiene un usuario por ID                 |
| GET    | `/users/{id}/tasks`                              | Obtiene todas las tareas asignadas a un usuario	|
| GET    | `/users/{id}/tasks/{taskId}`                     | Obtiene una tarea asignada a un usuario   |
| POST   | `/users`                                         | Crea un nuevo usuario                     |
| PATCH  | `/users/{id}`                                    | Actualiza datos de un usuario             |
| DELETE| `/users/{id}`                                     | Elimina un usuario                        |


### ✅ Tasks
| Método | Endpoint                                         | Descripción                              |
|--------|--------------------------------------------------|------------------------------------------|
| GET    | `/tasks`                                         | Obtiene todas las tareas                  |
| GET    | `/tasks/{id}`                                    | Obtiene una tarea por ID                  |
| POST   | `/tasks`                                         | Crea una nueva tarea                      |
| PATCH  | `/tasks/{id}`                                    | Actualiza datos de una tarea              |
| DELETE | `/tasks/{id}`                                    | Elimina una tarea             			|

> ℹ️ Todos los endpoints devuelven y aceptan datos en formato **JSON**.  
> La variable `{{baseUrl}}` en la colección de Postman define la URL base de la API.
