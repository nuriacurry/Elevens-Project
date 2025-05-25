using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// Abstract base class for solitaire game boards
/// Provides common functionality that all solitaire games share
/// Specific games inherit from this and implement their own rules
/// </summary>
public abstract class Board
{
    protected Card[] cards;      // Cards currently on the board
    protected Deck deck;         // Deck of remaining cards
    protected int boardSize;     // Number of cards on board

    /// <summary>
    /// Creates a new game board with specified size
    /// </summary>
    /// <param name="size">Number of cards to display on board</param>
    public Board(int size)
    {
        boardSize = size;
        cards = new Card[boardSize];
        deck = new Deck();
    }

    /// <summary>
    /// Starts a new game by shuffling deck and dealing initial cards
    /// </summary>
    public virtual void NewGame()
    {
        deck = new Deck();
        deck.Shuffle();
        Deal(boardSize);
    }

    /// <summary>
    /// Deals k cards from the deck to fill empty board positions
    /// </summary>
    /// <param name="k">Number of cards to deal</param>
    public void Deal(int k)
    {
        int cardsDealt = 0;
        for (int i = 0; i < boardSize && cardsDealt < k; i++)
        {
            if (cards[i] == null && !deck.IsEmpty())
            {
                cards[i] = deck.DealCard();
                cardsDealt++;
            }
        }
    }

    /// <summary>
    /// Gets the size of the board
    /// </summary>
    /// <returns>Number of card positions on board</returns>
    public int Size()
    {
        return boardSize;
    }

    /// <summary>
    /// Checks if the board is empty (no cards)
    /// </summary>
    /// <returns>True if board has no cards</returns>
    public bool IsEmpty()
    {
        foreach (Card card in cards)
        {
            if (card != null)
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Gets the card at a specific board position
    /// </summary>
    /// <param name="k">Board position (0-based index)</param>
    /// <returns>Card at position k, or null if position is empty</returns>
    public Card CardAt(int k)
    {
        if (k >= 0 && k < boardSize)
        {
            return cards[k];
        }
        return null;
    }

    /// <summary>
    /// Replaces selected cards with new cards from deck
    /// </summary>
    /// <param name="selectedIndexes">Positions of cards to replace</param>
    public void ReplaceSelected(List<int> selectedIndexes)
    {
        // Remove selected cards
        foreach (int index in selectedIndexes)
        {
            if (index >= 0 && index < boardSize)
            {
                cards[index] = null;
            }
        }

        // Replace with new cards from deck
        Deal(selectedIndexes.Count);
    }

    /// <summary>
    /// Creates a string representation of the current board
    /// </summary>
    /// <returns>String showing all cards on board with positions</returns>
    public string BoardDisplay()
    {
        StringBuilder display = new StringBuilder();
        display.AppendLine("Current Board:");
        
        for (int i = 0; i < boardSize; i++)
        {
            if (cards[i] != null)
            {
                display.AppendLine($"Position {i}: {cards[i]}");
            }
            else
            {
                display.AppendLine($"Position {i}: Empty");
            }
        }
        
        display.AppendLine($"Cards remaining in deck: {deck.Size()}");
        return display.ToString();
    }

    /// <summary>
    /// Checks if the game is won (all cards removed from deck and board)
    /// </summary>
    /// <returns>True if game is won</returns>
    public bool GameIsWon()
    {
        return deck.IsEmpty() && IsEmpty();
    }

    /// <summary>
    /// Abstract method to check if another play is possible
    /// Each game type implements its own logic for detecting valid moves
    /// </summary>
    /// <returns>True if more moves are available</returns>
    public abstract bool AnotherPlayIsPossible();

    /// <summary>
    /// Abstract method to check if a selection of cards is legal
    /// Each game type implements its own rules for valid card combinations
    /// </summary>
    /// <param name="selectedIndexes">Indexes of selected cards</param>
    /// <returns>True if selection follows game rules</returns>
    public abstract bool IsLegal(List<int> selectedIndexes);
}