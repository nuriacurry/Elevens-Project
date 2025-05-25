using System;
using System.Collections.Generic;

/// <summary>
/// Interactive Elevens Solitaire Game
/// Complete implementation with user interface and gameplay loop
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("🎴 WELCOME TO ELEVENS SOLITAIRE! 🎴");
        Console.WriteLine("===================================");
        
        ElevensGame game = new ElevensGame();
        bool playAgain = true;
        
        while (playAgain)
        {
            // Start new game
            game.StartNewGame();
            
            // Main game loop
            while (!game.IsGameOver())
            {
                Console.Clear();
                game.DisplayBoard();
                
                Console.WriteLine("\n🎮 GAME COMMANDS:");
                Console.WriteLine("   0-8: Select/deselect card at position");
                Console.WriteLine("   P: Process move with selected cards");
                Console.WriteLine("   C: Clear selection");
                Console.WriteLine("   H: Show detailed help");
                Console.WriteLine("   Q: Quit current game");
                Console.Write("\nEnter command: ");
                
                string input = Console.ReadLine()?.ToUpper().Trim();
                
                if (string.IsNullOrEmpty(input))
                {
                    continue;
                }
                
                switch (input)
                {
                    case "0": case "1": case "2": case "3": case "4":
                    case "5": case "6": case "7": case "8":
                        if (int.TryParse(input, out int position))
                        {
                            game.ToggleCardSelection(position);
                            Console.WriteLine("Press any key to continue...");
                            Console.ReadKey();
                        }
                        break;
                        
                    case "P":
                        bool validMove = game.ProcessMove();
                        if (validMove)
                        {
                            Console.WriteLine("✨ Cards replaced! Press any key to continue...");
                        }
                        else
                        {
                            Console.WriteLine("Press any key to try again...");
                        }
                        Console.ReadKey();
                        break;
                        
                    case "C":
                        game.ClearSelection();
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadKey();
                        break;
                        
                    case "H":
                        ShowDetailedHelp();
                        break;
                        
                    case "Q":
                        Console.WriteLine("❌ Game ended by player");
                        goto GameEnd;
                        
                    default:
                        Console.WriteLine("❌ Invalid command! Press any key to continue...");
                        Console.ReadKey();
                        break;
                }
            }
            
            GameEnd:
            // Game over - show results
            Console.Clear();
            if (game.HasWon())
            {
                Console.WriteLine("🎉 CONGRATULATIONS! YOU WON! 🎉");
                Console.WriteLine("===============================");
                Console.WriteLine("You successfully removed all cards!");
                
                // Victory animation
                for (int i = 0; i < 3; i++)
                {
                    Console.Write("🎊 ");
                    System.Threading.Thread.Sleep(500);
                }
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("😔 GAME OVER - NO MORE MOVES");
                Console.WriteLine("============================");
                Console.WriteLine("Better luck next time!");
            }
            
            // Show statistics
            Console.WriteLine($"\n{game.GetGameStats()}");
            
            // Ask to play again
            Console.WriteLine("\n🔄 Would you like to play again? (Y/N)");
            string playAgainInput = Console.ReadLine()?.ToUpper().Trim();
            playAgain = playAgainInput == "Y" || playAgainInput == "YES";
        }
        
        Console.WriteLine("\n👋 Thanks for playing Elevens Solitaire!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
    
    /// <summary>
    /// Shows detailed help and rules
    /// </summary>
    static void ShowDetailedHelp()
    {
        Console.Clear();
        Console.WriteLine("📖 ELEVENS SOLITAIRE - DETAILED HELP");
        Console.WriteLine("=====================================");
        
        Console.WriteLine("\n🎯 OBJECTIVE:");
        Console.WriteLine("   Remove all 52 cards from the deck by making valid selections");
        
        Console.WriteLine("\n📏 RULES:");
        Console.WriteLine("   1. Select exactly 2 cards that add up to 11:");
        Console.WriteLine("      • Ace = 1 point");
        Console.WriteLine("      • Number cards = face value (2-10)");
        Console.WriteLine("      • Jack = 11, Queen = 12, King = 13");
        Console.WriteLine("      • Examples: Ace + 10, 2 + 9, 5 + 6");
        
        Console.WriteLine("\n   2. OR select exactly 3 cards: Jack, Queen, King");
        Console.WriteLine("      • Suits don't matter");
        Console.WriteLine("      • Must have one of each: J, Q, K");
        
        Console.WriteLine("\n🎮 HOW TO PLAY:");
        Console.WriteLine("   1. Type a number (0-8) to select/deselect cards");
        Console.WriteLine("   2. Selected cards appear in [brackets]");
        Console.WriteLine("   3. Type 'P' to process your selection");
        Console.WriteLine("   4. Valid selections are removed and replaced");
        Console.WriteLine("   5. Continue until all cards are gone (WIN!)");
        Console.WriteLine("   6. If no moves are possible, you lose");
        
        Console.WriteLine("\n💡 STRATEGY TIPS:");
        Console.WriteLine("   • Look for pairs first (easier to spot)");
        Console.WriteLine("   • Save J-Q-K combinations for when needed");
        Console.WriteLine("   • Plan ahead - some moves may block others");
        Console.WriteLine("   • Use 'C' to clear selection if you change your mind");
        
        Console.WriteLine("\n🎲 WINNING ODDS:");
        Console.WriteLine("   • Elevens has roughly 1 in 9 chance of winning");
        Console.WriteLine("   • Don't get discouraged - it's meant to be challenging!");
        
        Console.WriteLine("\nPress any key to return to game...");
        Console.ReadKey();
    }
}