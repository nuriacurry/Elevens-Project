# ELEVENS SOLITAIRE GAME - PROJECT 1 (PARTIAL IMPLEMENTATION)

## Project Overview
This is the first submission for the Elevens solitaire game project, implementing the core class design and partial game functionality based on object-oriented programming principles.

## What Has Been Implemented

### ✅ Core Classes Complete

#### **1. Supporting Enumerations**
- **`Rank.cs`** - Card ranks (Ace=1 through King=13) with point values
- **`Suit.cs`** - Four card suits (Clubs, Diamonds, Hearts, Spades)

#### **2. Card Class (`Card.cs`)**
- **Fields:** rank, suit, faceUp status
- **Constructor:** Creates card with specified rank and suit
- **Methods:** 
  - `GetRank()`, `GetSuit()`, `IsFaceUp()` - Property accessors
  - `FlipCard()` - Toggle face up/down state
  - `PointValue()` - Returns numeric value for game logic
  - `ToString()` - String representation
  - `Equals()`, `GetHashCode()` - Object comparison

#### **3. Deck Class (`Deck.cs`)**
- **Functionality:** Complete 52-card deck management
- **Methods:**
  - `Shuffle()` - Fisher-Yates algorithm randomization
  - `DealCard()` - Remove and return top card (flipped face up)
  - `IsEmpty()`, `Size()` - Deck state checking
  - `ToString()` - Deck information display

#### **4. Abstract Board Class (`Board.cs`)**
- **Purpose:** Base class for all solitaire game variants
- **Common Functionality:**
  - `NewGame()` - Initialize new game with shuffled deck
  - `Deal(k)` - Deal k cards to empty board positions
  - `CardAt(k)` - Access card at specific position
  - `ReplaceSelected()` - Remove selected cards