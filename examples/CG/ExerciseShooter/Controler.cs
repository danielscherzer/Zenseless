namespace SpaceInvadersMvc
{
	using System;
	using Zenseless.ExampleFramework;
	using Zenseless.HLGL;

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
				window.Update += (dt) => Update(window.Input, model);
				window.Run();
	
			}
		}

		private static void Update(IInput input, Model logic)
		{
			float axisLeftRight = input.IsButtonDown("Left") ? -1f : input.IsButtonDown("Right") ? 1f : 0f;
			bool shoot = input.IsButtonDown("Space");
			logic.Update(axisLeftRight, shoot);
		}
	}
}