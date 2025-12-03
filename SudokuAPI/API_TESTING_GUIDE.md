# API Testing Guide

Complete guide to testing your Sudoku API endpoints.

---

## Quick Test Workflow

### 1. **Register a New User**
```http
POST http://localhost:5000/api/auth/register
Content-Type: application/json

{
  "username": "testplayer",
  "password": "Test@123",
  "confirmPassword": "Test@123"
}
```

**Expected Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "username": "testplayer",
  "role": "Player",
  "expiresAt": "2025-12-03T18:00:00Z"
}
```

**Copy the `token` value!**

---

### 2. **Login (Alternative to Register)**
```http
POST http://localhost:5000/api/auth/login
Content-Type: application/json

{
  "username": "testplayer",
  "password": "Test@123"
}
```

---

### 3. **Get Current User Info (Protected)**
```http
GET http://localhost:5000/api/auth/me
Authorization: Bearer YOUR_TOKEN_HERE
```

**Expected Response (200 OK):**
```json
{
  "id": 1,
  "username": "testplayer",
  "role": "Player",
  "createdAt": "2025-12-02T10:00:00Z",
  "lastLogin": "2025-12-02T10:05:00Z"
}
```

---

### 4. **Get All Puzzles**
```http
GET http://localhost:5000/api/puzzles
```

**Expected Response (200 OK):**
```json
[
  {
    "id": 1,
    "name": "Easy Puzzle 1",
    "difficulty": "Easy",
    "emptyCells": 40,
    "timesPlayed": 0,
    "completionRate": 0
  }
]
```

---

### 5. **Create a New Puzzle (Protected)**
```http
POST http://localhost:5000/api/puzzles
Authorization: Bearer YOUR_TOKEN_HERE
Content-Type: application/json

{
  "name": "Test Puzzle",
  "initialGrid": "530070000600195000098000060800060003400803001700020006060000280000419005000080079",
  "solutionGrid": "534678912672195348198342567859761423426853791713924856961537284287419635345286179",
  "difficulty": "Medium",
  "emptyCells": 40
}
```

---

## Testing Tools

### Option 1: Swagger UI (Recommended)

**1. Start the API:**
```powershell
dotnet run
```

**2. Open Swagger:**
```
http://localhost:5000/swagger
```

**3. Authorize:**
- Click the "Authorize" button (🔒)
- Enter: `Bearer YOUR_TOKEN` (from step 1)
- Click "Authorize"
- Click "Close"

**4. Test any endpoint:**
- Click on an endpoint to expand it
- Click "Try it out"
- Fill in parameters
- Click "Execute"
- View the response!

---

### Option 2: PowerShell (curl)

**Register:**
```powershell
curl -X POST http://localhost:5000/api/auth/register `
  -H "Content-Type: application/json" `
  -d '{\"username\":\"testuser\",\"password\":\"Test@123\",\"confirmPassword\":\"Test@123\"}'
```

**Login:**
```powershell
curl -X POST http://localhost:5000/api/auth/login `
  -H "Content-Type: application/json" `
  -d '{\"username\":\"testuser\",\"password\":\"Test@123\"}'
```

**Get Puzzles:**
```powershell
curl http://localhost:5000/api/puzzles
```

**Protected Endpoint:**
```powershell
$token = "YOUR_JWT_TOKEN"
curl http://localhost:5000/api/auth/me -H "Authorization: Bearer $token"
```

---

### Option 3: Postman

**1. Import Collection:**
Create a new collection called "Sudoku API"

**2. Add Environment Variable:**
- Name: `token`
- Initial Value: (leave empty)
- Current Value: (paste your JWT token after login)

**3. Use `{{token}}` in headers:**
```
Authorization: Bearer {{token}}
```

---

## Complete Test Checklist

### Authentication Endpoints
- [ ] `POST /api/auth/register` - Register new user
- [ ] `POST /api/auth/login` - Login existing user
- [ ] `GET /api/auth/me` - Get current user (requires token)

### Puzzle Endpoints
- [ ] `GET /api/puzzles` - Get all puzzles
- [ ] `GET /api/puzzles/{id}` - Get specific puzzle
- [ ] `GET /api/puzzles/random` - Get random puzzle
- [ ] `GET /api/puzzles/difficulty/easy` - Get puzzles by difficulty
- [ ] `POST /api/puzzles` - Create new puzzle (requires token)
- [ ] `PUT /api/puzzles/{id}` - Update puzzle (requires token)
- [ ] `DELETE /api/puzzles/{id}` - Delete puzzle (requires token)
- [ ] `POST /api/puzzles/{id}/play` - Record puzzle play

### Player Endpoints
- [ ] `GET /api/players` - Get all players
- [ ] `GET /api/players/{id}` - Get specific player
- [ ] `GET /api/players/username/{username}` - Get by username
- [ ] `POST /api/players` - Create player
- [ ] `PUT /api/players/{id}` - Update player
- [ ] `DELETE /api/players/{id}` - Delete player
- [ ] `POST /api/players/{id}/game` - Record game result
- [ ] `GET /api/players/leaderboard` - Get top players
- [ ] `GET /api/players/fastest` - Get fastest players
- [ ] `GET /api/players/stats` - Get overall statistics

### Session Endpoints
- [ ] `GET /api/sessions` - Get all sessions
- [ ] `GET /api/sessions/{id}` - Get specific session
- [ ] `GET /api/sessions/player/{playerId}` - Get player's sessions
- [ ] `POST /api/sessions` - Start new session
- [ ] `PUT /api/sessions/{id}` - Update session
- [ ] `POST /api/sessions/{id}/complete` - Complete session
- [ ] `POST /api/sessions/{id}/abandon` - Abandon session

---

## 🎓 Test Scenarios

### Scenario 1: New Player Registration & First Game

**1. Register:**
```json
POST /api/auth/register
{
  "username": "alice",
  "password": "Alice@123",
  "confirmPassword": "Alice@123"
}
```

**2. Create Player Profile:**
```json
POST /api/players
{
  "username": "alice",
  "email": "alice@example.com"
}
```

**3. Get a Random Puzzle:**
```http
GET /api/puzzles/random?difficulty=easy
```

**4. Start Game Session:**
```json
POST /api/sessions
{
  "playerId": 1,
  "puzzleId": 1,
  "initialGrid": "530070000600195000..."
}
```

**5. Complete the Session:**
```http
POST /api/sessions/1/complete
```

**6. Record Results:**
```json
POST /api/players/1/game
{
  "completed": true,
  "timeSeconds": 300,
  "hintsUsed": 2
}
```

**7. Check Leaderboard:**
```http
GET /api/players/leaderboard
```

---

### Scenario 2: Admin Creates Puzzles

**1. Login as Admin:**
```json
POST /api/auth/login
{
  "username": "admin",
  "password": "Admin@123"
}
```

**2. Create Multiple Puzzles:**
```json
POST /api/puzzles
{
  "name": "Hard Challenge 1",
  "initialGrid": "...",
  "solutionGrid": "...",
  "difficulty": "Hard",
  "emptyCells": 60
}
```

**3. View All Puzzles:**
```http
GET /api/puzzles
```

**4. Get Difficulty Statistics:**
```http
GET /api/puzzles/stats/difficulty
```

---

## Error Testing

### Test 1: Invalid Registration
```json
POST /api/auth/register
{
  "username": "ab",
  "password": "123",
  "confirmPassword": "456"
}
```

**Expected: 400 Bad Request**
```json
{
  "errors": {
    "Username": ["Username must be between 3 and 50 characters"],
    "Password": ["Password must be at least 6 characters"],
    "ConfirmPassword": ["Passwords do not match"]
  }
}
```

---

### Test 2: Unauthorized Access
```http
GET /api/auth/me
(No Authorization header)
```

**Expected: 401 Unauthorized**

---

### Test 3: Duplicate Username
```json
POST /api/auth/register
{
  "username": "alice",
  "password": "Test@123",
  "confirmPassword": "Test@123"
}
```

**Expected: 400 Bad Request**
```json
{
  "message": "Username already exists"
}
```

---

## Success Criteria

Your API is working correctly if:

- ✅ Registration returns a valid JWT token
- ✅ Login authenticates and returns token
- ✅ Protected endpoints reject requests without token
- ✅ Protected endpoints accept requests with valid token
- ✅ CRUD operations work for all entities
- ✅ Leaderboards return sorted data
- ✅ Statistics endpoints calculate correctly
- ✅ Validation prevents invalid data
- ✅ Swagger UI displays all endpoints
- ✅ Database persists data across restarts

---

## Performance Testing

**Test database performance:**
```sql
-- Create 1000 test players
INSERT INTO Players (Username, Email, ...) VALUES ...

-- Query leaderboard
GET /api/players/leaderboard?count=100

-- Check response time (should be < 100ms)
```

---

## Sample Test Data

**Valid Sudoku Grids:**

**Easy:**
- Initial: `530070000600195000098000060800060003400803001700020006060000280000419005000080079`
- Solution: `534678912672195348198342567859761423426853791713924856961537284287419635345286179`

**Medium:**
- Initial: `000000907000420180000705026100904000050000040000507009920108000034059000507000000`
- Solution: `362184957794623185185796426176934258853271649249587369921348576634859712587612834`

---

**Happy Testing!** 

For automated testing, check out `DATABASE_SETUP.md` for database management commands.
