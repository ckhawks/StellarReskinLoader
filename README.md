# StellarReskinLoader
Reskin/Texture loader Il2Cpp BepInEx plugin for Puck

# Vision
Create a standardized format for reskin mods to be created by the community. Players install this plugin which loads all reskins available from their game files, and allows them to dynamically change between them from in game.

# Download
This plugin will be available through the Steam Workshop - but it is not ready yet!

# To-do List
## Stage 1
- Verify texture loading is possible at runtime

## Stage 2
- Create a prototype in-game interface for changing selected textures
- Sticks: select a vanilla public-facing skin, and then select a modded private skin
- Jerseys: select a jersey to replace an entire team color's jerseys
- View loaded packs
- View packs that have failed to load

## Stage 3
- Prototype loading different arenas
- 

# Creating a Reskin pack
pack.json
```json
{
  "name": "Toaster's Reskin Pack",
  "version": "1.0.0",
  "pack_version": 1,
  "reskins": [
    {
      "type": "attacker_stick",
      "series": "ToastStick",
      "model": "Very cool stick",
      "file": "textures/sticks/toaststick_very_cool_stick.png"
    },
    {
      "type": "goalie_stick",
      "series": "ToastStick",
      "model": "Very cool goalie stick",
      "file": "textures/sticks/toaststick_very_cool_goalie_stick.png"
    },
    {
      "type": "rink_ice",
      "name": "Toast's Ice"
      "file": "textures/ice/toasts_ice.png"
    },
    {
      "type": "jersey",
      "series": "PHL Jerseys",
      "model": "Team Awesome Jersey - Away",
      "team": "red",
      "file": "textures/jerseys/phl_team_awesome_away.png"
    },
  ]
}
```

# Help and Discussion
[Join the Toaster's Rink - Puck Modding Discord server.](http://discord.puckstats.io)

# Contributing
Pull requests are welcome!
