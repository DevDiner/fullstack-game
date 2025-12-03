#  Video Game Backend API

[![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-12.0-239120?logo=csharp&logoColor=white)](https://docs.microsoft.com/dotnet/csharp/)
[![Entity Framework](https://img.shields.io/badge/Entity_Framework-Core_8.0-512BD4)](https://docs.microsoft.com/ef/core/)
[![SQLite](https://img.shields.io/badge/SQLite-3-003B57?logo=sqlite)](https://sqlite.org/)
[![JWT](https://img.shields.io/badge/JWT-Authentication-000000?logo=json-web-tokens)](https://jwt.io/)
[![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?logo=swagger)](https://swagger.io/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)

> **A production-ready RESTful API for video game data management featuring Entity Framework Core, SQLite database, JWT authentication, and comprehensive CRUD operations.**

Built as a backend system for video games, this API demonstrates professional ASP.NET Core development with secure authentication, database design, and scalable architecture. Uses SQLite for zero-configuration, cross-platform database deployment.

---

##  **Features**

###  **Security & Authentication**
- **JWT Bearer Authentication** - Secure token-based auth
- **Password Hashing** - PBKDF2 with 100,000 iterations
- **Role-Based Authorization** - Admin and Player roles
- **Protected Endpoints** - Secure access control

### **Database & ORM**
- **SQL Server** - Relational database
- **Entity Framework Core 8.0** - Modern ORM
- **Code-First Migrations** - Database versioning
- **Optimized Queries** - Indexed fields and relationships
- **Seed Data** - Pre-populated sample data

### **RESTful API**
- **25+ Endpoints** - Complete CRUD operations
- **Standard HTTP Methods** - GET, POST, PUT, DELETE
- **Proper Status Codes** - 200, 201, 400, 401, 404
- **JSON Responses** - Consistent data format
- **CORS Enabled** - Cross-origin support

### **Game Data Management**
- **Puzzles** - Sudoku puzzle library
- **Players** - User profiles and statistics
- **Sessions** - Game play tracking
- **Analytics** - LINQ-based data analysis
- **Leaderboards** - Competitive rankings

###  **Documentation**
- **Swagger UI** - Interactive API documentation
- **OpenAPI Spec** - Standard API definition
- **Setup Guides** - Complete installation docs
- **Testing Guide** - Comprehensive test scenarios

---

##  **Quick Start**

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- That's it! SQLite is included automatically

### Installation

```bash
# Clone the repository
git clone https://github.com/YOUR_USERNAME/video-game-backend-api.git
cd video-game-backend-api

# Navigate to API directory
cd SudokuAPI

# Restore dependencies
dotnet restore

# Install EF Core tools
dotnet tool install --global dotnet-ef

# Create database
dotnet ef migrations add InitialCreate
dotnet ef database update

# Run the API
dotnet run
```

### Access

- **API**: http://localhost:5000
- **Swagger UI**: http://localhost:5000/swagger

---

## **API Endpoints**

### Authentication
```http
POST   /api/auth/register      # Register new user
POST   /api/auth/login         # Login and get JWT token
GET    /api/auth/me            # Get current user info (protected)
```

### Puzzles
```http
GET    /api/puzzles                    # Get all puzzles
GET    /api/puzzles/{id}               # Get specific puzzle
GET    /api/puzzles/random             # Get random puzzle
GET    /api/puzzles/difficulty/{level} # Get by difficulty
POST   /api/puzzles                    # Create puzzle (protected)
PUT    /api/puzzles/{id}               # Update puzzle (protected)
DELETE /api/puzzles/{id}               # Delete puzzle (protected)
```

### Players
```http
GET    /api/players                  # Get all players
GET    /api/players/{id}             # Get specific player
GET    /api/players/leaderboard      # Get top players
GET    /api/players/fastest          # Get fastest players
POST   /api/players                  # Create player
PUT    /api/players/{id}             # Update player
DELETE /api/players/{id}             # Delete player
```

### Sessions
```http
GET    /api/sessions              # Get all sessions
GET    /api/sessions/{id}         # Get specific session
POST   /api/sessions              # Start new session
PUT    /api/sessions/{id}         # Update session
POST   /api/sessions/{id}/complete # Mark session complete
```

**Full API Documentation**: Available at `/swagger` when running

---

## **Database Schema**

### Entity Relationship Diagram

```
┌─────────────┐       ┌──────────────┐       ┌─────────────┐
│    Users    │       │   Players    │       │   Puzzles   │
├─────────────┤       ├──────────────┤       ├─────────────┤
│ Id (PK)     │       │ Id (PK)      │       │ Id (PK)     │
│ Username    │       │ Username     │       │ Name        │
│ PasswordHash│       │ Email        │       │ InitialGrid │
│ Role        │       │ TotalGames   │       │ SolutionGrid│
│ CreatedAt   │       │ BestTime     │       │ Difficulty  │
└─────────────┘       │ CurrentStreak│       │ TimesPlayed │
                      └──────────────┘       └─────────────┘
                             │                      │
                             └──────┬───────────────┘
                                    │
                             ┌──────▼──────┐
                             │  Sessions   │
                             ├─────────────┤
                             │ Id (PK)     │
                             │ PlayerId(FK)│
                             │ PuzzleId(FK)│
                             │ CurrentGrid │
                             │ ElapsedTime │
                             │ IsCompleted │
                             └─────────────┘
```

### Tables

- **Users** - Authentication and user accounts
- **Players** - Player profiles and statistics
- **Puzzles** - Game puzzle library
- **Sessions** - Individual game sessions

---

## **Authentication Flow**

### 1. Register
```bash
curl -X POST http://localhost:5000/api/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "username": "player1",
    "password": "SecurePass@123",
    "confirmPassword": "SecurePass@123"
  }'
```

**Response:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "player1",
  "role": "Player",
  "expiresAt": "2025-12-03T18:00:00Z"
}
```

### 2. Use Token
```bash
curl http://localhost:5000/api/auth/me \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

### 3. Swagger Authentication
1. Click the **Authorize** 🔒 button
2. Enter: `Bearer YOUR_JWT_TOKEN`
3. Click **Authorize**
4. Now you can test protected endpoints!

---

## **Testing**

### Using Swagger UI
1. Navigate to http://localhost:5000/swagger
2. Register a new user via `/api/auth/register`
3. Copy the JWT token from response
4. Click **Authorize** and paste token
5. Test any endpoint!

### Using curl
```bash
# Get all puzzles
curl http://localhost:5000/api/puzzles

# Create a puzzle (requires authentication)
curl -X POST http://localhost:5000/api/puzzles \
  -H "Authorization: Bearer YOUR_TOKEN" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Puzzle",
    "difficulty": "Medium",
    ...
  }'
```

### Using Postman
Import the [API collection](docs/postman_collection.json) and environment.

---

## **Project Structure**

```
SudokuAPI/
├── Controllers/         # API endpoint controllers
│   ├── AuthController.cs
│   ├── PuzzlesController.cs
│   ├── PlayersController.cs
│   └── SessionsController.cs
│
├── Models/              # Data models
│   ├── SudokuPuzzle.cs
│   ├── PlayerProfile.cs
│   ├── GameSession.cs
│   └── User.cs
│
├── Services/            # Business logic
│   ├── AuthService.cs
│   ├── PuzzleManager.cs
│   ├── PlayerManager.cs
│   └── SessionManager.cs
│
├── Data/                # Database context
│   └── SudokuDbContext.cs
│
├── Program.cs           # Application entry point
├── appsettings.json     # Configuration
└── SudokuAPI.csproj     # Project file
```

---

##  **Technologies Used**

| Technology | Purpose |
|------------|---------|
| **ASP.NET Core 8.0** | Web API framework |
| **Entity Framework Core** | ORM for database access |
| **SQL Server** | Relational database |
| **JWT** | Secure authentication |
| **Swagger/OpenAPI** | API documentation |
| **LINQ** | Data queries and analytics |
| **PBKDF2** | Password hashing |

---

## **Requirements Met**

This project demonstrates:

✅ **Database-Backed Storage** - SQL Server with normalized schema  
✅ **RESTful Endpoints** - 25+ well-structured endpoints  
✅ **Full CRUD Operations** - Complete Create, Read, Update, Delete  
✅ **JSON Serialization** - All responses in JSON format  
✅ **JWT Authentication** - Secure token-based auth  
✅ **Data Validation** - Comprehensive input validation  
✅ **Entity Framework** - Complete ORM integration  
✅ **Migrations** - Database version control  
✅ **Role-Based Auth** - Admin and Player roles  

See [REQUIREMENTS_FULFILLMENT.md](SudokuAPI/REQUIREMENTS_FULFILLMENT.md) for detailed verification.

---

## **Documentation**

- **[Installation Guide](INSTALLATION.md)** - Complete setup instructions
- **[Database Setup](SudokuAPI/DATABASE_SETUP.md)** - Database configuration
- **[API Testing Guide](SudokuAPI/API_TESTING_GUIDE.md)** - How to test endpoints
- **[Quick Start](QUICK_START.md)** - Quick reference guide

---

## **Learning Outcomes**

This project showcases proficiency in:

- **Database Design** - Relational schema with proper normalization
- **Entity Framework** - Code-first approach with migrations
- **Web API Development** - RESTful architecture and best practices
- **Authentication** - JWT implementation and password security
- **Authorization** - Role-based access control
- **LINQ** - Advanced query operations
- **Dependency Injection** - Modern ASP.NET Core patterns
- **API Documentation** - Swagger/OpenAPI integration

---

## **Security Features**

- **PBKDF2 Password Hashing** - 100,000 iterations with unique salts
- **JWT Tokens** - Secure, stateless authentication
- **HTTPS Support** - Encrypted communication
- **CORS Policy** - Controlled cross-origin access
- **Input Validation** - Protection against invalid data
- **SQL Injection Protection** - Parameterized queries via EF Core

---

## **Deployment**

### Azure App Service
```bash
# Publish the application
dotnet publish -c Release -o ./publish

# Deploy to Azure
az webapp up --name your-app-name --resource-group your-rg
```

### Docker
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY publish/ .
ENTRYPOINT ["dotnet", "SudokuAPI.dll"]
```

---

##  **Contributing**

Contributions welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

---

## **License**

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙏 **Acknowledgments**

- Built with [ASP.NET Core](https://dotnet.microsoft.com/apps/aspnet)
- Database powered by [Entity Framework Core](https://docs.microsoft.com/ef/core/)
- API documentation by [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)

---

