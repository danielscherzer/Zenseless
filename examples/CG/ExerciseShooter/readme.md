In this exercise you will implement a first prototype of a scrolling shoot 'em up arcade game, like Xevious ([YouTube](https://www.youtube.com/watch?v=Jlq360e3bdI)) or Darius. We start out with the code (MVC architecture) for a space shooter with advancing enemy fronts and a single screen level.
Implement the following items and answer questions about your implementation and the used theory from the lecture (color, blending, textures, transformations, particle systems) for up to 35 points:
1. Implement a scrolling background
1. Implement flying and shooting enemies that come in waves.
1. Use your own sprites that use alpha and blending.
1. Play animations when enemies are destroyed (explosions, blood splat, ...) (look at [TextureAnim](../TextureAnim))
1. Implement enemies that are fixed relative to the scrolling background.
1. Render your content with correct aspect ratio when the window changes size (react on `Window.Resize` and scale your coordinate system with the aspect ratio of the window).
1. Implement hierarchical movement (using combined transformations) for some enemy groups (look at [Transformation](../Transformation)).
1. Use a Bitmap font to show points (look at [TextureFont](../TextureFont)).
1. Use a particle system with textured particles for some effects (thruster of player/enemy space ships, fire on the ground, ...) (look at [SimpleParticleSystem2D](../SimpleParticleSystem2D)
1. Add other game elements, like different weapons, shield, parallax background, more animations, ... your own ideas. (up to 7 points)