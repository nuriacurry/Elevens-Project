using System;
using System.Collections.Generic;

/// <summary>
/// Game controller for Elevens solitaire
/// Manages game state, player input, and coordinates between board and user interface
/// </summary>
public class ElevensGame
{
    private ElevensBoard board;
    private List<int> selectedCards;
    private int gamesWon;
    private int gamesPlayed;
    private bool gameActive;

    /// <summary>
    /// Creates a new Elevens game controller
    /// </summary>
    public ElevensGame()
    {
        board = new ElevensBoard();
        selectedCards = new List<int>();
        gamesWon = 0;
        gamesPlayed = 0;
        gameActive = false;
    }

    /// <summary>
    /// Starts a new game
    /// </summary>
    public void StartNewGame()
    {
        board.NewGame();
        selectedCards.Clear();
        gameActive = true;
        gamesPlayed++;
        
        Console.Clear();
        Console.WriteLine("🎴 NEW ELEVENS GAME STARTED! 🎴");
        Console.WriteLine("================================");
        Console.WriteLine("\nGAME RULES:");
        Console.WriteLine("• Select 2 cards that add up to 11 (Ace=1, face cards=11/12/13)");
        Console.WriteLine("• OR select 3 cards: Jack, Queen, King together");
        Console.WriteLine("• Remove all cards to win!");
        Console.WriteLine("\nPress any key to begin...");
        Console.ReadKey();
    }

    /// <summary>
    /// Toggles selection of a card at the specified position
    /// </summary>
    /// <param name="position">Board position (0-8)</param>
    public void ToggleCardSelection(int position)
    {
        if (position < 0 || position >= board.Size())
        {
            Console.WriteLine("❌ Invalid position! Please select 0-8.");
            return;
        }

        if (board.CardAt(position) == null)
        {
            Console.WriteLine("❌ No card at that position!");
            return;
        }

        if (selectedCards.Contains(position))
        {
            selectedCards.Remove(position);
            Console.WriteLine($"🔄 Card at position {position} deselected");
        }
        else
        {
            selectedCards.Add(position);
            Console.WriteLine($"✅ Card at position {position} selected");
        }
    }

    /// <summary>
    /// Processes the current move with selected cards
    /// </summary>
    /// <returns>True if move was valid and processed</returns>
    public bool ProcessMove()
    {
        if (selectedCards.Count == 0)
        {
            Console.WriteLine("❌ No cards selected!");
            return false;
        }

        if (board.IsLegal(selectedCards))
        {
            // Valid move - remove cards and replace
            Console.WriteLine("🎉 Valid move! Removing selected cards...");
            
            // Show what was removed
            Console.Write("Removed: ");
            foreach (int pos in selectedCards)
            {
                Card card = board.CardAt(pos);
                Console.Write($"{card.GetRank()} ");
            }
            Console.WriteLine();

            board.ReplaceSelected(new List<int>(selectedCards));
            selectedCards.Clear();
            
            return true;
        }
        else
        {
            Console.WriteLine("❌ Invalid selection! Check the rules:");
            if (selectedCards.Count == 2)
            {
                Console.WriteLine("   • Two cards must add up to 11");
            }
            else if (selectedCards.Count == 3)
            {
                Console.WriteLine("   • Three cards must be Jack, Queen, King");
            }
            else
            {
                Console.WriteLine("   • Select exactly 2 or 3 cards");
            }
            return false;
        }
    }

    /// <summary>
    /// Checks if the game is over (won or lost)
    /// </summary>
    /// <returns>True if game has ended</returns>
    public bool IsGameOver()
    {
        if (board.GameIsWon())
        {
            gamesWon++;
            gameActive = false;
            return true;
        }
        
        if (!board.AnotherPlayIsPossible())
        {
            gameActive = false;
            return true;
        }
        
        return false;
    }

    /// <summary>
    /// Checks if the player has won the current game
    /// </summary>
    /// <returns>True if game is won</returns>
    public bool HasWon()
    {
        return board.GameIsWon();
    }

    /// <summary>
    /// Gets current game statistics
    /// </summary>
    /// <returns>String with win/loss record</returns>
    public string GetGameStats()
    {
        int gamesLost = gamesPlayed - gamesWon;
        double winRate = gamesPlayed > 0 ? (double)gamesWon / gamesPlayed * 100 : 0;
        
        return $"📊 GAME STATISTICS:\n" +
               $"   Games Played: {gamesPlayed}\n" +
               $"   Games Won: {gamesWon}\n" +
               $"   Games Lost: {gamesLost}\n" +
               $"   Win Rate: {winRate:F1}%";
    }

    /// <summary>
    /// Displays the current game board with visual formatting
    /// </summary>
    public void DisplayBoard()
    {
        Console.WriteLine("\n🎴 CURRENT BOARD 🎴");
        Console.WriteLine("===================");
        
        // Display board in 3x3 grid format
        for (int row = 0; row < 3; row++)
        {
            Console.Write("│ ");
            for (int col = 0; col < 3; col++)
            {
                int position = row * 3 + col;
                Card card = board.CardAt(position);
                
                if (card != null)
                {
                    string cardStr = $"{card.GetRank()}{GetSuitSymbol(card.GetSuit())}";
                    
                    // Highlight selected cards
                    if (selectedCards.Contains(position))
                    {
                        Console.Write($"[{cardStr,4}] ");
                    }
                    else
                    {
                        Console.Write($" {cardStr,4}  ");
                    }
                }
                else
                {
                    Console.Write("  --   ");
                }
                Console.Write("│ ");
            }
            Console.WriteLine();
            
            // Position numbers row
            if (row < 2) // Don't show after last row
            {
                Console.Write("│ ");
                for (int col = 0; col < 3; col++)
                {
                    int position = row * 3 + col;
                    Console.Write($" ({position})  │ ");
                }
                Console.WriteLine();
                Console.WriteLine("├───────┼───────┼───────┤");
            }
        }
        
        // Final position numbers for bottom row
        Console.Write("│ ");
        for (int col = 0; col < 3; col++)
        {
            int position = 6 + col; // Bottom row positions 6, 7, 8
            Console.Write($" ({position})  │ ");
        }
        Console.WriteLine();
        
        Console.WriteLine("\n📋 GAME INFO:");
        Console.WriteLine($"   Cards in deck: {board.Size() - CountCardsOnBoard()}");
        Console.WriteLine($"   Selected positions: {string.Join(", ", selectedCards)}");
        
        // Show available moves hint
        ShowAvailableMoves();
    }

    /// <summary>
    /// Shows hints about available moves
    /// </summary>
    private void ShowAvailableMoves()
    {
        var pairs = board.FindPairSum11();
        var jqkSets = board.FindJQK();
        
        Console.WriteLine($"\n💡 AVAILABLE MOVES:");
        Console.WriteLine($"   Pairs that sum to 11: {pairs.Count}");
        Console.WriteLine($"   J-Q-K combinations: {jqkSets.Count}");
        
        if (pairs.Count == 0 && jqkSets.Count == 0)
        {
            Console.WriteLine("   ⚠️  No valid moves available!");
        }
    }

    /// <summary>
    /// Gets symbol for card suit
    /// </summary>
    /// <param name="suit">Card suit</param>
    /// <returns>Unicode symbol for suit</returns>
    private string GetSuitSymbol(Suit suit)
    {
        return suit switch
        {
            Suit.Hearts => "♥",
            Suit.Diamonds => "♦",
            Suit.Clubs => "♣",
            Suit.Spades => "♠",
            _ => "?"
        };
    }

    /// <summary>
    /// Counts cards currently on the board
    /// </summary>
    /// <returns>Number of cards on board</returns>
    private int CountCardsOnBoard()
    {
        int count = 0;
        for (int i = 0; i < board.Size(); i++)
        {
            if (board.CardAt(i) != null)
            {
                count++;
            }
        }
        return count;
    }

    /// <summary>
    /// Clears current card selection
    /// </summary>
    public void ClearSelection()
    {
        selectedCards.Clear();
        Console.WriteLine("🔄 Selection cleared");
    }

    /// <summary>
    /// Gets the game board for external access
    /// </summary>
    /// <returns>Current ElevensBoard instance</returns>
    public ElevensBoard GetBoard()
    {
        return board;
    }
}