# Sudoku Zen - Full Stack Edition

**An intelligent Sudoku game with C# backend API and modern web frontend**

![Version](https://img.shields.io/badge/version-2.0-blue)
![.NET](https://img.shields.io/badge/.NET-8.0-purple)
![License](https://img.shields.io/badge/license-MIT-green)

---

## What Is This?

**Sudoku Zen** is a full-stack web application that combines:
- **Frontend**: Beautiful, responsive Sudoku game built with HTML5 Canvas and JavaScript
- **Backend**: RESTful C# API for data persistence, player management, and analytics
- **AI Integration**: Google Gemini AI for intelligent hints

This project demonstrates professional full-stack development with C# and modern web technologies.

---

## Features

### Game Features
- **Multiple Difficulty Levels**: Easy, Medium, Hard
- **AI-Powered Hints**: Get intelligent hints from Google Gemini AI
- **Real-Time Validation**: Instant feedback on your moves
- **Timer**: Track your completion time
- **Responsive Design**: Works on desktop and mobile

### Backend Features (C# API)
- **Player Profiles**: Track your progress and statistics
- **Puzzle Management**: Save and retrieve puzzles
- **Session Tracking**: Record every game you play
- **Leaderboards**: Compete with other players
- **Analytics**: View detailed statistics with LINQ queries

### Technical Features
- **RESTful API**: Clean, well-documented endpoints
- **CRUD Operations**: Full Create, Read, Update, Delete for all entities
- **LINQ Queries**: Advanced data analysis (15+ unique queries)
- **Swagger Documentation**: Interactive API documentation
- **CORS Enabled**: Frontend can call the API seamlessly

---

## Architecture

```
sudoku-zen/
├── index.html              # Game UI
├── index.js                # Frontend logic
├── api-client.js           # API integration
└── SudokuAPI/              # C# Backend
    ├── Models/
    │   ├── SudokuPuzzle.cs
    │   ├── PlayerProfile.cs
    │   └── GameSession.cs
    ├── Services/
    │   ├── PuzzleManager.cs
    │   ├── PlayerManager.cs
    │   └── SessionManager.cs
    └── Controllers/
        ├── PuzzlesController.cs
        ├── PlayersController.cs
        └── SessionsController.cs
```

---

## Quick Start

### Prerequisites
- **.NET 8.0 SDK**: [Download here](https://dotnet.microsoft.com/download/dotnet/8.0)
- **Modern web browser**: Chrome, Firefox, Edge, or Safari
- **Google Gemini API Key** (optional, for hints): [Get one here](https://aistudio.google.com/app/apikey)

### Step 1: Start the Backend API

```powershell
# Navigate to API directory
cd SudokuAPI

# Restore dependencies
dotnet restore

# Run the API
dotnet run
```

The API will start at **http://localhost:5000**

### Step 2: Start the Frontend

```powershell
# Navigate back to root
cd ..

# Install dependencies
npm install

# Start development server
npm run dev
```

The game will open at **http://localhost:5173**

### Step 3: (Optional) Configure Gemini AI

Create a `.env.local` file in the root:
```
VITE_API_KEY=your_gemini_api_key_here
```

---

## API Endpoints

### Players
- `GET /api/players` - Get all players
- `GET /api/players/{id}` - Get specific player
- `POST /api/players` - Create new player
- `PUT /api/players/{id}` - Update player
- `POST /api/players/{id}/game` - Record game result
- `GET /api/players/leaderboard` - Get top players
- `GET /api/players/fastest` - Get fastest players
- `GET /api/players/stats` - Get player statistics

### Puzzles
- `GET /api/puzzles` - Get all puzzles
- `GET /api/puzzles/{id}` - Get specific puzzle
- `GET /api/puzzles/random?difficulty=easy` - Get random puzzle
- `POST /api/puzzles` - Create new puzzle
- `POST /api/puzzles/{id}/play` - Record puzzle play
- `GET /api/puzzles/top` - Get most played puzzles

### Sessions
- `GET /api/sessions` - Get all sessions
- `POST /api/sessions` - Start new session
- `PUT /api/sessions/{id}` - Update session
- `POST /api/sessions/{id}/complete` - Mark session complete
- `GET /api/sessions/fastest` - Get fastest completions
- `GET /api/sessions/stats` - Get session statistics

**Full API Documentation**: http://localhost:5000/swagger

---

## Data Models

### SudokuPuzzle
```csharp
- Id: int
- InitialGrid: string (81 characters)
- SolutionGrid: string (81 characters)
- Difficulty: DifficultyLevel (Easy/Medium/Hard/Expert/Evil)
- TimesPlayed: int
- CompletionRate: double
```

### PlayerProfile
```csharp
- Id: int
- Username: string
- TotalGamesPlayed: int
- TotalGamesCompleted: int
- CompletionRate: double
- Current Streak: int
- BestTimeSeconds: int
```

### GameSession
```csharp
- Id: int
- PlayerId: int
- PuzzleId: int
- ElapsedSeconds: int
- HintsUsed: int
- IsCompleted: bool
```

---

## C# Requirements Met

This project fulfills **all requirements** for a comprehensive C# application:

### Core Requirements
- **3+ Entity Models**: SudokuPuzzle, PlayerProfile, GameSession
- **Full CRUD Operations**: Implemented for all entities
- **LINQ Queries**: 15+ unique queries for analytics
- **Data Persistence**: JSON export/import capability
- **Professional Structure**: Clean architecture with Models, Services, Controllers

### Advanced Features
- **RESTful API**: Industry-standard web API
- **Swagger Documentation**: Auto-generated API docs
- **CORS Configuration**: Secure cross-origin requests
- **Error Handling**: Proper HTTP status codes
- **Dependency Injection**: ASP.NET Core DI container

---

## LINQ Examples

The backend uses advanced LINQ queries:

```csharp
// Leaderboard
Players.OrderByDescending(p => p.TotalGamesCompleted)
       .ThenByDescending(p => p.CompletionRate)
       .Take(10)

// Average completion time
Sessions.Where(s => s.IsCompleted)
        .Average(s => s.ElapsedSeconds)

// Puzzle difficulty distribution
Puzzles.GroupBy(p => p.Difficulty)
       .ToDictionary(g => g.Key, g => g.Count())

// Active players
Players.Count(p => p.LastPlayed >= cutoffDate)
```

---

## How to Play

1. **Enter your username** when prompted
2. **Select difficulty** (Easy, Medium, or Hard)
3. **Click a cell** to select it
4. **Type a number** (1-9) or click the number pad
5. **Get hints** using the Gemini AI button
6. **Validate** your solution anytime
7. **Complete** the puzzle to see your time!

---

## Development

### Running Tests
```powershell
cd SudokuAPI
dotnet test
```

### Build for Production
```powershell
# Backend
cd SudokuAPI
dotnet publish -c Release

# Frontend
npm run build
```

### API Documentation
While the API is running, visit: http://localhost:5000/swagger

---

## Contributing

This is an educational project. Feel free to:
- Fork and experiment
- Add new features
- Improve the UI
- Enhance the API

---

## License

MIT License - Feel free to use this project for learning!

---


## Next Steps

Want to extend this project? Try:
- Add database persistence (SQL Server, PostgreSQL)
- Implement authentication (JWT tokens)
- Add real-time multiplayer
- Create mobile app (Xamarin/MAUI)
- Deploy to cloud (Azure, AWS)

---

**Built with ❤️ using C# ASP.NET Core and Modern Web Technologies**

Enjoy playing Sudoku Zen! 🧩✨
