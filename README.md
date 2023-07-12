# CowtasticMods
This repo contains ~most of~ all the BepInEx 5 plugins I've written for Cowtastic Cafe. This document covers the _recommended_ way to install mods for Cowtastic Cafe 1.1.0.0.

Check [releases](https://github.com/telnetr4/CowtasticMods/releases) for the downloads.

If you have never installed mods or are setting up a new install of the game, start reading from the "Prereq Installations" section and work your way down this document. If you are adding a mod to an already modded game you can start at the "Mod Installation" section.


# Prereq Installations
## ~~Unstripped DLLs (CorLibs)~~
_Cowtastic Cafe 1.1.0.0 includes the unstripped DLLs, so you can skip this step. For instructions on how to install these DLLs for the previous version of Cowtastic Cafe, please refer to the file history for README.md  or the Readme included with the mod you are installing._

## Install BepInEx
The following installation instructions are derivative of the [BepInEx install guide](https://docs.bepinex.dev/articles/user_guide/installation/index.html):

1. Download the BepInEx 5 release for your OS from the [BepInEx Github releases](https://github.com/BepInEx/BepInEx/releases/tag/v5.4.21).
	- On Windows, download the `x64` version.
	- On Linux/MacOS, download archive with designation `nix`.

2. Extract the contents into the game root. After you have downloaded the correct game version, extact the contents of the archive into the game folder.

	- On Windows, the game root folder is where the game executable is located.
	- On macOS, the root folder is where the game <Game>.app is located.
  	- On Linux, the game root folder is where the executable <Game>.x86_64 is located.

3. Do a first run to generate configuration files
	- On Windows, simply run the game executable. This should generate BepInEx configuration file into `BepInEx/config` folder and an initial log file `BepInEx/LogOutput.txt`. If the log and config show up that means you have installed BepInEx. You can now install mods (go to the "Mod Installation" section)
	- On Linux/MacOS, first, open the included run script `run_bepinex.sh` in a text editor of your choice. Edit the line `executable_name="";` to the name of the executable.
		- On Linux, this is simply the name of the game executable
		
		- On macOS, this is `Cowtastic Cafe.app`
	
		 Finally, open the terminal in the game folder and make `run_bepinex.sh` script executable:
	
		 `chmod u+x run_bepinex.sh`
		 
		 You can now run BepInEx by executing the run script:
		 
		 `./run_bepinex.sh`
		 
		 This should generate a BepInEx configuration file into `BepInEx/config` folder and an initial log file 'BepInEx/LogOutput.txt'.

# Mod Installation:
You can get pre-compiled versions of the mods from the [releases](https://github.com/telnetr4/CowtasticMods/releases) section of the repo. I would *highly* recommend reading the instructions from the Readme included with the mod. Even though many of the mods are for older versions of Cowtastic Cafe, many of them will still work and are some variation of:

1. Make sure you have BepInEx 5 installed on your system. If you don't, follow the instructions provided above or BepInEx's documentation to install it.
2. Extract the mod's DLLs into the `BepInEx/plugins` folder in your game's directory.
3. Launch the game and check if the plugin is working properly.
4. If the config file `telnet.INSERTMODNAMEHERE.cfg` (where `INSERTMODNAMEHERE` is the name or abbreviation of the mod name) appears in `BepInEx/config`, that means it's working! If it doesn't, check the troubleshooting section. (Note: naming convention may vary depending on who wrote the mod).

# Mod Configuration:
Some mods have configurable settings, located in the `BepInEx/config` folder. You can modify these settings using a text editor like Notepad or Visual Studio Code.

# Mod Uninstallation:
To uninstall the plugin, simply delete the plugin files from the BepInEx/plugins folder in your game's directory.

# Troubleshooting:
If you encounter any issues with the plugin, try the following steps:

- Check if the plugin is compatible with your game version.
- Check if the plugin is conflicting with any other plugins you have installed.
- Check if the plugin is installed correctly and in the right folder.
- Check the plugin's documentation or readme file for any troubleshooting tips.
- Check that you didn't download the mod's source files instead of the compiled mods. There should not be .cs files included with any installable mod.
- Scream in frustration.
- Check that BepInEx runs. If `BepInEx/config` does not generate on starting your game, that means BepInEx isn't starting.
- Check that you have installed the correct version of BepInEx 5 for the OS you are using. These mods have not been tested with BepInEx 6 (if you test them in 6 and they work, let me know).
- If you are playing a version of Cowtastic Cafe lower than 1.1.0.0, make sure your game has the unstripped DLLs.

# Contact:
If you encounter any issues that you cannot resolve, you can contact me on Discord at telnet.8242.

You can also try the [Cowtastic Cafe mod channel](https://discord.com/channels/740342492599156876/1122014015032406036) in PreggoPixel's Games Discord server, but that may be slower.

Bugs can be reported at https://github.com/telnetr4/CowtasticMods/issues


Please provide as much information as possible, including any error messages or logs, to help identify and resolve the issue

*Original README.TXT generated by ChatGPT then edited. I'm lazy.*
