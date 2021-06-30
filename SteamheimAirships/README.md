## Steamheim Airships

This mod adds two airships, the Titan and the Sun Scout, into the game in what is hopefully the first of several mods focused on getting some steampunk elements into Valheim!

Showcase: https://www.youtube.com/watch?v=AhuGtn5LhQU

Adaptation of the large ship and the coding was done by myself, with the model and effects for the small ship put together by MagikarppSushii021194! It looks great and has some effects that aren't even enabled yet, make sure to tell him it's excellent! Also shoutout to Zethin64 for helping me test!

### Version 1.1.0

- Fixes config and asset bundle lookup to allow the renaming of the folder to any name.
- Allows the addition of custom ships through the use of asset bundles and configs.
- Added a check to prevent players from falling to death if ships are moved when they are offline.
- Added custom drag script to hopefully make small ships stay on top of the carrier better. Should not be very noticeable.
- Updated JVL to version 2.1.0 and moved airships into their own category on the hammer.

#### Custom Ships and Configs

- Guide: https://www.notion.so/Custom-Airships-e04d023133494a8b9be0d2a59862ff92

To use the sample working Tolroko Flyer, download the optional Tolroko file from the files page. Then, place the AssetBundles/tolroko file in the mod's Assets/AssetBundles folder, and the Configs/tolroko.json file in the mod's Assets/Configs folder. If you are reading this on Thunderstore, I can't add optional files, so go grab them from Nexus Mods.

If you would like to edit ship configs, you should copy the config to SteamheimAirships\Assets\CustomConfigs before changing it. I realized that without this option, your configs will be overwritten after each version update of this mod. If that already happened to you, I'm sorry, that's on me, but this should prevent it in the future.

### Details:

- Each ship has one control seat and some other seating for passengers, as well as some storage.
- Ships are controlled using standard directional movement to adjust thrust and turn the ship, and jump and crouch to adjust lift.
- Thrust and lift will stay at their set position until someone changes them, or everyone gets off the ship.
- The large ship is not affected by gravity, but the small ship is so it can land on the carrier. Due to how physics work in Unity, the large ship may still be pushed down if you put a lot of stuff on top of it.

Multiplayer was tested, but there may still be some networking bugs. Please report them if you run into any, and I will address them as soon as possible.

Since there may be bugs, the resource costs for both ships are relatively low. These will probably increase in a future release.

If you need anything else, feel free to contact me on here, or at any of the below links:

Discord: https://discord.gg/xcCnhNf4hN \
YouTube: https://www.youtube.com/channel/UCQmgRGWDJFXVYoin2UzUt7Q \
Twitter: https://twitter.com/MeatwareMonster \
​Reddit: https://www.reddit.com/user/MeatwareMonster/

### Mod developers:

Code: https://github.com/MeatwareMonster/Steamheim

Feel free to use my code as an example for your own mods, or if you would like to contribute to the overall steampunk theme, I would love that as well! I have a channel in the above discord dedicated to the idea, but I'm also in the other modding discords. Or, if you just like working solo, let me know if you make your own steampunk mod and I would be thrilled to see it!
