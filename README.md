#  __***Group 12 Project MVP***__ - LSU CSC 3380 Object Oriented Design Fall 2025

Group Members
- Aidan Penton
- Vanik Makaryan
- Michael Jennings
- Joshua Walther
- Corbin Brescher

## PROJECT MVP OUTLINE:

First Person Shooter, rouge-like game created in the Unity Engine. In our game, the player clears randomly generated rooms of enemies to unlock rewards that make them stronger and give them new abilites

### MAIN FEATURE: 

Run-n-gun game, player can move jump and shoot their weapon to deal damage to enemies. There is one enemy type called the runner that tries to chase the player and attack then in close range. The player enters rooms with enemies and defeats them to progress

### Supplemental Features:
 - **Random Room Generation** - Four rooms are generated each round, and each is created from four randomly chosen tiles placed together, allowing for a large variety of unique room combinations. These tiles have set spawn points for enemies, and the room has a portal that connects it to a portal in the main room. From the main room, the player can see upgrades assigned to each room on a screen, and enter the portal for their desired upgrade, clear the room of enemies, then return to the main room. At that point four new rooms are created
   
 - **Upgrade and Unlock System** - When all of the enemies are defeated in a room, the player receives an upgrade reward that can increase their base stats, or unlock a movement ability. Some upgrades are repeatable while others can only be acquired once. These upgrades are randomly assigned to each room when they are created. Stats include things like the players health, stamina, speed, damage, number of jumps etc. These upgrades and stats can be seen in a menu while playing the game
   
 - **Advanced Movement System** -The player can crouch, run, slide, jump mulitple times in the air, and dash foward, all at the cost of stamina that regenerates over time. These abilities can be unlocked and improved with upgrades. The player can go up and slide down slopes, and speed is gained and converserved

## ACCESSING OUR PROJECT:
 - There is an Windows excutable called "CSC3380_Group12_Project" in the build folder "Builds" that allow you to run are game
 - Our C# scripts our contained in *CSC3380_Group12_Project/CSC3380_Group12_Project/Assets/Scripts*
 - If you wish to open our game in the Unity Editor, our project folder is the folder *CSC3380_Group12_Project* in our Repository, not the Repository folder itself. Our project uses unity editor version 6000.2.24f1. To open it in Unity, download the Unity Hub([Download](https://unity.com/download)) and the correct editor version, then click *Add/Add project* from disk and select the project folder

## HOW TO PLAY:
### Controls
- W A S D - horizontal movemment controls
- SPACE - jump
- CTRL - Slide, need to be moving to slide, REQUIRES UNLOCK
- R - Reload weapon, need to manually reload when gun is empty
- C - Crouch
- F - Dash, REQUIRES UNLOCK
- TAB - Acquired Upgrades and Player Stats menu
- ESC - Pause menu, under option can change mouse sensitivity

### Directions
The game opens on the start screen, press PLAY to begin. In the center of the main room, there are four doorways each with a portal inside that links to a room. Above them are screens with the rewards for each room. Move into a portal to select a room. In the room there will be enemies that need to be killed in order to leave. Once all enemies are dead, the portal will reactivate and you will recieve an  ugprade. You can press TAB to see your upgrades and your current stat values. Return to the portal to go back to the main room and select a new reward. If you fall of or need to restart, just press ESC and click QUIT to head back to the start screen.

Enjoy!

