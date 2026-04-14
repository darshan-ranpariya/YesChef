# YesChef - Cooking Management Game

A fast-paced cooking simulation game built with Unity where players manage orders, prepare ingredients, and serve customers within a time limit.

## 🎮 Game Overview

YesChef is a cooking management game where you play as a chef in a busy restaurant. Your goal is to fulfill customer orders by collecting ingredients, preparing them at various stations, and serving them before time runs out. The game features a 3-minute time limit with scoring based on speed and accuracy.

## 🕹️ How to Play

### Basic Controls
- **Movement**: Use WASD or arrow keys to move around the kitchen
- **Interaction**: Press the interact button (default: Space or E) to interact with stations and customers

### Game Mechanics

#### Ingredients & Preparation
- **Vegetables**: Must be chopped at the Table station before serving
- **Meat**: Must be cooked at the Stove station before serving
- **Cheese**: Ready to serve immediately from the Fridge

#### Stations
- **Fridge**: Dispenses ingredients (Vegetables, Cheese, Meat)
- **Table**: Chops vegetables into prepared state
- **Stove**: Cooks meat into prepared state
- **Trash**: Discards unwanted ingredients
- **Customer Windows**: 4 windows where you deliver completed orders

#### Scoring System
- **Base Score**: Determined by the ingredients in each order
- **Time Penalty**: 1 point deducted per second the order takes to complete
- **Final Score**: Base Score - Time Penalty
- **High Score**: Persistent high score tracking

### Game Flow
1. **Main Menu**: Start the game or quit
2. **Playing**: 3-minute timer begins, manage orders and score points
3. **Game Over**: View final score and compare with high score

## 🛠️ Technical Details

### Architecture
- **Event-Driven Design**: Uses a static GameEvents class for decoupled communication
- **State Management**: GameManager handles game states (MainMenu, Playing, Paused, GameOver)
- **Component-Based**: Modular station system with IInteractable interface

### Key Components

#### Core Systems
- **GameManager**: Main game loop, state management, scoring
- **OrderManager**: Generates random orders, manages customer windows
- **UIManager**: Handles all UI updates and state transitions
- **PlayerController**: Character movement and input handling
- **PlayerInteractor**: Raycast-based interaction system

#### Stations
- **Fridge**: Ingredient dispenser
- **Table**: Vegetable preparation station
- **Stove**: Meat cooking station with 2 slots
- **Trash**: Item disposal
- **CustomerWindow**: Order fulfillment points

#### Data Structures
- **IngredientData**: ScriptableObject for ingredient definitions
- **Ingredient**: Runtime ingredient instances with state management

### Technologies Used
- **Unity 6000.0.45f1**: Game engine
- **C#**: Primary programming language
- **Unity Input System**: Modern input handling
- **TextMesh Pro**: UI text rendering
- **Async/Await**: For timing operations (Unity 6+)

## 📁 Project Structure

```
Assets/YesChef/
├── Scripts/
│   ├── Core/
│   │   ├── GameEvents.cs      # Event bus for decoupled communication
│   │   ├── IInteractable.cs   # Interaction interface
│   │   ├── Ingredient.cs      # Runtime ingredient component
│   │   └── IngredientData.cs  # Ingredient ScriptableObject
│   ├── Managers/
│   │   ├── GameManager.cs     # Main game state and loop
│   │   ├── OrderManager.cs    # Order generation and scoring
│   │   └── UIManager.cs       # UI state management
│   ├── Player/
│   │   ├── PlayerController.cs    # Character movement
│   │   └── PlayerInteractor.cs    # Interaction system
│   └── Stations/
│       ├── CustomerWindow.cs  # Order delivery points
│       ├── Fridge.cs         # Ingredient dispenser
│       ├── Stove.cs          # Meat cooking station
│       ├── Table.cs          # Vegetable chopping station
│       └── Trash.cs          # Item disposal
├── Scenes/
│   └── SampleScene.unity     # Main game scene
├── Prefabs/                  # Game object prefabs
├── Materials/                # Visual materials
└── Ingredients/              # Ingredient ScriptableObjects
```

## 🚀 Getting Started

### Prerequisites
- Unity 6000.0.45f1 or later
- Windows/Mac/Linux development environment

### Installation
1. Clone or download the project
2. Open in Unity Hub
3. Open the `SampleScene` in `Assets/YesChef/Scenes/`
4. Press Play to start the game

### Building
1. Go to `File > Build Settings`
2. Select your target platform
3. Click `Build` and choose output directory

## 🎯 Game Features

- **Real-time Cooking**: Asynchronous cooking and preparation timers
- **Dynamic Orders**: Randomly generated customer orders
- **Time Pressure**: 3-minute game sessions with scoring penalties
- **Persistent High Scores**: Save system for high score tracking
- **Intuitive UI**: Clean interface showing score, time, and order status
- **Smooth Controls**: Responsive character movement and interaction

## 🔧 Development Notes

### Code Quality
- Clean, readable C# code with consistent naming conventions
- Event-driven architecture for maintainable systems
- Modular station design with interface-based interactions
- Comprehensive error handling and state management

### Performance Considerations
- Efficient async operations using Unity's Awaitable
- Minimal allocations in update loops
- Optimized raycast-based interaction system

## 📝 Assignment Notes

This project demonstrates:
- **Object-Oriented Programming**: Inheritance, interfaces, and encapsulation
- **Game Architecture**: State machines, event systems, and component patterns
- **Unity Development**: Modern Unity features, input systems, and UI
- **Asynchronous Programming**: Unity's async/await patterns
- **Data Management**: ScriptableObjects for game data

## 👥 Credits

Developed as a Unity learning project demonstrating modern game development practices and clean code architecture.

---

**Enjoy cooking up some high scores in YesChef! 🍳**
