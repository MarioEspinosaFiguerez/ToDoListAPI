# 📝 To‑Do List API – Minimal API in .NET 8

![Status](https://img.shields.io/badge/Status-%20Completed-brightgreen?style=flat)
![Visual Studio 2022](https://img.shields.io/badge/Visual%20Studio-2022-blue?style=flat&logo=visual-studio&logoColor=white)
![.NET 8](https://img.shields.io/badge/.NET-8.0-purple?style=flat)
![EF Core](https://img.shields.io/badge/Entity%20Framework-Core-blue)
![Serilog](https://img.shields.io/badge/Logging-Serilog-green)
![Swagger](https://img.shields.io/badge/API%20Docs-Swagger-orange)
![SQL Server](https://img.shields.io/badge/Database-SQL%20Server-lightgrey)
![Open for Feedback](https://img.shields.io/badge/Open_for-Feedback-brightgreen?style=flat)
  
[🌐English](README.en.md) | [🌐Spanish](README.es.md)  

## 📌 About the Project

This project was developed with the goal of practicing and exploring the latest features of .NET 8 and Minimal API.

The architecture follows the Clean Architecture approach, aiming for maintainable, scalable, and easily testable code.

It serves as a portfolio project to showcase best practices in RESTful API development and can be used as a foundation for future projects.

## 🛠 Tools Used

-  **Visual Studio Community 2022** – IDE
  
-  **.NET 8** with **Minimal API**
   
-  **Entity Framework Core** – ORM with persistence in **SQL Server**
   
-  **Serilog** – Structured logging
  
-  **Swagger** – Interactive API documentation
  
-  **Postman** – Manual API testing
  
-  **Clean Architecture** – Layer separation and maintainability

## ⚙️ Setup and Installation

### 📋 Prerequisites

Make sure you have the following installed:

- 🟣 [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download)
- 🗃️ [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

---

### 📥 Clone the repository

```git
git clone https://github.com/MarioEspinosaFiguerez/ToDoListAPI.git
```

---

### 📦 Restore dependencies (optional)

From Visual Studio:

1. Open the solution

2. Right-click the solution in Solution Explorer

3. Select Restore NuGet Packages
 
---

### 🔧 Database configuration

Open the `appsettings.json` file and update the `ConnectionStrings` section with the correct connection string for your environment:

#### 🔹Option 1: Locally installed SQL Server
```json
"ConnectionStrings": {
  "Connection": "Server=localhost;Database=ToDoList;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true",
  }
```

#### 🔹Option 2: SQL Server LocalDB (ideal if you don’t have SQL Server installed)
```json
{
	"ConnectionStrings": {
		"Connection": "Server=(localdb)\\MSSQLLocalDB;Database=ToDoList;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
	}
}
```
 
 >⚠️ Remember to install the Data Storage and Processing component in Visual Studio Installer to use LocalDB.

 ---

### 🚀 Apply migrations

Open the Package Manager Console:

1. Go to View > Other Windows > Package Manager Console

2. In Default project, select Infrastructure

3. Run the following command:

```bash
Update-Database
```

---

### 📮 Postman Collection
In the /Postman folder you will find:
- `ToDoListAPI.postman_collection.json` → API requests.
- `ToDoListAPI.postman_environment.json` → Environment variables for the API.

#### 📌 How to import them into Postman:
1. Open **Postman**.  
2. Go to **File** > **Import**.  
3. Select the corresponding .json file (collection or environment).

> ℹ️ The collection uses the variable {{baseUrl}} for the API address.
Change its value in the environment if your port/localhost differs.

## 📑 Main Endpoints

### 👤 Users
| Método | Endpoint                                         | Descripción                              |
|--------|--------------------------------------------------|------------------------------------------|
| GET    | `/users`                                         | Get all users                            |
| GET    | `/users/{id}`                                    | Get a user by ID                         |
| GET    | `/users/{id}/tasks`                              | Get all tasks assigned to a user         |
| GET    | `/users/{id}/tasks/{taskId}`                     | Get a specific task assigned to a user   |
| POST   | `/users`                                         | Create a new user                        |
| PATCH  | `/users/{id}`                                    | Update user data                         |
| DELETE  | `/users/{id}`                                   | Delete a user                            |

### ✅ Tasks
| Método | Endpoint                                         | Descripción                              |
|--------|--------------------------------------------------|------------------------------------------|
| GET    | `/tasks`                                         | Get all tasks                            |
| GET    | `/tasks/{id}`                                    | Get a task by ID                         |
| POST   | `/tasks`                                         | Create a new task                        |
| PATCH  | `/tasks/{id}`                                    | Update task data                         |
| DELETE  | `/tasks/{id}`                                   | Delete a task                            |

> ℹ️ All endpoints accept and return data in JSON format.
The variable {{baseUrl}} in the Postman collection defines the base URL of the API.
