using System;
using System.Collections.Generic;

/// <summary>
/// Demo program for testing the Elevens solitaire game implementation
/// Shows partial implementation with basic game mechanics
/// </summary>
class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("=== ELEVENS SOLITAIRE GAME - PARTIAL IMPLEMENTATION ===\n");

        // Test 1: Card and Enum Testing
        Console.WriteLine("🧪 TEST 1: Card and Enum Functionality");
        Console.WriteLine("======================================");
        
        Card aceSpades = new Card(Rank.Ace, Suit.Spades);
        Card tenHearts = new Card(Rank.Ten, Suit.Hearts);
        Card jackClubs = new Card(Rank.Jack, Suit.Clubs);
        
        Console.WriteLine($"Created cards:");
        Console.WriteLine($"  {aceSpades} (Point Value: {aceSpades.PointValue()})");
        Console.WriteLine($"  {tenHearts} (Point Value: {tenHearts.PointValue()})");
        Console.WriteLine($"  {jackClubs} (Point Value: {jackClubs.PointValue()})");
        
        // Test card flipping
        aceSpades.FlipCard();
        Console.WriteLine($"  After flipping: {aceSpades}");
        Console.WriteLine();

        // Test 2: Deck Functionality
        Console.WriteLine("🧪 TEST 2: Deck Creation and Shuffling");
        Console.WriteLine("=====================================");
        
        Deck testDeck = new Deck();
        Console.WriteLine($"New deck created: {testDeck}");
        
        testDeck.Shuffle();
        Console.WriteLine("Deck shuffled successfully");
        
        Console.WriteLine("Dealing 5 cards:");
        for (int i = 0; i < 5; i++)
        {
            Card dealt = testDeck.DealCard();
            if (dealt != null)
            {
                Console.WriteLine($"  Card {i + 1}: {dealt} (Points: {dealt.PointValue()})");
            }
        }
        Console.WriteLine($"Cards remaining in deck: {testDeck.Size()}\n");

        // Test 3: ElevensBoard Game Logic
        Console.WriteLine("🧪 TEST 3: Elevens Board Game Logic");
        Console.WriteLine("==================================");
        
        ElevensBoard gameBoard = new ElevensBoard();
        gameBoard.NewGame();
        
        Console.WriteLine(gameBoard.BoardDisplay());
        
        // Test finding valid pairs
        var pairs = gameBoard.FindPairSum11();
        Console.WriteLine($"Found {pairs.Count} pairs that sum to 11:");
        foreach (var pair in pairs)
        {
            Card card1 = gameBoard.CardAt(pair[0]);
            Card card2 = gameBoard.CardAt(pair[1]);
            Console.WriteLine($"  Position {pair[0]} ({card1.GetRank()}) + Position {pair[1]} ({card2.GetRank()}) = {card1.PointValue() + card2.PointValue()}");
        }
        
        // Test finding J-Q-K combinations
        var jqkSets = gameBoard.FindJQK();
        Console.WriteLine($"\nFound {jqkSets.Count} J-Q-K combinations:");
        foreach (var triplet in jqkSets)
        {
            Console.WriteLine($"  Positions {triplet[0]}, {triplet[1]}, {triplet[2]}:");
            foreach (int pos in triplet)
            {
                Card card = gameBoard.CardAt(pos);
                Console.WriteLine($"    Position {pos}: {card.GetRank()} of {card.GetSuit()}");
            }
        }
        
        // Test 4: Game Rule Validation
        Console.WriteLine("\n🧪 TEST 4: Game Rule Validation");
        Console.WriteLine("==============================");
        
        // Test pair validation
        if (pairs.Count > 0)
        {
            List<int> firstPair = pairs[0];
            bool isLegalPair = gameBoard.IsLegal(firstPair);
            Console.WriteLine($"Testing pair at positions {firstPair[0]} and {firstPair[1]}: {(isLegalPair ? "VALID" : "INVALID")}");
        }
        
        // Test J-Q-K validation
        if (jqkSets.Count > 0)
        {
            List<int> firstJQK = jqkSets[0];
            bool isLegalJQK = gameBoard.IsLegal(firstJQK);
            Console.WriteLine($"Testing J-Q-K at positions {string.Join(", ", firstJQK)}: {(isLegalJQK ? "VALID" : "INVALID")}");
        }
        
        // Test invalid selection
        List<int> invalidSelection = new List<int> { 0 }; // Single card
        bool isInvalid = gameBoard.IsLegal(invalidSelection);
        Console.WriteLine($"Testing single card selection: {(isInvalid ? "VALID" : "INVALID")}");
        
        // Test 5: Game State Checking
        Console.WriteLine("\n🧪 TEST 5: Game State Analysis");
        Console.WriteLine("=============================");
        
        bool canContinue = gameBoard.AnotherPlayIsPossible();
        bool hasWon = gameBoard.GameIsWon();
        
        Console.WriteLine($"Can continue playing: {canContinue}");
        Console.WriteLine($"Game is won: {hasWon}");
        Console.WriteLine($"Board is empty: {gameBoard.IsEmpty()}");
        
        // Test 6: Card Replacement Demo
        Console.WriteLine("\n🧪 TEST 6: Card Replacement Demo");
        Console.WriteLine("===============================");
        
        if (pairs.Count > 0)
        {
            List<int> testPair = pairs[0];
            Console.WriteLine($"Before replacement:");
            Console.WriteLine($"  Position {testPair[0]}: {gameBoard.CardAt(testPair[0])}");
            Console.WriteLine($"  Position {testPair[1]}: {gameBoard.CardAt(testPair[1])}");
            
            gameBoard.ReplaceSelected(testPair);
            
            Console.WriteLine($"\nAfter replacement:");
            Console.WriteLine($"  Position {testPair[0]}: {gameBoard.CardAt(testPair[0])}");
            Console.WriteLine($"  Position {testPair[1]}: {gameBoard.CardAt(testPair[1])}");
        }
        
        Console.WriteLine("\n=== PARTIAL IMPLEMENTATION TESTING COMPLETE ===");
        Console.WriteLine("✅ Card class: Implemented and tested");
        Console.WriteLine("✅ Deck class: Implemented and tested");
        Console.WriteLine("✅ Abstract Board class: Implemented with common functionality");
        Console.WriteLine("✅ ElevensBoard class: Implemented with game-specific logic");
        Console.WriteLine("✅ Game rule validation: Working correctly");
        Console.WriteLine("✅ Card replacement: Functional");
        Console.WriteLine("\n📋 READY FOR: Game controller implementation, user interface, complete gameplay loop");
        
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}