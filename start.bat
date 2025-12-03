@echo off
echo ╔══════════════════════════════════════════════════════════╗
echo ║     🧩 Starting Sudoku Zen Full Stack Application 🧩   ║
echo ╚══════════════════════════════════════════════════════════╝
echo.

echo [1/2] Starting C# Backend API...
start "Sudoku API" cmd /k "cd SudokuAPI && dotnet run"
timeout /t 5 /nobreak > nul

echo [2/2] Starting Frontend...
start "Sudoku Frontend" cmd /k "npm run dev"

echo.
echo ✅ Both servers are starting!
echo.
echo 📍 API: http://localhost:5000
echo 📍 Game: http://localhost:5173
echo 📚 API Docs: http://localhost:5000/swagger
echo.
echo Press any key to stop all servers...
pause > nul

echo Stopping servers...
taskkill /FI "WINDOWTITLE eq Sudoku API" /T /F
taskkill /FI "WINDOWTITLE eq Sudoku Frontend" /T /F
echo Done!
