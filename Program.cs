using System;
using System.Collections.Generic;

/// <summary>
/// Complete Elevens Solitaire Game - Final Submission
/// Advanced features: Save/Load, Undo, Hints, Statistics, Enhanced UI
/// </summary>
class Program
{
    static GameManager gameManager = new GameManager();
    
    static void Main(string[] args)
    {
        ShowWelcomeScreen();
        
        ElevensGame game = new ElevensGame();
        bool playAgain = true;
        
        // Try to load previous session
        gameManager.LoadGameFromFile();
        
        while (playAgain)
        {
            // Start new game
            game.StartNewGame();
            gameManager.SaveGameState(game);
            
            // Main game loop with enhanced features
            while (!game.IsGameOver())
            {
                Console.Clear();
                ShowGameHeader();
                game.DisplayBoard();
                ShowAdvancedCommands();
                
                Console.Write("\n🎮 Enter command: ");
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
                            gameManager.SaveGameState(game); // Save state before move
                            game.ToggleCardSelection(position);
                            PauseForInput();
                        }
                        break;
                        
                    case "P":
                        bool validMove = game.ProcessMove();
                        if (validMove)
                        {
                            gameManager.SaveGameState(game);
                            Console.WriteLine("✨ Cards replaced! Excellent move!");
                        }
                        else
                        {
                            Console.WriteLine("💭 Try a different combination...");
                        }
                        PauseForInput();
                        break;
                        
                    case "C":
                        game.ClearSelection();
                        PauseForInput();
                        break;
                        
                    case "H":
                        ShowDetailedHelp();
                        break;
                        
                    case "HINT":
                        gameManager.ProvideHint(game.GetBoard());
                        PauseForInput();
                        break;
                        
                    case "U":
                    case "UNDO":
                        gameManager.UndoLastMove(game);
                        PauseForInput();
                        break;
                        
                    case "S":
                    case "SAVE":
                        gameManager.SaveGameToFile(game);
                        PauseForInput();
                        break;
                        
                    case "STATS":
                        Console.Clear();
                        Console.WriteLine(gameManager.GetDetailedStats());
                        PauseForInput();
                        break;
                        
                    case "Q":
                        Console.WriteLine("❌ Game ended by player");
                        goto GameEnd;
                        
                    default:
                        Console.WriteLine("❌ Invalid command! Type 'H' for help");
                        PauseForInput();
                        break;
                }
            }
            
            GameEnd:
            // Enhanced game over sequence
            ShowGameResults(game);
            gameManager.UpdateStats(game.HasWon());
            
            // Ask to play again with enhanced options
            playAgain = AskPlayAgain();
        }
        
        ShowFarewellScreen();
    }
    
    static void ShowWelcomeScreen()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║        🎴 ELEVENS SOLITAIRE 🎴         ║");
        Console.WriteLine("║            FINAL VERSION               ║");
        Console.WriteLine("╠════════════════════════════════════════╣");
        Console.WriteLine("║  Advanced Features:                    ║");
        Console.WriteLine("║  • Save/Load Games                     ║");
        Console.WriteLine("║  • Undo Moves                         ║");
        Console.WriteLine("║  • Intelligent Hints                  ║");
        Console.WriteLine("║  • Detailed Statistics                 ║");
        Console.WriteLine("║  • Enhanced Visual Interface           ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine("\nPress any key to begin your solitaire journey...");
        Console.ReadKey();
    }
    
    static void ShowGameHeader()
    {
        Console.WriteLine("🎴═══════════════════════════════════════════════════════════🎴");
        Console.WriteLine("                    ELEVENS SOLITAIRE - FINAL VERSION");
        Console.WriteLine("🎴═══════════════════════════════════════════════════════════🎴");
    }
    
    static void ShowAdvancedCommands()
    {
        Console.WriteLine("\n🎮 GAME COMMANDS:");
        Console.WriteLine("┌─────────────────────────────────────────────────────────────┐");
        Console.WriteLine("│ 0-8: Select/deselect card    │ P: Process move             │");
        Console.WriteLine("│ C: Clear selection           │ H: Help & rules             │");
        Console.WriteLine("│ HINT: Get intelligent hint   │ U/UNDO: Undo last move     │");
        Console.WriteLine("│ S/SAVE: Save game            │ STATS: View statistics      │");
        Console.WriteLine("│ Q: Quit game                 │                             │");
        Console.WriteLine("└─────────────────────────────────────────────────────────────┘");
    }
    
    static void ShowGameResults(ElevensGame game)
    {
        Console.Clear();
        
        if (game.HasWon())
        {
            Console.WriteLine("🎉╔═══════════════════════════════════════╗🎉");
            Console.WriteLine("  ║         VICTORY ACHIEVED!             ║  ");
            Console.WriteLine("🎊║      CONGRATULATIONS CHAMPION!       ║🎊");
            Console.WriteLine("  ╚═══════════════════════════════════════╝  ");
            Console.WriteLine();
            
            // Victory animation
            string[] celebrations = { "🎊", "🎉", "✨", "🏆", "⭐" };
            Console.Write("Celebration: ");
            for (int i = 0; i < 10; i++)
            {
                Console.Write(celebrations[i % celebrations.Length] + " ");
                System.Threading.Thread.Sleep(200);
            }
            Console.WriteLine("\n");
            
            Console.WriteLine("🏆 You successfully removed all 52 cards!");
            Console.WriteLine("🧠 Your strategic thinking paid off!");
            Console.WriteLine("💪 Elevens mastery achieved!");
        }
        else
        {
            Console.WriteLine("😔╔═══════════════════════════════════════╗😔");
            Console.WriteLine("  ║            GAME OVER                  ║  ");
            Console.WriteLine("  ║        No More Valid Moves            ║  ");
            Console.WriteLine("  ╚═══════════════════════════════════════╝  ");
            Console.WriteLine();
            Console.WriteLine("🎯 Don't give up! Elevens requires patience and strategy.");
            Console.WriteLine("💡 Try using HINT command next time for guidance.");
            Console.WriteLine("📈 Each game makes you a better player!");
        }
        
        Console.WriteLine($"\n{gameManager.GetDetailedStats()}");
    }
    
    static bool AskPlayAgain()
    {
        Console.WriteLine("\n🔄 PLAY AGAIN OPTIONS:");
        Console.WriteLine("   Y: Start new game");
        Console.WriteLine("   N: Exit to desktop");
        Console.WriteLine("   STATS: View detailed statistics");
        
        while (true)
        {
            Console.Write("\nYour choice: ");
            string input = Console.ReadLine()?.ToUpper().Trim();
            
            switch (input)
            {
                case "Y":
                case "YES":
                    return true;
                    
                case "N":
                case "NO":
                    return false;
                    
                case "STATS":
                    Console.Clear();
                    Console.WriteLine(gameManager.GetDetailedStats());
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadKey();
                    Console.WriteLine("\n🔄 PLAY AGAIN OPTIONS:");
                    Console.WriteLine("   Y: Start new game");
                    Console.WriteLine("   N: Exit to desktop");
                    break;
                    
                default:
                    Console.WriteLine("Please enter Y, N, or STATS");
                    break;
            }
        }
    }
    
    static void ShowFarewellScreen()
    {
        Console.Clear();
        Console.WriteLine("╔════════════════════════════════════════╗");
        Console.WriteLine("║     Thank You for Playing Elevens!    ║");
        Console.WriteLine("╠════════════════════════════════════════╣");
        Console.WriteLine("║  🎮 Game developed for CSC 350H       ║");
        Console.WriteLine("║  🎓 Object-Oriented Programming        ║");
        Console.WriteLine("║  📚 From Design to Implementation      ║");
        Console.WriteLine("║                                        ║");
        Console.WriteLine("║  Features Demonstrated:                ║");
        Console.WriteLine("║  ✅ Inheritance & Polymorphism         ║");
        Console.WriteLine("║  ✅ Encapsulation & Abstraction        ║");
        Console.WriteLine("║  ✅ File I/O & Serialization           ║");
        Console.WriteLine("║  ✅ Algorithm Implementation           ║");
        Console.WriteLine("║  ✅ User Interface Design              ║");
        Console.WriteLine("║  ✅ Error Handling & Validation        ║");
        Console.WriteLine("╚════════════════════════════════════════╝");
        Console.WriteLine("\n🌟 May your coding journey continue to flourish!");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
    
    static void ShowDetailedHelp()
    {
        Console.Clear();
        Console.WriteLine("📖 ELEVENS SOLITAIRE - COMPREHENSIVE GUIDE");
        Console.WriteLine("==========================================");
        
        Console.WriteLine("\n🎯 OBJECTIVE:");
        Console.WriteLine("   Remove all 52 cards by making valid selections");
        
        Console.WriteLine("\n📏 GAME RULES:");
        Console.WriteLine("   1. Select 2 cards that sum to 11:");
        Console.WriteLine("      • Ace = 1, Cards 2-10 = face value");
        Console.WriteLine("      • Jack = 11, Queen = 12, King = 13");
        Console.WriteLine("      • Examples: A+10, 2+9, 5+6, 3+8");
        
        Console.WriteLine("\n   2. OR select 3 cards: Jack + Queen + King");
        Console.WriteLine("      • Any suits (♠♥♦♣) - suits don't matter");
        Console.WriteLine("      • Must be exactly one J, one Q, one K");
        
        Console.WriteLine("\n🎮 ADVANCED COMMANDS:");
        Console.WriteLine("   HINT    - Get intelligent move suggestions");
        Console.WriteLine("   UNDO    - Undo your last move");
        Console.WriteLine("   SAVE    - Save game to continue later");
        Console.WriteLine("   STATS   - View detailed performance metrics");
        
        Console.WriteLine("\n💡 WINNING STRATEGIES:");
        Console.WriteLine("   • Scan for easy pairs first (A+10, 2+9, etc.)");
        Console.WriteLine("   • Use J-Q-K when pairs become scarce");
        Console.WriteLine("   • Plan moves - some choices may block future options");
        Console.WriteLine("   • Use HINT when stuck - it analyzes the best moves");
        Console.WriteLine("   • Don't be afraid to UNDO and try different approaches");
        
        Console.WriteLine("\n📊 DIFFICULTY & ODDS:");
        Console.WriteLine("   • Elevens has ~11% win rate for random play");
        Console.WriteLine("   • Strategic play can improve odds significantly");
        Console.WriteLine("   • Average game length: 15-25 moves");
        
        Console.WriteLine("\n🏆 MASTERY TIPS:");
        Console.WriteLine("   • Track your statistics to see improvement");
        Console.WriteLine("   • Aim for win streaks to build consistency");
        Console.WriteLine("   • Learn to recognize losing positions early");
        Console.WriteLine("   • Practice pattern recognition for faster play");
        
        PauseForInput();
    }
    
    static void PauseForInput()
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
    }
}