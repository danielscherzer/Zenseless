This first exercise has the goal of getting to know OpenGL. Just for this exercise we will not press OOP rules. This exercise does not require the Zenseless framework.
1. Execute program
1. Give each vertex a different color
	+ Note that OpenGL is a state machine. Color stays until other color is selected.
1. Execute and compare!
1. Move the draw code for the quad into a function `void DrawBox(Vector2 min, Vector2 size)`.
    + When your write `Vector2` Visual Studio will give you an error. Hit the light bulb to let Visual Studio suggest fixes for your code.
1. Uncomment `wnd.KeyDown += ...`. Execute. Hit `Escape`.
1. Make the quad change directions when it leaves the window.
1. Draw a number of boxes that move into different directions.
    + Each box should change direction when it hits the window border.
	+ These will be our enemies
1. Add a box that will be our player
	1. Make this box move to the left if `Keyboard.GetState().IsKeyDown(Key.Left)` is `true`
	1. Make this box move to the right if `Keyboard.GetState().IsKeyDown(Key.Right)` is `true`
1. Check if any of the boxes overlaps the player. Close the window when this happens.
1. Resize the window. What happens? 
1. Add `wnd.Resize += (s, a) => GL.Viewport(0, 0, wnd.Width, wnd.Height);` after creating the window. What happens when you resize the window now?
1. Switch off Vsync (uncomment `wnd.VSync...`). What happens to the animations?
    1. Fix the animations by scaling with `wnd.GetTime()`.
    2. Move calculation of the time for one frame into the `MyWindow class`, so you end up with a method `float wnd.GetLastFrameTime()`.

We created our first mini game. All the code is in one class, which will make maintaining it very hard. We will look at better ways of organizing your code in the next exercise.