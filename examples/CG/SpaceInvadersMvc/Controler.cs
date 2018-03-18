using OpenTK.Input;
using System;
using Zenseless.ExampleFramework;

namespace SpaceInvadersMvc
{
	class Controler
	{
		[STAThread]
		private static void Main()
		{
			using (var window = new ExampleWindow())
			{
				var model = new Model();
				var view = new View(window.RenderContext.RenderState, window.ContentLoader);
				var sound = new Sound(window.ContentLoader);
				model.OnShoot += (sender, args) => { sound.Shoot(); };
				model.OnEnemyDestroy += (sender, args) => { sound.DestroyEnemy(); };
				model.OnLost += (sender, args) => { sound.Lost(); };
				sound.Background();

				window.Render += () => view.DrawScreen(model.Enemies, model.Bullets, model.Player);
				window.Update += (dt) => Update(model);
				window.Run();
			}
		}

		private static void Update(Model logic)
		{
			float axisLeftRight = Keyboard.GetState()[Key.Left] ? -1.0f : Keyboard.GetState()[Key.Right] ? 1.0f : 0.0f;
			bool shoot = Keyboard.GetState()[Key.Space];
			logic.Update(axisLeftRight, shoot);
		}
	}
}