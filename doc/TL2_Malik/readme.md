# 🔥 Terrain Damage System (Unity)

This Unity system handles terrain-based damage using object-oriented principles like `virtual` and `override`. It supports a toggleable **"BC Mode"**, which changes how much damage is applied based on the player's settings.

---

## 📜 Overview

This script is attached to damaging terrain tiles. When the player or enemy enters the tile's range, damage is applied depending on whether **BC Mode** is active or not.

BC Mode is controlled through a toggle in the main menu and stored in Unity's `PlayerPrefs`.

---

## ⚙️ Features

- ✅ Virtual/Override damage system
- ✅ Switches behavior at runtime using BC Mode toggle
- ✅ Applies damage to both player and enemy tanks
- ✅ Disables after one use (`isactive`)
- ✅ Visual deactivation via `TurnOff()` method

---

## 📊 Behavior Table

| Mode      | Damage Class       | Damage Amount |
|-----------|--------------------|---------------|
| BC Mode   | `TerrainDamageBC`  | `0`           |
| Normal    | `TerrainDamage`    | `10`          |

---

## 🧩 Component Breakdown

### `TerrainDamageBC`
- Base class
- Returns `0` damage

### `TerrainDamage`
- Derived class
- Overrides `getDamage()` to return `10`

### `Terrains`
- Main behavior class attached to terrain tiles
- Uses polymorphism to determine damage
- Detects player or enemy entering the tile
- Disables after activation to prevent multiple triggers

---

## 🎮 How It Works

- During `Start()`, it checks `PlayerPrefs.GetInt("BCMode", 0)`
  - If set to `1`, uses base class: `Damage = new TerrainDamageBC()`
  - If set to `0`, uses override: `Damage = new TerrainDamage()`
- Uses `InvokeRepeating()` to keep references to player and enemy fresh
- Applies damage when the player is directly on the tile or when an enemy comes within `1.1f` units

---

## 🧪 Requirements

This system relies on the following external components:

- **PlayerTank**: Must contain:
  - `int GetHealth()`
  - `void SetHealth(int)`
- **TankType** (for enemy):
  - Must include `int health`
- **Tiles** (base class):
  - Must include a method `void TurnOff()` to visually disable the tile

---

## 📝 Usage Notes

- Ensure the player GameObject is named **`"PlayerTank"`**
- Enemies must have the tag **`"EnemyTank"`**
- The terrain GameObject this script is attached to should be placed at the correct map positions

---

## 🛠 Future Improvements

- Replace `GameObject.Find` with cached references or dependency injection
- Add a cooldown instead of one-time activation
- Trigger UnityEvents instead of directly handling damage
- Add editor GUI to preview damage state
- Better visual feedback for active/inactive tiles

---

## 📁 Folder Structure Suggestion


