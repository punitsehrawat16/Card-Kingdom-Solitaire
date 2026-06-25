# Card Kingdom Solitaire

## Candidate Name

**Punit Sehrawat**

## Unity Version

**Unity 6000.3.9f1**

## Time Taken

Approximately **2 Days**

---

# Project Overview

Card Kingdom Solitaire is a 2D card game developed in Unity as part of the Unity Developer Internship assignment.

The objective is to play cards from the player's hand onto the center pile by following the game rules:

* Play a card with the **same suit**, or
* Play a card with a **higher rank** than the current center card.

The project focuses on demonstrating Unity fundamentals, clean architecture, object-oriented programming principles, and event-driven communication between gameplay systems.

---

# Project Structure

### Assets/Scenes

Contains the gameplay scene and project scenes.

### Assets/Scripts

#### Card

Contains the behaviour and interaction logic for individual cards.

#### Data

Contains:

* CardData
* Suit enum
* Rank enum

#### Managers

**GameManager**

* Controls gameplay flow
* Handles game rules and card validation
* Manages player hand
* Updates the center card
* Controls overall game state

**DeckManager**

* Creates the 52-card deck
* Shuffles the deck
* Draws cards
* Tracks remaining cards

**ScoreManager**

* Handles score updates
* Stores and updates the high score using PlayerPrefs

**UIManager**

* Updates gameplay UI
* Displays score and high score
* Displays remaining deck count
* Displays gameplay and error messages

### Assets/Asset_PlayingCards

Contains the imported playing card sprites.

### Assets/Graphics

Contains the background and supporting visual assets.

### Assets/Settings

Contains the Universal Render Pipeline configuration.

---

# Architecture

The project follows a modular architecture with clear separation of responsibilities.

Gameplay systems communicate using C# Events (Observer Pattern), reducing direct dependencies between managers and improving maintainability.

---

# Implemented Features

* Standard 52-card deck generation
* Random deck shuffling
* Initial card distribution
* Center card gameplay
* Card validation (Same Suit / Higher Rank)
* Dynamic card drawing
* Score system (+10 per valid move)
* High Score using PlayerPrefs
* Remaining deck counter
* Gameplay feedback messages
* Event-driven communication between gameplay systems

---

# Bonus Features

Due to the assignment deadline, optional bonus features such as drag-and-drop interaction, card animations, sound effects, and background music were not implemented.

---

# Assets Used

* Playing card sprites from **Assets/Asset_PlayingCards**
* Background texture from **Assets/Graphics**
* TextMesh Pro for UI

---

# Notes

This project was developed using **Unity 6000.3.9f1 (URP 2D)**.

The submission focuses on implementing the core gameplay systems, modular architecture, and clean separation of responsibilities within the assignment timeframe.
