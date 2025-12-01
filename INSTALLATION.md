# Installation Guide - Sudoku Zen Full Stack

Complete setup instructions for the Sudoku Zen application.

---

## Prerequisites

Before you begin, ensure you have:

### 1. .NET 8.0 SDK
**Required for the C# backend API**

**Windows:**
1. Download from: https://dotnet.microsoft.com/download/dotnet/8.0
2. Run the installer
3. Restart your terminal
4. Verify installation:
```powershell
dotnet --version
```

**macOS:**
```bash
brew install dotnet-sdk
```

**Linux (Ubuntu/Debian):**
```bash
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt-get update
sudo apt-get install -y dotnet-sdk-8.0
```

### 2. Node.js (v18+)
**Required for the frontend development server**

Download from: https://nodejs.org/

Verify installation:
```powershell
node --version
npm --version
```

### 3. Git (Optional)
For version control:
```powershell
git --version
```

---

## Installation Steps

### Step 1: Navigate to Project
```powershell
cd "c:\Users\"
```

### Step 2: Install Frontend Dependencies
```powershell
npm install
```

This installs:
- Vite (build tool)
- Google GenAI (for hints)
- Other dependencies

### Step 3: Restore Backend Dependencies
```powershell
cd SudokuAPI
dotnet restore
```

This installs:
- ASP.NET Core packages
- Swashbuckle (Swagger)
- Newtonsoft.Json

### Step 4: (Optional) Configure Gemini AI

If you want AI hints, create `.env.local` in the root:
```
VITE_API_KEY=your_api_key_here
```

Get your API key: https://aistudio.google.com/app/apikey

---

## Verify Installation

### Test Backend
```powershell
cd SudokuAPI
dotnet build
```

Expected output:
```
Build succeeded.
    0 Warning(s)
    0 Error(s)
```

### Test Frontend
```powershell
cd..
npm run build
```

Expected output:
```
✓ built in XXXms
```

---

## Running the Application

### Option 1: Quick Start (Recommended)
```powershell
.\start.bat
```

This launches both:
- Backend API on http://localhost:5000
- Frontend on http://localhost:5173

### Option 2: Manual Start

**Terminal 1 - Backend:**
```powershell
cd SudokuAPI
dotnet run
```

**Terminal 2 - Frontend:**
```powershell
npm run dev
```

---

## Troubleshooting

### Error: "dotnet command not found"
**Solution**: Install .NET SDK and restart your terminal

### Error: "npm command not found"
**Solution**: Install Node.js and restart your terminal

### Error: "Port 5000 already in use"
**Solution**: Change the port in `SudokuAPI/appsettings.json`:
```json
{
  "Urls": "http://localhost:5001"
}
```

Then update `api-client.js`:
```javascript
const API_BASE_URL = 'http://localhost:5001/api';
```

### Error: "CORS policy error"
**Solution**: Ensure the API is running before starting the frontend

### Frontend can't connect to API
**Solution**: 
1. Check API is running: http://localhost:5000/swagger
2. Check CORS is enabled in `Program.cs`
3. Verify API_BASE_URL in `api-client.js`

---

## Development Mode

### Hot Reload
Both servers support hot reload:
- **Frontend**: Edit files and see changes instantly
- **Backend**: Changes require restart (or use `dotnet watch run`)

### Using Watch Mode
```powershell
cd SudokuAPI
dotnet watch run
```

This auto-restarts the API when you change C# files.

---

## Production Build

### Backend
```powershell
cd SudokuAPI
dotnet publish -c Release -o ./publish
```

Output in: `SudokuAPI/publish/`

### Frontend
```powershell
npm run build
```

Output in: `dist/`

---

## Docker (Optional)

Want to run in Docker?

**Dockerfile for API:**
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY SudokuAPI/*.csproj ./
RUN dotnet restore
COPY SudokuAPI/. ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:5000
EXPOSE 5000
ENTRYPOINT ["dotnet", "SudokuAPI.dll"]
```

Build and run:
```powershell
docker build -t sudoku-api .
docker run -p 5000:5000 sudoku-api
```

---

## Next Steps

After installation:
1. Run `start.bat` to launch everything
2. Open http://localhost:5173
3. Enter your username
4. Start playing!
5. Check API docs at http://localhost:5000/swagger

---

## Tips

- **First time?** Start with Easy difficulty
- **Want hints?** Configure Gemini API key
- **Explore API?** Use Swagger UI to test endpoints
- **Check stats?** View leaderboard in the game

---

## Still Having Issues?

Common solutions:
1. Restart your computer
2. Reinstall .NET SDK
3. Clear npm cache: `npm cache clean --force`
4. Delete `node_modules` and run `npm install` again
5. Delete `SudokuAPI/bin` and `SudokuAPI/obj` folders

---

**Installation complete! You're ready to play Sudoku Zen!**
