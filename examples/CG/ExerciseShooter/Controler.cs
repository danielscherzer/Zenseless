namespace SpaceInvadersMvc
{
	using OpenTK.Input;
	using System;
	using Zenseless.ExampleFramework;

	class Controler
	{
		[STAThread]
		private static void Main()
		{
			using (var window = new ExampleWindow())
			{
				var model = new Model();
				var view = new View(window.ContentLoader);
				var sound = new Sound(window.ContentLoader);
				model.OnShoot += (sender, args) => { sound.Shoot(); };
				model.OnEnemyDestroy += (sender, args) => { sound.DestroyEnemy(); };
				model.OnLost += (sender, args) => { sound.Lost(); };
				sound.Background();

				window.Render += () =>
				{
					view.Clear();
					view.DrawEnemies(model.Enemies);
					view.DrawBullets(model.Bullets);
					view.DrawPlayer(model.Player);
				};
				window.Update += (dt) => Update(model);
				window.Run();
				window.Dispose();
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