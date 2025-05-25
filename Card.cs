using System;

/// <summary>
/// Represents a playing card with rank and suit
/// Cards can be face up or face down and provide point values for game logic
/// </summary>
public class Card
{
    private Rank rank;
    private Suit suit;
    private bool faceUp;

    /// <summary>
    /// Creates a new card with specified rank and suit
    /// Cards start face down by default
    /// </summary>
    /// <param name="rank">The rank of the card</param>
    /// <param name="suit">The suit of the card</param>
    public Card(Rank rank, Suit suit)
    {
        this.rank = rank;
        this.suit = suit;
        this.faceUp = false;
    }

    /// <summary>
    /// Gets the rank of this card
    /// </summary>
    /// <returns>The card's rank</returns>
    public Rank GetRank()
    {
        return rank;
    }

    /// <summary>
    /// Gets the suit of this card
    /// </summary>
    /// <returns>The card's suit</returns>
    public Suit GetSuit()
    {
        return suit;
    }

    /// <summary>
    /// Checks if this card is face up
    /// </summary>
    /// <returns>True if face up, false if face down</returns>
    public bool IsFaceUp()
    {
        return faceUp;
    }

    /// <summary>
    /// Flips the card over (face up becomes face down, face down becomes face up)
    /// </summary>
    public void FlipCard()
    {
        faceUp = !faceUp;
    }

    /// <summary>
    /// Gets the point value of this card for game logic
    /// Ace = 1, Number cards = face value, Face cards = 11, 12, 13
    /// </summary>
    /// <returns>The point value of the card</returns>
    public int PointValue()
    {
        return (int)rank;
    }

    /// <summary>
    /// Returns a string representation of this card
    /// </summary>
    /// <returns>String showing rank and suit (e.g., "Ace of Spades")</returns>
    public override string ToString()
    {
        if (faceUp)
        {
            return $"{rank} of {suit}";
        }
        else
        {
            return "Face Down Card";
        }
    }

    /// <summary>
    /// Checks if this card equals another card (same rank and suit)
    /// </summary>
    /// <param name="obj">Object to compare with</param>
    /// <returns>True if cards are equal</returns>
    public override bool Equals(object obj)
    {
        if (obj is Card other)
        {
            return this.rank == other.rank && this.suit == other.suit;
        }
        return false;
    }

    /// <summary>
    /// Gets hash code for this card
    /// </summary>
    /// <returns>Hash code based on rank and suit</returns>
    public override int GetHashCode()
    {
        return rank.GetHashCode() ^ suit.GetHashCode();
    }
}