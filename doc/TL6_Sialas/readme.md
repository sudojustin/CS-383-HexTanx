# Sound Manager
This prefab represents the SoundManager in Unity, a singleton responsible for managing and playing audio in the game. It includes essential components for playing sound effects such as movement, shooting, and pickup sounds, ensuring consistent audio playback across scenes.
## Components
1. Sound Manager Script
This script implements the singleton pattern to manage audio playback. It provides methods to play specific sound effects (e.g., PlayMoveSound, ShootSound, PickupSound), music and stop sounds as needed.
Customize each sound effect by assigning the specific sound mp4 in the inspector.
Add/Delete types of sounds by editing the serialized field variables at the top of script.
Change the scene the sound plays in by changing the names of the switch cases in the script.
3. Audio Source
Audio source for sound effects
Adjust the sound of sound effects
Ensure that the sorting layer and order in layer are set appropriately to display correctly in your game's environment.
4. Audio Source
Audio source for music.
Adjust volume of all music in inspector.
Can toggle if you want music to infinitley loop in inspector.
5. Audio Source
Audio source for picking up items sound effects.
Adjust volume for item pickup sounds in inspector.
## Setup Instructions
Drag the SoundManager prefab into the scene.
Ensure the all the soundManager script is assigned to the prefab.
Download any mp3 files that will be used.
Assign all relevant music and or sound effects to the components in the inspector for the script.
Adjust volume as needed.
Test the sound in play mode to confirm audio output.
## Requirements
Unity 6000.0.34f1 or later
### Author 
Sialas Tripp

