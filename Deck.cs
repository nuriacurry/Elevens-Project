using System;
using System.Collections.Generic;

/// <summary>
/// Represents a deck of 52 playing cards
/// Provides functionality to shuffle, deal cards, and track remaining cards
/// </summary>
public class Deck
{
    private List<Card> cards;
    private Random random;

    /// <summary>
    /// Creates a new standard 52-card deck
    /// All cards start face down and unshuffled
    /// </summary>
    public Deck()
    {
        cards = new List<Card>();
        random = new Random();
        
        // Create all 52 cards (13 ranks × 4 suits)
        foreach (Suit suit in Enum.GetValues<Suit>())
        {
            foreach (Rank rank in Enum.GetValues<Rank>())
            {
                cards.Add(new Card(rank, suit));
            }
        }
    }

    /// <summary>
    /// Shuffles the deck using Fisher-Yates algorithm
    /// Randomizes the order of all remaining cards
    /// </summary>
    public void Shuffle()
    {
        for (int i = cards.Count - 1; i > 0; i--)
        {
            int randomIndex = random.Next(0, i + 1);
            
            // Swap cards at positions i and randomIndex
            Card temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Deals the top card from the deck
    /// </summary>
    /// <returns>The top card, or null if deck is empty</returns>
    public Card DealCard()
    {
        if (cards.Count > 0)
        {
            Card topCard = cards[0];
            cards.RemoveAt(0);
            topCard.FlipCard(); // Dealt cards are face up
            return topCard;
        }
        return null;
    }

    /// <summary>
    /// Checks if the deck is empty
    /// </summary>
    /// <returns>True if no cards remain</returns>
    public bool IsEmpty()
    {
        return cards.Count == 0;
    }

    /// <summary>
    /// Gets the number of cards remaining in the deck
    /// </summary>
    /// <returns>Number of cards left</returns>
    public int Size()
    {
        return cards.Count;
    }

    /// <summary>
    /// Returns a string representation of the deck
    /// </summary>
    /// <returns>String showing deck size and sample cards</returns>
    public override string ToString()
    {
        if (cards.Count == 0)
        {
            return "Empty deck";
        }
        
        string result = $"Deck with {cards.Count} cards. ";
        if (cards.Count > 0)
        {
            result += $"Top card: {cards[0].GetRank()} of {cards[0].GetSuit()}";
        }
        return result;
    }
}