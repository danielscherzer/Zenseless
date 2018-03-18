You will add shadow mapping to this example.
1. Create a first pass that renders the geometry to an off-screen buffer. A `FBO` for this purpose is already created in the code.
1. This first pass should be rendered from the point-of-view of the light source. A second `cameraLight` is already created in the code.
1. Adapt the `depth*.glsl` shaders so that the shadow map texture stores the depth of the scene from the point of view of a light source.
1. Adapt the other shaders to use the shadow map while rendering the scene.
1. Add phong lighting (inside the shadow only ambient)
1. Add a second shadow casting lightsource that moves
1. Add depth biasing
1. Add percentage-Closer-Filtering