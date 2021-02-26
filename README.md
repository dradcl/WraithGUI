# WraithGUI
Features and showcase of a Phasmophobia mod menu. Stars ⭐ are appreciated <3
![Alt text](https://media.discordapp.net/attachments/630277945507708928/814988839184891904/game.png?width=1039&height=584 "")

# Information
This is a very early release of the mod with minimal features. Features are as of now and subject to change. Bugs are to be expected and reporting them would be appreciated :)
- Note: Only built for the last main branch Mono version of the game. Installation instructions can be found [here](#game-installation)

# Features
- Ghost Menu
  - Ghost spawning that is completely customized (type, name, age, favorite room, etc)
  - Force hunting mode to activate hunts (needs to be fixed, works at random)
  - Always visible or never visible ghosts
  - List information about all ghosts in the current level

- Player Menu
  - Player speed slider
  - Inventory editor (IDEA)
  - FOV slider (IDEA)

- Level Menu
  - TODO

- Color Menu
  - Change the colors of the menu.
  - Enable or disable fade (IDEA)

# Game Installation
This mod functions on the last Mono version of the game because it relies on direct editing of Assembly-Csharp.dll.  

- Note: You **must** own this game on Steam to proceed with this method, I don't condone pirating of games.  

**How to install**:  
1. Download the latest release of [DepotDownloader](https://github.com/SteamRE/DepotDownloader/releases) and extract it to a folder  
2. Download "PhasDownloader.bat" from this repo, put it in the same folder as the DepotDownloader files, right-click the bat, select edit, and add your Steam username and password next to their respective arguments ``-username`` and ``-password`` (Your credentials are not logged or vulnerable by doing this)  
3. Run the batch file and if you have Steam Guard on it will ask for a 2FA code. If not, it should automatically start to download  
4. Afterwards, it should create a ``depots`` folder and contained within it is your game installation. Mine was named a bunch of numbers so I assume it always is.

# Mod Installation
1. Follow the "Automated Installation" instructions for MelonLoader [here](https://melonwiki.xyz/#/?id=requirements)  
2. Download both DLLs from the [DLLs folder](https://github.com/karmakittenx/WraithGUI/tree/main/DLLS)  
3. Open your game folder to this directory ``Phasmophobia_Data\Managed`` and drag + replace those DLLs in  
4. Put the latest [WraithGUI](https://github.com/karmakittenx/WraithGUI/releases) in the ``Mods\`` folder.
5. Launch the game, start a level, click F1, and enjoy!
