In this exercise you will implement a first sketch of a top-down tile-based action-adventure, like Zelda ([YouTube](https://www.youtube.com/watch?v=Z6hjG6MCcZ8)). We start out with the code that loads a tile based map from [Tiled](https://www.mapeditor.org/).
Implement the following items and answer questions about your implementation and the used theory from the lecture (color, blending, textures, transformations, particle systems) for up to 40 points:
1. Implement reading and drawing from multiple tile layers.
1. Add an animated player sprite that should move smoothly over the tiles.
   1. The animation should reflect the movement direction.
1. Add a collision layer (a layer that contains no visible tiles, but collision information) in [Tiled](https://www.mapeditor.org/) and load this collision information (for instance for each tile a boolean stores if the tile is walkable) to restrict the players movements.
1. Add a tile for the start position and goal of your player.
1. Add a key-like pick-up to open doors/allow movement to certain parts of the level.
   1. The pick-up should be removed from the map after pick-up.
1. The camera shows only part of the level and moves with your player.
1. Only draw visible tiles
1. Make your collision test *efficient* (do not test tiles were the player cannot be at the moment).
1. Make your own big level (at least 100x100 tiles) were you have to reach a certain goal tile. You do not have to make your own tile-set.
1. Add other game elements, like portals, other pick-ups, power-ups, enemies/NPCs, a tile layer that is in front of the player, an inventory, animated tiles (fire), mini-map, ... or your own ideas. (up to 8 points)