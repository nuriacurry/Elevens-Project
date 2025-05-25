using System;
using System.Collections.Generic;

/// <summary>
/// Implements the Elevens solitaire game board
/// Players remove pairs that sum to 11 or J-Q-K combinations
/// </summary>
public class ElevensBoard : Board
{
    public const int BOARD_SIZE = 9;

    /// <summary>
    /// Creates a new Elevens game board with 9 card positions
    /// </summary>
    public ElevensBoard() : base(BOARD_SIZE)
    {
    }

    /// <summary>
    /// Checks if selected cards follow Elevens game rules
    /// Valid selections: 2 cards that sum to 11, or 3 cards that are J-Q-K
    /// </summary>
    /// <param name="selectedIndexes">Positions of selected cards</param>
    /// <returns>True if selection is valid</returns>
    public override bool IsLegal(List<int> selectedIndexes)
    {
        if (selectedIndexes.Count == 2)
        {
            return ContainsPairSum11(selectedIndexes);
        }
        else if (selectedIndexes.Count == 3)
        {
            return ContainsJQK(selectedIndexes);
        }
        return false;
    }

    /// <summary>
    /// Checks if another play is possible on the current board
    /// Game continues if there are pairs summing to 11 or J-Q-K combinations
    /// </summary>
    /// <returns>True if valid moves remain</returns>
    public override bool AnotherPlayIsPossible()
    {
        return ContainsPairSum11() || ContainsJQK();
    }

    /// <summary>
    /// Finds all pairs on the board that sum to 11
    /// </summary>
    /// <returns>List of pairs represented as lists of indexes</returns>
    public List<List<int>> FindPairSum11()
    {
        List<List<int>> pairs = new List<List<int>>();
        
        for (int i = 0; i < boardSize; i++)
        {
            for (int j = i + 1; j < boardSize; j++)
            {
                if (cards[i] != null && cards[j] != null)
                {
                    if (cards[i].PointValue() + cards[j].PointValue() == 11)
                    {
                        List<int> pair = new List<int> { i, j };
                        pairs.Add(pair);
                    }
                }
            }
        }
        
        return pairs;
    }

    /// <summary>
    /// Finds all J-Q-K combinations on the board
    /// </summary>
    /// <returns>List of J-Q-K triplets represented as lists of indexes</returns>
    public List<List<int>> FindJQK()
    {
        List<List<int>> triplets = new List<List<int>>();
        
        // Find all positions with Jacks, Queens, and Kings
        List<int> jacks = new List<int>();
        List<int> queens = new List<int>();
        List<int> kings = new List<int>();
        
        for (int i = 0; i < boardSize; i++)
        {
            if (cards[i] != null)
            {
                Rank rank = cards[i].GetRank();
                if (rank == Rank.Jack) jacks.Add(i);
                else if (rank == Rank.Queen) queens.Add(i);
                else if (rank == Rank.King) kings.Add(i);
            }
        }
        
        // Create all possible J-Q-K combinations
        foreach (int jack in jacks)
        {
            foreach (int queen in queens)
            {
                foreach (int king in kings)
                {
                    List<int> triplet = new List<int> { jack, queen, king };
                    triplets.Add(triplet);
                }
            }
        }
        
        return triplets;
    }

    /// <summary>
    /// Checks if the board contains any pair that sums to 11
    /// </summary>
    /// <returns>True if at least one valid pair exists</returns>
    private bool ContainsPairSum11()
    {
        return FindPairSum11().Count > 0;
    }

    /// <summary>
    /// Checks if specific selected cards contain a pair that sums to 11
    /// </summary>
    /// <param name="selectedIndexes">Indexes of selected cards</param>
    /// <returns>True if the two selected cards sum to 11</returns>
    private bool ContainsPairSum11(List<int> selectedIndexes)
    {
        if (selectedIndexes.Count != 2) return false;
        
        int index1 = selectedIndexes[0];
        int index2 = selectedIndexes[1];
        
        if (cards[index1] != null && cards[index2] != null)
        {
            return cards[index1].PointValue() + cards[index2].PointValue() == 11;
        }
        
        return false;
    }

    /// <summary>
    /// Checks if the board contains any J-Q-K combination
    /// </summary>
    /// <returns>True if at least one J-Q-K triplet exists</returns>
    private bool ContainsJQK()
    {
        return FindJQK().Count > 0;
    }

    /// <summary>
    /// Checks if specific selected cards contain J-Q-K combination
    /// </summary>
    /// <param name="selectedIndexes">Indexes of selected cards</param>
    /// <returns>True if selection contains one Jack, one Queen, and one King</returns>
    private bool ContainsJQK(List<int> selectedIndexes)
    {
        if (selectedIndexes.Count != 3) return false;
        
        bool hasJack = false, hasQueen = false, hasKing = false;
        
        foreach (int index in selectedIndexes)
        {
            if (cards[index] != null)
            {
                Rank rank = cards[index].GetRank();
                if (rank == Rank.Jack) hasJack = true;
                else if (rank == Rank.Queen) hasQueen = true;
                else if (rank == Rank.King) hasKing = true;
            }
        }
        
        return hasJack && hasQueen && hasKing;
    }
}