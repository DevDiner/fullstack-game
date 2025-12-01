// API Client for Sudoku Backend
const API_BASE_URL = 'http://localhost:5000/api';

export const SudokuAPI = {
    // Player Operations
    async createPlayer(username, email = '') {
        const response = await fetch(`${API_BASE_URL}/players`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ username, email, preferredDifficulty: 'Medium' })
        });
        return response.json();
    },

    async getPlayer(id) {
        const response = await fetch(`${API_BASE_URL}/players/${id}`);
        return response.json();
    },

    async getPlayerByUsername(username) {
        const response = await fetch(`${API_BASE_URL}/players/username/${username}`);
        return response.json();
    },

    async recordGame(playerId, completed, timeSeconds, hintsUsed) {
        const response = await fetch(`${API_BASE_URL}/players/${playerId}/game`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ completed, timeSeconds, hintsUsed })
        });
        return response.json();
    },

    async getLeaderboard(count = 10) {
        const response = await fetch(`${API_BASE_URL}/players/leaderboard?count=${count}`);
        return response.json();
    },

    async getStreakLeaders(count = 10) {
        const response = await fetch(`${API_BASE_URL}/players/streaks?count=${count}`);
        return response.json();
    },

    async getFastestPlayers(count = 10) {
        const response = await fetch(`${API_BASE_URL}/players/fastest?count=${count}`);
        return response.json();
    },

    // Puzzle Operations
    async createPuzzle(initialGrid, solutionGrid, difficulty, emptyCells) {
        const response = await fetch(`${API_BASE_URL}/puzzles`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({
                name: `${difficulty} Puzzle`,
                initialGrid,
                solutionGrid,
                difficulty: difficulty.charAt(0).toUpperCase() + difficulty.slice(1),
                emptyCells
            })
        });
        return response.json();
    },

    async getRandomPuzzle(difficulty = null) {
        const url = difficulty 
            ? `${API_BASE_URL}/puzzles/random?difficulty=${difficulty.charAt(0).toUpperCase() + difficulty.slice(1)}`
            : `${API_BASE_URL}/puzzles/random`;
        const response = await fetch(url);
        return response.json();
    },

    async recordPuzzlePlay(puzzleId, completed, timeSeconds) {
        const response = await fetch(`${API_BASE_URL}/puzzles/${puzzleId}/play`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ completed, timeSeconds })
        });
        return response.json();
    },

    // Session Operations
    async startSession(playerId, puzzleId, initialGrid) {
        const response = await fetch(`${API_BASE_URL}/sessions`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ playerId, puzzleId, initialGrid })
        });
        return response.json();
    },

    async updateSession(sessionId, currentGrid, elapsedSeconds, hintsUsed, mistakes) {
        const response = await fetch(`${API_BASE_URL}/sessions/${sessionId}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ currentGrid, elapsedSeconds, hintsUsed, mistakes })
        });
        return response.ok;
    },

    async completeSession(sessionId) {
        const response = await fetch(`${API_BASE_URL}/sessions/${sessionId}/complete`, {
            method: 'POST'
        });
        return response.json();
    },

    async abandonSession(sessionId) {
        const response = await fetch(`${API_BASE_URL}/sessions/${sessionId}/abandon`, {
            method: 'POST'
        });
        return response.json();
    },

    async getPlayerSessions(playerId) {
        const response = await fetch(`${API_BASE_URL}/sessions/player/${playerId}`);
        return response.json();
    },

    // Statistics
    async getPlayerStats() {
        const response = await fetch(`${API_BASE_URL}/players/stats`);
        return response.json();
    },

    async getSessionStats() {
        const response = await fetch(`${API_BASE_URL}/sessions/stats`);
        return response.json();
    }
};

// Helper function to convert 2D array to string
export function gridToString(grid) {
    return grid.map(row => row.map(cell => cell === null ? '0' : cell).join('')).join('');
}

// Helper function to convert string to 2D array
export function stringToGrid(str) {
    const grid = [];
    for (let i = 0; i < 9; i++) {
        const row = [];
        for (let j = 0; j < 9; j++) {
            const val = parseInt(str[i * 9 + j]);
            row.push(val === 0 ? null : val);
        }
        grid.push(row);
    }
    return grid;
}
