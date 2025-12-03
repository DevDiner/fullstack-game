# Database Setup Guide - SQLite Edition

Complete guide to setting up the SQLite database for Sudoku API.

---

##  **Why SQLite?**

✅ **Zero Configuration** - No database server installation needed  
✅ **Single File** - Database stored in one `.db` file  
✅ **Cross-Platform** - Works on Windows, Linux, Mac  
✅ **Perfect for Demos** - Anyone can run it immediately  
✅ **Production Ready** - Used by billions of devices  
✅ **Still SQL** - Full relational database with ACID compliance  

---

##  **Quick Setup**

### Super Easy!

```powershell
cd SudokuAPI

# Restore dependencies (includes SQLite)
dotnet restore

# Create database
dotnet ef migrations add InitialCreate
dotnet ef database update

# Run the API
dotnet run
```

**That's it!** The database file `SudokuGame.db` will be created automatically.

---

## **EF Core Commands**

### Create Initial Migration
```powershell
cd SudokuAPI
dotnet ef migrations add InitialCreate
```

### Apply Migrations (Create Database)
```powershell
dotnet ef database update
```

**Output:**
```
Build succeeded.
Building database...
Done.
```

**Check for database file:**
```powershell
ls *.db
```

You should see: `SudokuGame.db`

### Reset Database
```powershell
# Delete the database file
rm SudokuGame.db

# Recreate it
dotnet ef database update
```

### View Migrations
```powershell
dotnet ef migrations list
```

---

## **Verify Database**

### Using DB Browser for SQLite (Recommended)

**1. Download:**
- https://sqlitebrowser.org/dl/

**2. Open Database:**
- File → Open Database
- Select `SudokuGame.db`

**3. View Tables:**
- Browse Data tab
- Select table: Users, Players, Puzzles, Sessions

### Using Command Line (sqlite3)

**Install sqlite3:**
```powershell
# Windows (via winget)
winget install --id=SQLite.SQLite -e

# Or download from: https://sqlite.org/download.html
```

**Query Database:**
```powershell
sqlite3 SudokuGame.db

# Inside sqlite3:
.tables                  # List all tables
SELECT * FROM Users;     # View users
SELECT * FROM Puzzles;   # View puzzles
.exit                    # Exit
```

---

## **Database Schema**

### Tables

**Users** (Authentication)
```sql
CREATE TABLE Users (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    PasswordHash TEXT NOT NULL,
    Role TEXT NOT NULL,
    CreatedAt TEXT NOT NULL,
    LastLogin TEXT NULL
);
```

**Puzzles** (Sudoku Puzzles)
```sql
CREATE TABLE Puzzles (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Name TEXT NOT NULL,
    InitialGrid TEXT NOT NULL,
    SolutionGrid TEXT NOT NULL,
    Difficulty INTEGER NOT NULL,
    EmptyCells INTEGER NOT NULL,
    CreatedDate TEXT NOT NULL,
    TimesPlayed INTEGER NOT NULL DEFAULT 0,
    TimesCompleted INTEGER NOT NULL DEFAULT 0,
    BestTimeSeconds INTEGER NULL,
    AverageTimeSeconds REAL NULL
);
```

**Players** (Player Profiles)
```sql
CREATE TABLE Players (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Username TEXT NOT NULL UNIQUE,
    Email TEXT NULL UNIQUE,
    TotalGamesPlayed INTEGER NOT NULL DEFAULT 0,
    TotalGamesCompleted INTEGER NOT NULL DEFAULT 0,
    -- ... (additional stats fields)
);
```

**Sessions** (Game Sessions)
```sql
CREATE TABLE Sessions (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    PlayerId INTEGER NOT NULL,
    PuzzleId INTEGER NOT NULL,
    CurrentGrid TEXT NOT NULL,
    StartTime TEXT NOT NULL,
    ElapsedSeconds INTEGER NOT NULL DEFAULT 0,
    IsCompleted INTEGER NOT NULL DEFAULT 0,
    -- Foreign keys for PlayerId and PuzzleId
);
```

---

##  **Seed Data**

The database will be automatically seeded with:

**Puzzles:**
- Easy Puzzle 1
- Medium Puzzle 1

**Users:**
- Username: `admin`
- Password: `Admin@123`
- Role: `Admin`

---

##  **Advantages Over SQL Server**

| Feature | SQLite | SQL Server LocalDB |
|---------|--------|-------------------|
| **Installation** | None needed | Requires installation |
| **File Size** | Single .db file | Multiple files |
| **Portability** | Copy one file | Complex backup |
| **Cross-Platform** | Windows/Linux/Mac | Windows only |
| **Demo Ready** | Yes! | Requires setup |
| **Learning Curve** | Minimal | More complex |

---

##  **Troubleshooting**

### Error: "No such table: Users"
**Solution:** Run migrations:
```powershell
dotnet ef database update
```

### Error: "Database is locked"
**Solution:** Close DB Browser or any tools viewing the database

### Error: "Unable to open database file"
**Solution:** Ensure you're in the SudokuAPI directory

### Database file missing
**Solution:** It will be created on first run or when you run `dotnet ef database update`

---

##  **Connection String**

**In `appsettings.json`:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=SudokuGame.db"
  }
}
```

**That's all you need!** No server, no port, no credentials.

---

## **Test Database**

### 1. Register a user via API:
```http
POST /api/auth/register
{
  "username": "testuser",
  "password": "Test@123",
  "confirmPassword": "Test@123"
}
```

### 2. Check SQLite database:
```powershell
sqlite3 SudokuGame.db "SELECT * FROM Users WHERE Username='testuser';"
```

### 3. Verify in Swagger:
```
http://localhost:5000/swagger
```

---

## **Database File Location**

```
SudokuAPI/
├── SudokuGame.db        ← Your database!
├── SudokuGame.db-shm    ← Shared memory file (temporary)
└── SudokuGame.db-wal    ← Write-Ahead Log (temporary)
```

**Only `SudokuGame.db` is needed.**

---

##  **Deployment**

### Include Database in Git? 

**For Demo:** ✅ Yes (include seed data)
```powershell
# Remove from .gitignore
git add SudokuAPI/SudokuGame.db
```

**For Production:** ❌ No (let it be created)
- Already in .gitignore
- Will be created on first run

### Backup Database

**Simple backup:**
```powershell
copy SudokuAPI\SudokuGame.db SudokuAPI\SudokuGame-backup.db
```

**Restore:**
```powershell
copy SudokuAPI\SudokuGame-backup.db SudokuAPI\SudokuGame.db
```

---

##  **Production Usage**

SQLite is production-ready for:
- ✅ Read-heavy applications
- ✅ Low to medium traffic
- ✅ Single-server deployments
- ✅ Mobile/Desktop apps
- ✅ Embedded systems

SQLite powers:
- Every iPhone and Android device
- Most web browsers
- Thousands of major applications

**It's a real, production database!**

---

##  **Migration to Other Databases**

If you later need SQL Server or PostgreSQL:

**1. Change NuGet Package:**
```xml
<!-- From -->
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="8.0.0" />

<!-- To -->
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
```

**2. Update Connection String**

**3. Update Program.cs:**
```csharp
// From
options.UseSqlite(connectionString)

// To
options.UseSqlServer(connectionString)
```

**Your code stays the same!** That's the power of Entity Framework.

---

## **Verification Checklist**

- [ ] .NET SDK installed
- [ ] EF Core tools installed (`dotnet ef`)
- [ ] Migrations created
- [ ] Database file exists (`SudokuGame.db`)
- [ ] Seed data present (check with DB Browser)
- [ ] API can connect to database
- [ ] Swagger UI accessible

---

##  **Further Reading**

- [SQLite Official Docs](https://sqlite.org/docs.html)
- [When to Use SQLite](https://sqlite.org/whentouse.html)
- [EF Core with SQLite](https://docs.microsoft.com/ef/core/providers/sqlite)
- [DB Browser for SQLite](https://sqlitebrowser.org/)

---


**Ready to run?**
```powershell
cd SudokuAPI
dotnet ef database update
dotnet run
```

Visit: http://localhost:5000/swagger 


