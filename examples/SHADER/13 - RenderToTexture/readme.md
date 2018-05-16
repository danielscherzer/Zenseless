1. Look at example post-processing shaders.
1. Create a bad TV shader.
1. Create some other screen-space effect, like the dot matrix shader.
1. Bloom/Blur
   1. Create a 3x3 blur shader with one pass.
   1. Make the blur kernel adaptable to different kernel sizes.
   1. Try out increasing kernel sizes and look how this slows down your program.
   1. Create a two pass blur shader: First filter vertical then filter the filtered result of this step horizontal.
   1. Try out increasing kernel sizes and look how this slows down your program.
   1. Create a bloom shader: Not everything is blurred, but only very bright parts of your scene.
1. Deferred shading
   1. Render into normal, depth and material textures.
   1. Execute shading (with many lights) using these textures as input.
