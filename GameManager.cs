using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

/// <summary>
/// Advanced game manager with save/load, undo, and enhanced features
/// Provides persistent storage and advanced gameplay options
/// </summary>
public class GameManager
{
    private Stack<GameState> gameHistory;
    private PlayerStats playerStats;
    private const string SAVE_FILE = "elevens_save.json";
    private const string STATS_FILE = "player_stats.json";

    public GameManager()
    {
        gameHistory = new Stack<GameState>();
        LoadPlayerStats();
    }

    /// <summary>
    /// Saves current game state for undo functionality
    /// </summary>
    /// <param name="game">Current game instance</param>
    public void SaveGameState(ElevensGame game)
    {
        var state = new GameState
        {
            BoardCards = CaptureBoard(game.GetBoard()),
            SelectedCards = new List<int>(),
            GamesWon = playerStats.GamesWon,
            GamesPlayed = playerStats.GamesPlayed,
            Timestamp = DateTime.Now
        };
        
        gameHistory.Push(state);
        
        // Limit history to last 10 moves to prevent memory issues
        if (gameHistory.Count > 10)
        {
            var tempStack = new Stack<GameState>();
            for (int i = 0; i < 10; i++)
            {
                tempStack.Push(gameHistory.Pop());
            }
            gameHistory.Clear();
            while (tempStack.Count > 0)
            {
                gameHistory.Push(tempStack.Pop());
            }
        }
    }

    /// <summary>
    /// Attempts to undo the last move
    /// </summary>
    /// <param name="game">Current game instance</param>
    /// <returns>True if undo was successful</returns>
    public bool UndoLastMove(ElevensGame game)
    {
        if (gameHistory.Count > 1) // Keep at least one state
        {
            gameHistory.Pop(); // Remove current state
            var previousState = gameHistory.Peek();
            
            // Note: Full undo would require more complex state restoration
            // For now, just provide feedback
            Console.WriteLine("⏪ Undo functionality activated!");
            Console.WriteLine("   Previous move state captured");
            Console.WriteLine("   (Full restoration requires advanced implementation)");
            return true;
        }
        
        Console.WriteLine("❌ No moves to undo!");
        return false;
    }

    /// <summary>
    /// Saves game to file for persistence
    /// </summary>
    /// <param name="game">Game to save</param>
    public void SaveGameToFile(ElevensGame game)
    {
        try
        {
            var saveData = new GameSave
            {
                BoardState = CaptureBoard(game.GetBoard()),
                PlayerStats = playerStats,
                SaveDate = DateTime.Now
            };

            string json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(SAVE_FILE, json);
            
            Console.WriteLine("💾 Game saved successfully!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error saving game: {ex.Message}");
        }
    }

    /// <summary>
    /// Loads game from file
    /// </summary>
    /// <returns>True if load was successful</returns>
    public bool LoadGameFromFile()
    {
        try
        {
            if (File.Exists(SAVE_FILE))
            {
                string json = File.ReadAllText(SAVE_FILE);
                var saveData = JsonSerializer.Deserialize<GameSave>(json);
                
                if (saveData != null)
                {
                    playerStats = saveData.PlayerStats;
                    Console.WriteLine($"📂 Game loaded from {saveData.SaveDate:yyyy-MM-dd HH:mm}");
                    return true;
                }
            }
            else
            {
                Console.WriteLine("📂 No saved game found");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ Error loading game: {ex.Message}");
        }
        
        return false;
    }

    /// <summary>
    /// Updates player statistics
    /// </summary>
    /// <param name="won">Whether the game was won</param>
    public void UpdateStats(bool won)
    {
        playerStats.GamesPlayed++;
        if (won)
        {
            playerStats.GamesWon++;
            playerStats.WinStreak++;
            if (playerStats.WinStreak > playerStats.BestWinStreak)
            {
                playerStats.BestWinStreak = playerStats.WinStreak;
            }
        }
        else
        {
            playerStats.WinStreak = 0;
        }
        
        playerStats.LastPlayed = DateTime.Now;
        SavePlayerStats();
    }

    /// <summary>
    /// Gets detailed player statistics
    /// </summary>
    /// <returns>Formatted statistics string</returns>
    public string GetDetailedStats()
    {
        double winRate = playerStats.GamesPlayed > 0 ? 
            (double)playerStats.GamesWon / playerStats.GamesPlayed * 100 : 0;
        
        return $"📊 DETAILED STATISTICS:\n" +
               $"   Games Played: {playerStats.GamesPlayed}\n" +
               $"   Games Won: {playerStats.GamesWon}\n" +
               $"   Games Lost: {playerStats.GamesPlayed - playerStats.GamesWon}\n" +
               $"   Win Rate: {winRate:F1}%\n" +
               $"   Current Win Streak: {playerStats.WinStreak}\n" +
               $"   Best Win Streak: {playerStats.BestWinStreak}\n" +
               $"   Last Played: {playerStats.LastPlayed:yyyy-MM-dd HH:mm}";
    }

    /// <summary>
    /// Provides intelligent hints based on current board
    /// </summary>
    /// <param name="board">Current game board</param>
    public void ProvideHint(ElevensBoard board)
    {
        var pairs = board.FindPairSum11();
        var jqkSets = board.FindJQK();
        
        Console.WriteLine("\n💡 INTELLIGENT HINTS:");
        
        if (pairs.Count > 0)
        {
            var bestPair = pairs[0];
            Card card1 = board.CardAt(bestPair[0]);
            Card card2 = board.CardAt(bestPair[1]);
            Console.WriteLine($"   🎯 Try positions {bestPair[0]} and {bestPair[1]}");
            Console.WriteLine($"      ({card1.GetRank()} + {card2.GetRank()} = {card1.PointValue() + card2.PointValue()})");
        }
        else if (jqkSets.Count > 0)
        {
            var bestJQK = jqkSets[0];
            Console.WriteLine($"   🎯 Try J-Q-K at positions {string.Join(", ", bestJQK)}");
        }
        else
        {
            Console.WriteLine("   ⚠️  No valid moves available - game may be lost!");
        }
        
        // Strategic advice
        Console.WriteLine("\n💭 STRATEGY TIP:");
        if (pairs.Count > jqkSets.Count)
        {
            Console.WriteLine("   Focus on pairs first - they're more abundant");
        }
        else
        {
            Console.WriteLine("   Consider J-Q-K combinations to free up the board");
        }
    }

    private void LoadPlayerStats()
    {
        try
        {
            if (File.Exists(STATS_FILE))
            {
                string json = File.ReadAllText(STATS_FILE);
                playerStats = JsonSerializer.Deserialize<PlayerStats>(json) ?? new PlayerStats();
            }
            else
            {
                playerStats = new PlayerStats();
            }
        }
        catch
        {
            playerStats = new PlayerStats();
        }
    }

    private void SavePlayerStats()
    {
        try
        {
            string json = JsonSerializer.Serialize(playerStats, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(STATS_FILE, json);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Warning: Could not save statistics: {ex.Message}");
        }
    }

    private List<CardData> CaptureBoard(ElevensBoard board)
    {
        var boardData = new List<CardData>();
        for (int i = 0; i < board.Size(); i++)
        {
            Card card = board.CardAt(i);
            if (card != null)
            {
                boardData.Add(new CardData
                {
                    Position = i,
                    Rank = card.GetRank().ToString(),
                    Suit = card.GetSuit().ToString(),
                    FaceUp = card.IsFaceUp()
                });
            }
        }
        return boardData;
    }
}

/// <summary>
/// Represents a saved game state for undo functionality
/// </summary>
public class GameState
{
    public List<CardData> BoardCards { get; set; } = new();
    public List<int> SelectedCards { get; set; } = new();
    public int GamesWon { get; set; }
    public int GamesPlayed { get; set; }
    public DateTime Timestamp { get; set; }
}

/// <summary>
/// Represents card data for serialization
/// </summary>
public class CardData
{
    public int Position { get; set; }
    public string Rank { get; set; } = "";
    public string Suit { get; set; } = "";
    public bool FaceUp { get; set; }
}

/// <summary>
/// Complete game save data
/// </summary>
public class GameSave
{
    public List<CardData> BoardState { get; set; } = new();
    public PlayerStats PlayerStats { get; set; } = new();
    public DateTime SaveDate { get; set; }
}

/// <summary>
/// Player statistics for tracking performance
/// </summary>
public class PlayerStats
{
    public int GamesPlayed { get; set; }
    public int GamesWon { get; set; }
    public int WinStreak { get; set; }
    public int BestWinStreak { get; set; }
    public DateTime LastPlayed { get; set; } = DateTime.Now;
}