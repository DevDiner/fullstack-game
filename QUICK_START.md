# ⚡ Quick Start Guide - Sudoku Zen

**Get up and running in 2 minutes!**

---

## 🚀 One-Command Start

```powershell
.\start.bat
```

This launches:
- ✅ Backend API on http://localhost:5000
- ✅ Frontend game on http://localhost:3000

---

## 📋 Prerequisites Checklist

Before running, ensure you have:

- [ ] **.NET 8.0 SDK** installed
  - Check: `dotnet --version`
  - Download: https://dotnet.microsoft.com/download/dotnet/8.0

- [ ] **Node.js** installed  
  - Check: `node --version`
  - Download: https://nodejs.org/

- [ ] **Dependencies installed**
  - Run: `npm install` (in root folder)

---

## 🎮 Manual Start (Alternative)

### Terminal 1 - Backend API
```powershell
cd SudokuAPI
dotnet run
```
✅ API running at http://localhost:5000

### Terminal 2 - Frontend
```powershell
npm run dev
```
✅ Game running at http://localhost:3000

---

## 🌐 URLs to Know

| Service | URL | Purpose |
|---------|-----|---------|
| **Game** | http://localhost:3000 | Play Sudoku |
| **API** | http://localhost:5000 | Backend endpoints |
| **API Docs** | http://localhost:5000/swagger | Test API |

---

## 🎯 First Time Setup

```powershell
# 1. Install dependencies
npm install

# 2. Restore .NET packages
cd SudokuAPI
dotnet restore

# 3. Start everything
cd ..
.\start.bat
```

---

## 💡 Common Commands

### Development
```powershell
# Start with hot reload (backend)
cd SudokuAPI
dotnet watch run

# Start frontend dev server
npm run dev

# Install new npm package
npm install <package-name>
```

### Building
```powershell
# Build frontend for production
npm run build

# Build backend for production
cd SudokuAPI
dotnet publish -c Release
```

### Testing API
```powershell
# Example: Get all players
curl http://localhost:5000/api/players

# Example: Create a player
curl -X POST http://localhost:5000/api/players ^
  -H "Content-Type: application/json" ^
  -d "{\"username\":\"TestPlayer\"}"
```

---

## 🔧 Troubleshooting

### "dotnet command not found"
→ Install .NET SDK: https://dotnet.microsoft.com/download/dotnet/8.0

### "npm command not found"
→ Install Node.js: https://nodejs.org/

### Port already in use
Change ports in:
- Backend: `SudokuAPI/appsettings.json` → "Urls"
- Frontend: Check terminal output for alternate port

### CORS errors
Ensure backend is running before starting frontend

### Frontend can't connect to API
Check `api-client.js` has correct URL:
```javascript
const API_BASE_URL = 'http://localhost:5000/api';
```

---

## 📊 API Quick Reference

### Players
```http
GET    /api/players              # List all
GET    /api/players/{id}         # Get one
POST   /api/players              # Create
GET    /api/players/leaderboard  # Top 10
```

### Puzzles
```http
GET    /api/puzzles              # List all
GET    /api/puzzles/random       # Random puzzle
POST   /api/puzzles              # Create
POST   /api/puzzles/{id}/play    # Record play
```

### Sessions
```http
GET    /api/sessions             # List all
POST   /api/sessions             # Start game
PUT    /api/sessions/{id}        # Update
POST   /api/sessions/{id}/complete # Finish
```

**Full API docs**: http://localhost:5000/swagger

---

## 🎮 How to Play

1. Open http://localhost:3000
2. Enter your username
3. Select difficulty (Easy/Medium/Hard)
4. Click a cell, type a number (1-9)
5. Click "Get Hint" for AI assistance (requires Gemini API key)
6. Click "Validate" to check for errors
7. Complete the puzzle to see your time!

---

## 🔑 Optional: Gemini AI Setup

For AI-powered hints:

1. Get API key: https://aistudio.google.com/app/apikey
2. Create `.env.local` in root folder:
   ```
   VITE_API_KEY=your_key_here
   ```
3. Restart frontend: `npm run dev`

---

## 📁 Key Files

| File | Purpose |
|------|---------|
| `index.html` | Game UI |
| `index.js` | Game logic |
| `api-client.js` | API integration |
| `SudokuAPI/Program.cs` | API startup |
| `start.bat` | Launch script |

---

## 🎓 Learning Path

**New to the project?** Read in this order:

1. **README.md** - Overview and features
2. **INSTALLATION.md** - Detailed setup
3. **This file** - Quick commands
4. **PROJECT_STRUCTURE.md** - File organization
5. **Swagger UI** - API exploration

---

## ✅ Health Check

Verify everything works:

```powershell
# 1. Check .NET
dotnet --version
# Should show: 8.0.x

# 2. Check Node
node --version
# Should show: v18.x or higher

# 3. Build backend
cd SudokuAPI
dotnet build
# Should succeed with 0 errors

# 4. Build frontend
cd ..
npm run build
# Should create dist/ folder

# 5. Start and test
.\start.bat
# Visit http://localhost:3000
```

---

## 🚦 Status Indicators

**Backend Running:**
```
✓ Sudoku API is running!
📍 API URL: http://localhost:5000
```

**Frontend Running:**
```
VITE ready in XXXms
➜ Local: http://localhost:3000/
```

---

## 💾 Data Persistence

**Current**: In-memory (resets on restart)

**To save data**:
1. Play some games
2. API stores in memory
3. Data persists until server restart

**Future**: Add database for permanent storage

---

## 🎯 Next Steps

After getting it running:

1. ✅ Play a game to test
2. ✅ Check leaderboard
3. ✅ Visit Swagger UI to explore API
4. ✅ Try creating a player via API
5. ✅ View statistics dashboard

---

**Need more help?** See `INSTALLATION.md` for detailed troubleshooting!

---

**Version**: 2.0  
**Last Updated**: 2025-11-30  
**Status**: ✅ Ready to Run
