using System;

/// <summary>
/// Enumeration for card ranks in the Elevens solitaire game
/// Values correspond to card point values for game logic
/// </summary>
public enum Rank
{
    Ace = 1,     // Ace = 1 point in Elevens
    Two = 2,
    Three = 3,
    Four = 4,
    Five = 5,
    Six = 6,
    Seven = 7,
    Eight = 8,
    Nine = 9,
    Ten = 10,
    Jack = 11,   // Used in J-Q-K combinations
    Queen = 12,  // Used in J-Q-K combinations  
    King = 13    // Used in J-Q-K combinations
}