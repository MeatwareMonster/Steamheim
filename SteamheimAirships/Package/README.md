Steamheim Airships

This mod adds two airships, the Titan and the Sun Scout, into the game in what is hopefully the first of several mods focused on getting some steampunk elements into Valheim!

Showcase: https://www.youtube.com/watch?v=AhuGtn5LhQU

Adaptation of the large ship and the coding was done by myself, with the model and effects for the small ship put together by MagikarppSushii021194! It looks great and has some effects that aren't even enabled yet, make sure to tell him it's excellent! Also shoutout to Zethin64 for helping me test!

Details:

- Each ship has one control seat and some other seating for passengers, as well as some storage.
- Ships are controlled using standard directional movement to adjust thrust and turn the ship, and jump and crouch to adjust lift.
- Thrust and lift will stay at their set position until someone changes them, or everyone gets off the ship.
- The large ship is not affected by gravity, but the small ship is so it can land on the carrier.

DO NOT LOG OUT ON TOP OF THE SHIP IN MULTIPLAYER. YOU WILL SPAWN IN THE AIR IF IT IS MOVED, AND YOU WILL DIE. \
I'm hoping to add some safety checks in a future update, but for now, just DON'T DO IT.

Multiplayer was tested, but there may still be some networking bugs. Please report them if you run into any, and I will address them as soon as possible.

Since there may be bugs, the resource costs for both ships are relatively low. These will probably increase in a future release.

If you would like to change the costs, or any other airship settings, all of them are located in Assets/airshipConfig.json.

If you need anything else, feel free to contact me on here, or at any of the below links:

Discord: https://discord.gg/xcCnhNf4hN \
YouTube: https://www.youtube.com/channel/UCQmgRGWDJFXVYoin2UzUt7Q \
Twitter: https://twitter.com/MeatwareMonster \
​Reddit: https://www.reddit.com/user/MeatwareMonster/

Mod developers:\
Code: https://github.com/MeatwareMonster/Steamheim \
Adding your own airships is pretty straightforward if you would like to give it a shot! Simply make anything that you would like to fly, add a rigidbody and the necessary scripts (below), and a single sitpoint named "Controls" somewhere in the hierarchy.﻿ At the moment you will need to embed your asset bundle in the project, but I will probably change that in the near future to help with extensibility. Then, add a new config to airshipConfig.json referencing your asset bundle and the settings you would like to apply to your ship. I'll gladly help out if you need it!

Scripts:

- Z Net View
- Z Sync Transform
- Piece
- Wear N Tear

I'll probably add a readme on this shortly.

If you would like to contribute to the overall steampunk theme, I would love that as well! I have a channel in the above discord dedicated to the idea, but I'm also in the other modding discords. Or, if you just like working solo, let me know if you make your own steampunk mod and I would be thrilled to see it!
