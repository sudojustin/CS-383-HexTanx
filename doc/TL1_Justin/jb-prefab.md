# Shield Prefab Documentation

**Author:** Justin Baldwin

**Version:** 1.0

**Video Demo:** [Shield Prefab Demo](./ShieldPrefab.mp4) (MP4 format)

**Description:**
This prefab represents a basic Shield item in Unity. It increases the amount
of damage that the player can take by increasing the armor attribute.

- **ItemManager Script**
  - Controls the Shield's behavior, including the amount of armor that is added to the player.
  - Attach the ItemManager script to allow the player to increase their total armor.
  - Customize variables in the script to adjust the amount of armor given to the player.

- **Sprite Renderer**
  - Displays the Shield's sprite.
  - Customize the Sprite Renderer component to change the Shield's appearance.
  - Ensure that the sorting layer is set properly so the shield doesn't appear behind the game tiles.

- **Box Collider 2D**
  - Manages the Shield's collision boundary for player interaction.
  - Set the "Is Trigger" checkbox to allow for item pickup by the player.
  - Set the size of the collider to match the size of the Shield sprite for accurate collision detection.
  - Customize Offset, Size, and other properties to suit the desired physics behavior.

**Setup Instructions:**
1. Drag the Shield prefab into the scene.
2. Ensure the ItemManager script is configured with appropriate settings for movement and attack mechanics.
3. Adjust the Circle Collider 2D to ensure accurate collision detection with other game objects.
4. Test the Shield in play mode to confirm behavior and animations.

**Requirements:** Unity 6000.0.34f1 or later

**Spritesheet author:** [https://free-game-assets.itch.io/](https://free-game-assets.itch.io/)