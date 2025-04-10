

https://github.com/user-attachments/assets/b32fee92-8f62-4a30-bd56-281157792e2e

# level4TankPrefab
This prefab represents a level4Tank or a boss tank in Unity. It includes essential components for level 4 tank Attritube, AIControl, animation, and collision detection.
## Components
1. Level 4 Tank Script
This script controls the attributes for the level 4 tank, which is Health, actionpoints, and damage that it can inflict.
2. Sprite Renderer
Displays the Level 4 Tank sprite.
Customize the Sprite Renderer component to change the Tank's appearance.
Ensure that the sorting layer and order in layer are set appropriately to display correctly in your game's environment.
3. Animator
Controls the Tamks idle animation.
Attach an Animator Controller with states and transitions for each animation.
Adjust the Animator's parameters to trigger animations through the Slime script (e.g., isMoving, isAttacking, isDead).
4. Polygon Collider 2D 2D
Allows the Tank to get hit by player projectiles.
5. AI Control Script
Manages the movement, and shooting algorithms for the enemy tank. 
## Setup Instructions
Drag the tank prefab into the scene.
Ensure the all the relavent scripts are assigned to the prefab to control movement, or attributes.
Assign an Animator Controller with relevant animations to the Animator component.
Test the tank in play mode to confirm behavior and animations.
## Requirements
Unity 6000.0.34f1 or later
### Author 
Brenden Godbehere
## Spritesheet author
(https://free-game-assets.itch.io/)
