# 📚 ADProject

ADProject is a full-stack **ASP.NET Core 8.0** web application with **MySQL** as the backend database.  
It provides REST APIs for managing activities, channels, user accounts, and system messages.  
The project is containerized using Docker and supports deployment to **Azure App Service (Linux)**

You can locally run this application with cloud database,just need to change the appsettings.Development.json into 
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=adproject-database.mysql.database.azure.com;Database=adproject;User Id=huerji;Password=HuErJi123;SslMode=Required;"
  }
}

---

## ✨ Features
- **.NET 8 Web API** backend for activity and channel management.
- **Entity Framework Core** ORM for database access.
- **MySQL 8 integration** with automatic initialization scripts.
- **Docker multi-stage build** for optimized production images.
- **Azure App Service** ready for cloud deployment.
- **Integration & Unit Tests** for code quality assurance.
- **Machine Learning API** integration via `MLController`.

---

## 🛠 Tech Stack
| Layer            | Technology |
|------------------|------------|
| Backend          | ASP.NET Core 8.0 (C#) |
| Database         | MySQL 8 |
| ORM              | Entity Framework Core |
| Containerization | Docker |
| Cloud Deployment | Azure App Service (Linux) |
| Testing          | xUnit (Unit & Integration Tests) |

---

## 📂 Project Structure
```
ADProject_clouddatabase/
│── ADProject/                   # Main backend project
│   ├── Program.cs               # Entry point
│   ├── Controllers/             # API endpoints
│   │   ├── ActivityController.cs
│   │   ├── ChannelController.cs
│   │   ├── LoginController.cs
│   │   ├── MLController.cs
│   │   ├── SystemMessageController.cs
│   │   ├── TagController.cs
│   │   └── UserController.cs
│   ├── Models/                   # Entity models & DTOs
│   ├── Services/                 # AppDbContext & business logic
│   ├── appsettings.json          # Configuration
│── ADProject.UnitTests/          # Unit test project
│── ADProject.IntegrationTests/   # Integration test project
│── db/Add_new.sql                 # MySQL initialization script
│── docker/entrypoint.sh           # Startup script
│── Dockerfile                     # Multi-stage build config
│── ADProject.sln                  # Solution file
```

---

## 🚀 Getting Started

### 1️⃣ Prerequisites
- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [MySQL 8](https://dev.mysql.com/downloads/mysql/) (optional for local dev)

---

### 2️⃣ Local Development
```bash
# Restore dependencies
dotnet restore ADProject/ADProject.csproj

# Run database migrations (if applicable)
dotnet ef database update --project ADProject/ADProject.csproj

# Start the application
dotnet run --project ADProject/ADProject.csproj
```



### 3️⃣ Docker Deployment

#### Build Image
```bash
docker build -t adproject:latest .
```

#### Run Container
```bash
docker run -d -p 8080:8080 -p 2222:2222 adproject:latest
```
- API available at: `http://localhost:8080`

---

### 4️⃣ Azure Deployment
1. Push image to Docker Hub:
```bash
docker tag adproject:latest <dockerhub_username>/adproject:latest
docker push <dockerhub_username>/adproject:latest
```
2. Configure Azure App Service → **Deployment Center** → **Docker Hub**.

---

## 🔌 API Endpoints Overview
| Controller | Endpoint Example | Description |
|------------|------------------|-------------|
| ActivityController | `GET /activities` | Manage activities |
| ChannelController | `GET /channels` | Manage channels |
| LoginController | `POST /login` | User authentication |
| MLController | `POST /predictTags` | ML tag prediction |
| SystemMessageController | `GET /systemMessages` | Manage system notices |
| TagController | `GET /tags` | Manage tags |
| UserController | `GET /users` | User management |

---

## 🧪 Running Tests
```bash
# Unit tests
dotnet test ADProject.UnitTests/ADProject.UnitTests.csproj

# Integration tests
dotnet test ADProject.IntegrationTests/ADProject.IntegrationTests.csproj
```

