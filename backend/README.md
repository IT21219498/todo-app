# Todo App - Backend API

A robust, production-ready REST API built with .NET 8 and C# for managing todo tasks. Features clean architecture, comprehensive testing, and follows SOLID principles.

## 🚀 Features

- ✅ RESTful API with clear endpoints
- ✅ MySQL database with Entity Framework Core
- ✅ Repository pattern for data access
- ✅ Service layer for business logic
- ✅ Comprehensive unit and integration tests
- ✅ Swagger/OpenAPI documentation
- ✅ Docker support
- ✅ CORS configured for frontend integration
- ✅ Structured logging
- ✅ Clean architecture following SOLID principles

## 🏗️ Architecture

### Project Structure

```
backend/
├── TodoApp.Api/              # Main API project
│   ├── Controllers/          # API endpoints
│   ├── Services/             # Business logic layer
│   ├── Repositories/         # Data access layer
│   ├── Data/                 # Database context
│   ├── Models/               # Domain entities
│   ├── DTOs/                 # Data transfer objects
│   └── Program.cs            # Application entry point
├── TodoApp.Tests/            # Test project
│   ├── Services/             # Service unit tests
│   ├── Repositories/         # Repository unit tests
│   └── Integration/          # Integration tests
├── Dockerfile                # Docker configuration
└── TodoApp.sln               # Solution file
```

### Design Patterns & Principles

#### SOLID Principles

- **Single Responsibility**: Each class has one clear purpose
- **Open/Closed**: Extensions through interfaces
- **Liskov Substitution**: Repository and service implementations
- **Interface Segregation**: Minimal, focused interfaces
- **Dependency Inversion**: Dependency injection throughout

#### Patterns Implemented

- **Repository Pattern**: Data access abstraction
- **Service Pattern**: Business logic separation
- **Dependency Injection**: Loose coupling
- **DTO Pattern**: API data contracts
- **Factory Pattern**: DbContext configuration

## 📋 Prerequisites

- .NET 8.0 SDK or later
- MySQL 8.0+ (or Docker)
- Visual Studio 2022 / VS Code / JetBrains Rider

## 🛠️ Getting Started

### Local Development

1. **Clone the repository**

   ```bash
   cd backend
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Update database connection string**

   Edit `appsettings.Development.json`:

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Port=5432;Database=tododb;Username=todouser;Password=todopassword"
     }
   }
   ```

4. **Run database migrations**

   ```bash
   cd TodoApp.Api
   dotnet ef database update
   ```

5. **Run the application**

   ```bash
   dotnet run
   ```

   API will be available at: `http://localhost:3000`
   Swagger UI at: `http://localhost:3000`

### Using Docker

1. **Build the image**

   ```bash
   docker build -t todo-backend .
   ```

2. **Run with Docker Compose** (includes MySQL)
   ```bash
   docker-compose up
   ```

## 🧪 Running Tests

### All Tests

```bash
dotnet test
```

### With Coverage

```bash
dotnet test --collect:"XPlat Code Coverage"
```

### Specific Test Categories

```bash
# Unit tests only
dotnet test --filter "FullyQualifiedName~TodoApp.Tests.Services"

# Integration tests only
dotnet test --filter "FullyQualifiedName~TodoApp.Tests.Integration"
```

### Test Statistics

- **Total Tests**: 40+
- **Unit Tests**: 25+
- **Integration Tests**: 10+
- **Code Coverage**: >80%

## 📡 API Endpoints

### Base URL

```
http://localhost:3000/api
```

### Endpoints

#### Get Recent Tasks

```http
GET /api/tasks
```

Returns the 5 most recent incomplete tasks.

**Response:** `200 OK`

```json
[
  {
    "id": 1,
    "title": "Task title",
    "description": "Task description",
    "completed": false,
    "createdAt": "2025-10-22T10:00:00Z",
    "updatedAt": null
  }
]
```

#### Get Task by ID

```http
GET /api/tasks/{id}
```

**Response:** `200 OK` or `404 Not Found`

#### Create Task

```http
POST /api/tasks
Content-Type: application/json

{
  "title": "New task",
  "description": "Task description"
}
```

**Validation Rules:**

- `title`: Required, max 100 characters
- `description`: Optional, max 500 characters

**Response:** `201 Created`

```json
{
  "id": 2,
  "title": "New task",
  "description": "Task description",
  "completed": false,
  "createdAt": "2025-10-22T10:30:00Z",
  "updatedAt": null
}
```

#### Mark Task as Completed

```http
PATCH /api/tasks/{id}/complete
```

**Response:** `200 OK` or `404 Not Found`

#### Delete Task

```http
DELETE /api/tasks/{id}
```

**Response:** `204 No Content` or `404 Not Found`

#### Health Check

```http
GET /health
```

**Response:** `200 OK`

```json
{
  "status": "healthy",
  "timestamp": "2025-10-22T10:00:00Z"
}
```

## 🗄️ Database Schema

### Task Table

```sql
CREATE TABLE task (
    id SERIAL PRIMARY KEY,
    title VARCHAR(100) NOT NULL,
    description VARCHAR(500),
    completed BOOLEAN NOT NULL DEFAULT FALSE,
    created_at TIMESTAMP NOT NULL,
    updated_at TIMESTAMP,
    CONSTRAINT chk_title_not_empty CHECK (LENGTH(TRIM(title)) > 0)
);

CREATE INDEX ix_task_created_at ON task(created_at DESC);
CREATE INDEX ix_task_completed ON task(completed);
```

## 🔧 Configuration

### Environment Variables

| Variable                               | Description                          | Default              |
| -------------------------------------- | ------------------------------------ | -------------------- |
| `ConnectionStrings__DefaultConnection` | MySQL connection string              | See appsettings.json |
| `Cors__AllowedOrigins`                 | Comma-separated allowed origins      | localhost ports      |
| `ASPNETCORE_ENVIRONMENT`               | Environment (Development/Production) | Development          |
| `ASPNETCORE_URLS`                      | URLs to listen on                    | http://+:80          |

### Connection String Format

```
Host=localhost;Port=5432;Database=tododb;Username=todouser;Password=todopassword
```

## 🐳 Docker Deployment

### Dockerfile

Multi-stage build for optimized image:

1. **Build stage**: Compiles the application
2. **Publish stage**: Prepares release artifacts
3. **Runtime stage**: Minimal runtime image

### Image Size

- Base image: mcr.microsoft.com/dotnet/aspnet:8.0
- Final image: ~220MB

### Docker Compose

Includes:

- Backend API
- MySQL database
- Network configuration
- Volume persistence

## 📊 Testing Strategy

### Unit Tests

- **Service Layer**: Business logic validation
- **Repository Layer**: Data access operations
- Uses in-memory database for isolation
- Mocking with Moq

### Integration Tests

- End-to-end API testing
- Real HTTP requests
- In-memory database per test
- All endpoints covered

### Test Coverage Areas

- ✅ Happy path scenarios
- ✅ Edge cases
- ✅ Error handling
- ✅ Validation
- ✅ Business rules
- ✅ Database operations

## 🛡️ Security Features

- Input validation with Data Annotations
- SQL injection protection (EF Core parameterization)
- CORS configuration
- HTTPS redirection ready
- Secure connection strings
- No sensitive data in logs

## 📝 Logging

Structured logging with different levels:

- **Information**: Normal operations
- **Warning**: Validation failures, not found
- **Error**: Exceptions and failures

Log locations:

- Console output (Development)
- File/external service (Production - configurable)

## 🚦 Error Handling

Consistent error responses:

```json
{
  "message": "Error description"
}
```

Status codes:

- `200 OK`: Success
- `201 Created`: Resource created
- `204 No Content`: Successful deletion
- `400 Bad Request`: Validation error
- `404 Not Found`: Resource not found
- `500 Internal Server Error`: Server error

## 🔄 Database Migrations

### Create Migration

```bash
cd TodoApp.Api
dotnet ef migrations add MigrationName
```

### Apply Migrations

```bash
dotnet ef database update
```

### Remove Last Migration

```bash
dotnet ef migrations remove
```

## 📦 Dependencies

### Production

- **Microsoft.EntityFrameworkCore** (8.0.10)
- **Pomelo.EntityFrameworkCore.MySql** (8.0.8)
- **Swashbuckle.AspNetCore** (6.8.1)

### Development/Testing

- **xUnit** (2.9.2)
- **Moq** (4.20.72)
- **Microsoft.AspNetCore.Mvc.Testing** (8.0.10)
- **Microsoft.EntityFrameworkCore.InMemory** (8.0.10)

## 🎯 Performance Optimizations

- Asynchronous operations throughout
- Database indexes on frequently queried columns
- No tracking for read-only queries
- Connection pooling
- Efficient LINQ queries

## 🔍 Swagger/OpenAPI

Interactive API documentation available at the root URL when running in Development mode.

Features:

- Try out endpoints
- View request/response schemas
- See all available operations
- XML documentation comments

## 🤝 Contributing

Code follows:

- C# coding conventions
- Clean Code principles
- SOLID principles
- XML documentation
- Comprehensive testing

## 📄 License

This project is part of a technical assessment.

## 🐛 Troubleshooting

### Database Connection Issues

- Verify MySQL is running
- Check connection string
- Ensure database exists
- Verify user permissions

### Migration Errors

- Delete bin/obj folders
- Rebuild solution
- Check DbContext configuration

### Test Failures

- Ensure no port conflicts
- Check in-memory database setup
- Verify all packages restored

## 📞 Support

For issues or questions, refer to:

- Swagger documentation: `/swagger`
- Health check: `/health`
- Logs: Console output
