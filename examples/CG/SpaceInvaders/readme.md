### Implement MVC (Model-View-Controller)
The view is responsible for the visual representation of your game. The model is responsible for the logic of your game. The controller handles inputs and is responsible to setup the data-flow between model and view.
The model does not know about the view.
1. Create a class `Visual`
1. Move all code releated to drawing into this class (including all OpenGL calls)
1. Create a class `GameLogic`
1. Move all code that handles the game logic (game mechanics, collision, ...) into this class. Note: this will be most of the code and variables in the `Program.cs`
1. In `Visual` draw your objects using textures
1. Compare your solution to [MVCSpaceInvaders](../MVCSpaceInvaders) 