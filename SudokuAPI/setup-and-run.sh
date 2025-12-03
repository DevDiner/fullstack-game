#!/bin/bash

echo "================================================"
echo "🎮 Sudoku API - Database Setup Script"
echo "================================================"
echo ""

# Navigate to API directory
cd ~/folder/fullstack-game/SudokuAPI

echo "📦 Step 1: Installing EF Core tools..."
dotnet tool install --global dotnet-ef
export PATH="$PATH:$HOME/.dotnet/tools"

echo ""
echo "📥 Step 2: Restoring NuGet packages..."
dotnet restore

echo ""
echo "🗄️ Step 3: Creating database migration..."
dotnet ef migrations add InitialCreate

echo ""
echo "💾 Step 4: Creating database..."
dotnet ef database update

echo ""
echo "✅ Database setup complete!"
echo ""
echo "================================================"
echo "🚀 Starting API Server..."
echo "================================================"
dotnet run
